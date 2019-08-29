using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.ComponentModel;

using Process_Page.Util;
using Process_Page.Cards;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using Process_Page.ToothTemplate;
using Process_Page.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;
using MaterialDesignColors.WpfExample.Domain;
using Process_Page_Change.Util;
using Process_Page.ToothTemplate.Utils;
using Process_Page.ToothTemplate.ArrowLine;
using Process_Page.Domain;
using System.Xml.Serialization;

using IOText = System.IO;
using System.Xml;

namespace Process_Page.ViewModel
{
    public class FaceAlign_PageViewModel : ViewModelBase
    {
     
        #region constructor
        public FaceAlign_PageViewModel()
        {
            GagFacePoints.eye.Add(new OpenCvSharp.Point(0, 0));
            GagFacePoints.eye.Add(new OpenCvSharp.Point(0, 0));
            GagFacePoints.mouse.Add(new OpenCvSharp.Point(0, 0));
            GagFacePoints.mouse.Add(new OpenCvSharp.Point(0, 0));
            GagFacePoints.midline.Add(new OpenCvSharp.Point(0, 0));
            GagFacePoints.midline.Add(new OpenCvSharp.Point(0, 0));

            FrontalFacePoints.eye.Add(new OpenCvSharp.Point(0, 0));
            FrontalFacePoints.eye.Add(new OpenCvSharp.Point(0, 0));
            FrontalFacePoints.mouse.Add(new OpenCvSharp.Point(0, 0));
            FrontalFacePoints.mouse.Add(new OpenCvSharp.Point(0, 0));
            FrontalFacePoints.midline.Add(new OpenCvSharp.Point(0, 0));
            FrontalFacePoints.midline.Add(new OpenCvSharp.Point(0, 0));

            // 단계별 내용
            flowname.Add("Face Line \ncoordinates");
            flowname.Add("Face Align \n& Measurement");

            _changeText = flowname.ElementAt(0);
            _showControl0 = Visibility.Visible;
            _showControl1 = Visibility.Hidden;
            _FaceLineVisiblity = Visibility.Visible;
            _LineVisiblity = Visibility.Visible;
            _FrontalRefVisiblity = Visibility.Hidden;
            _GagRefVisiblity = Visibility.Hidden;

            RaisePropertyChanged("showControl0");
            RaisePropertyChanged("showControl1");
            RaisePropertyChanged("FaceLineVisiblity");
            RaisePropertyChanged("LineVisiblity");

            RaisePropertyChanged("changeText");
            RaisePropertyChanged("openFileClick");

            RaisePropertyChanged("FrontalRefVisiblity");
            RaisePropertyChanged("GagRefVisiblity");
        }
        #endregion

        #region Page Change command
        private RelayCommand<object> _NextPageClick;
        public RelayCommand<object> NextPageClick
        {
            get
            {
                if (_NextPageClick == null)
                    return _NextPageClick = new RelayCommand<object>(param => this.NextFlowClicked());
                return _NextPageClick;
            }
            set
            {
                _NextPageClick = value;
            }
        }

        //  Manual

        private RelayCommand<object> _Teeth_point_manual;
        public RelayCommand<object> Teeth_point_manual
        {
            get
            {
                if (_Teeth_point_manual == null)
                    return _Teeth_point_manual = new RelayCommand<object>(param => this.teethManual());
                return _Teeth_point_manual;
            }
            set
            {
                _Teeth_point_manual = value;
            }
        }
        public void teethManual()
        {
            ExecuteRunDialoga(0);
        }

