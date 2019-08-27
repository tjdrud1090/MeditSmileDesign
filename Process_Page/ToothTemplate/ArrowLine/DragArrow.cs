using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Process_Page.ToothTemplate.ArrowLine
{
    public class DragArrow : ArrowLineBase
    {
        #region TopX

        public static readonly DependencyProperty TopXProperty =
            DependencyProperty.Register("TopX", typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double TopX
        {
            set { SetValue(TopXProperty, value); }
            get { return (double)GetValue(TopXProperty); }
        }


        #endregion

        #region TopY

        public static readonly DependencyProperty TopYProperty =
            DependencyProperty.Register("TopY",
                typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double TopY
        {
            set { SetValue(TopYProperty, value); }
            get { return (double)GetValue(TopYProperty); }
        }

        #endregion

        #region BottomX

        public static readonly DependencyProperty BottomXProperty =
            DependencyProperty.Register("BottomX",
                typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double BottomX
        {
            set { SetValue(BottomXProperty, value); }
            get { return (double)GetValue(BottomXProperty); }
        }

        #endregion

        #region BottomY

        public static readonly DependencyProperty BottomYProperty =
            DependencyProperty.Register("BottomY",
                typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double BottomY
        {
            set { SetValue(BottomYProperty, value); }
            get { return (double)GetValue(BottomYProperty); }
        }

        #endregion

        #region LeftX

        public static readonly DependencyProperty LeftXProperty =
            DependencyProperty.Register("LeftX",
                typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double LeftX
        {
            set { SetValue(LeftXProperty, value); }
            get { return (double)GetValue(LeftXProperty); }
        }

        #endregion

        #region LeftY

        public static readonly DependencyProperty LeftYProperty =
            DependencyProperty.Register("LeftY",
                typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double LeftY
        {
            set { SetValue(LeftYProperty, value); }
            get { return (double)GetValue(LeftYProperty); }
        }

        #endregion

        #region RightX

        public static readonly DependencyProperty RightXProperty =
            DependencyProperty.Register("RightX",
                typeof(double), typeof(DragArrow),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double RightX
        {
            set { SetValue(RightXProperty, value); }
            get { return (double)GetValue(RightXProperty); }
        }

        #endregion

        #region RightY

        public static readonly DependencyProperty RightYProperty =
           DependencyProperty.Register("RightY",
               typeof(double), typeof(DragArrow),
               new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double RightY
        {
            set { SetValue(RightYProperty, value); }
            get { return (double)GetValue(RightYProperty); }
        }

        #endregion


        public DragArrow()
        {
            ArrowLine VerticalArrow = new ArrowLine();
            VerticalArrow.X1 = TopX;
            VerticalArrow.Y1 = TopY;
            VerticalArrow.X2 = BottomX;
            VerticalArrow.Y2 = BottomY;

            VerticalArrow.ArrowEnds = ArrowEnds;
            VerticalArrow.ArrowLength = ArrowLength;
            VerticalArrow.IsArrowClosed = IsArrowClosed;
            VerticalArrow.ArrowAngle = ArrowAngle;
            VerticalArrow.Visibility = Visibility;

            ArrowLine HorizontalArrow = new ArrowLine();
            HorizontalArrow.X1 = LeftX;
            HorizontalArrow.Y1 = LeftY;
            HorizontalArrow.X2 = RightX;
            HorizontalArrow.Y2 = RightY;

            HorizontalArrow.ArrowEnds = ArrowEnds;
            HorizontalArrow.ArrowLength = ArrowLength;
            HorizontalArrow.IsArrowClosed = IsArrowClosed;
            HorizontalArrow.ArrowAngle = ArrowAngle;
            HorizontalArrow.Visibility = Visibility;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Clear out the PathGeometry.
                pathgeo.Figures.Clear();

                // Define a single PathFigure with the points.
                pathfigLine.StartPoint = new Point(TopX, TopY);
                polysegLine.Points.Clear();
                polysegLine.Points.Add(new Point(BottomX, BottomY));
                pathgeo.Figures.Add(pathfigLine);

                // Call the base property to add arrows on the ends.
                return base.DefiningGeometry;
            }
        }
    }
}
