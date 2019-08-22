using Process_Page;
using Process_Page.ViewModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MaterialDesignColors.WpfExample.Domain
{
    /// <summary>
    /// Interaction logic for SampleProgressDialog.xaml
    /// </summary>
    public partial class SampleSaveDialog : UserControl
    {
        public SampleSaveDialog()
        {
            InitializeComponent();
            this.DataContext = new SampleSaveDialogViewModel();
        }

        private void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {

            if (e.Parameter is string stringValue)
            {
                try
                {
                    Clipboard.SetDataObject(stringValue);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }

        private void click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue("저장되었습니다.");
            }, TaskScheduler.FromCurrentSynchronizationContext());
            //this.DataContext = new page1veiwmodel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
            //currentPage.Faceline_layer0.Visibility = Visibility.Hidden;
            Snapshot(currentPage.image_view, 1, 100);

        }

        private void Snapshot(UIElement source, double scale, int quality)
        {

            double actualHeight = source.RenderSize.Height;
            double actualWidth = source.RenderSize.Width;

            double renderHeight = actualHeight * scale;
            double renderWidth = actualWidth * scale;

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
            VisualBrush sourceBrush = new VisualBrush(source);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                //drawingContext.PushTransform(new ScaleTransform(scale, scale));
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }
            renderTarget.Render(drawingVisual);


            SmileDesign_Page.jpgEncoder.QualityLevel = quality;
            SmileDesign_Page.jpgEncoder.Frames.Add(BitmapFrame.Create(renderTarget));




            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                SampleSaveDialogViewModel.bi = bitmapImage;
            }


        }
    }
}
