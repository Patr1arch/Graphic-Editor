using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class Ellipse : Figure
    {
        public Ellipse(Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
            color = NotArtist.SelectedColor;
            brushColor = NotArtist.BrushNow;
        }

        public override void Draw(DrawingContext drawignContext)
        {
            drawignContext.DrawEllipse(brushColor, new Pen(color, 4), coordinates[0], (coordinates[1].X - coordinates[0].X), (coordinates[1].X - coordinates[0].X));
        }

        public override void ChangeCoord(Point point) => coordinates[1] = point;
    }
}
