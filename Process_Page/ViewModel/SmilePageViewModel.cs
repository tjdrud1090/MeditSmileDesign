﻿using System;
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
            var current = mainWnd.Content as SmileDesign_Page ;

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
            FrontalFaceImage = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).FrontalFaceImage;
            GagFaceImage = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).GagFaceImage;
            RaisePropertyChanged("FrontalFaceSource");
            RaisePropertyChanged("GagFaceSource");

            _TransCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).TransCenter;
            RaisePropertyChanged("TransCenter");

            _TransGagCenter = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).TransGagCenter;
            RaisePropertyChanged("TransGagCenter");

            draw_faceline();
            SetAlign();

            _ViewScale= ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).ViewScale;
            RaisePropertyChanged("ViewScale");

            // Upper Tooth 위치 선정
            _ToothUpperCenter.X = midline.StartPoint.X;
            _ToothUpperCenter.Y = _lipline.StartPoint.Y;
            RaisePropertyChanged("ToothUpperCenter");

            // Lower Tooth 위치 선정
            _ToothLowerCenter.X = midline.StartPoint.X;
            _ToothLowerCenter.Y = _lipline.StartPoint.Y + 10;
            RaisePropertyChanged("ToothLowerCenter");
        }

        public void draw_faceline()
        {
            //set picture center
            _GagPoints = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).GagPoints;

            _noseline_L = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).noseline_L;
            _noseline_R = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).noseline_R;

            _midline = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).midline;

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

            _lipline = ((FaceAlign_PageViewModel)(faceAlignInfo.DataContext)).lipline;
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
          SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
            currentPage.MagnifiyingGlass.ZoomFactor = 0.5;
            if (e.Delta > 0)
             {
                if (_ViewScale < 7)
                {
                    _ViewScale += 0.1;

                    RaisePropertyChanged("ViewScale");
                    currentPage.MagnifiyingGlass.ZoomFactor = 0.5;

                }
            }
            else
            {
                if (_ViewScale > 1)
                {
                    _ViewScale -= 0.1;
                    RaisePropertyChanged("ViewScale");
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
                    currentclicked = ((UserControl)(e.Source));
                    if (!currentclicked.Name.Equals("FrontalFaceImage"))
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

        #region Tooth mouse events

        List<Teeth> SelectedList = new List<Teeth>();

        #region DragDrop

        private bool leftdown = false;
        private bool leftdown_with_ctrl = false;
        private bool dragging = false;

        #region DragDrop for Teeth 

        private Point originalPoint;

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
            Rectangle rect = e.Source as Rectangle;
            Border border = ViewUtils.FindParent(rect, (new Border()).GetType()) as Border;

            Teeth th = ViewUtils.FindParent(rect, Type.GetType("Process_Page.ToothTemplate.Teeth")) as Teeth;
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
                    SelectedList.Add(th);

                    border.Opacity = 1;
                    th.list.Visibility = Visibility.Visible;
                    rotate.RotatePin.Visibility = Visibility.Visible;
                    draw.path.Stroke = draw.FindResource("Selected_StrokeBrush") as Brush;
                    draw.path.Fill = draw.FindResource("FillBrush") as Brush;
                }
                else if (SelectedList.Contains(th))
                {
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

            // padding
            originalPoint.X += 5;
            originalPoint.Y += 5;
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
            main = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
            if (dragging)
            {
                if (leftdown == true)
                {
                    Rectangle rect = e.Source as Rectangle;
                    Teeth me_rect = ViewUtils.FindParent(rect, Type.GetType("Process_Page.ToothTemplate.Teeth")) as Teeth;
                    if (SelectedList.Contains(me_rect))
                    {
                        foreach (Teeth me in SelectedList)
                        {
                            Teeth you = null;
                            if (main.ToothControl.mirror.IsChecked == true)
                            {
                                Grid grid_me = ViewUtils.FindParent(me, (new Grid()).GetType()) as Grid;

                                int idx_me = main.ToothControl.dic[me.Name];
                                int idx_you = idx_me + (idx_me >= 0 && idx_me < 3 ? +3 : -3);
                                var myKey = main.ToothControl.dic.FirstOrDefault(p => p.Value == idx_you).Key;
                                you = grid_me.FindName(myKey) as Teeth;
                            }

                            Point curPoint = e.GetPosition((IInputElement)e.Source);
                            var dragDelta = curPoint - originalPoint;
                            foreach (PointViewModel point in me.Points)
                            {
                                point.X += dragDelta.X;
                                point.Y += dragDelta.Y;
                            }

                            if (you != null)
                            {
                                foreach (PointViewModel point in you.Points)
                                {
                                    point.X -= dragDelta.X;
                                    point.Y += dragDelta.Y;
                                }
                            }
                        }
                    }
                }
            }
            else if (leftdown)
                dragging = true;
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
                            border_dragdrop.Opacity = 0;
                            draw.path.Fill = null;
                            rotate.RotatePin.Visibility = Visibility.Hidden;
                        }

                        else
                        {
                            SelectedList.Add(th);
                            th.list.Visibility = Visibility.Visible;
                            draw.path.Fill = draw.FindResource("FillBrush") as Brush;
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
                e.Handled = true;
            }

            dragging = false;
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
            //Rectangle me = e.Source as Rectangle;
            ArrowLine me = e.Source as ArrowLine;
            //DragArrow me = e.Source as DragArrow;
            Border me_border = ViewUtils.FindParent(me, (new Border()).GetType()) as Border;
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

            //padding
            originalPoint.X += 5;
            originalPoint.Y += 5;

            orgBrush2 = me_border.BorderBrush;
            //me.Stroke = Brushes.Red;
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
            //Rectangle me = e.Source as Rectangle;
            ArrowLine me = e.Source as ArrowLine;
            //DragArrow me = e.Source as DragArrow;
            WrapTooth wrap = ViewUtils.FindParent(me, Type.GetType("Process_Page.ToothTemplate.WrapTooth")) as WrapTooth;
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
            //rect2.Stroke = orgBrush2;
            //border2.BorderBrush = orgBrush2;

            //Rectangle me = e.Source as Rectangle;
            ArrowLine me = e.Source as ArrowLine;
            //DragArrow me = e.Source as DragArrow;
            Border me_border = ViewUtils.FindParent(me, (new Border()).GetType()) as Border;
            //me.Stroke = orgBrush2;
            me_border.BorderBrush = orgBrush2;

            leftdown = false;
            Mouse.Capture(null);
        }

        #endregion

        #endregion

        #region Resize 

        private bool isSizing = false;
        private readonly double sizeThreshold = 10;

        private bool[] isFirstTimeMovedOnSizing = new bool[10];
        private Point[] anchorMin = new Point[10];
        private Point[] anchorMax = new Point[10];
        #region Resize for Teeth


        #region Resize for CommandProperties

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
            Border border = (Border)e.Source;
            int i = 0;

            foreach (Teeth teeth in SelectedList)
            {
                //Grid grid = (Grid)border.Parent;
                //Border borderSecond = (Border)grid.Parent;
                //WrapTeeth wrapTeeth = (WrapTeeth)borderSecond.Parent;
                //Canvas canvas = wrapTeeth.Parent as Canvas;

                Point maxPoint = Numerics.GetMaxPointTeeth(teeth);
                Point minPoint = Numerics.GetMinPointTeeth(teeth);

                if (isFirstTimeMovedOnSizing[i])
                {
                    anchorMin[i] = new Point(minPoint.X, minPoint.Y);
                    anchorMax[i] = new Point(maxPoint.X, maxPoint.Y);
                    isFirstTimeMovedOnSizing[i] = false;
                }

                if (e.LeftButton != MouseButtonState.Pressed)
                    return;

                //double actualWidth1 = minPoint.X + wrapTeeth.ActualWidth;
                //double actualHeight1 = minPoint.Y + wrapTeeth.ActualHeight;
                //double actualWidth2 = maxPoint.X - wrapTeeth.ActualWidth;
                //double actualHeight2 = maxPoint.X - wrapTeeth.ActualHeight;

                Point moved = e.GetPosition(e.Source as IInputElement);
                double changedWidth = maxPoint.X - minPoint.X + moved.X;
                double changedHeight = maxPoint.Y - minPoint.Y + moved.Y;
                double changedWidth_rev = maxPoint.X - minPoint.X - moved.X;
                double changedHeight_rev = maxPoint.Y - minPoint.Y - moved.Y;

                double changedWidth_ori = maxPoint.X - minPoint.X;
                double changedHeight_ori = maxPoint.Y - minPoint.Y;

                Console.WriteLine("moved : " + moved);
                Console.WriteLine("maxPoint : " + maxPoint);


                //double dx = Math.Abs((actualWidth1 + curPoint.X) - anchorMin.X);
                //double dy = Math.Abs((actualHeight1 + curPoint.Y) - anchorMin.Y);

                if (border.Name.Equals("Border_Top"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedHeight_rev > sizeThreshold)
                        {
                            //double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            double RatioY = changedHeight_ori / changedHeight;
                            point.Y = point.Y * RatioY + anchorMax[i].Y * (1 - RatioY);
                        }
                    }

                }
                else if (border.Name.Equals("Border_Bottom"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        if (changedHeight > sizeThreshold)
                        {
                            //double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            double RatioY = changedHeight / changedHeight_ori;

                            point.Y = point.Y * RatioY + anchorMin[i].Y * (1 - RatioY);
                        }

                        //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                    }
                }
                else if (border.Name.Equals("Border_Left"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            //double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            double RatioX = changedWidth_ori / changedWidth;
                            point.X = point.X * RatioX + anchorMax[i].X * (1 - RatioX);
                        }
                    }

                }
                else if (border.Name.Equals("Border_Right"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {
                        //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                        if (changedWidth > sizeThreshold)
                        {
                            //double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            double RatioX = changedWidth / changedWidth_ori;

                            point.X = point.X * RatioX + anchorMin[i].X * (1 - RatioX);
                        }
                    }

                }
                else if (border.Name.Equals("Border_TopLeft"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {

                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = changedWidth_ori / changedWidth;

                            point.X = point.X * RatioX + anchorMax[i].X * (1 - RatioX);
                        }
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = changedHeight_ori / changedHeight;

                            point.Y = point.Y * RatioY + anchorMax[i].Y * (1 - RatioY);
                        }

                    }
                }
                else if (border.Name.Equals("Border_TopRight"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {

                        //point.X = point.X * ((actualWidth2 + curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = changedWidth / changedWidth_ori;

                            point.X = point.X * RatioX + anchorMin[i].X * (1 - RatioX);
                        }
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = changedHeight_ori / changedHeight;

                            point.Y = point.Y * RatioY + anchorMax[i].Y * (1 - RatioY);
                        }
                    }

                }
                else if (border.Name.Equals("Border_BottomLeft"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {

                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 + curPoint.Y) / actualHeight2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = changedWidth_ori / changedWidth;
                            point.X = point.X * RatioX + anchorMax[i].X * (1 - RatioX);
                        }
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = changedHeight / changedHeight_ori;
                            point.Y = point.Y * RatioY + anchorMin[i].Y * (1 - RatioY);
                        }

                    }
                }
                else if (border.Name.Equals("Border_BottomRight"))
                {
                    foreach (PointViewModel point in teeth.Points)
                    {

                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = changedWidth / changedWidth_ori;
                            point.X = point.X * RatioX + anchorMin[i].X * (1 - RatioX);
                        }
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = changedHeight / changedHeight_ori;
                            point.Y = point.Y * RatioY + anchorMin[i].Y * (1 - RatioY);
                        }
                        //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                        //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                    }
                }
                i++;
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
            isSizing = false;

            for (int i = 0; i < 10; i++)
                isFirstTimeMovedOnSizing[i] = true;

            Mouse.Capture(null);
        }

        #endregion

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

            Point minPoint = Numerics.GetMinPoint(wrapTooth);
            Point maxPoint = Numerics.GetMaxPoint(wrapTooth);

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

        #region SmileLine

        //private Rectangle rect3;
        private Point arc_origin;
        private bool captured_arc;

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

            //arc_origin = e.GetPosition(e.Source as IInputElement);
            captured_arc = true;
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
            if (smile == null)
                return;

            Ellipse me = e.Source as Ellipse;

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

        #region Rotate

        private bool captured_rotate = false;
        private List<bool> firstRotate = new List<bool>(10);

        private double[] degrees = new double[10];
        private double[] accAlangle = new double[10];
        private List<Point> RotateAnchor = new List<Point>();

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
            if (captured_rotate)
                return;
            captured_rotate = true;
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

            RotateTeeth me = e.Source as RotateTeeth;
            Teeth teeth = ViewUtils.FindParent(me, Type.GetType("Process_Page.ToothTemplate.Teeth")) as Teeth;

            List<Point> rotate = Numerics.TeethToList(teeth);
            Point min = new Point(Numerics.GetMinX_Teeth(rotate).X, Numerics.GetMinY_Teeth(rotate).Y);
            Point max = new Point(Numerics.GetMaxX_Teeth(rotate).X, Numerics.GetMaxY_Teeth(rotate).Y);

            Point ctrl = new Point((max.X + min.X) / 2, (max.Y + min.Y) / 2);
            Point cur = e.GetPosition(e.Source as IInputElement);

            int i = 0;
            foreach (Teeth t in SelectedList)
            {
                List<Point> l = Numerics.TeethToList(t);
                Point min_t = new Point(Numerics.GetMinX_Teeth(l).X, Numerics.GetMinY_Teeth(l).Y);
                Point max_t = new Point(Numerics.GetMaxX_Teeth(l).X, Numerics.GetMaxY_Teeth(l).Y);

                if (firstRotate[i])
                {
                    if (i == 0)
                        RotateAnchor.Clear();
                    RotateAnchor.Add(new Point((max_t.X + min_t.X) / 2, (max_t.Y + min_t.Y) / 2));
                    firstRotate[i] = false;
                    Console.WriteLine($"{i}, {RotateAnchor[i]}");
                    i++;
                }
            }

            double rad = Math.Atan2(cur.Y - ctrl.Y, cur.X - ctrl.X);
            double deg = Numerics.Rad2Deg(rad);

            int j = 0;


            foreach (Teeth t in SelectedList)
            {
                degrees[j] = deg;
                degrees[j] += accAlangle[j] + 90;
                if (degrees[j] <= -30 || degrees[j] >= 30)
                    continue;
                RotateTransform rotatetransform = new RotateTransform(degrees[j], RotateAnchor[j].X, RotateAnchor[j].Y);
                t.RenderTransform = rotatetransform;
                accAlangle[j] = degrees[j];
                j++;
            }
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
            for (int i = 0; i < firstRotate.Count; i++)
                firstRotate[i] = true;
            Mouse.Capture(null);
        }

        #endregion

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
            main = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
            if (main.ToothControl.EditPoints.IsChecked == false)
                return;

            if (!isSizing)
            {
                UserControl tooth = e.Source as UserControl;        // UpperTooth, LowerTooth
                WrapTooth wrap = tooth.FindName("WrapToothInTooth") as WrapTooth;
                //Grid grid = tooth.FindName("GridInTooth") as Grid;

                Point curPoint = e.GetPosition(e.Source as IInputElement);

                // Find the 1st-nearest point from the curPoint
                double min_dist1 = double.MaxValue;
                PointViewModel nearest1 = null;
                TeethType target1 = null;
                foreach (TeethType teeth in wrap.Points)
                {
                    foreach (PointViewModel point in teeth)
                    {
                        double dist = Numerics.Distance(curPoint, new Point(point.X, point.Y));
                        if (dist < min_dist1)
                        {
                            min_dist1 = dist;
                            nearest1 = point;
                            target1 = teeth;
                        }
                    }
                }

                // Find the 2nd-nearest point from the curPoint
                double min_dist2 = double.MaxValue;
                PointViewModel nearest2 = null;
                TeethType target2 = null;
                foreach (TeethType teeth in wrap.Points)
                {
                    foreach (PointViewModel point in teeth)
                    {
                        double dist = Numerics.Distance(curPoint, new Point(point.X, point.Y));
                        if (dist < min_dist2)
                        {
                            min_dist2 = dist;
                            nearest2 = point;
                            //nearest2 = new Point(point.X, point.Y);
                            target2 = teeth;
                        }
                    }
                }

                if (target1 == target2)
                {
                    List<Point> tolist = new List<Point>();
                    foreach (var p in target1)
                        tolist.Add(new Point(p.X, p.Y));
                    List<Point> segment = new List<Point>();

                    for (int i = 0; i < tolist.Count; i++)
                    {
                        if (tolist[i].X == nearest1.X && tolist[i].Y == nearest1.Y)     // tolist[i] == nearest1
                        {
                            if (i == 0)
                            {
                                segment.Add(tolist[tolist.Count - 1]);
                                segment.Add(tolist[i]);
                                segment.Add(tolist[i + 1]);
                            }
                            else if (i == tolist.Count - 1)
                            {
                                segment.Add(tolist[i - 1]);
                                segment.Add(tolist[i]);
                                segment.Add(tolist[0]);
                            }
                            else
                            {
                                segment.Add(tolist[i - 1]);
                                segment.Add(tolist[i]);
                                segment.Add(tolist[i + 1]);
                            }
                            break;
                        }
                    }

                    Point control = InterpolationUtils.InterpolatePointWithBezierCurves(segment, true)[0].FirstControlPoint;

                    Numerics.Sign tangentline = Numerics.TangentLineTest(new Point(nearest1.X, nearest1.Y), control, curPoint);
                    Numerics.Sign normalline = Numerics.NormalLineTest(new Point(nearest1.X, nearest1.Y), control, curPoint);

                    double dst = float.MaxValue;
                    double min_dst = float.MaxValue;
                    int mem = 0;
                    for (int i = 0; i < tolist.Count; i++)
                    {
                        if (Numerics.TangentLineTest(new Point(nearest1.X, nearest1.Y), control, tolist[i]) == tangentline
                            && Numerics.NormalLineTest(new Point(nearest1.X, nearest1.Y), control, tolist[i]) == normalline)
                        {
                            dst = Numerics.Distance(curPoint, tolist[i]);
                            Console.WriteLine($"{i}, {dst}, {min_dst}");
                            if (dst < min_dst)
                            {
                                min_dst = dst;
                                mem = i;
                            }
                        }
                    }

                    Console.WriteLine($"{mem}");

                    int pos = nearest1.I < mem ? mem : nearest1.I;
                    if (nearest1.I == 0 && nearest2.I == tolist.Count - 1 || nearest1.I == tolist.Count - 1 && nearest2.I == 0)
                        target1.Add(new PointViewModel(curPoint.X, curPoint.Y, tolist.Count));
                    else
                    {
                        target1.Insert(pos, new PointViewModel(curPoint.X, curPoint.Y, pos));
                        foreach (var p in target1)
                        {
                            if (p.I == pos)
                            {
                                for (int i = 0; i < target1.Count - pos - 1; i++)
                                    target1[i + pos + 1].I++;
                                break;
                            }
                        }
                    }
                    RaisePropertyChanged("UpperPoints");
                    RaisePropertyChanged("LowerPoints");
                }
            }
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

            #endregion

            #endregion
        }
    }
}