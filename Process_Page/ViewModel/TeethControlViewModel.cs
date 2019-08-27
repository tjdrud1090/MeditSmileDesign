using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Process_Page.ToothTemplate;
using Process_Page.ToothTemplate.ArrowLine;
using Process_Page.ToothTemplate.Utils;
using Process_Page.Util;
using Process_Page_Change.Cards;

namespace Process_Page.ViewModel
{
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

    class TeethControlViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // <summary>
        //  - Teeth control function
        //      1) selected template
        //      2) show real teeth size
        //      3) symmetric control mode

        double ratiox = 0.3;

        public TeethControlViewModel()
        {
            UpperToothList = Enumerable.Repeat(false, 6).ToList();
            LowerToothList = Enumerable.Repeat(false, 6).ToList();
        }

        #region ToothSelection

        // Upper Tooth
        private List<bool> UpperToothList;

        private bool _UpperTmpNo;
        public bool UpperTmpNo
        {
            get { return _UpperTmpNo; }
            set
            {
                if (_UpperTmpNo != value)
                {
                    _UpperTmpNo = value;
                    UpperToothList[0] = value;
                    RaisePropertyChanged("UpperTmp0");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp1;
        public bool UpperTmp1
        {
            get {
             
                return _UpperTmp1; }
            set
            {
                if (_UpperTmp1 != value)
                {
                    SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                    currentPage.UserUpper.Visibility = Visibility.Visible;
                    _UpperTmp1 = value;
                    UpperToothList[1] = value;
                    RaisePropertyChanged("UpperTmp1");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp2;
        public bool UpperTmp2
        {
            get {
             
                return _UpperTmp2; }
            set
            {
                if (_UpperTmp2 != value)
                {
                    _UpperTmp2 = value;
                    UpperToothList[2] = value;
                    RaisePropertyChanged("Points");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp3;
        public bool UpperTmp3
        {
            get { return _UpperTmp3; }
            set
            {
                if (_UpperTmp3 != value)
                {
                    _UpperTmp3 = value;
                    UpperToothList[3] = value;
                    RaisePropertyChanged("_UpperTmp3");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp4;
        public bool UpperTmp4
        {
            get { return _UpperTmp4; }
            set
            {
                if (_UpperTmp4 != value)
                {
                    _UpperTmp4 = value;
                    UpperToothList[4] = value;
                    RaisePropertyChanged("UpperTmp4");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp5;
        public bool UpperTmp5
        {
            get { return _UpperTmp5; }
            set
            {
                if (_UpperTmp5 != value)
                {
                    _UpperTmp5 = value;
                    UpperToothList[5] = value;
                    RaisePropertyChanged("UpperTmp5");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }


        // Lower Tooth
        private List<bool> LowerToothList;

        private bool _LowerTmpNo;
        public bool LowerTmpNo
        {
            get { return _LowerTmpNo; }
            set
            {
                if (_LowerTmpNo != value)
                {
                    _LowerTmpNo = value;
                    LowerToothList[0] = value;
                    RaisePropertyChanged("LowerTmpNo");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        private bool _LowerTmp1;
        public bool LowerTmp1
        {
            get
            {
                return _LowerTmp1; }
            set
            {
                if (_LowerTmp1 != value)
                {
                    SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                    currentPage.UserLower.Visibility = Visibility.Visible;
                    _LowerTmp1 = value;
                    LowerToothList[1] = value;
                    RaisePropertyChanged("LowerTmp1");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        private bool _LowerTmp2;
        public bool LowerTmp2
        {
            get { return _LowerTmp2; }
            set
            {
                if (_LowerTmp2 != value)
                {
                    _LowerTmp2 = value;
                    LowerToothList[2] = value;
                    RaisePropertyChanged("LowerTmp2");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        private bool _LowerTmp3;
        public bool LowerTmp3
        {
            get { return _LowerTmp3; }
            set
            {
                if (_LowerTmp3 != value)
                {
                    _LowerTmp3 = value;
                    LowerToothList[3] = value;
                    RaisePropertyChanged("LowerTmp3");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        private bool _LowerTmp4;
        public bool LowerTmp4
        {
            get { return _LowerTmp4; }
            set
            {
                if (_LowerTmp4 != value)
                {
                    _LowerTmp4 = value;
                    LowerToothList[4] = value;
                    RaisePropertyChanged("LowerTmp4");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        private bool _LowerTmp5;
        public bool LowerTmp5
        {
            get { return _LowerTmp5; }
            set
            {
                if (_LowerTmp5 != value)
                {
                    _LowerTmp5 = value;
                    LowerToothList[5] = value;
                    RaisePropertyChanged("LowerTmp5");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        #endregion

        #region Points

        // Upper
        private ToothType _UpperPoints;
        public ToothType UpperPoints
        {
            get { return _UpperPoints = GetAllPointsUpper(); }
            set
            {
                _UpperPoints = value;
                RaisePropertyChanged("UpperPoints");
            }
        }

        private ToothType GetAllPointsUpper()
        {
            int idx_up = Idx_Templates(UpperToothList);
            if (idx_up > 0) {
                SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                ToothType uppertooth = currentPage.ToothControl.UpperTooth[idx_up - 1];
                ToothType sizechaged = new ToothType();

                Point minPoint = Numerics.GetMinXY_Tooth(uppertooth);
                Point maxPoint = Numerics.GetMaxXY_Tooth(uppertooth);

                double nosewidth = ((LineGeometry)(currentPage.noseline_R.Data)).StartPoint.X - ((LineGeometry)(currentPage.noseline_L.Data)).StartPoint.X;

                ratiox = nosewidth / (maxPoint.X - minPoint.X);

                foreach (var temp in uppertooth)
                {
                    TeethType changed = new TeethType();

                    foreach (var teethtemp in (TeethType)temp)
                    {
                        PointViewModel pt = teethtemp;
                        pt.X *= ratiox;
                        pt.Y *= ratiox;

                        changed.Add(pt);
                    }
                    sizechaged.Add(changed);
                }

                return sizechaged;
            }
                //return ((SmileDesign_Page)(Application.Current.MainWindow).Content).ToothControl.UpperTooth[idx_up - 1];
            return null;
        }

        // Lower
        private ToothType _LowerPoints;
        public ToothType LowerPoints
        {
            get { return _LowerPoints = GetAllPointsLower(); }
        }

        private ToothType GetAllPointsLower()
        {
            int idx_up = Idx_Templates(LowerToothList);
            if (idx_up > 0)
            {
                SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;
                ToothType lowertooth = currentPage.ToothControl.LowerTooth[idx_up - 1];
                ToothType sizechaged = new ToothType();

                Point minPoint = Numerics.GetMinXY_Tooth(lowertooth);
                Point maxPoint = Numerics.GetMaxXY_Tooth(lowertooth);

                double nosewidth = ((LineGeometry)(currentPage.noseline_R.Data)).StartPoint.X - ((LineGeometry)(currentPage.noseline_L.Data)).StartPoint.X;

                ratiox = nosewidth / ((maxPoint.X - minPoint.X) + (maxPoint.X - minPoint.X) / 2);

                foreach (var temp in lowertooth)
                {
                    TeethType changed = new TeethType();

                    foreach (var teethtemp in (TeethType)temp)
                    {
                        PointViewModel pt = teethtemp;
                        pt.X *= ratiox;
                        pt.Y *= ratiox;

                        changed.Add(pt);
                    }
                    sizechaged.Add(changed);
                }

                return sizechaged;
            }
            return null;
        }

        private int Idx_Templates(List<bool> list)
        {
            int indexOfTemplates = -1;
            foreach (bool selected in list)
            {
                if (selected)
                {
                    indexOfTemplates = list.IndexOf(selected);
                    break;
                }
            }
            return indexOfTemplates;
        }

        #endregion

        #region IsClosedCurve
        private const string IsClosedCurveName = "IsClosedCurve";
        public bool IsClosedCurve
        {
            get { return true; }
        }
        #endregion

        #region ColorTeeth
        private const string ColorTeethName = "ColorTooth";
        private bool _ColorTeeth;
        public bool ColorTeeth
        {
            get { return _ColorTeeth; }
            set
            {
                if (_ColorTeeth != value)
                {
                    _ColorTeeth = value;
                    RaisePropertyChanged("ColorTeeth");
                }
            }
        }
        #endregion

        #region ShowLength
        private const string ShowLengthName = "ShowLength";
        private bool _ShowLength;
        public bool ShowLength
        {
            get { return _ShowLength; }
            set {
                if (_ShowLength != value)
                {
                    _ShowLength = value;
                    RaisePropertyChanged("ShowLength");
                }
            }
        }
        #endregion

        #region delete faceline
        private bool _lineDelete;
        public bool lineDelete
        {
            get {
                return _lineDelete;
            }
            set
            {
                if (_lineDelete != value)
                {
                    _lineDelete = value;

                    SmileDesign_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as SmileDesign_Page;

                    if (_lineDelete == true)
                    {
                        currentPage.midline.Visibility = Visibility.Hidden;
                        currentPage.noseline_L.Visibility = Visibility.Hidden;
                        currentPage.noseline_R.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        currentPage.midline.Visibility = Visibility.Visible;
                        currentPage.noseline_L.Visibility = Visibility.Visible;
                        currentPage.noseline_R.Visibility = Visibility.Visible;
                    }

                    RaisePropertyChanged("lineDelete");
                }
            }
        }
        #endregion

    }
}
