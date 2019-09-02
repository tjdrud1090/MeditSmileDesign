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

using Process_Page;
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

namespace Process_Page.ViewModel
{
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

    public class SmilePageViewModel : ViewModelBase
    {
        public int check = 0;
        SmileDesign_Page main;

        // page간 중간 정보 객체
        public FaceAlign_Page faceAlignInfo;

        #region constructor
        public SmilePageViewModel()
        {
            faceAlignInfo = ((MainWindow)System.Windows.Application.Current.MainWindow).OldPage as FaceAlign_Page;

            isSizing = false;
            isFirstTimeMovedOnSizingTooth = true;
            firstRotate = Enumerable.Repeat(true, 10).ToList();

            _FaceLineVisiblity = Visibility.Visible;
            RaisePropertyChanged("FaceLineVisiblity");

        }
        #endregion

        #region Page Change command
        private RelayCommand<object> _smile_manual;
        public RelayCommand<object> smile_manual
        {
            get
            {
                if (_smile_manual == null)
                    return _smile_manual = new RelayCommand<object>(param => this.teethManual());
                return _smile_manual;
            }
            set
            {
                _smile_manual = value;
            }
        }
        public void teethManual()
        {
            ExecuteRunDialoga(0);
        }

        private async void ExecuteRunDialoga(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new Manual3
            {
                DataContext = new SmilePageViewModel()
            };

            // show the dialog
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
                Init();
                if (_PrePageClick == null)
                    return _PrePageClick = new RelayCommand<object>(param => this.SetnewPage());
                return _PrePageClick;
            }
            set
            {
                _PrePageClick = value;
            }
        }

        public void SetnewPage()
        {
            System.Windows.Application.Current.MainWindow.UpdateLayout();
            var mainWnd = ((MainWindow)System.Windows.Application.Current.MainWindow) as MainWindow;
            var current = mainWnd.Content as SmileDesign_Page;

            if (mainWnd.OldPage != null)
            {
                System.Windows.Application.Current.MainWindow.Content = mainWnd.OldPage as FaceAlign_Page;
                //mainWnd.OldPage = (FrameworkElement)current;
                mainWnd.OldPage = null;
                return;
            }

            mainWnd.OldPage = (FrameworkElement)(System.Windows.Application.Current.MainWindow.Content);
            FaceAlign_Page page = new FaceAlign_Page();
            System.Windows.Application.Current.MainWindow.Content = page;
        }

        private RelayCommand<object> _Magnifybutton;
        public RelayCommand<object> Magnifybutton
        {
            get
            {
                if (_Magnifybutton == null)
                    return _Magnifybutton = new RelayCommand<object>(param => this.Magnifyclick());
                return _Magnifybutton;
            }
            set
            {
                _Magnifybutton = value;
            }
        }

        public void Magnifyclick()
        {
            SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
            if (check == 0)
            {
                currentPage.MagnifiyingGlass.Visibility = Visibility.Visible;
                check = 1;
            }
            else if (check == 1)
            {
                currentPage.MagnifiyingGlass.Visibility = Visibility.Hidden;
                check = 0;
            }
        }

        #endregion

        #region Visibility Set
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
        #endregion

        #region face_landmark line draw property
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

        //face line
        LineGeometry _midline = new LineGeometry();
        LineGeometry _noseline_L = new LineGeometry();
        LineGeometry _noseline_R = new LineGeometry();
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

        public LineGeometry lipline
        {
            get { return _lipline; }
            set { }
        }
        #endregion

        #region Init image layer
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

        private void Init()
        {
            FrontalFaceImage = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalFaceImage.CloneCurrentValue();
            GagFaceImage = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).GagFaceImage.CloneCurrentValue();
            RaisePropertyChanged("FrontalFaceSource");
            RaisePropertyChanged("GagFaceSource");

