using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Process_Page
{
    /// <summary>
    /// PatientInfo_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PatientInfo_Page : Page
    {
        public PatientInfo_Page()
        {
            InitializeComponent();
            this.DataContext = new DialogsViewModel();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue(LoginViewModel.logDr_name + "님 안녕하세요");
            }, TaskScheduler.FromCurrentSynchronizationContext());
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

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Snapshot(patient, 1, 100);

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

            JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
            jpgEncoder.QualityLevel = quality;
            jpgEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (FileStream stm = File.OpenWrite(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\test2.png"))
                jpgEncoder.Save(stm);
        }

   
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(400);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue("수정되었습니다.");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(400);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue("삭제되었습니다.");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
