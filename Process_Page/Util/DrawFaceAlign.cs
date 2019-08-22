using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using W_Point = System.Windows.Point;

namespace Process_Page.Util
{
    class DrawFaceAlign
    {
        //얼굴좌표 정렬
        FaceDetector.face_point face_landmark;
        double img_height;
        double img_width;

        private ToothTemplate_Step mainwnd;

        public DrawFaceAlign()
        {
            face_landmark = new FaceDetector.face_point();
            mainwnd = System.Windows.Application.Current.MainWindow as ToothTemplate_Step;
        }

        public DrawFaceAlign(FaceDetector.face_point face_Point, double height, double width)
        {
            img_height = height;
            img_width = width;
            face_landmark = face_Point;
            Init_faceAlign(face_landmark);
        }

        public LineGeometry midline, noseline_L, noseline_R, eyeline, lipline;
        public EllipseGeometry eye_L, eye_R, mouth_L, mouth_R;

        public void Init_faceAlign(FaceDetector.face_point fp)
        {
            //face_landmark
            //  - eye   : 5~6
            //  - mouth : 7~8
            eye_L = new EllipseGeometry();
            eye_L.Center = OpenCVPoint2W_Point(face_landmark.eye[0]);
            eye_L.RadiusX = 3;
            eye_L.RadiusY = 3;

            eye_R = new EllipseGeometry();
            eye_R.Center = OpenCVPoint2W_Point(face_landmark.eye[1]);
            eye_R.RadiusX = 3;
            eye_R.RadiusY = 3;

            mouth_L = new EllipseGeometry();
            mouth_L.Center = OpenCVPoint2W_Point(face_landmark.mouse[0]);
            mouth_L.RadiusX = 3;
            mouth_L.RadiusY = 3;

            mouth_R = new EllipseGeometry();
            mouth_R.Center = OpenCVPoint2W_Point(face_landmark.mouse[1]);
            mouth_R.RadiusX = 3;
            mouth_R.RadiusY = 3;

            // midline 연장하기
            midline = new LineGeometry();

            W_Point midpoint = new W_Point();
            midpoint.X = (eye_L.Center.X + eye_R.Center.X) / 2;
            midpoint.Y = 0;

            midline.StartPoint = midpoint;
            midline.EndPoint = new W_Point(midpoint.X, img_height);

            // noseline_L 연장하기
            noseline_L = new LineGeometry();

            W_Point LeyelinePoint = new W_Point();
            LeyelinePoint.X = eye_L.Center.X + ((midpoint.X - eye_L.Center.X) / 2);
            LeyelinePoint.Y = 0;

            noseline_L.StartPoint = LeyelinePoint;
            noseline_L.EndPoint = new W_Point(LeyelinePoint.X, img_height);

            // noseline_R 연장하기
            noseline_R = new LineGeometry();

            W_Point ReyelinePoint = new W_Point();
            ReyelinePoint.X = eye_R.Center.X - ((eye_R.Center.X - midpoint.X) / 2);
            ReyelinePoint.Y = 0;

            noseline_R.StartPoint = ReyelinePoint;
            noseline_R.EndPoint = new W_Point(ReyelinePoint.X, img_height);

            // eyeline
            eyeline = new LineGeometry();
            eyeline.StartPoint = new System.Windows.Point(face_landmark.eye[0].X, face_landmark.eye[0].Y);
            eyeline.EndPoint = new System.Windows.Point(face_landmark.eye[1].X, face_landmark.eye[0].Y);

            // lipline
            lipline = new LineGeometry();
            lipline.StartPoint = new System.Windows.Point(face_landmark.mouse[0].X, face_landmark.mouse[0].Y);
            lipline.EndPoint = new System.Windows.Point(face_landmark.mouse[1].X, face_landmark.mouse[0].Y);
        }

        // 기울기 함수
        public W_Point slope(double x1, double y1, double x2, double y2, double height = 0)
        {
            double slope = (x2 - x1) / (y2 - y1);
            double X = slope * (height - y1) + x1;
            double Y = height;

            W_Point pt = new W_Point();
            pt.X = X;
            pt.Y = Y;

            return pt;
        }

        //opencv_point -> W_Point로 바꾸기
        private W_Point OpenCVPoint2W_Point(OpenCvSharp.Point pt)
        {
            W_Point result = new W_Point();
            result = new W_Point(pt.X, pt.Y);

            return result;
        }
    }
}
