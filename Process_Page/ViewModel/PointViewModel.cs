using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Process_Page.ToothTemplate.Utils;

namespace Process_Page.ViewModel
{
    public class PointViewModel : ViewModelBase
    {
        public PointViewModel(double x, double y, int s)
        {
            this.x = x;
            this.y = y;
            this.i = i;
        }

        #region X

        private double x;
        public double X
        {
            get { return x; }
            set
            {
                if (Numerics.DoubleEquals(x, value)) return;
                x = value;
                RaisePropertyChanged("X");
            }
        }

        #endregion

        #region Y

        private double y;
        public double Y
        {
            get { return y; }
            set
            {
                if (Numerics.DoubleEquals(y, value)) return;
                y = value;
                RaisePropertyChanged("Y");
            }
        }

        #endregion

        private int i;
        public int I
        {
            get { return i; }
            set { i = value; }
        }

    }


}
