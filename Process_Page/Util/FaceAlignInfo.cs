using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Process_Page.Util
{
    public class FaceAlignInfo
    {
        // 필요 넘김 정보들

        // 1) Face Points 보정 후의 정보들
        public ObservableCollection<Point> FrontalPoints;
        public ObservableCollection<Point> GagPoints;

        // 2) image original
        public BitmapImage FrontalFaceImage;
        public BitmapImage GagFaceImage;

        // 3) line Geometry
        LineGeometry _midline = new LineGeometry();
        LineGeometry _noseline_L = new LineGeometry();
        LineGeometry _noseline_R = new LineGeometry();

        // 4) image layer 정보들
        public Point FrontalCenter;
        public double FrontalAngle;
        public double FrontalScale;

        public Point GagCenter;
        public double GagAngle;
        public double GagScale;

        // 5) mouse event 정보들
    }
}
