﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Process_Page_Change.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Process_Page.ViewModel
{
    class SampleSaveDialogViewModel : ViewModelBase
    {
        public static int firstcheck =0;
        public static BitmapImage bi = new BitmapImage();
        public static BitmapImage upbi = new BitmapImage();
        public static BitmapImage Lowbi = new BitmapImage();
        public int i = 0;
        public ImageSource finalimage
        {
            get
            {
                return bi;
            }
            set { }
        }
        public ImageSource upperimage
        {
            get
            {
                return upbi;
            }
            set { }
        }
        public ImageSource Lowerimage
        {
            get
            {
                return Lowbi;
            }
            set { }
        }


        private bool _checkteeth = true;
        public bool checkteeth
        {
            get { return _checkteeth; }
            set
            {
                if (_checkteeth != value)
                {
                    _checkteeth = value;
                    if (value == true)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.UserUpper.Visibility = Visibility.Visible;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    if (value == false)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.UserUpper.Visibility = Visibility.Hidden;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    RaisePropertyChanged("checkfaceline");
                }
            }
        }
        private bool _checkdownteeth = true;
        public bool checkdownfaceline
        {
            get { return _checkdownteeth; }
            set
            {
                if (_checkdownteeth != value)
                {
                    _checkdownteeth = value;
                    if (value == true)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.UserLower.Visibility = Visibility.Visible;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    if (value == false)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.UserLower.Visibility = Visibility.Hidden;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    RaisePropertyChanged("checkfaceline");
                }
            }
        }
        private bool _checksmile = true;
        public bool checksmile
        {
            get { return _checksmile; }
            set
            {
                if (_checksmile != value)
                {
                    _checksmile = value;
                    if (value == true)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.FrontalFaceImage.Visibility = Visibility.Visible;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    if (value == false)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.FrontalFaceImage.Visibility = Visibility.Hidden;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    RaisePropertyChanged("checkfaceline");
                }
            }
        }
        private bool _checkopener = true;
        public bool checkopener
        {
            get { return _checkopener; }
            set
            {
                if (_checkopener != value)
                {
                    _checkopener = value;
                    if (value == true)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.GagFaceImage.Visibility = Visibility.Visible;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    if (value == false)
                    {
                        SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                        currentPage.GagFaceImage.Visibility = Visibility.Hidden;
                        Snapshot(currentPage.cap_canvas, 1, 100);
                        RaisePropertyChanged("finalimage");
                    }
                    RaisePropertyChanged("checkfaceline");
                }
            }
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

            SmileDesign_Page.jpgEncoder = new JpegBitmapEncoder();
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
        private RelayCommand<object> _createfile;
        public RelayCommand<object> createfile
        {
            get
            {
                if (_createfile == null)
                    return _createfile = new RelayCommand<object>(param => this.saveFile());
                return _createfile;
            }
            set
            {
                _createfile = value;
            }
        }

        private void saveFile()
        {
            using (FileStream stm = File.OpenWrite(@"./finalimage/" + PatientInfo.Patient_Info.Name + ".png"))
                SmileDesign_Page.jpgEncoder.Save(stm);

            if (firstcheck==0)
            {
                using (FileStream stm1 = File.OpenWrite(@"./finalimage/" + PatientInfo.Patient_Info.Name + "up" + ".png"))
                    SmileDesign_Page.jpgupEncoder.Save(stm1);




                using (FileStream stm2 = File.OpenWrite(@"./finalimage/" + PatientInfo.Patient_Info.Name + "Low" + ".png"))
                    SmileDesign_Page.jpgdownEncoder.Save(stm2);
                firstcheck = 1;
            }
           
             
              

           
        }


    }
}
