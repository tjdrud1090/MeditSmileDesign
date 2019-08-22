using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DlibDotNet;
using faceDetect = DlibDotNet.FrontalFaceDetector;
using OpenCvSharp;
using DlibDotNet.Extensions;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Process_Page.Util
{
    public class FaceDetector
    {
        public class face_point
        {
            public List<OpenCvSharp.Point> eye;
            public List<OpenCvSharp.Point> mouse;
            public List<OpenCvSharp.Point> midline;
            public List<OpenCvSharp.Point> nose;

            public int element_capacity = 2;

            public face_point()
            {
                eye = new List<OpenCvSharp.Point>();
                mouse = new List<OpenCvSharp.Point>();
                midline = new List<OpenCvSharp.Point>();
                nose = new List<OpenCvSharp.Point>();
            }
        }

        public face_point fp;
        public BitmapImage face;

        public FaceDetector()
        {
            fp = new face_point();
        }

        public FaceDetector(string filename)
        {
            fp = new face_point();
            face = faceDetect(filename);
        }

        Mat original_img;
        public BitmapImage faceDetect(string filename)
        {
            // file paths
            faceDetect fd = Dlib.GetFrontalFaceDetector();
            var sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat");

            // the rest of the code goes here....

            // load input image
            var input = new Mat();
            input = Cv2.ImRead(filename, ImreadModes.AnyColor);
            original_img = input;
            var image = input;

            var img = Mat2array2D(image);

            // find all faces in the image
            var faces = fd.Operator(img);
            foreach (var face in faces)
            {
                // find the landmark points for this face
                var shape = sp.Detect(img, face);

                // draw the landmark points on the image
                //입 끝 좌표받아오기
                OpenCvSharp.Point Mpoint1;
                var pointm1 = shape.GetPart((uint)48);
                Mpoint1.X = pointm1.X;
                Mpoint1.Y = pointm1.Y;
                fp.mouse.Add(Mpoint1);

                OpenCvSharp.Point Mpoint2;
                var pointm2 = shape.GetPart((uint)54);
                Mpoint2.X = pointm2.X;
                Mpoint2.Y = pointm2.Y;
                fp.mouse.Add(Mpoint2);

                //눈 좌표 받아오기(눈중앙점)
                OpenCvSharp.Point cpointe1;
                var pointe1 = shape.GetPart((uint)36);
                var pointe2 = shape.GetPart((uint)39);
                cpointe1.X = (pointe1.X + pointe2.X) / 2;
                cpointe1.Y = (pointe1.Y + pointe2.Y) / 2;
                fp.eye.Add(cpointe1);

                OpenCvSharp.Point cpointe2;
                var pointe3 = shape.GetPart((uint)42);
                var pointe4 = shape.GetPart((uint)45);
                cpointe2.X = (pointe3.X + pointe4.X) / 2;
                cpointe2.Y = (pointe3.Y + pointe4.Y) / 2;
                fp.eye.Add(cpointe2);

                //눈 끝 좌표 구하기
                OpenCvSharp.Point Npointe1;
                var pointn1 = shape.GetPart((uint)39);
                Npointe1.X = pointn1.X;
                Npointe1.Y = pointn1.Y;
                fp.nose.Add(Npointe1);

                OpenCvSharp.Point Npointe2;
                var pointn2 = shape.GetPart((uint)42);
                Npointe2.X = pointn2.X;
                Npointe2.Y = pointn2.Y;
                fp.nose.Add(Npointe2);

                //눈간 사이, 입 중앙
                OpenCvSharp.Point Mcpointe;
                Mcpointe.X = (Mpoint1.X + Mpoint2.X) / 2;
                Mcpointe.Y = (Mpoint1.Y + Mpoint2.Y) / 2;
                fp.midline.Add(Mcpointe);

                OpenCvSharp.Point Ecpointe;
                Ecpointe.X = (cpointe1.X + cpointe2.X) / 2;
                Ecpointe.Y = (cpointe1.Y + cpointe2.Y) / 2;
                fp.midline.Add(Ecpointe);
            }

            //image 반환
            BitmapImage control = Mat2Bmp(image);
            control.DecodePixelHeight = 1100;

            return control;
        }

        //image mat -> array2d
        public Array2D<RgbPixel> Mat2array2D(Mat mat_image)
        {
            //image mat -> array2d

            //이미지 바이트로 저장
            var array = new byte[mat_image.Width * mat_image.Height * mat_image.ElemSize()];
            //데이터 형식 복사(array2D)
            Marshal.Copy(mat_image.Data, array, 0, array.Length);

            //array2D 전환
            var array2D = Dlib.LoadImageData<RgbPixel>(array, (uint)mat_image.Height,
                (uint)mat_image.Width, (uint)(mat_image.Width * mat_image.ElemSize()));

            //array2d로 반환
            return array2D;
        }

        //image mat -> bmpimage
        public BitmapImage Mat2Bmp(Mat mat_image)
        {
            //image 변환 -> bitmapimage
            Bitmap showImg = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat_image);

            MemoryStream ms = new MemoryStream();
            showImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = ms;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();

            //image control에 등록
            return bmp;
        }
    }
}
