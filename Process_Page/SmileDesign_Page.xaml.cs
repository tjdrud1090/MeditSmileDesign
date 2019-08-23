using System;
using System.Collections.Generic;
using System.IO;
using Process_Page_Change.ViewModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Process_Page.ViewModel;

namespace Process_Page
{
    /// <summary>
    /// SmileDesign_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SmileDesign_Page : Page
    {
        public static JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
        public SmileDesign_Page()
        {
            InitializeComponent();
            this.DataContext = new FaceAlign_PageViewModel();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Snapshot(image_view, 1, 100);
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


            jpgEncoder.QualityLevel = quality;
            jpgEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login_Page page = new Login_Page();
            System.Windows.Application.Current.MainWindow.Content = page;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