        private async void ExecuteRunDialoga(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new Manual1
            {
                DataContext = new FaceAlign_PageViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandlerr);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void ClosingEventHandlerr(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }











        private RelayCommand<object> _PrePageClick;
        public RelayCommand<object> PrePageClick
        {
            get
            {
                if (_PrePageClick == null)
                    return _PrePageClick = new RelayCommand<object>(param => this.PrevFlowClicked());
                return _PrePageClick;
            }
            set
            {
                _PrePageClick = value;
            }
        }
       
           



            List<string> flowname = new List<string>();
        // 단계 : Face Line coordinates(0) => Face Align & Measurement(1)

        public void NextFlowClicked()
        {
            int index = flowname.IndexOf(changeText) + 1;
            switch (index)
            {
                case 1:
                    _showControl0 = Visibility.Hidden;
                    _showControl1 = Visibility.Visible;
                    _FaceLineVisiblity = Visibility.Hidden;
                    _LineVisiblity = Visibility.Hidden;

                    _changeText = flowname.ElementAt(1);
                    RaisePropertyChanged("showControl0");
                    RaisePropertyChanged("showControl1");
                    RaisePropertyChanged("FaceLineVisiblity");
                    RaisePropertyChanged("LineVisiblity");

                    RaisePropertyChanged("changeText");
                    break;
                case 2:
                    if (RefernceCount < 4)
                    {
                        // 메시지 추가
                        return;
                    }
                    //SaveXML();
                    SetnewPage();
                    break;
                default:
                    break;
            }
        }

        public void SetnewPage()
        {

            System.Windows.Application.Current.MainWindow.UpdateLayout();
            var mainWnd = ((MainWindow)System.Windows.Application.Current.MainWindow) as MainWindow;
            var current = mainWnd.Content as FaceAlign_Page;

            if (mainWnd.OldPage != null)
            {
                System.Windows.Application.Current.MainWindow.Content = mainWnd.OldPage as SmileDesign_Page;
                mainWnd.OldPage = (FrameworkElement)current;
                return;
            }

            mainWnd.OldPage = (FrameworkElement)(System.Windows.Application.Current.MainWindow.Content);

            // 다음 페이지에 넘겨주어야할 정보

            SmileDesign_Page page = new SmileDesign_Page();
            System.Windows.Application.Current.MainWindow.Content = page;
        }

        public void PrevFlowClicked()
        {
            int index = flowname.IndexOf(changeText) - 1;
            switch (index)
            {
                case 0:
                    _showControl0 = Visibility.Visible;
                    _showControl1 = Visibility.Hidden;
                    _FaceLineVisiblity = Visibility.Visible;
                    _LineVisiblity = Visibility.Visible;

                    _changeText = flowname.ElementAt(0);
                    RaisePropertyChanged("showControl0");
                    RaisePropertyChanged("showControl1");
                    RaisePropertyChanged("FaceLineVisiblity");
                    RaisePropertyChanged("LineVisiblity");

                    RaisePropertyChanged("changeText");

                    _FrontalRefVisiblity = Visibility.Hidden;
                    _GagRefVisiblity = Visibility.Hidden;
                    RaisePropertyChanged("FrontalRefVisiblity");
                    RaisePropertyChanged("GagRefVisiblity");

                    break;
                default:
                    break;
            }
        }

        private string _changeText;
        public string changeText
        {
            get { return _changeText; }
            set
            {
                if (_changeText != value)
                {
                    _changeText = value;
                    RaisePropertyChanged("changeText");
                }
            }
        }

        private Visibility _showControl0;
        public Visibility showControl0
        {
            get { return _showControl0; }
            set
            {
                if (_showControl0 != value)
                {
                    _showControl0 = value;
                    RaisePropertyChanged("showControl0");
                }
            }
        }
        private Visibility _showControl1;
        public Visibility showControl1
        {
            get { return _showControl1; }
            set
            {
                if (_showControl1 != value)
                {
                    _showControl1 = value;
                    RaisePropertyChanged("showControl1");
                }
            }
        }

        private Visibility _FaceLineVisiblity;
        public Visibility FaceLineVisiblity
        {
            get { return _FaceLineVisiblity; }
            set
            {
                if (_FaceLineVisiblity != value)
                {
                    _FaceLineVisiblity = value;
                    RaisePropertyChanged("FaceLineVisiblity");
                }
            }
        }

        private Visibility _LineVisiblity;
        public Visibility LineVisiblity
        {
            get { return _LineVisiblity; }
            set
            {
                if (_LineVisiblity != value)
                {
                    _LineVisiblity = value;
                    RaisePropertyChanged("LineVisiblity");
                }
            }
        }

        private Visibility _FrontalRefVisiblity;
        public Visibility FrontalRefVisiblity
        {
            get { return _FrontalRefVisiblity; }
            set
            {
                if (_FrontalRefVisiblity != value)
                {
                    _FrontalRefVisiblity = value;
                    RaisePropertyChanged("FrontalRefVisiblity");
                }
            }
        }

        private Visibility _GagRefVisiblity;
        public Visibility GagRefVisiblity
        {
            get { return _GagRefVisiblity; }
            set
            {
                if (_GagRefVisiblity != value)
                {
                    _GagRefVisiblity = value;
                    RaisePropertyChanged("GagRefVisiblity");
                }
            }
        }

        private Visibility _RotateControlVisiblity;
        public Visibility RotateControlVisiblity
        {
            get { return _RotateControlVisiblity; }
            set
            {
                if (_RotateControlVisiblity != value)
                {
                    _RotateControlVisiblity = value;
                    RaisePropertyChanged("RotateControlVisiblity");
                }
            }
        }
        #endregion

        #region face_landmark line draw property

        // face point Get
        public FaceDetector.face_point GagFacePoints = new FaceDetector.face_point();
        public FaceDetector.face_point FrontalFacePoints = new FaceDetector.face_point();

        // landmark
        private ObservableCollection<Point> _FrontalPoints;
        public ObservableCollection<Point> FrontalPoints
        {
            get
            {
                return _FrontalPoints;
            }
        }

        private ObservableCollection<Point> _GagPoints;
        public ObservableCollection<Point> GagPoints
        {
            get
            {
                return _GagPoints;
            }
        }

        private ObservableCollection<PointViewModel> _FrontalMouthPoints;
        public ObservableCollection<PointViewModel> FrontalMouthPoints
        {
            get
            {
                return _FrontalMouthPoints;
            }
        }

        //opencv_point -> W_Point로 바꾸기
        private Point OpenCVPoint2W_Point(OpenCvSharp.Point pt)
        {
            Point result = new Point();
            result = new Point(pt.X, pt.Y);

            return result;
        }

        #region Face Line
        public LineGeometry _midline = new LineGeometry();
        public LineGeometry _noseline_L = new LineGeometry();
        public LineGeometry _noseline_R = new LineGeometry();
        public LineGeometry _eyeline = new LineGeometry();
        public LineGeometry _lipline = new LineGeometry();

        public LineGeometry midline
        {
            get { return _midline; }
            set { }
        }

        public LineGeometry noseline_L
        {
            get { return _noseline_L; }
            set { }
        }

        public LineGeometry noseline_R
        {
            get { return _noseline_R; }
            set { }
        }

        public LineGeometry eyeline
        {
            get { return _eyeline; }
            set { }
        }


        public LineGeometry lipline
        {
            get { return _lipline; }
            set { }
        }
        #endregion

        #region align reference
        // teeth reference points
        public EllipseGeometry _teethL = new EllipseGeometry();
        public EllipseGeometry teethL
        {
            get { return _teethL; }
            set { }
        }
        public EllipseGeometry _teethR = new EllipseGeometry();
        public EllipseGeometry teethR
        {
            get { return _teethR; }
            set { }
        }

        public EllipseGeometry _FrontalteethL = new EllipseGeometry();
        public EllipseGeometry FrontalteethL
        {
            get { return _FrontalteethL; }
            set { }
        }
        public EllipseGeometry _FrontalteethR = new EllipseGeometry();
        public EllipseGeometry FrontalteethR
        {
            get { return _FrontalteethR; }
            set { }
        }

        #endregion

        #endregion

        #region image file loading by openfileDialog
        //command binding
        private RelayCommand<object> _openFileClick;
        public RelayCommand<object> openFileClick
        {
            get
            {
                Init();
                if (_openFileClick == null)
                    return _openFileClick = new RelayCommand<object>(param => this.Init());
                return _openFileClick;
            }
            set
            {
                _openFileClick = value;
            }
        }
        
        //image source property binding
        public ImageSource GagFaceSource
        {
            get { return GagFaceImage; }
            set
            {
                RaisePropertyChanged("GagFaceSource");
            }
        }
        public ImageSource FrontalFaceSource
        {
            get { return FrontalFaceImage; }
            set
            {
                RaisePropertyChanged("FrontalFaceSource");
            }
        }

        //image original
        public BitmapImage FrontalFaceImage;
        public BitmapImage GagFaceImage;

        //command에 들어갈 file 열기 명령
        private void Init()
        {
            // 개구기 사진을 기준으로 명령 확인하기
            // 파일 열기
            FaceDetector faceDetector = new FaceDetector(PatientInfo.Patient_Info.teeth_opener_filename);
            GagFaceImage = faceDetector.face;

            // face align data draw
            this.GagFacePoints = faceDetector.fp;

            ObservableCollection<Point> savepoint = new ObservableCollection<Point>();

            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.midline[0]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.midline[1]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.eye[0]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.eye[1])); 
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.mouse[0]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.mouse[1]));

            _GagPoints = savepoint;

            //face point
            RaisePropertyChanged("GagPoints");
            RaisePropertyChanged("GagFaceSource");

            draw_faceline();

            // 미소 사진 미리 다운로드
            // 파일 열기
            FaceDetector faceDetector2 = new FaceDetector(PatientInfo.Patient_Info.frontfilename);
            FrontalFaceImage = faceDetector2.face;

            // face align data draw
            this.FrontalFacePoints = faceDetector2.fp;

            ObservableCollection<Point> savepoint2 = new ObservableCollection<Point>();

            savepoint2.Add(OpenCVPoint2W_Point(FrontalFacePoints.midline[0]));
            savepoint2.Add(OpenCVPoint2W_Point(FrontalFacePoints.midline[1]));
            savepoint2.Add(OpenCVPoint2W_Point(FrontalFacePoints.eye[0]));
            savepoint2.Add(OpenCVPoint2W_Point(FrontalFacePoints.eye[1]));
            savepoint2.Add(OpenCVPoint2W_Point(FrontalFacePoints.mouse[0]));
            savepoint2.Add(OpenCVPoint2W_Point(FrontalFacePoints.mouse[1]));

            _FrontalPoints = savepoint2;
        }

        //face point 보정
        public FaceDetector.face_point change_point_position(double percentage, double curposition, FaceDetector.face_point points)
        {
            FaceDetector.face_point sizechange = new FaceDetector.face_point();

            OpenCvSharp.Point changing;
            changing.X = (int)(((double)points.eye[0].X) * percentage + curposition);
            changing.Y = (int)(((double)points.eye[0].Y) * percentage);
            sizechange.eye.Add(changing);

            changing.X = (int)(((double)points.eye[1].X) * percentage + curposition);
            changing.Y = (int)(((double)points.eye[1].Y) * percentage);
            sizechange.eye.Add(changing);

            changing.X = (int)(((double)points.midline[0].X) * percentage + curposition);
            changing.Y = (int)(((double)points.midline[0].Y) * percentage);
            sizechange.midline.Add(changing);

            changing.X = (int)(((double)points.midline[1].X) * percentage + curposition);
            changing.Y = (int)(((double)points.midline[1].Y) * percentage);
            sizechange.midline.Add(changing);

            changing.X = (int)(((double)points.mouse[0].X) * percentage + curposition);
            changing.Y = (int)(((double)points.mouse[0].Y) * percentage);
            sizechange.mouse.Add(changing);

            changing.X = (int)(((double)points.mouse[1].X) * percentage + curposition);
            changing.Y = (int)(((double)points.mouse[1].Y) * percentage);
            sizechange.mouse.Add(changing);

            changing.X = (int)(((double)points.nose[0].X) * percentage + curposition);
            changing.Y = (int)(((double)points.nose[0].Y) * percentage);
            sizechange.nose.Add(changing);

            changing.X = (int)(((double)points.nose[1].X) * percentage + curposition);
            changing.Y = (int)(((double)points.nose[1].Y) * percentage);
            sizechange.nose.Add(changing);

            foreach (var point in points.mouth)
            {
                changing.X = (int)(((double)point.X) * percentage + curposition);
                changing.Y = (int)(((double)point.Y) * percentage);
                sizechange.mouth.Add(changing);
            }

            return sizechange;
        }

        public void draw_faceline()
        {
            // 현재 이미지 캔버스의 사이즈를 측정
            System.Windows.Application.Current.MainWindow.UpdateLayout();
            FaceAlign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as FaceAlign_Page;
            double height = currentPage.CanvasView.ActualHeight;
            double width = currentPage.CanvasView.ActualWidth;

            double imageCanvasHeight = currentPage.GagFaceImage.ActualHeight;
            double imageCanvasWidth = currentPage.GagFaceImage.ActualWidth;

            double imageHeight = GagFaceImage.Height;
            double imagewidth = GagFaceImage.Width;

            double percentage = imageCanvasHeight / imageHeight;

            double curimagePosition = (imageCanvasWidth - imagewidth * percentage) / 2;

            GagFacePoints = change_point_position(percentage, curimagePosition, GagFacePoints);
            ObservableCollection<Point> savepoint = new ObservableCollection<Point>();

            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.midline[0]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.midline[1]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.eye[0]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.eye[1]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.mouse[0]));
            savepoint.Add(OpenCVPoint2W_Point(GagFacePoints.mouse[1]));

            _GagPoints = savepoint;
            RaisePropertyChanged("GagPoints");

            _TransGagCenter = _GagPoints.ElementAt(2);
            RaisePropertyChanged("TransGagCenter");

            //set picture center
            _GagCenter.X = width / 2 - GagFacePoints.eye[0].X - (GagFacePoints.eye[1].X - GagFacePoints.eye[0].X)/2;
            _GagCenter.Y = 50;

            _noseline_L = new LineGeometry();
            _noseline_L.StartPoint = new Point((_GagCenter.X + GagFacePoints.eye[0].X) + (GagFacePoints.midline[0].X - GagFacePoints.eye[0].X) / 2, 0);
            _noseline_L.EndPoint = new Point((_GagCenter.X + GagFacePoints.eye[0].X) + (GagFacePoints.midline[0].X - GagFacePoints.eye[0].X) / 2, height);

            _noseline_R = new LineGeometry();
            _noseline_R.StartPoint = new Point((_GagCenter.X + GagFacePoints.eye[1].X) - (GagFacePoints.eye[1].X - GagFacePoints.midline[0].X) / 2, 0);
            _noseline_R.EndPoint = new Point((_GagCenter.X + GagFacePoints.eye[1].X) - (GagFacePoints.eye[1].X - GagFacePoints.midline[0].X) / 2, height);

            _midline = new LineGeometry();
            _midline.StartPoint = new Point(width / 2, 0);
            _midline.EndPoint = new Point(width / 2, height);

            _eyeline = new LineGeometry();
            _eyeline.StartPoint = new Point(0, _GagCenter.Y + GagFacePoints.eye[0].Y);
            _eyeline.EndPoint = new Point(width, _GagCenter.Y + GagFacePoints.eye[0].Y);

            // Align set
            double angle = ((double)(GagFacePoints.eye[1].Y - GagFacePoints.eye[0].Y)) / ((double)(GagFacePoints.eye[1].X - GagFacePoints.eye[0].X));
            double rotate = (Math.Atan(angle)) * (180 / Math.PI);
            _GagAngle = -rotate;

            RaisePropertyChanged("GagAngle");
            RaisePropertyChanged("GagCenter");

            //propertychanged 알리기
            RaisePropertyChanged("midline");
            RaisePropertyChanged("noseline_L");
            RaisePropertyChanged("noseline_R");
            RaisePropertyChanged("eyeline");

            // mouse wheel center set
            _WheelMouseCenter.X = _midline.StartPoint.X;
            _WheelMouseCenter.Y = height / 2;
            RaisePropertyChanged("WheelMouseCenter");
        }

        public void SetAlign()
        {
            //face point
            RaisePropertyChanged("FrontalFaceSource");

            // 현재 이미지 캔버스의 사이즈를 측정
            System.Windows.Application.Current.MainWindow.UpdateLayout();
            FaceAlign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as FaceAlign_Page;
            double height = currentPage.CanvasView.ActualHeight;
            double width = currentPage.CanvasView.ActualWidth;

            double imageCanvasHeight = currentPage.FrontalFaceImage.ActualHeight;
            double imageCanvasWidth = currentPage.FrontalFaceImage.ActualWidth;

            double imageHeight = FrontalFaceImage.Height;
            double imagewidth = FrontalFaceImage.Width;

            double percentage = imageCanvasHeight / imageHeight;

            double curimagePosition = (imageCanvasWidth - imagewidth * percentage) / 2;

            // face point 보정
            FrontalFacePoints = change_point_position(percentage, curimagePosition, FrontalFacePoints);
            ObservableCollection<Point> savepoint = new ObservableCollection<Point>();

            savepoint.Add(OpenCVPoint2W_Point(FrontalFacePoints.midline[0]));
            savepoint.Add(OpenCVPoint2W_Point(FrontalFacePoints.midline[1]));
            savepoint.Add(OpenCVPoint2W_Point(FrontalFacePoints.eye[0]));
            savepoint.Add(OpenCVPoint2W_Point(FrontalFacePoints.eye[1]));
            savepoint.Add(OpenCVPoint2W_Point(FrontalFacePoints.mouse[0]));
            savepoint.Add(OpenCVPoint2W_Point(FrontalFacePoints.mouse[1]));

            _FrontalPoints = savepoint;
            RaisePropertyChanged("FrontalPoints");

            // Mouth pointviewmodel
            ObservableCollection<PointViewModel> savepoint2 = new ObservableCollection<PointViewModel>();
            int count = 0;
            foreach (var point in FrontalFacePoints.mouth)
            {
                Point pt = OpenCVPoint2W_Point(point);
                savepoint2.Add(new PointViewModel(pt.X,pt.Y, count));
                count++;
            }

            _FrontalMouthPoints = savepoint2;

            // Scale set
            _FrontalScale = 1;

            // FrontalCenter Set
            _FrontalCenter.X = _midline.StartPoint.X - FrontalFacePoints.eye[0].X - (FrontalFacePoints.eye[1].X - FrontalFacePoints.eye[0].X) / 2;
            _FrontalCenter.Y = _eyeline.StartPoint.Y - FrontalFacePoints.eye[0].Y;

            RaisePropertyChanged("FrontalCenter");
            RaisePropertyChanged("FrontalScale");
            //RaisePropertyChanged("FrontalAngle");

            // lip line Set
            _lipline = new LineGeometry();
            _lipline.StartPoint = new Point(0, _GagCenter.Y + FrontalFacePoints.mouse[0].Y);
            _lipline.EndPoint = new Point(width, _GagCenter.Y + FrontalFacePoints.mouse[0].Y);
            RaisePropertyChanged("lipline");

            // mouse wheel center set
            _WheelMouseCenter.X = _midline.StartPoint.X;
            _WheelMouseCenter.Y = height / 2;
            RaisePropertyChanged("WheelMouseCenter");
        }

        private void ToothAlign()
        {
            // Frontal teeth ref translate
            double diffx = (_teethL.Center.X - _FrontalteethL.Center.X);
            double diffy = (_teethL.Center.Y - _FrontalteethL.Center.Y);

            FrontalteethL.Center = new Point(_FrontalteethL.Center.X + diffx, _FrontalteethL.Center.Y + diffy);
            FrontalteethR.Center = new Point(_FrontalteethR.Center.X + diffx, _FrontalteethR.Center.Y + diffy);

            // Frontal Center X offset
            _FrontalCenter.X += diffx;
            _FrontalCenter.Y += diffy;

            RaisePropertyChanged("FrontalCenter");

            // Scale set
            double scalesize = (_teethL.Center.X - _teethR.Center.X) / (_FrontalteethL.Center.X - _FrontalteethR.Center.X);
            _FrontalScale = scalesize;

            // Align set
            double angle1 = (_FrontalteethR.Center.Y - teethL.Center.Y) / (_FrontalteethR.Center.X - teethL.Center.X);
            double angle2 = (_teethR.Center.Y - teethL.Center.Y) / (_teethR.Center.X - teethL.Center.X);

            double angle = Math.Abs(angle1) + Math.Abs(angle2);

            double rotate = (Math.Atan(angle)) * (180 / Math.PI);       //angle
            _FrontalAngle = -rotate;

            RaisePropertyChanged("FrontalScale");
            RaisePropertyChanged("FrontalAngle");

            RaisePropertyChanged("FrontalMouthPoints");

            _RotateControlCenter.X = _noseline_L.StartPoint.X;
            _RotateControlCenter.Y = _lipline.StartPoint.Y;
            RaisePropertyChanged("RotateControlCenter");

            offset_frontalangle = _FrontalAngle;
        }
        #endregion

        #region Align Property
        // Ratio
        public double ratio = 0;

        // Frontal Face Canvas Center
        private Point _FrontalCenter;
        public Point FrontalCenter
        {
            get { return _FrontalCenter; }
            set { }
        }

        private double _FrontalAngle;
        public double FrontalAngle
        {
            get { return _FrontalAngle; }
            set { }
        }

        private double _FrontalScale;
        public double FrontalScale
        {
            get { return _FrontalScale; }
            set { }
        }

        // Gag Image
        private Point _GagCenter;
        public Point GagCenter
        {
            get { return _GagCenter; }
            set { }
        }

        private double _GagAngle;
        public double GagAngle
        {
            get { return _GagAngle; }
            set { }
        }

        private double _GagScale;
        public double GagScale
        {
            get { return _GagScale; }
            set { }
        }

        // Transform Center
        private Point _TransCenter;
        public Point TransCenter
        {
            get { return _TransCenter; }
            set { }
        }

        private Point _TransGagCenter;
        public Point TransGagCenter
        {
            get { return _TransGagCenter; }
            set { }
        }
        // Rotate Control Center
        private Point _RotateControlCenter;
        public Point RotateControlCenter
        {
            get { return _RotateControlCenter; }
            set { }
        }
        #endregion

        #region sizeChange MouseWheel

        // mouse wheel sizechanged Center point
        private Point _WheelMouseCenter;
        public Point WheelMouseCenter
        {
            get { return _WheelMouseCenter; }
            set { }
        }

        //Mouse Wheel size changed
        private double _ViewScale = 1;
        public double ViewScale
        {
            get { return _ViewScale; }
            set
            {
                _ViewScale = value;
                RaisePropertyChanged("ViewScale");
            }
        }

        private RelayCommand<object> _SizeChangedWheel;
        public RelayCommand<object> SizeChangedWheel
        {
            get
            {
                if (_SizeChangedWheel == null) return _SizeChangedWheel = new RelayCommand<object>(param => ExecuteMouseWheel((MouseWheelEventArgs)param));
                return _SizeChangedWheel;
            }
            set { _SizeChangedWheel = value; }
        }

        private void ExecuteMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (_ViewScale < 7)
                {
                    _ViewScale += 0.1;
                    RaisePropertyChanged("ViewScale");
                }
            }
            else
            {
                if (_ViewScale > 1)
                {
                    _ViewScale -= 0.1;
                    RaisePropertyChanged("ViewScale");
                }
            }
        }
        #endregion

        // mouse event 수정해야될 부분
        //  - 사진 rotation 기능 추가 imagecanvas control을 회전
        #region MouseEvent

        private bool captured = false;
        private double orginal_width;
        private double orginal_height;
        private Point origMouseDownPoint;

        #region control offset
        // Frontal Image angle offset
        double offset_frontalangle;
        private double _TransfrontalAngle;

        #endregion

        private RelayCommand<object> _mouseMoveCommand;
        public RelayCommand<object> MouseMoveCommand
        {
            get
            {
                if (_mouseMoveCommand == null) return _mouseMoveCommand = new RelayCommand<object>(param => ExecuteMouseMove((MouseEventArgs)param));
                return _mouseMoveCommand;
            }
            set { _mouseMoveCommand = value; }
        }

        private void ExecuteMouseMove(MouseEventArgs e)
        {
            if (captured == true && e.LeftButton == MouseButtonState.Pressed)
            {
                if (changeText.Equals(flowname.ElementAt(1)) && RefernceCount < 4 && refclicked == true)
                {
                    return;
                }
                if (e.Source.GetType() == typeof(RotationControl))
                    return;
                if (e.Source.GetType() == typeof(ImageCanvas))
                {
                    Canvas imageCanvas = ((UserControl)e.Source).Parent as Canvas;

                    UIElement Uppertooth = new UIElement();
                    UIElement Lowertooth = new UIElement();

                    foreach (var temp in imageCanvas.Children)
                    {
                        if (temp.GetType() == typeof(UpperTooth))
                            Uppertooth = (UIElement)temp;
                        if (temp.GetType() == typeof(LowerTooth))
                            Lowertooth = (UIElement)temp;
                    }

                    Point curMouseDownPoint = e.GetPosition((IInputElement)((UserControl)e.Source).Parent);

                    //face line moving
                    double diffX = (curMouseDownPoint.X - orginal_width);
                    double diffY = (curMouseDownPoint.Y - orginal_height);

                    midline.StartPoint = new Point(_midline.StartPoint.X + diffX, _midline.StartPoint.Y);
                    midline.EndPoint = new Point(_midline.EndPoint.X + diffX, _midline.EndPoint.Y);

                    noseline_L.StartPoint = new Point(_noseline_L.StartPoint.X + diffX, _noseline_L.StartPoint.Y);
                    noseline_L.EndPoint = new Point(_noseline_L.EndPoint.X + diffX, _noseline_L.EndPoint.Y);

                    noseline_R.StartPoint = new Point(_noseline_R.StartPoint.X + diffX, _noseline_R.StartPoint.Y);
                    noseline_R.EndPoint = new Point(_noseline_R.EndPoint.X + diffX, _noseline_R.EndPoint.Y);

                    eyeline.StartPoint = new Point(_eyeline.StartPoint.X, _eyeline.StartPoint.Y + diffY);
                    eyeline.EndPoint = new Point(_eyeline.EndPoint.X, _eyeline.EndPoint.Y + diffY);

                    lipline.StartPoint = new Point(_lipline.StartPoint.X, _lipline.StartPoint.Y + diffY);
                    lipline.EndPoint = new Point(_lipline.EndPoint.X, _lipline.EndPoint.Y + diffY);

                    //teeth line moving
                    teethL.Center = new Point(_teethL.Center.X + diffX, _teethL.Center.Y + diffY);
                    teethR.Center = new Point(_teethR.Center.X + diffX, _teethR.Center.Y + diffY);

                    FrontalteethL.Center = new Point(_FrontalteethL.Center.X + diffX, _FrontalteethL.Center.Y + diffY);
                    FrontalteethR.Center = new Point(_FrontalteethR.Center.X + diffX, _FrontalteethR.Center.Y + diffY);

                    // image canvas
                    Point Center = new Point(_RotateControlCenter.X + diffX, _RotateControlCenter.Y + diffY);
                    _RotateControlCenter = Center;
                    RaisePropertyChanged("RotateControlCenter");

                    Center = new Point(_FrontalCenter.X + diffX, _FrontalCenter.Y + diffY);
                    _FrontalCenter = Center;
                    RaisePropertyChanged("FrontalCenter");

                    Center = new Point(_GagCenter.X + diffX, _GagCenter.Y + diffY);
                    _GagCenter = Center;
                    RaisePropertyChanged("GagCenter");

                    //Center = new Point(_ToothUpperCenter.X + diffX, _ToothUpperCenter.Y + diffY);
                    //_ToothUpperCenter = Center;
                    //RaisePropertyChanged("ToothUpperCenter");

                    //Center = new Point(_ToothLowerCenter.X + diffX, _ToothLowerCenter.Y + diffY);
                    //_ToothLowerCenter = Center;
                    //RaisePropertyChanged("ToothLowerCenter");

                    orginal_width = curMouseDownPoint.X;
                    orginal_height = curMouseDownPoint.Y;
                }
                else
                {
                    if (((Path)e.Source).Data == midline)
                    {
                        double diff = e.GetPosition((IInputElement)e.Source).X - orginal_width;
                        Point pt = new Point(_midline.StartPoint.X + diff, _midline.StartPoint.Y);
                        _midline.StartPoint = pt;

                        Point pt2 = new Point(_midline.EndPoint.X + diff, _midline.EndPoint.Y);
                        _midline.EndPoint = pt2;
                        RaisePropertyChanged("midline");

                        orginal_width = e.GetPosition((IInputElement)e.Source).X;
                    }
                    else if (((Path)e.Source).Data == noseline_L)
                    {
                        double diff = e.GetPosition((IInputElement)e.Source).X - orginal_width;
                        Point pt = new Point(_noseline_L.StartPoint.X + diff, _noseline_L.StartPoint.Y);
                        _noseline_L.StartPoint = pt;

                        Point pt2 = new Point(_noseline_L.EndPoint.X + diff, _noseline_L.EndPoint.Y);
                        _noseline_L.EndPoint = pt2;
                        RaisePropertyChanged("noseline_L");

                        orginal_width = e.GetPosition((IInputElement)e.Source).X;
                    }
                    else if (((Path)e.Source).Data == noseline_R)
                    {
                        double diff = e.GetPosition((IInputElement)e.Source).X - orginal_width;
                        Point pt = new Point(_noseline_R.StartPoint.X + diff, _noseline_R.StartPoint.Y);
                        _noseline_R.StartPoint = pt;

                        Point pt2 = new Point(_noseline_R.EndPoint.X + diff, _noseline_R.EndPoint.Y);
                        _noseline_R.EndPoint = pt2;
                        RaisePropertyChanged("noseline_R");

                        orginal_width = e.GetPosition((IInputElement)e.Source).X;
                    }
                    else if (((Path)e.Source).Data == eyeline)
                    {
                        double diff = e.GetPosition((IInputElement)e.Source).Y - orginal_height;
                        Point pt = new Point(_eyeline.StartPoint.X, _eyeline.StartPoint.Y + diff);
                        _eyeline.StartPoint = pt;

                        Point pt2 = new Point(_eyeline.EndPoint.X, _eyeline.EndPoint.Y + diff);
                        _eyeline.EndPoint = pt2;
                        RaisePropertyChanged("eyeline");

                        orginal_height = e.GetPosition((IInputElement)e.Source).Y;
                    }
                    else if (((Path)e.Source).Data == lipline)
                    {
                        double diff = e.GetPosition((IInputElement)e.Source).Y - orginal_height;
                        Point pt = new Point(_lipline.StartPoint.X, _lipline.StartPoint.Y + diff);
                        _lipline.StartPoint = pt;

                        Point pt2 = new Point(_lipline.EndPoint.X, _lipline.EndPoint.Y + diff);
                        _lipline.EndPoint = pt2;
                        RaisePropertyChanged("lipline");

                        orginal_height = e.GetPosition((IInputElement)e.Source).Y;
                    }
                }
            }
        }

        private RelayCommand<object> _LeftDown;
        public RelayCommand<object> LeftDown
        {
            get
            {
                if (_LeftDown == null) return _LeftDown = new RelayCommand<object>(param => ExecuteMouseLeftDown((MouseEventArgs)param));
                return _LeftDown;
            }
            set { _LeftDown = value; }
        }

        public bool refclicked = false;
        public bool rotationclicked = false;
        public int rotatedir = 1;

        public int RefernceCount = 0;

        private void ExecuteMouseLeftDown(MouseEventArgs e)
        {
            if (changeText.Equals(flowname.ElementAt(1)) && refclicked == true && e.Source.GetType() == typeof(ImageCanvas))
            {
                origMouseDownPoint = e.GetPosition((IInputElement)((UserControl)e.Source).Parent);
                if (RefernceCount == 0)
                {
                    _GagRefVisiblity = Visibility.Visible;
                    RaisePropertyChanged("GagRefVisiblity");

                    _teethL.Center = origMouseDownPoint;
                    RaisePropertyChanged("teethL");

                    RefernceCount++;
                }
                else if (RefernceCount == 1)
                {
                    _teethR.Center = origMouseDownPoint;
                    RaisePropertyChanged("teethR");
                    RefernceCount++;

                    currentclicked = ((UserControl)(e.Source));
                    currentclicked.Opacity = 0;

                    SetAlign();

                    _GagRefVisiblity = Visibility.Hidden;
                    RaisePropertyChanged("GagRefVisiblity");
                }
                else if (RefernceCount == 2)
                {
                    _FrontalRefVisiblity = Visibility.Visible;
                    RaisePropertyChanged("FrontalRefVisiblity");

                    _FrontalteethL.Center = origMouseDownPoint;
                    RaisePropertyChanged("FrontalteethL");

                    // TransCenter
                    _TransCenter = e.GetPosition((IInputElement)e.Source);
                    RaisePropertyChanged("TransCenter");
                    RefernceCount++;
                }
                else if (RefernceCount == 3)
                {
                    _FrontalteethR.Center = origMouseDownPoint;
                    RaisePropertyChanged("FrontalteethR");
                    RefernceCount++;

                    currentclicked.Opacity = 0.5;

                    // 좌표 옮기기
                    ToothAlign();

                    _FrontalRefVisiblity = Visibility.Hidden;
                    RaisePropertyChanged("FrontalRefVisiblity");

                    _RotateControlVisiblity = Visibility.Visible;
                    RaisePropertyChanged("RotateControlVisiblity");
                }
                return;
            }

            if (e.Source.GetType() == typeof(ImageCanvas))
            {
                captured = true;
                origMouseDownPoint = e.GetPosition((IInputElement)((UserControl)e.Source).Parent);
                orginal_width = e.GetPosition((IInputElement)((UserControl)e.Source).Parent).X;
                orginal_height = e.GetPosition((IInputElement)((UserControl)e.Source).Parent).Y;

                Mouse.Capture((IInputElement)e.Source);
                return;
            }

            if (e.Source.GetType() == typeof(Teeth))
            {
                captured = true;
                origMouseDownPoint = e.GetPosition((IInputElement)e.Source);
                Mouse.Capture((IInputElement)e.Source);
                return;
            }
            if (e.Source.GetType() == typeof(UpperTooth))
            {
                return;
            }
            if (e.Source.GetType() == typeof(LowerTooth))
            {
                return;
            }
            if (e.Source.GetType() == typeof(MouthControl))
            {
                return;
            }
            if (e.Source.GetType() == typeof(RotationControl))
            {
                return;
            }

            if (((Path)e.Source).Data.GetType() == typeof(LineGeometry))
                ((Path)e.Source).Stroke = Brushes.Black;

            Path rewrite = new Path();
            rewrite.Name = ((Path)e.Source).Name;
            rewrite.Data = ((Path)e.Source).Data.CloneCurrentValue();
            undo.Push(rewrite);

            captured = true;
            orginal_width = e.GetPosition((IInputElement)e.Source).X;
            orginal_height = e.GetPosition((IInputElement)e.Source).Y;
            origMouseDownPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _LeftUp;
        public RelayCommand<object> LeftUp
        {
            get
            {
                if (_LeftUp == null) return _LeftUp = new RelayCommand<object>(param => ExecuteMouseLeftUp((MouseEventArgs)param));
                return _LeftUp;
            }
            set { _LeftUp = value; }
        }

        private void ExecuteMouseLeftUp(MouseEventArgs e)
        {
            if (changeText.Equals(flowname.ElementAt(1)) && refclicked == true)
            {
                if (RefernceCount == 4)
                    refclicked = false;
                return;
            }

            if (e.Source.GetType() == typeof(Teeth))
            {
                captured = false;
                Mouse.Capture(null);
                return;
            }
            if (e.Source.GetType() == typeof(ImageCanvas))
            {
                captured = false;
                Mouse.Capture(null);
                return;
            }
            else if (e.Source.GetType() == typeof(Path))
            {
                ((Path)e.Source).Stroke = Brushes.Black;
                captured = false;
                Mouse.Capture(null);
            }
        }

        #region image tuning events
        private RelayCommand<object> _RotateControlUp;
        public RelayCommand<object> RotateControlUp
        {
            get
            {
                if (_RotateControlUp == null) return _RotateControlUp = new RelayCommand<object>(param => ExecuteRotateControlUp((MouseEventArgs)param));
                return _RotateControlUp;
            }
            set { _RotateControlUp = value; }
        }

        private void ExecuteRotateControlUp(MouseEventArgs e)
        {
            if (e.Source.GetType() == typeof(Rectangle))
            {
                rotationclicked = false;
                rotatedir = 1;
                captured = false;
                Mouse.Capture(null);
            }
        }

        private RelayCommand<object> _RotateControlDown;
        public RelayCommand<object> RotateControlDown
        {
            get
            {
                if (_RotateControlDown == null) return _RotateControlDown = new RelayCommand<object>(param => ExecuteRotateControlDown((MouseEventArgs)param));
                return _RotateControlDown;
            }
            set { _RotateControlDown = value; }
        }

        private void ExecuteRotateControlDown(MouseEventArgs e)
        {
            if (e.Source.GetType() == typeof(Rectangle))
            {
                if (((Rectangle)e.Source).Name.Equals("DragIconImage"))
                {
                    captured = true;
                    origMouseDownPoint = e.GetPosition(((IInputElement)e.Source));

                    Mouse.Capture((IInputElement)e.Source);
                    return;
                }
                if (((Rectangle)e.Source).Name.Equals("RightRotateIcon"))
                {
                    captured = true;
                    origMouseDownPoint = e.GetPosition((IInputElement)e.Source);

                    rotatedir = 1;

                    Mouse.Capture((IInputElement)e.Source);
                    return;
                }
                if (((Rectangle)e.Source).Name.Equals("LeftRotateIcon"))
                {
                    captured = true;
                    origMouseDownPoint = e.GetPosition((IInputElement)e.Source);

                    rotatedir = -1;

                    Mouse.Capture((IInputElement)e.Source);
                    return;
                }
            }
        }

        private RelayCommand<object> _RotateControlMove;
        public RelayCommand<object> RotateControlMove
        {
            get
            {
                if (_RotateControlMove == null) return _RotateControlMove = new RelayCommand<object>(param => ExecuteRotateControlMove((MouseEventArgs)param));
                return _RotateControlMove;
            }
            set { _RotateControlMove = value; }
        }

        private void ExecuteRotateControlMove(MouseEventArgs e)
        {
            if (captured == true)
            {
                if (((Rectangle)e.Source).Name.Equals("DragIconImage"))
                {
                    // 중심점 위치 조정
                    Point curMouseDownPoint = e.GetPosition((IInputElement)e.Source);

                    double diffX = (curMouseDownPoint.X - origMouseDownPoint.X);
                    double diffY = (curMouseDownPoint.Y - origMouseDownPoint.Y);

                    Point Center = new Point(_FrontalCenter.X + diffX, _FrontalCenter.Y + diffY);
                    _FrontalCenter = Center;
                    RaisePropertyChanged("FrontalCenter");

                    FrontalteethL.Center = new Point(_FrontalteethL.Center.X + diffX, _FrontalteethL.Center.Y + diffY);
                    FrontalteethR.Center = new Point(_FrontalteethR.Center.X + diffX, _FrontalteethR.Center.Y + diffY);

                    Center = new Point(_RotateControlCenter.X + diffX, _RotateControlCenter.Y + diffY);
                    _RotateControlCenter = Center;
                    RaisePropertyChanged("RotateControlCenter");
                }
                else if (((Rectangle)e.Source).Name.Equals("RightRotateIcon") || ((Rectangle)e.Source).Name.Equals("LeftRotateIcon"))
                {
                    // 각도 조정
                    double rotate = 0.5;
                    if (rotationclicked == false)
                    {
                        rotationclicked = true;
                    }

                    rotate *= rotatedir;

                    _TransfrontalAngle = offset_frontalangle;
                    _TransfrontalAngle += rotate;

                    if (_TransfrontalAngle < 0)
                        _TransfrontalAngle += 360;
                    else
                        _TransfrontalAngle -= 360;

                    _FrontalAngle = _TransfrontalAngle;
                    RaisePropertyChanged("FrontalAngle");

                    offset_frontalangle = _TransfrontalAngle;
                }
            }
        }

        #endregion
        #endregion

        #region Set face ZIndex
        private bool clickedRight = false;
        private UserControl currentclicked;

        private RelayCommand<object> _RightDown;
        public RelayCommand<object> RightDown
        {
            get
            {
                if (_RightDown == null) return _RightDown = new RelayCommand<object>(param => ExecuteMouseRigthDown((MouseEventArgs)param));
                return _RightDown;
            }
            set { _RightDown = value; }
        }

        private void ExecuteMouseRigthDown(MouseEventArgs e)
        {
            if (clickedRight == false)
            {
                if (e.Source.GetType() == typeof(ImageCanvas))
                {
                    currentclicked = ((UserControl)(e.Source));
                    if (!currentclicked.Name.Equals("GagFaceImage"))
                        return;
                    int currentZIndex = Canvas.GetZIndex(currentclicked);
                    Canvas.SetZIndex(currentclicked, currentZIndex - 1);

                    currentclicked.Opacity = 0.5;

                    Mouse.Capture((IInputElement)e.Source);
                }
                clickedRight = true;
            }
            else
            {
                int currentZIndex = Canvas.GetZIndex(currentclicked);
                Canvas.SetZIndex(currentclicked, currentZIndex + 1);
                currentclicked.Opacity = 1;
                clickedRight = false;
            }
        }

        private RelayCommand<object> _RightUp;
        public RelayCommand<object> RightUp
        {
            get
            {
                if (_RightUp == null) return _RightUp = new RelayCommand<object>(param => ExecuteMouseRightUp((MouseEventArgs)param));
                return _RightUp;
            }
            set { _RightUp = value; }
        }

        private void ExecuteMouseRightUp(MouseEventArgs e)
        {
            captured = false;
            Mouse.Capture(null);
        }
        #endregion
        #region redo_undo

        Stack<Path> undo = new Stack<Path>();
        Stack<Path> redo = new Stack<Path>();

        private RelayCommand<object> _RedoCommand;
        public RelayCommand<object> RedoCommand
        {
            get
            {
                if (_RedoCommand == null)
                    return _RedoCommand = new RelayCommand<object>(param => this.do_it());
                return _RedoCommand;
            }
            set
            {
                _RedoCommand = value;
            }
        }

        private RelayCommand<object> _UndoCommand;
        public RelayCommand<object> UndoCommand
        {
            get
            {
                if (_UndoCommand == null)
                    return _UndoCommand = new RelayCommand<object>(param => this.redo_it());
                return _UndoCommand;
            }
            set
            {
                _UndoCommand = value;
            }
        }

        public void do_it()
        {
            if (undo.Count == 0)
            {
                //draw_faceline();
                return;
            }
            Path rewrite = new Path();
            Path redo_path = new Path();
            rewrite = undo.Pop();

            //if (rewrite.Name.Equals("eye_L"))
            //{
            //    redo_path.Name = rewrite.Name;
            //    redo_path.Data = eye_L.CloneCurrentValue();
            //    eye_L.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    eyeline.StartPoint = eye_L.Center;
            //}
            //else if (rewrite.Name.Equals("eye_R"))
            //{
            //    redo_path.Name = rewrite.Name;
            //    redo_path.Data = eye_R.CloneCurrentValue();
            //    eye_R.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    eyeline.EndPoint = eye_R.Center;
            //}
            //else if (rewrite.Name.Equals("mouth_L"))
            //{
            //    redo_path.Name = rewrite.Name;
            //    redo_path.Data = mouth_L.CloneCurrentValue();
            //    mouth_L.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    lipline.StartPoint = mouth_L.Center;
            //}
            //else if (rewrite.Name.Equals("mouth_R"))
            //{
            //    redo_path.Name = rewrite.Name;
            //    redo_path.Data = mouth_R.CloneCurrentValue();
            //    mouth_R.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    lipline.EndPoint = mouth_R.Center;
            //}
            if (rewrite.Name.Equals("midline"))
            {
                redo_path.Name = rewrite.Name;
                redo_path.Data = midline.CloneCurrentValue();
                midline.StartPoint = ((LineGeometry)rewrite.Data).StartPoint;
                midline.EndPoint = ((LineGeometry)rewrite.Data).EndPoint;
            }
            else if (rewrite.Name.Equals("noseline_L"))
            {
                redo_path.Name = rewrite.Name;
                redo_path.Data = noseline_L.CloneCurrentValue();
                noseline_L.StartPoint = ((LineGeometry)rewrite.Data).StartPoint;
                noseline_L.EndPoint = ((LineGeometry)rewrite.Data).EndPoint;
            }
            else if (rewrite.Name.Equals("noseline_R"))
            {
                redo_path.Name = rewrite.Name;
                redo_path.Data = noseline_R.CloneCurrentValue();
                noseline_R.StartPoint = ((LineGeometry)rewrite.Data).StartPoint;
                noseline_R.EndPoint = ((LineGeometry)rewrite.Data).EndPoint;
            }

            redo.Push(redo_path);

        }



        public void redo_it()
        {
            if (redo.Count == 0)
            {
                //draw_faceline();
                return;
            }
            Path rewrite = new Path();
            Path undo_path = new Path();
            rewrite = redo.Pop();
            //if (rewrite.Name.Equals("eye_L"))
            //{
            //    undo_path.Name = rewrite.Name;
            //    undo_path.Data = eye_L.CloneCurrentValue();
            //    eye_L.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    eyeline.StartPoint = eye_L.Center;
            //}
            //else if (rewrite.Name.Equals("eye_R"))
            //{
            //    undo_path.Name = rewrite.Name;
            //    undo_path.Data = eye_R.CloneCurrentValue();
            //    eye_R.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    eyeline.EndPoint = eye_R.Center;
            //}
            //else if (rewrite.Name.Equals("mouth_L"))
            //{
            //    undo_path.Name = rewrite.Name;
            //    undo_path.Data = mouth_L.CloneCurrentValue();
            //    mouth_L.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    lipline.StartPoint = mouth_L.Center;
            //}
            //else if (rewrite.Name.Equals("mouth_R"))
            //{
            //    undo_path.Name = rewrite.Name;
            //    undo_path.Data = mouth_R.CloneCurrentValue();
            //    mouth_R.Center = ((EllipseGeometry)rewrite.Data).Center;
            //    lipline.EndPoint = mouth_R.Center;
            //}

            if (rewrite.Name.Equals("midline"))
            {
                undo_path.Name = rewrite.Name;
                undo_path.Data = midline.CloneCurrentValue();
                midline.StartPoint = ((LineGeometry)rewrite.Data).StartPoint;
                midline.EndPoint = ((LineGeometry)rewrite.Data).EndPoint;
            }
            else if (rewrite.Name.Equals("noseline_L"))
            {
                undo_path.Name = rewrite.Name;
                undo_path.Data = noseline_L.CloneCurrentValue();
                noseline_L.StartPoint = ((LineGeometry)rewrite.Data).StartPoint;
                noseline_L.EndPoint = ((LineGeometry)rewrite.Data).EndPoint;
            }
            else if (rewrite.Name.Equals("noseline_R"))
            {
                undo_path.Name = rewrite.Name;
                undo_path.Data = noseline_R.CloneCurrentValue();
                noseline_R.StartPoint = ((LineGeometry)rewrite.Data).StartPoint;
                noseline_R.EndPoint = ((LineGeometry)rewrite.Data).EndPoint;
            }
            undo.Push(undo_path);

        }
        #endregion

        #region save_program

        private string XML_PATH = @".\save\wat_setup.xml";
        public  SaveInfo MySetup = new SaveInfo();

        public void Set_SaveInfo()
        {
            MySetup.GagFacePoints = this.GagFacePoints;
            MySetup.FrontalFacePoints = this.FrontalFacePoints;

            // 보정 후 점
           // MySetup._FrontalPoints = this.FrontalPoints;
           //MySetup._GagPoints = this.GagPoints;
           // MySetup._FrontalMouthPoints = this.FrontalMouthPoints;

            // face line
            MySetup._midline = this.midline;
            MySetup._noseline_L = this.noseline_L;
            MySetup._noseline_R = this.noseline_R;
            MySetup._eyeline = this.eyeline;
            MySetup._lipline = this.lipline;

            // align reference teeth points
            MySetup._teethL = this.teethL;
            MySetup._teethR = this.teethR;
            MySetup._FrontalteethL = this.FrontalteethL;
            MySetup._FrontalteethR = this.FrontalteethR;

            // scale wheelmouse
            MySetup._ViewScale = this.ViewScale;
            MySetup._WheelMouseCenter = this.WheelMouseCenter;


            // Frontal Face Canvas Center
            MySetup._FrontalCenter = this.FrontalCenter;
            MySetup._FrontalAngle = this.FrontalAngle;
            MySetup._FrontalScale = this.FrontalScale;

            // Gag Image
            MySetup._GagCenter = this.GagCenter;
            MySetup._GagAngle = this.GagAngle;
            MySetup._GagScale = this.GagScale;

            // Transform Center
            MySetup._TransCenter = this.TransCenter;
            MySetup._TransGagCenter = this.TransGagCenter;

            // Rotate Control Center
            MySetup._RotateControlCenter=this.RotateControlCenter;
            }

        public void LoadXML()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(SaveInfo));
                IOText.TextReader textReader = new IOText.StreamReader(XML_PATH);

                MySetup = (SaveInfo)deserializer.Deserialize(textReader);
                if (MySetup == null) MySetup = new SaveInfo();
                textReader.Close();
            }
            catch
            {
                MySetup = new SaveInfo();
            }

            //txbID.Text = MySetup.MyID.ToString();
            //this.textBox1.Text = MySetup.MyString;

        }

        public void SaveXML()
        {
            //MySetup.MyID = Convert.ToInt32(txbID.Text);
            //MySetup.MyString = this.textBox1.Text;

            try
            {
                Set_SaveInfo();
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.NewLineHandling = NewLineHandling.Entitize;

                XmlSerializer serializer = new XmlSerializer(typeof(SaveInfo));
                using (XmlWriter wr = XmlWriter.Create(XML_PATH, ws))
                {
                    serializer.Serialize(wr, MySetup);
                }

            }
            catch (Exception ex)
            {
                Console.Write("write2에러:" + ex.Message);
            }
        }

        #endregion
    }
}
