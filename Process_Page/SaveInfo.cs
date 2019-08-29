using Process_Page.Util;
using Process_Page.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Process_Page
{
    using ToothList = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;

    public class SaveInfo
    {
        /*==========================================================
         * 두 페이지는 ToothTemplate 부분 이 외의 모든 부분을 공유
         ===========================================================*/

        #region Face Align Page Info
        // face detect 초기점
        public FaceDetector.face_point GagFacePoints = new FaceDetector.face_point();
        public FaceDetector.face_point FrontalFacePoints = new FaceDetector.face_point();

        // 보정 후 점
        public ObservableCollection<Point> _FrontalPoints;
        public ObservableCollection<Point> _GagPoints;
        public ObservableCollection<PointViewModel> _FrontalMouthPoints;

        //image original
        public BitmapImage FrontalFaceImage;
        public BitmapImage GagFaceImage;

        // face line
        public LineGeometry _midline = new LineGeometry();
        public LineGeometry _noseline_L = new LineGeometry();
        public LineGeometry _noseline_R = new LineGeometry();
        public LineGeometry _eyeline = new LineGeometry();
        public LineGeometry _lipline = new LineGeometry();

        // align reference teeth points
        public EllipseGeometry _teethL = new EllipseGeometry();
        public EllipseGeometry _teethR = new EllipseGeometry();
        public EllipseGeometry _FrontalteethL = new EllipseGeometry();
        public EllipseGeometry _FrontalteethR = new EllipseGeometry();

        // scale wheelmouse
        private double _ViewScale;
        private Point _WheelMouseCenter;

        #region Align Property
        // Frontal Face Canvas Center
        private Point _FrontalCenter;
        private double _FrontalAngle;
        private double _FrontalScale;

        // Gag Image
        private Point _GagCenter;
        private double _GagAngle;
        private double _GagScale;

        // Transform Center
        private Point _TransCenter;
        private Point _TransGagCenter;

        // Rotate Control Center
        private Point _RotateControlCenter;
        #endregion

        #endregion

        #region Smile design page info
        // Tooth Control Position
        private Point _ToothUpperCenter;
        private Point _ToothLowerCenter;

        // Tooth Templates
        public ToothList UpperTooth, LowerTooth;
        #endregion
    }
}
