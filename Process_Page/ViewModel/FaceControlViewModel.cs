using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Process_Page.ViewModel
{
    class FaceControlViewModel : ViewModelBase
    {
        #region constructor
        public FaceControlViewModel()
        {
        }
        #endregion

        #region image size set
        double imageHeight;
        public double ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }
        double imageWidth;
        public double ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; }
        }

        private RectangleGeometry _Rect;
        public RectangleGeometry Rect
        {
            get { return _Rect; }
            set
            {
                _Rect = value;
                RaisePropertyChanged("Rect");
            }
        }
        #endregion

        #region Image mouse event
        //Image position changed
        private bool captured = false;

        private RelayCommand<object> _MoveImageCommand;
        public RelayCommand<object> MoveImageCommand
        {
            get
            {
                if (_MoveImageCommand == null) return _MoveImageCommand = new RelayCommand<object>(param => ExecuteImageMove((MouseEventArgs)param));
                return _MoveImageCommand;
            }
            set { _MoveImageCommand = value; }
        }

        private void ExecuteImageMove(MouseEventArgs e)
        {
            if (captured == true)
            {
                if (e.Source.GetType() == typeof(Path))
                {
                    ((EllipseGeometry)((Path)(e.Source)).Data).Center = e.GetPosition((IInputElement)e.Source);
                }
            }
        }

        private RelayCommand<object> _MouseDown;
        public RelayCommand<object> MouseDown
        {
            get
            {
                if (_MouseDown == null) return _MouseDown = new RelayCommand<object>(param => ExecuteMouseDown((MouseEventArgs)param));
                return _MouseDown;
            }
            set { _MouseDown = value; }
        }

        private void ExecuteMouseDown(MouseEventArgs e)
        {
            captured = true;
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _MouseUp;
        public RelayCommand<object> MouseUp
        {
            get
            {
                if (_MouseUp == null) return _MouseUp = new RelayCommand<object>(param => ExecuteMouseUp((MouseEventArgs)param));
                return _MouseUp;
            }
            set { _MouseUp = value; }
        }

        private void ExecuteMouseUp(MouseEventArgs e)
        {
            captured = false;
            Mouse.Capture(null);
        }
        #endregion

    }
}