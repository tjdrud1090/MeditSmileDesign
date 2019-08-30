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
using Wpoint = System.Windows.Point;
namespace Process_Page
{
    using ToothList = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;
    [System.Xml.Serialization.XmlInclude(typeof(Wpoint))]
    [System.Xml.Serialization.XmlInclude(typeof(PointViewModel))]
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
        //public ObservableCollection<Wpoint> _FrontalPoints;
        //public ObservableCollection<Wpoint> _GagPoints;
        //public ObservableCollection<PointViewModel> _FrontalMouthPoints;

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
        public double _ViewScale;
        public Wpoint _WheelMouseCenter;

        #region Align Property
        // Frontal Face Canvas Center
        public Wpoint _FrontalCenter;
        public double _FrontalAngle;
        public double _FrontalScale;

        // Gag Image
        public Wpoint _GagCenter;
        public double _GagAngle;
        public double _GagScale;

        // Transform Center
        public Wpoint _TransCenter;
        public Wpoint _TransGagCenter;

        // Rotate Control Center
        public Wpoint _RotateControlCenter;
        #endregion

        #endregion

        #region Smile design page info
        // Tooth Control Position
        public Wpoint _ToothUpperCenter;
        public Wpoint _ToothLowerCenter;

        // Tooth Templates
        public ToothList UpperTooth, LowerTooth;
        #endregion

        public SaveInfo()
        {

        }

    }
}