            _TransCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).TransCenter;
            RaisePropertyChanged("TransCenter");

            _TransGagCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).TransGagCenter;
            RaisePropertyChanged("TransGagCenter");

            draw_faceline();
            SetAlign();

            _ViewScale = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).ViewScale;
            RaisePropertyChanged("ViewScale");

            // Upper Tooth 위치 선정
            _ToothUpperCenter.X = midline.StartPoint.X;
            _ToothUpperCenter.Y = lipline.StartPoint.Y;
            RaisePropertyChanged("ToothUpperCenter");

            // Lower Tooth 위치 선정
            _ToothLowerCenter.X = midline.StartPoint.X;
            _ToothLowerCenter.Y = lipline.StartPoint.Y + 10;
            RaisePropertyChanged("ToothLowerCenter");
        }

        public void draw_faceline()
        {
            //set picture center
            _GagPoints = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).GagPoints;

            _noseline_L = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).noseline_L.CloneCurrentValue();
            _noseline_R = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).noseline_R.CloneCurrentValue();

            _midline = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).midline.CloneCurrentValue();

            _GagCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).GagCenter;
            _GagAngle = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).GagAngle;

            RaisePropertyChanged("GagPoints");
            RaisePropertyChanged("GagAngle");
            RaisePropertyChanged("GagCenter");
            RaisePropertyChanged("midline");
            RaisePropertyChanged("noseline_L");
            RaisePropertyChanged("noseline_R");
        }

        public void SetAlign()
        {
            _FrontalPoints = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalPoints;

            _FrontalScale = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalScale;

            _FrontalAngle = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalAngle;

            _FrontalCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalCenter;

            RaisePropertyChanged("FrontalPoints");
            RaisePropertyChanged("FrontalCenter");
            RaisePropertyChanged("FrontalScale");
            RaisePropertyChanged("FrontalAngle");

            _lipline = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).lipline.CloneCurrentValue();
            RaisePropertyChanged("lipline");

            // mouse wheel center set
            _WheelMouseCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).WheelMouseCenter;
            RaisePropertyChanged("WheelMouseCenter");

            _FrontalMouthPoints = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalMouthPoints;
            RaisePropertyChanged("FrontalMouthPoints");
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

        // Upper Tooth Control
        private Point _ToothUpperCenter;
        public Point ToothUpperCenter
        {
            get { return _ToothUpperCenter; }
            set { }
        }

        // Lower Tooth Control
        private Point _ToothLowerCenter;
        public Point ToothLowerCenter
        {
            get { return _ToothLowerCenter; }
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
        #endregion

        #region sizeChange MouseWheel

        //Mouse Wheel size changed
        private readonly double ScaleFactor = 0.5;
        private readonly double _dotSize = 1;//닷 사이즈와 wrapping rect의 strokeThickness
        private readonly double _dotTranslation = -0.5;
        private readonly double _emphaDotSize = 2;
        private readonly double _emphaDotTranslation = -1.0;
        //점 및 강조점 보정은 점 및 강조점의 크기의 -1/2배로 해주면 됩니당ㅇ.ㅇ!!
        private readonly double _drawTeethThickness = 0.5;
        private readonly double _resizeBorderThickness = 1;
        private readonly double _alignLineThickness = 2.5;
        private readonly double _pinHeadSize = 2.5;

        // mouse wheel sizechanged Center point
        private Point _WheelMouseCenter;
        public Point WheelMouseCenter
        {
            get { return _WheelMouseCenter; }
            set { }
        }

        public double alignLineThickness
        {
            get
            {
                return ScaleFactor * _alignLineThickness / ViewScale;
            }
        }

        public double pinHeadSize
        {
            get
            {
                return ScaleFactor * _pinHeadSize / ViewScale;
            }
        }

        public double drawTeethThickness
        {
            get
            {
                return ScaleFactor * _drawTeethThickness / ViewScale;
            }
        }

        public double resizeBorderThickness
        {
            get
            {
                return ScaleFactor * _resizeBorderThickness / ViewScale;
            }
        }

        public double emphaDotSize
        {
            get
            {
                return ScaleFactor * _emphaDotSize / ViewScale;
            }
        }

        public double emphaDotTranslation
        {
            get
            {
                return ScaleFactor * _emphaDotTranslation / ViewScale;
            }
        }

        public double dotSize
        {
            get
            {
                return ScaleFactor * _dotSize / ViewScale;
            }
        }

        public double dotTranslation
        {
            get
            {
                return ScaleFactor * _dotTranslation / ViewScale;
            }
        }

        private double _ViewScale = 1;
        public double ViewScale
        {
            get { return _ViewScale; }
            set
            {
                _ViewScale = value;
                RaisePropertyChanged("ViewScale");
                RaisePropertyChanged("dotSize");
                RaisePropertyChanged("dotTranslation");
                RaisePropertyChanged("emphaDotSize");
                RaisePropertyChanged("emphaDotTranslation");
                RaisePropertyChanged("teethLineThickness");
                RaisePropertyChanged("pinHeadSize");
                RaisePropertyChanged("alignLineThickness");

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
            SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
            currentPage.MagnifiyingGlass.ZoomFactor = 0.5;
            if (e.Delta > 0)
            {
                if (_ViewScale < 8)
                {
                    _ViewScale += 0.25;
                    RaisePropertyChanged("ViewScale");
                    RaisePropertyChanged("dotSize");
                    RaisePropertyChanged("dotTranslation");
                    RaisePropertyChanged("emphaDotSize");
                    RaisePropertyChanged("emphaDotTranslation");
                    RaisePropertyChanged("teethLineThickness");
                    RaisePropertyChanged("pinHeadSize");
                    RaisePropertyChanged("alignLineThickness");
                    currentPage.MagnifiyingGlass.ZoomFactor = 0.5;
                }
            }
            else
            {
                if (_ViewScale > 1)
                {
                    _ViewScale -= 0.25;
                    RaisePropertyChanged("ViewScale");
                    RaisePropertyChanged("dotSize");
                    RaisePropertyChanged("dotTranslation");
                    RaisePropertyChanged("emphaDotSize");
                    RaisePropertyChanged("emphaDotTranslation");
                    RaisePropertyChanged("teethLineThickness");
                    RaisePropertyChanged("pinHeadSize");
                    RaisePropertyChanged("alignLineThickness");
                    currentPage.MagnifiyingGlass.ZoomFactor = 0.5;
                }
            }
        }

        #endregion

        #region MouseEvent

        private bool captured = false;
        private double orginal_width;
        private double orginal_height;
        private Point origMouseDownPoint;

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
                if ((e.Source).GetType() == typeof(Teeth))
                {
                    Teeth Test = (Teeth)e.Source;
                    Point curMouseDownPoint = e.GetPosition((IInputElement)e.Source);
                    var dragDelta = curMouseDownPoint - origMouseDownPoint;
                    origMouseDownPoint = curMouseDownPoint;

                    foreach (PointViewModel point in Test.Points)
                    {
                        point.X += (float)dragDelta.X;
                        point.Y += (float)dragDelta.Y;
                    }
                }
                else if (e.Source.GetType() == typeof(ImageCanvas))
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

                    lipline.StartPoint = new Point(_lipline.StartPoint.X, _lipline.StartPoint.Y + diffY);
                    lipline.EndPoint = new Point(_lipline.EndPoint.X, _lipline.EndPoint.Y + diffY);

                    Point Center = new Point(_FrontalCenter.X + diffX, _FrontalCenter.Y + diffY);
                    _FrontalCenter = Center;
                    RaisePropertyChanged("FrontalCenter");

                    Center = new Point(_GagCenter.X + diffX, _GagCenter.Y + diffY);
                    _GagCenter = Center;
                    RaisePropertyChanged("GagCenter");

                    Center = new Point(_ToothUpperCenter.X + diffX, _ToothUpperCenter.Y + diffY);
                    _ToothUpperCenter = Center;
                    RaisePropertyChanged("ToothUpperCenter");

                    Center = new Point(_ToothLowerCenter.X + diffX, _ToothLowerCenter.Y + diffY);
                    _ToothLowerCenter = Center;
                    RaisePropertyChanged("ToothLowerCenter");

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

                        Point Center = new Point(_ToothUpperCenter.X + diff, _ToothUpperCenter.Y);
                        _ToothUpperCenter = Center;
                        RaisePropertyChanged("ToothUpperCenter");

                        Center = new Point(_ToothLowerCenter.X + diff, _ToothLowerCenter.Y);
                        _ToothLowerCenter = Center;
                        RaisePropertyChanged("ToothLowerCenter");

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

        private void ExecuteMouseLeftDown(MouseEventArgs e)
        {
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
            Path rewrite = new Path();
            rewrite.Name = ((Path)e.Source).Name;
            rewrite.Data = ((Path)e.Source).Data.CloneCurrentValue();
            undo.Push(rewrite);

            captured = true;
            orginal_width = e.GetPosition((IInputElement)e.Source).X;
            orginal_height = e.GetPosition((IInputElement)e.Source).Y;
            origMouseDownPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);

            if (((Path)e.Source).Data.GetType() == typeof(EllipseGeometry))
                ((Path)e.Source).Stroke = Brushes.OrangeRed;
            else if (((Path)e.Source).Data.GetType() == typeof(LineGeometry))
                ((Path)e.Source).Stroke = Brushes.Violet;
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
                if (((Path)e.Source).Data.GetType() == typeof(EllipseGeometry))
                {
                    captured = false;
                    Mouse.Capture(null);
                    return;
                }
                captured = false;
                Mouse.Capture(null);
            }
        }
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
                    Canvas cap_Canvas = ((UserControl)(e.Source)).Parent as Canvas;

                    // 판넬의 위치를 수정
                    int MaxZIndex = 0;
                    foreach (var find in cap_Canvas.Children)
                    {
                        if (find.GetType() == typeof(ImageCanvas))
                        {
                            if (((UserControl)find).Name.Equals("FrontalFaceImage"))
                                currentclicked = find as UserControl;
                        }
                        MaxZIndex++;
                    }

                    Canvas.SetZIndex(currentclicked, MaxZIndex);
                    Mouse.Capture((IInputElement)e.Source);
                }
                clickedRight = true;
            }
            else
            {
                if (currentclicked != null)
                {
                    int currentZIndex = Canvas.GetZIndex(currentclicked);
                    Canvas.SetZIndex(currentclicked, 0);
                    clickedRight = false;
                }
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

        #region save
        private RelayCommand<object> _Savecommand;
        public RelayCommand<object> Savecommand
        {
            get
            {
                if (_Savecommand == null)
                    return _Savecommand = new RelayCommand<object>(param => this.Savejonopen());
                return _Savecommand;
            }
            set
            {
                _Savecommand = value;
            }
        }

        private void Savejonopen()
        {
            ExecuteRunDialog(0);
        }

        private async void ExecuteRunDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SampleSaveDialog
            {
                DataContext = new SampleSaveDialogViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        #endregion

        #region Events for Tooth Control

        List<Teeth> SelectedList = new List<Teeth>();
        private bool inTeeth = false;

        #region DragDrop

        private bool leftdown = false;
        private bool leftdown_with_ctrl = false;
        private bool dragging = false;
        private Teeth own = null;

        #region DragDrop for Teeth 

        private Point originalPoint;
        private List<Teeth> dragged = new List<Teeth>();

        private RelayCommand<object> _mouseLeftDownForDragAndDropTeeth;
        public RelayCommand<object> MouseLeftDownForDragAndDropTeeth
        {
            get
            {
                if (_mouseLeftDownForDragAndDropTeeth == null)
                    return _mouseLeftDownForDragAndDropTeeth = new RelayCommand<object>(param => ExecuteMouseLeftDownForDragAndDropTeeth((MouseEventArgs)param));
                return _mouseLeftDownForDragAndDropTeeth;
            }
            set { _mouseLeftDownForDragAndDropTeeth = value; }
        }
        public void ExecuteMouseLeftDownForDragAndDropTeeth(MouseEventArgs e)
        {
            inTeeth = true;
            Rectangle rect = e.Source as Rectangle;
            Border border = ViewUtils.FindParent(rect, (new Border()).GetType()) as Border;

            Teeth th = ViewUtils.FindParent(rect, (new Teeth()).GetType()) as Teeth;
            RotateTeeth rotate = th.FindName("rotateTeeth") as RotateTeeth;
            DrawTeeth draw = th.FindName("drawTeeth") as DrawTeeth;

            leftdown = true;
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
                leftdown_with_ctrl = true;
            else
            {
                leftdown_with_ctrl = false;
                if (SelectedList.Count == 0)
                {
                    own = th;
                    SelectedList.Add(th);

                    border.Opacity = 1;
                    th.list.Visibility = Visibility.Visible;
                    rotate.RotatePin.Visibility = Visibility.Visible;
                    draw.path.Stroke = draw.FindResource("Selected_StrokeBrush") as Brush;
                    draw.path.Fill = draw.FindResource("FillBrush") as Brush;
                }
                else if (SelectedList.Contains(th))
                {
                    own = th;
                    //draw.path.Fill = null;
                    //rotate.RotatePin.Visibility = Visibility.Hidden;
                }
                else
                {
                    foreach (Teeth del in SelectedList)
                    {
                        RotateTeeth rotate_del = del.FindName("rotateTeeth") as RotateTeeth;
                        DrawTeeth draw_del = del.FindName("drawTeeth") as DrawTeeth;
                        WrapTeeth wrap_del = del.FindName("wrapTeeth") as WrapTeeth;

                        Border border_del = wrap_del.FindName("Border_WrapTeeth") as Border;
                        Rectangle rect_del = wrap_del.FindName("Rectangle_WrapTeeth") as Rectangle;

                        border_del.Opacity = 0;
                        draw_del.path.Stroke = draw_del.FindResource("NonSelected_StrokeBrush") as Brush;
                        draw_del.path.Fill = null;
                        rotate_del.RotatePin.Visibility = Visibility.Hidden;
                        del.list.Visibility = Visibility.Hidden;
                    }
                    SelectedList.Clear();

                    own = th;
                    SelectedList.Add(th);
                    th.list.Visibility = Visibility.Visible;
                    border.Opacity = 1;
                    draw.path.Fill = draw.FindResource("FillBrush") as Brush;
                    draw.path.Stroke = draw.FindResource("Selected_StrokeBrush") as Brush;
                    rotate.RotatePin.Visibility = Visibility.Visible;

                }

            }

            Mouse.Capture((IInputElement)e.Source);
            originalPoint = e.GetPosition((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForDragAndDropTeeth;
        public RelayCommand<object> MouseMoveForDragAndDropTeeth
        {
            get
            {
                if (_mouseMoveForDragAndDropTeeth == null)
                    return _mouseMoveForDragAndDropTeeth = new RelayCommand<object>(param => ExecuteMouseMoveForDragAndDropTeeth((MouseEventArgs)param));
                return _mouseMoveForDragAndDropTeeth;
            }
            set { _mouseMoveForDragAndDropTeeth = value; }
        }
        public void ExecuteMouseMoveForDragAndDropTeeth(MouseEventArgs e)
        {
            main = Application.Current.MainWindow.Content as SmileDesign_Page;
            if (dragging)
            {
                if (leftdown == true)
                {
                    Rectangle rect = e.Source as Rectangle;
                    Teeth me_rect = ViewUtils.FindParent(rect, (new Teeth()).GetType()) as Teeth;
                    if (SelectedList.Contains(me_rect))
                    {
                        foreach (Teeth me in SelectedList)
                        {
                            Point curPoint = e.GetPosition((IInputElement)e.Source);
                            var dragDelta = curPoint - originalPoint;
                            // Mirror Mode
                            if (main.ToothControl.mirror.IsChecked == true)
                            {
                                if (dragged.Contains(me) == false)
                                {
                                    int idx_me = main.ToothControl.dic[me.Name];
                                    int idx_own = main.ToothControl.dic[own.Name];

                                    // me & own : different side
                                    if ((idx_me < 3 && idx_own >= 3) || (idx_me >= 3 && idx_own < 3))
                                    {
                                        foreach (PointViewModel point in me.Points)
                                        {
                                            point.X -= dragDelta.X;
                                            point.Y += dragDelta.Y;
                                        }
                                    }
                                    // me & own : same side
                                    else
                                    {
                                        foreach (PointViewModel point in me.Points)
                                        {
                                            point.X += dragDelta.X;
                                            point.Y += dragDelta.Y;
                                        }
                                    }
                                    dragged.Add(me);

                                    // Mirror
                                    Teeth you = ViewUtils.FindSymmetric(me, main.ToothControl.dic);
                                    if (dragged.Contains(you) == false)
                                    {
                                        // you & own : same side
                                        if ((idx_me < 3 && idx_own >= 3) || (idx_me >= 3 && idx_own < 3))
                                        {
                                            foreach (PointViewModel point in you.Points)
                                            {
                                                point.X += dragDelta.X;
                                                point.Y += dragDelta.Y;
                                            }
                                        }
                                        // you & own : different side
                                        else
                                        {
                                            foreach (PointViewModel point in you.Points)
                                            {
                                                point.X -= dragDelta.X;
                                                point.Y += dragDelta.Y;
                                            }
                                        }
                                    }
                                    dragged.Add(you);
                                }
                            }
                            // No Mirror
                            else
                            {
                                foreach (PointViewModel point in me.Points)
                                {
                                    point.X += dragDelta.X;
                                    point.Y += dragDelta.Y;
                                }
                            }
                        }
                    }
                }
            }
            else if (leftdown)
                dragging = true;
            dragged.Clear();
        }

        private RelayCommand<object> _mouseLeftUpForDragAndDropTeeth;
        public RelayCommand<object> MouseLeftUpForDragAndDropTeeth
        {
            get
            {
                if (_mouseLeftUpForDragAndDropTeeth == null)
                    return _mouseLeftUpForDragAndDropTeeth = new RelayCommand<object>(param => ExecuteMouseLeftUpForDragAndDropTeeth((MouseEventArgs)param));
                return _mouseLeftUpForDragAndDropTeeth;
            }
            set { _mouseLeftUpForDragAndDropTeeth = value; }
        }
        public void ExecuteMouseLeftUpForDragAndDropTeeth(MouseEventArgs e)
        {
            int flag = 0;
            if (leftdown)
            {
                Rectangle rect_dragdrop = e.Source as Rectangle;
                Border border_dragdrop = ViewUtils.FindParent(rect_dragdrop, (new Border()).GetType()) as Border;

                Teeth th = ViewUtils.FindParent(rect_dragdrop, Type.GetType("Process_Page.ToothTemplate.Teeth")) as Teeth;
                Canvas cv = th.FindName("Canvas_Teeth") as Canvas;
                DrawTeeth draw = cv.FindName("drawTeeth") as DrawTeeth;
                RotateTeeth rotate = cv.FindName("rotateTeeth") as RotateTeeth;
                if (!dragging)
                {
                    if (leftdown_with_ctrl)
                    {
                        if (SelectedList.Contains(th))
                        {
                            flag = 1;
                            th.list.Visibility = Visibility.Hidden;
                            draw.path.Fill = null;
                            draw.path.Stroke = draw.FindResource("NonSelected_StrokeBrush") as Brush;
                            border_dragdrop.Opacity = 0;
                            rotate.RotatePin.Visibility = Visibility.Hidden;
                        }

                        else
                        {
                            SelectedList.Add(th);
                            th.list.Visibility = Visibility.Visible;
                            draw.path.Fill = draw.FindResource("FillBrush") as Brush;
                            draw.path.Stroke = draw.FindResource("Selected_StrokeBrush") as Brush;
                            border_dragdrop.Opacity = 1;
                            rotate.RotatePin.Visibility = Visibility.Visible;
                        }

                        if (flag == 1)
                        {
                            flag = 0;
                            SelectedList.Remove(th);
                        }
                    }
                    else
                    {
                        if (SelectedList.Count == 1 && SelectedList[0] == th)
                        {
                            //rect_dragdrop.Visibility = Visibility.Hidden;
                            //border_dragdrop.Visibility = Visibility.Hidden;

                            //draw.path.Fill = null;
                            //rotate.RotatePin.Visibility = Visibility.Hidden;
                            //th.list.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            foreach (Teeth th_del in SelectedList)
                            {
                                RotateTeeth rotate_del = th_del.FindName("rotateTeeth") as RotateTeeth;
                                DrawTeeth draw_del = th_del.FindName("drawTeeth") as DrawTeeth;
                                WrapTeeth wrap_del = th_del.FindName("wrapTeeth") as WrapTeeth;

                                Border border_del = wrap_del.FindName("Border_WrapTeeth") as Border;
                                Rectangle rect_del = wrap_del.FindName("Rectangle_WrapTeeth") as Rectangle;

                                th_del.list.Visibility = Visibility.Hidden;
                                draw_del.path.Fill = null;
                                draw_del.path.Stroke = draw_del.FindResource("NonSelected_StrokeBrush") as Brush;
                                border_del.Opacity = 0;
                                rotate_del.RotatePin.Visibility = Visibility.Hidden;
                            }

                            SelectedList.Clear();
                            SelectedList.Add(th);

                            th.list.Visibility = Visibility.Visible;
                            border_dragdrop.Opacity = 1;
                            draw.path.Fill = draw.FindResource("FillBrush") as Brush;
                            draw.path.Stroke = draw.FindResource("Selected_StrokeBrush") as Brush;
                            rotate.RotatePin.Visibility = Visibility.Visible;
                        }
                    }
                }

                Mouse.Capture(null);
                leftdown = false;
                leftdown_with_ctrl = false;
                // e.Handled = true;
            }

            dragging = false;
            inTeeth = false;
        }

        #endregion

        #region DragDrop for Tooth

        //private bool captured;            // teeth와 공유
        private Brush orgBrush2;

        private RelayCommand<object> _mouseLeftDownForDragAndDropTooth;
        public RelayCommand<object> MouseLeftDownForDragAndDropTooth
        {
            get
            {
                if (_mouseLeftDownForDragAndDropTooth == null)
                    return _mouseLeftDownForDragAndDropTooth = new RelayCommand<object>(param => ExecuteMouseLeftDownForDragAndDropTooth((MouseEventArgs)param));
                return _mouseLeftDownForDragAndDropTooth;
            }
            set { _mouseLeftDownForDragAndDropTooth = value; }
        }
        public void ExecuteMouseLeftDownForDragAndDropTooth(MouseEventArgs e)
        {
            //ArrowLine me = e.Source as ArrowLine;
            //Ellipse me = e.Source as Ellipse;
            Image me = e.Source as Image;
            Canvas cv = me.Parent as Canvas;
            WrapTooth wrap = cv.FindName("WrapToothInTooth") as WrapTooth;
            Border me_border = wrap.FindName("Border_WrapTooth") as Border;
            foreach (Teeth del in SelectedList)
            {
                RotateTeeth rotate_del = del.FindName("rotateTeeth") as RotateTeeth;
                DrawTeeth draw_del = del.FindName("drawTeeth") as DrawTeeth;
                WrapTeeth wrap_del = del.FindName("wrapTeeth") as WrapTeeth;

                Border border_del = wrap_del.FindName("Border_WrapTeeth") as Border;
                Rectangle rect_del = wrap_del.FindName("Rectangle_WrapTeeth") as Rectangle;

                border_del.Opacity = 0;
                draw_del.path.Stroke = draw_del.FindResource("NonSelected_StrokeBrush") as Brush;
                draw_del.path.Fill = null;
                rotate_del.RotatePin.Visibility = Visibility.Hidden;
                del.list.Visibility = Visibility.Hidden;
            }

            SelectedList.Clear();
            originalPoint = e.GetPosition((IInputElement)e.Source);


            orgBrush2 = me_border.BorderBrush;
            me_border.BorderBrush = Brushes.Red;

            leftdown = true;
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForDragAndDropTooth;
        public RelayCommand<object> MouseMoveForDragAndDropTooth
        {
            get
            {
                if (_mouseMoveForDragAndDropTooth == null)
                    return _mouseMoveForDragAndDropTooth = new RelayCommand<object>(param => ExecuteMouseMoveForDragAndDropTooth((MouseEventArgs)param));
                return _mouseMoveForDragAndDropTooth;
            }
            set { _mouseMoveForDragAndDropTooth = value; }
        }
        public void ExecuteMouseMoveForDragAndDropTooth(MouseEventArgs e)
        {
            //ArrowLine me = e.Source as ArrowLine;
            //Ellipse me = e.Source as Ellipse;
            Image me = e.Source as Image;
            Canvas cv = me.Parent as Canvas;
            WrapTooth wrap = cv.FindName("WrapToothInTooth") as WrapTooth;

            if (leftdown == true)
            {
                Point curMouseDownPoint = e.GetPosition((IInputElement)e.Source);
                var dragDelta = curMouseDownPoint - originalPoint;
                foreach (TeethType points in wrap.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        point.X += dragDelta.X;
                        point.Y += dragDelta.Y;
                    }
                }
            }
        }

        private RelayCommand<object> _mouseLeftUpForDragAndDropTooth;
        public RelayCommand<object> MouseLeftUpForDragAndDropTooth
        {
            get
            {
                if (_mouseLeftUpForDragAndDropTooth == null)
                    return _mouseLeftUpForDragAndDropTooth = new RelayCommand<object>(param => ExecuteMouseLeftUpForDragAndDropTooth((MouseEventArgs)param));
                return _mouseLeftUpForDragAndDropTooth;
            }
            set { _mouseLeftUpForDragAndDropTooth = value; }
        }
        public void ExecuteMouseLeftUpForDragAndDropTooth(MouseEventArgs e)
        {
            //ArrowLine me = e.Source as ArrowLine;
            //Ellipse me = e.Source as Ellipse;
            Image me = e.Source as Image;
            Canvas cv = me.Parent as Canvas;
            WrapTooth wrap = cv.FindName("WrapToothInTooth") as WrapTooth;
            Border me_border = wrap.FindName("Border_WrapTooth") as Border;
            me_border.BorderBrush = orgBrush2;

            leftdown = false;
            Mouse.Capture(null);
        }

        #endregion

        #endregion

        #region Resize 

        private List<bool> isFirstTimeMovedOnSizing = new List<bool>();
        private List<bool> isFirstTimeMovedOnSizing_you = new List<bool>();
        private bool isSizing = false;
        private readonly double sizeThreshold = 10;

        private Point[] anchorMin = new Point[10];
        private Point[] anchorMax = new Point[10];
        private Point[] anchorMin_you = new Point[10];
        private Point[] anchorMax_you = new Point[10];

        #region Resize for Teeth

        // For Teeth
        private RelayCommand<object> _mouseLeftDownForResizeTeeth;
        public RelayCommand<object> MouseLeftDownForResizeTeeth
        {
            get
            {
                if (_mouseLeftDownForResizeTeeth == null)
                    return _mouseLeftDownForResizeTeeth = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTeeth((MouseEventArgs)param));
                return _mouseLeftDownForResizeTeeth;
            }
            set { _mouseLeftDownForResizeTeeth = value; }
        }

        public void ExecuteMouseLeftDownForResizeTeeth(MouseEventArgs e)
        {
            if (isSizing)
                return;
            isSizing = true;
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForResizeTeeth;
        public RelayCommand<object> MouseMoveForResizeTeeth
        {
            get
            {
                if (_mouseMoveForResizeTeeth == null)
                    return _mouseMoveForResizeTeeth = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTeeth((MouseEventArgs)param));
                return _mouseMoveForResizeTeeth;
            }
            set { _mouseMoveForResizeTeeth = value; }
        }

        public void ExecuteMouseMoveForResizeTeeth(MouseEventArgs e)
        {
            if (!isSizing)
                return;
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point moved = e.GetPosition(e.Source as IInputElement);
            Border border = e.Source as Border;

            int i = 0, j = 0;
            if (border.Name.Equals("Border_Top"))
            {
                // Sibling
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_Height = maxPoint.Y - minPoint.Y;
                    double changedHeight = ori_Height - moved.Y;
                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = changedHeight / ori_Height;
                            point.Y = point.Y * RatioY + anchorMax[i].Y * (1 - RatioY);
                        }
                    }
                    i++;

                    // Mirror Mode
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_Height_you = maxPoint_you.Y - minPoint_you.Y;
                        double changedHeight_you = ori_Height_you - moved.Y;
                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedHeight_you > sizeThreshold)
                            {
                                double RatioY = changedHeight_you / ori_Height_you;
                                point.Y = point.Y * RatioY + anchorMax_you[j].Y * (1 - RatioY);
                            }
                        }
                    }
                    j++;
                }
            }
            else if (border.Name.Equals("Border_Bottom"))
            {
                // Sibling
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_height = maxPoint.Y - minPoint.Y;
                    double changedHeight = ori_height + moved.Y;
                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = changedHeight / ori_height;
                            point.Y = point.Y * RatioY + anchorMin[i].Y * (1 - RatioY);
                        }
                    }
                    i++;

                    // Mirror
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_height_you = maxPoint_you.Y - minPoint_you.Y;
                        double changedHeight_you = ori_height_you + moved.Y;
                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedHeight_you > sizeThreshold)
                            {
                                double RatioY = changedHeight_you / ori_height_you;
                                point.Y = point.Y * RatioY + anchorMin_you[j].Y * (1 - RatioY);
                            }
                        }
                    }
                    j++;
                }

            }
            else if (border.Name.Equals("Border_Left"))
            {
                // Sibling
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_width = maxPoint.X - minPoint.X;
                    double changedWidth = ori_width - moved.X;
                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = changedWidth / ori_width;
                            point.X = point.X * RatioX + anchorMax[i].X * (1 - RatioX);
                        }
                    }
                    i++;

                    // Mirror
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_width_you = maxPoint_you.X - minPoint_you.X;
                        double changedWidth_you = ori_width_you - moved.X;
                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedWidth_you > sizeThreshold)
                            {
                                double RatioX = changedWidth_you / ori_width_you;
                                point.X = point.X * RatioX + anchorMin_you[j].X * (1 - RatioX);
                            }
                        }
                    }
                    j++;
                }

            }
            else if (border.Name.Equals("Border_Right"))
            {
                // Sibling
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_width = maxPoint.X - minPoint.X;
                    double changedWidth = ori_width + moved.X;
                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = changedWidth / ori_width;
                            point.X = point.X * RatioX + anchorMin[i].X * (1 - RatioX);
                        }
                    }
                    i++;

                    // Mirror
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_width_you = maxPoint_you.X - minPoint_you.X;
                        double changedWidth_you = ori_width_you + moved.X;
                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedWidth_you > sizeThreshold)
                            {
                                double RatioX = changedWidth_you / ori_width_you;
                                point.X = point.X * RatioX + anchorMax_you[j].X * (1 - RatioX);
                            }
                        }
                    }
                    j++;
                }

            }
            else if (border.Name.Equals("Border_TopLeft"))
            {
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_width = maxPoint.X - minPoint.X;
                    double changedWidth = ori_width - moved.X;

                    double ori_height = maxPoint.Y - minPoint.Y;
                    double changedHeight = ori_height - moved.Y;

                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = changedWidth / ori_width;
                            point.X = point.X * RatioX + anchorMax[i].X * (1 - RatioX);
                        }

                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = changedHeight / ori_height;
                            point.Y = point.Y * RatioY + anchorMax[i].Y * (1 - RatioY);
                        }
                    }
                    i++;

                    // Mirror
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_width_you = maxPoint_you.X - minPoint_you.X;
                        double changedWidth_you = ori_width_you - moved.X;

                        double ori_height_you = maxPoint_you.Y - minPoint_you.Y;
                        double changedHeight_you = ori_height_you - moved.Y;

                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedWidth_you > sizeThreshold)
                            {
                                double RatioX = changedWidth_you / ori_width_you;
                                point.X = point.X * RatioX + anchorMin_you[j].X * (1 - RatioX);
                            }

                            if (changedHeight_you > sizeThreshold)
                            {
                                double RatioY = changedHeight_you / ori_height_you;
                                point.Y = point.Y * RatioY + anchorMax_you[j].Y * (1 - RatioY);
                            }
                        }
                        j++;
                    }
                }
            }
            else if (border.Name.Equals("Border_TopRight"))
            {
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_width = maxPoint.X - minPoint.X;         // right
                    double changedWidth = ori_width + moved.X;

                    double ori_height = maxPoint.Y - minPoint.Y;        // top
                    double changedHeight = ori_height - moved.Y;

                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedWidth > sizeThreshold)               // right
                        {
                            double RatioX = changedWidth / ori_width;
                            point.X = point.X * RatioX + anchorMin[i].X * (1 - RatioX);
                        }

                        if (changedHeight > sizeThreshold)              // top
                        {
                            double RatioY = changedHeight / ori_height;
                            point.Y = point.Y * RatioY + anchorMax[i].Y * (1 - RatioY);
                        }
                    }

                    // Mirror
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            Console.WriteLine($"you: {you.Name}, j: {j}");
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_width_you = maxPoint_you.X - minPoint_you.X;         // right
                        double changedWidth_you = ori_width_you + moved.X;

                        double ori_height_you = maxPoint_you.Y - minPoint_you.Y;        // top
                        double changedHeight_you = ori_height_you - moved.Y;

                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedWidth_you > sizeThreshold)                   // right
                            {
                                double RatioX = changedWidth_you / ori_width_you;
                                point.X = point.X * RatioX + anchorMax_you[j].X * (1 - RatioX);
                            }

                            if (changedHeight_you > sizeThreshold)                  // top
                            {
                                double RatioY = changedHeight_you / ori_height_you;
                                point.Y = point.Y * RatioY + anchorMax_you[j].Y * (1 - RatioY);
                            }
                        }
                    }

                    i++; j++;
                }

            }
            else if (border.Name.Equals("Border_BottomLeft"))
            {
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_width = maxPoint.X - minPoint.X;             // left
                    double changedWidth = ori_width - moved.X;

                    double ori_height = maxPoint.Y - minPoint.Y;            // bottom
                    double changedHeight = ori_height + moved.Y;

                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedWidth > sizeThreshold)                   // left
                        {
                            double RatioX = changedWidth / ori_width;
                            point.X = point.X * RatioX + anchorMax[i].X * (1 - RatioX);
                        }

                        if (changedHeight > sizeThreshold)                  // bottom
                        {
                            double RatioY = changedHeight / ori_height;
                            point.Y = point.Y * RatioY + anchorMin[i].Y * (1 - RatioY);
                        }
                    }
                    i++;

                    // Mirror Mode
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_width_you = maxPoint_you.X - minPoint_you.X;             // left
                        double changedWidth_you = ori_width_you - moved.X;

                        double ori_height_you = maxPoint_you.Y - minPoint_you.Y;            // bottom
                        double changedHeight_you = ori_height_you + moved.Y;
                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedWidth_you > sizeThreshold)                   // left
                            {
                                double RatioX = changedWidth_you / ori_width_you;
                                point.X = point.X * RatioX + anchorMin_you[j].X * (1 - RatioX);
                            }

                            if (changedHeight_you > sizeThreshold)                  // bottom
                            {
                                double RatioY = changedHeight_you / ori_height_you;
                                point.Y = point.Y * RatioY + anchorMin_you[j].Y * (1 - RatioY);
                            }
                        }
                    }
                    j++;
                }

            }
            else if (border.Name.Equals("Border_BottomRight"))
            {
                foreach (Teeth sibling in SelectedList)
                {
                    var pts_sibling = Numerics.TeethToList(sibling);
                    Point maxPoint = new Point(Numerics.GetMaxX_Teeth(pts_sibling).X, Numerics.GetMaxY_Teeth(pts_sibling).Y);
                    Point minPoint = new Point(Numerics.GetMinX_Teeth(pts_sibling).X, Numerics.GetMinY_Teeth(pts_sibling).Y);

                    if (isFirstTimeMovedOnSizing[i])
                    {
                        anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                        anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                        isFirstTimeMovedOnSizing[i] = false;
                    }

                    double ori_width = maxPoint.X - minPoint.X;         // right
                    double changedWidth = ori_width + moved.X;

                    double ori_height = maxPoint.Y - minPoint.Y;        // bottom
                    double changedHeight = ori_height + moved.Y;

                    foreach (PointViewModel point in sibling.Points)
                    {
                        if (changedWidth > sizeThreshold)               // right
                        {
                            double RatioX = changedWidth / ori_width;
                            point.X = point.X * RatioX + anchorMin[i].X * (1 - RatioX);
                        }

                        if (changedHeight > sizeThreshold)              // bottom
                        {
                            double RatioY = changedHeight / ori_height;
                            point.Y = point.Y * RatioY + anchorMin[i].Y * (1 - RatioY);
                        }
                    }
                    i++;

                    // Mirror Mode
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(sibling, main.ToothControl.dic);
                        var pts_you = Numerics.TeethToList(you);

                        Point maxPoint_you = new Point(Numerics.GetMaxX_Teeth(pts_you).X, Numerics.GetMaxY_Teeth(pts_you).Y);
                        Point minPoint_you = new Point(Numerics.GetMinX_Teeth(pts_you).X, Numerics.GetMinY_Teeth(pts_you).Y);
                        if (isFirstTimeMovedOnSizing_you[j])
                        {
                            anchorMin_you[j] = new Point(minPoint_you.X, minPoint_you.Y);
                            anchorMax_you[j] = new Point(maxPoint_you.X, maxPoint_you.Y);
                            isFirstTimeMovedOnSizing_you[j] = false;
                        }

                        double ori_width_you = maxPoint_you.X - minPoint_you.X;         // right
                        double changedWidth_you = ori_width_you + moved.X;

                        double ori_height_you = maxPoint_you.Y - minPoint_you.Y;        // bottom
                        double changedHeight_you = ori_height_you + moved.Y;
                        foreach (PointViewModel point in you.Points)
                        {
                            if (changedWidth_you > sizeThreshold)
                            {
                                double RatioX = changedWidth_you / ori_width_you;
                                point.X = point.X * RatioX + anchorMax_you[j].X * (1 - RatioX);
                            }

                            if (changedHeight_you > sizeThreshold)
                            {
                                double RatioY = changedHeight_you / ori_height_you;
                                point.Y = point.Y * RatioY + anchorMin_you[j].Y * (1 - RatioY);
                            }
                        }
                    }
                    j++;
                }

            }
        }

        private RelayCommand<object> _mouseLeftUpForResizeTeeth;
        public RelayCommand<object> MouseLeftUpForResizeTeeth
        {
            get
            {
                if (_mouseLeftUpForResizeTeeth == null)
                    return _mouseLeftUpForResizeTeeth = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTeeth((MouseEventArgs)param));
                return _mouseLeftUpForResizeTeeth;
            }
            set { _mouseLeftUpForResizeTeeth = value; }
        }

        public void ExecuteMouseLeftUpForResizeTeeth(MouseEventArgs e)
        {
            Mouse.Capture(null);
            isSizing = false;
            isFirstTimeMovedOnSizing = Enumerable.Repeat(true, 10).ToList();
            isFirstTimeMovedOnSizing_you = Enumerable.Repeat(true, 10).ToList();
        }

        #endregion

        #region Resize for Tooth

        bool isFirstTimeMovedOnSizingTooth = true;
        Point anchorToothMin;
        Point anchorToothMax;

        private void SetSizingTooth(MouseEventArgs e)
        {
            if (!isSizing)
                return;

            Border border = (Border)e.Source;
            Grid grid = (Grid)border.Parent;
            Border borderSecond = (Border)grid.Parent;
            WrapTooth wrapTooth = (WrapTooth)borderSecond.Parent;

            var list = Numerics.ToothToList(wrapTooth);
            Point minPoint = Numerics.GetMinXY_Tooth(list);
            Point maxPoint = Numerics.GetMaxXY_Tooth(list);

            if (isFirstTimeMovedOnSizingTooth)
            {
                //to memory the first location of anchor point
                anchorToothMin = new Point(minPoint.X, minPoint.Y);
                anchorToothMax = new Point(maxPoint.X, maxPoint.Y);
                isFirstTimeMovedOnSizingTooth = false;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point moved = e.GetPosition(e.Source as IInputElement);
            double changedWidth = maxPoint.X - minPoint.X + moved.X;
            double changedHeight = maxPoint.Y - minPoint.Y + moved.Y;
            double changedWidth_rev = maxPoint.X - minPoint.X - moved.X;
            double changedHeight_rev = maxPoint.Y - minPoint.Y - moved.Y;

            if (border.Name.Equals("Border_Top"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorToothMax.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_Bottom"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorToothMin.Y * (1 - RatioY);
                        }
                        //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                    }
                }
            }
            else if (border.Name.Equals("Border_Left"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorToothMax.X * (1 - RatioX);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_Right"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorToothMin.X * (1 - RatioX);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_TopLeft"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorToothMax.X * (1 - RatioX);
                        }
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorToothMax.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_TopRight"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 + curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorToothMin.X * (1 - RatioX);
                        }
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorToothMax.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_BottomLeft"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 + curPoint.Y) / actualHeight2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorToothMax.X * (1 - RatioX);
                        }
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorToothMin.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_BottomRight"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorToothMin.X * (1 - RatioX);
                        }
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorToothMin.Y * (1 - RatioY);
                        }
                        //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                        //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                    }
                }
            }
        }
        #region Resize for CommandProperties

        // For Tooth
        private RelayCommand<object> _mouseLeftDownForResizeTooth;
        public RelayCommand<object> MouseLeftDownForResizeTooth
        {
            get
            {
                if (_mouseLeftDownForResizeTooth == null)
                    return _mouseLeftDownForResizeTooth = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTooth((MouseEventArgs)param));
                return _mouseLeftDownForResizeTooth;
            }
            set { _mouseLeftDownForResizeTooth = value; }
        }

        public void ExecuteMouseLeftDownForResizeTooth(MouseEventArgs e)
        {
            isSizing = true;
            //anchorPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForResizeTooth;
        public RelayCommand<object> MouseMoveForResizeTooth
        {
            get
            {
                if (_mouseMoveForResizeTooth == null)
                    return _mouseMoveForResizeTooth = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTooth((MouseEventArgs)param));
                return _mouseMoveForResizeTooth;
            }
            set { _mouseMoveForResizeTooth = value; }
        }

        public void ExecuteMouseMoveForResizeTooth(MouseEventArgs e)
        {
            SetSizingTooth(e);
        }

        private RelayCommand<object> _mouseLeftUpForResizeTooth;
        public RelayCommand<object> MouseLeftUpForResizeTooth
        {
            get
            {
                if (_mouseLeftUpForResizeTooth == null)
                    return _mouseLeftUpForResizeTooth = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTooth((MouseEventArgs)param));
                return _mouseLeftUpForResizeTooth;
            }
            set { _mouseLeftUpForResizeTooth = value; }
        }

        public void ExecuteMouseLeftUpForResizeTooth(MouseEventArgs e)
        {
            isSizing = false;
            Mouse.Capture(null);
            isFirstTimeMovedOnSizingTooth = true;
        }

        #endregion

        #endregion

        #endregion

        #region Rotate

        private bool captured_rotate = false;
        private double[] accAlangle = new double[10];

        private List<bool> firstRotate = new List<bool>(10);
        private double[] degrees = new double[10];
        private List<Point> RotateAnchor = new List<Point>();

        private List<Teeth> rotatedlist = new List<Teeth>();

        private int limitAngle = 30;
        private bool isRotating = false;

        #region Rotate for Teeth

        private RelayCommand<object> _mouseLeftDownForRotateTeeth;
        public RelayCommand<object> MouseLeftDownForRotateTeeth
        {
            get
            {
                if (_mouseLeftDownForRotateTeeth == null)
                    return _mouseLeftDownForRotateTeeth = new RelayCommand<object>(param => ExecuteMouseLeftDownForRotateTeeth(param as MouseEventArgs));
                return _mouseLeftDownForRotateTeeth;
            }
            set { _mouseLeftDownForRotateTeeth = value; }
        }

        private void ExecuteMouseLeftDownForRotateTeeth(MouseEventArgs e)
        {
            if (captured_rotate) return;
            captured_rotate = true;
            isRotating = true;
            Mouse.Capture(e.Source as IInputElement);
        }

        private RelayCommand<object> _mouseMoveForRotateTeeth;
        public RelayCommand<object> MouseMoveForRotateTeeth
        {
            get
            {
                if (_mouseMoveForRotateTeeth == null)
                    return _mouseMoveForRotateTeeth = new RelayCommand<object>(param => ExecuteMouseMoveForRotateTeeth(param as MouseEventArgs));
                return _mouseMoveForRotateTeeth;
            }
            set { _mouseMoveForRotateTeeth = value; }
        }

        private void ExecuteMouseMoveForRotateTeeth(MouseEventArgs e)
        {
            if (!captured_rotate)
                return;

            RotateTeeth rotate = e.Source as RotateTeeth;
            Teeth teeth = ViewUtils.FindParent(rotate, (new Teeth()).GetType()) as Teeth;
            own = teeth;

            List<Point> pts = Numerics.TeethToList(teeth);
            Point min_t = new Point(Numerics.GetMinX_Teeth(pts).X, Numerics.GetMinY_Teeth(pts).Y);
            Point max_t = new Point(Numerics.GetMaxX_Teeth(pts).X, Numerics.GetMaxY_Teeth(pts).Y);

            Point ctrl = new Point((max_t.X + min_t.X) / 2, (max_t.Y + min_t.Y) / 2);

            Point cur = e.GetPosition(e.Source as IInputElement);
            double rad = Math.Atan2(cur.Y - ctrl.Y, cur.X - ctrl.X);
            double deg = Numerics.Rad2Deg(rad);

            // make a list of be rotated
            foreach (Teeth me in SelectedList)
            {
                if (rotatedlist.Contains(me) == false)
                {
                    rotatedlist.Add(me);
                    if (main.ToothControl.mirror.IsChecked == true)
                    {
                        Teeth you = ViewUtils.FindSymmetric(me, main.ToothControl.dic);
                        rotatedlist.Add(you);
                    }
                }
            }

            // set the anchor of rotate only when the rotate is first
            int i = 0;
            foreach (Teeth me in rotatedlist)
            {
                if (firstRotate[i])
                {
                    List<Point> l = Numerics.TeethToList(me);
                    Point min = new Point(Numerics.GetMinX_Teeth(l).X, Numerics.GetMinY_Teeth(l).Y);
                    Point max = new Point(Numerics.GetMaxX_Teeth(l).X, Numerics.GetMaxY_Teeth(l).Y);

                    if (i == 0)
                        RotateAnchor.Clear();

                    RotateAnchor.Add(new Point((max.X + min.X) / 2, (max.Y + min.Y) / 2));
                    firstRotate[i++] = false;
                }
            }

            int j = 0;
            foreach (Teeth me in rotatedlist)
            {
                degrees[j] = deg + 90 + accAlangle[j];

                int idx_own = main.ToothControl.dic[own.Name];
                int idx_me = main.ToothControl.dic[me.Name];
                // Mirror Mode
                if (main.ToothControl.mirror.IsChecked == true)
                    // me & own : different side
                    if (idx_me < 3 && idx_own >= 3 || idx_me >= 3 && idx_own < 3)
                        degrees[j] = -deg - 90 + accAlangle[j];

                if (degrees[j] <= -limitAngle || degrees[j] >= limitAngle)
                {
                    j++;
                    continue;
                }

                RotateTransform rotatetransform = new RotateTransform(degrees[j], RotateAnchor[j].X, RotateAnchor[j].Y);
                me.RenderTransform = rotatetransform;
                accAlangle[j] = degrees[j];
                j++;
            }
            //rotatedlist.Clear();
        }

        private RelayCommand<object> _mouseLeftUpForRotateTeeth;
        public RelayCommand<object> MouseLeftUpForRotateTeeth
        {
            get
            {
                if (_mouseLeftUpForRotateTeeth == null)
                    return _mouseLeftUpForRotateTeeth = new RelayCommand<object>(param => ExecuteMouseLeftUpForRotateTeeth(param as MouseEventArgs));
                return _mouseLeftUpForRotateTeeth;
            }
            set { _mouseLeftUpForRotateTeeth = value; }
        }

        private void ExecuteMouseLeftUpForRotateTeeth(MouseEventArgs e)
        {
            captured_rotate = false;
            firstRotate = Enumerable.Repeat(true, 10).ToList();
            isRotating = false;
            Mouse.Capture(null);
        }

        #endregion

        #endregion

        #region SmileLine

        private bool captured_arc;
        private Point org;

        private RelayCommand<object> _mouseLeftDownForSmileLine;
        public RelayCommand<object> MouseLeftDownForSmileLine
        {
            get
            {
                if (_mouseLeftDownForSmileLine == null)
                    return _mouseLeftDownForSmileLine = new RelayCommand<object>(param => ExecuteMouseLeftDownForSmileLine((MouseEventArgs)param));
                return _mouseLeftDownForSmileLine;
            }
            set { _mouseLeftDownForSmileLine = value; }
        }
        private void ExecuteMouseLeftDownForSmileLine(MouseEventArgs e)
        {
            Path smile = e.Source as Path;
            if (smile == null)
                return;

            captured_arc = true;
            org = e.GetPosition(e.Source as IInputElement);
            Mouse.Capture(smile);
        }

        private RelayCommand<object> _mouseMoveForSmileLine;
        public RelayCommand<object> MouseMoveForSmileLine
        {
            get
            {
                if (_mouseMoveForSmileLine == null)
                    return _mouseMoveForSmileLine = new RelayCommand<object>(param => ExecuteMouseMoveForSmileLine((MouseEventArgs)param));
                return _mouseMoveForSmileLine;
            }
            set { _mouseMoveForSmileLine = value; }
        }
        private void ExecuteMouseMoveForSmileLine(MouseEventArgs e)
        {
            Path smile = e.Source as Path;
            if (smile == null || captured_arc == false)
                return;

            Point cur = e.GetPosition(e.Source as IInputElement);
            var moved = cur - org;

            //var cv = smile.Parent as Canvas;
            //if (smile.Name.Equals("UpSmile_Left") || smile.Name.Equals("UpSmile_Right"))
            //{
            //    Path smileline = cv.FindName("SmileLineUp") as Path;
            //}
            //else if (smile.Name.Equals("DownSmile_Left") || smile.Name.Equals("DownSmile_Right"))
            //{
            //    Path smileline = cv.FindName("SmileLineDown") as Path;
            //    Down
            //}
            //else return;



        }

        private RelayCommand<object> _mouseLeftUpForSmileLine;
        public RelayCommand<object> MouseLeftUpForSmileLine
        {
            get
            {
                if (_mouseLeftUpForSmileLine == null)
                    return _mouseLeftUpForSmileLine = new RelayCommand<object>(param => ExecuteMouseLeftUpForSmileLine((MouseEventArgs)param));
                return _mouseLeftUpForSmileLine;
            }
            set { _mouseLeftUpForSmileLine = value; }
        }
        private void ExecuteMouseLeftUpForSmileLine(MouseEventArgs e)
        {
            captured_arc = false;
            Mouse.Capture(null);
        }

        #endregion

        #region Add Points

        private RelayCommand<object> _mouseLeftDownForAddPoints;
        public RelayCommand<object> MouseLeftDownForAddPoints
        {
            get
            {
                if (_mouseLeftDownForAddPoints == null)
                    return _mouseLeftDownForAddPoints = new RelayCommand<object>(param => ExecuteMouseLeftDownForAddPoints(param as MouseEventArgs));
                return _mouseLeftDownForAddPoints;
            }
            set { _mouseLeftDownForAddPoints = value; }
        }

        private void ExecuteMouseLeftDownForAddPoints(MouseEventArgs e)
        {
            if (isSizing || isRotating) return;

            main = Application.Current.MainWindow.Content as SmileDesign_Page;
            if (main.ToothControl.EditPoints.IsChecked == false)
            {
                if (inTeeth == false)
                {
                    foreach (Teeth del in SelectedList)
                    {
                        RotateTeeth rotate_del = del.FindName("rotateTeeth") as RotateTeeth;
                        DrawTeeth draw_del = del.FindName("drawTeeth") as DrawTeeth;
                        WrapTeeth wrap_del = del.FindName("wrapTeeth") as WrapTeeth;

                        Border border_del = wrap_del.FindName("Border_WrapTeeth") as Border;
                        Rectangle rect_del = wrap_del.FindName("Rectangle_WrapTeeth") as Rectangle;

                        border_del.Opacity = 0;
                        draw_del.path.Stroke = draw_del.FindResource("NonSelected_StrokeBrush") as Brush;
                        draw_del.path.Fill = null;
                        rotate_del.RotatePin.Visibility = Visibility.Hidden;
                        del.list.Visibility = Visibility.Hidden;
                    }
                    SelectedList.Clear();
                }
                return;
            }

            if (SelectedList.Count != 1)
            {
                MessageBox.Show("편집할 치아를 선택해주세요!");
                return;
            }

            UserControl tooth = e.Source as UserControl;     // UpperTooth, LowerTooth
            WrapTooth wrap = tooth.FindName("WrapToothInTooth") as WrapTooth;

            Teeth me = SelectedList[0];
            TeethType me_type = null;
            //TeethType me_type = wrap.Points[main.ToothControl.dic[me.Name]] as TeethType;
            int idx_me = main.ToothControl.dic[me.Name];
            int j = 0;
            foreach (TeethType tt in wrap.Points)
            {
                if (j++ == idx_me)
                {
                    me_type = tt;
                    break;
                }
            }

            Point curPoint = e.GetPosition(e.Source as IInputElement);

            double min_dist1 = double.MaxValue;
            PointViewModel nearest1 = null;
            foreach (PointViewModel point in me.Points)
            {
                double dist = Numerics.Distance(curPoint, new Point(point.X, point.Y));
                if (dist < min_dist1)
                {
                    min_dist1 = dist;
                    nearest1 = point;
                }
            }

            double min_dist2 = double.MaxValue;
            PointViewModel nearest2 = null;
            foreach (PointViewModel point in me.Points)
            {
                double dist = Numerics.Distance(curPoint, new Point(point.X, point.Y));
                if (dist < min_dist2 && dist > min_dist1)
                {
                    min_dist2 = dist;
                    nearest2 = point;
                }
            }

            List<Point> me_list = Numerics.TeethToList(me);
            var segment = new List<Point>();

            if (nearest1.I == 0)
            {
                segment.Add(me_list[me_list.Count - 1]);
                segment.Add(me_list[nearest1.I]);
                segment.Add(me_list[nearest1.I + 1]);
            }
            else if (nearest1.I == me_list.Count - 1)
            {
                segment.Add(me_list[nearest1.I - 1]);
                segment.Add(me_list[nearest1.I]);
                segment.Add(me_list[0]);
            }
            else
            {
                segment.Add(me_list[nearest1.I - 1]);
                segment.Add(me_list[nearest1.I]);
                segment.Add(me_list[nearest1.I + 1]);
            }

            Point ctrl = InterpolationUtils.InterpolatePointWithBezierCurves(segment, true)[0].FirstControlPoint;

            //make tangent and normal line
            Numerics.Sign tangentline = Numerics.TangentLineTest(me_list[nearest1.I], ctrl, curPoint);
            Numerics.Sign normalline = Numerics.NormalLineTest(me_list[nearest1.I], ctrl, curPoint);

            double dst = float.MaxValue;
            double min_dst = float.MaxValue;
            int mem = 0;
            for (int i = 0; i < me_list.Count; i++)
            {
                // Among points, Find points on the same side
                if (Numerics.TangentLineTest(me_list[nearest1.I], ctrl, me_list[i]) == tangentline
                    && Numerics.NormalLineTest(me_list[nearest1.I], ctrl, me_list[i]) == normalline)
                {
                    dst = Numerics.Distance(curPoint, me_list[i]);
                    if (dst < min_dst)
                    {
                        min_dst = dst;
                        mem = i;
                    }
                }
            }

            if (me_type == null) return;
            int pos = (nearest1.I < mem ? mem : nearest1.I);

            if ((nearest1.I == 0 && nearest2.I == me_list.Count - 1) || (nearest1.I == me_list.Count - 1 && nearest2.I == 0))
                me_type.Add(new PointViewModel(curPoint.X, curPoint.Y, me_list.Count));
            else
            {

                me_type.Insert(pos, new PointViewModel(curPoint.X, curPoint.Y, pos));
                foreach (var p in me_type)
                {
                    if (p.I == pos)
                    {
                        for (int i = 0; i < me_type.Count - pos - 1; i++)
                            me_type[i + pos + 1].I++;
                        break;
                    }
                }
            }

            RaisePropertyChanged("UpperPoints");
            RaisePropertyChanged("LowerPoints");
        }


        #endregion

        #region Move KeyBoard

        //키보드 키다운 커맨드,
        //지금은 1픽셀씩 이동하지만 zoom-scale을 참조해서
        //비율에 알맞게 이동시키는 것이 이상적임.
        private RelayCommand<object> _keyboardDown;
        public RelayCommand<object> keyboardDown
        {
            get
            {
                if (_keyboardDown == null)
                    return _keyboardDown = new RelayCommand<object>(param => ExecuteKeyboardDown((KeyboardEventArgs)param));
                return _keyboardDown;
            }
            set { _keyboardDown = value; }
        }

        public void ExecuteKeyboardDown(KeyboardEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.Down))
            {
                foreach (Teeth teeth in SelectedList)
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        point.Y += 1;
                    }
                }
            }
            else if (e.KeyboardDevice.IsKeyDown(Key.Up))
            {
                foreach (Teeth teeth in SelectedList)
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        point.Y -= 1;
                    }
                }
            }
            else if (e.KeyboardDevice.IsKeyDown(Key.Right))
            {
                foreach (Teeth teeth in SelectedList)
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        point.X += 1;
                    }
                }
            }
            else if (e.KeyboardDevice.IsKeyDown(Key.Left))
            {
                foreach (Teeth teeth in SelectedList)
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        point.X -= 1;
                    }
                }
            }
        }

        #endregion
        #endregion
    }
}