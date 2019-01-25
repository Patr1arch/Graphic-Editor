using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class ZoomRectangle : Figure
    {
        public ZoomRectangle(Point aPoint, Point aPoint_)
        {
            coordinates = new List<Point> { aPoint, aPoint_ };
        }

        public ZoomRectangle(Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
        }

        public override void Draw(DrawingContext drawingContext)
        {
            var diagonal = Point.Subtract(coordinates[0], coordinates[1]);
            drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 2) { DashStyle = DashStyles.Dash }, new Rect(coordinates[1], diagonal));
        }

        public override void ChangeCoord(Point aPoint) => coordinates[1] = aPoint;
    }
}
