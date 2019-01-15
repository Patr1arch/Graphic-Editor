using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baranov_sArtist.Model.DifferentFigures
{
    class Figure
    {
        public List<Point> coordinates;

        protected SolidColorBrush color;

        protected SolidColorBrush brushColor;

        //protected List<Point> coordinates;

        public Figure()
        {

        }

        public Figure(Point aPoint)
        {
            coordinates.Add(aPoint);
        }

        public virtual void Draw(DrawingContext graphics)
        {

        }

        public virtual void ChangeCoord(Point aPoint)
        {
            coordinates.Add(aPoint);
        }
    }
}
