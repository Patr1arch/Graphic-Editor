using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;
using System.Windows;
using Baranov_sArtist;

namespace BaranovsArtist.Model.Tools
{
    class HandForFigureTool : Tool
    {
        Point StartPoint;
        Point LastPoint;

        public override void MouseDown(Point aPoint)
        {
            StartPoint = aPoint;
        }

        public override void MouseMove(Point aPoint)
        {
            LastPoint = aPoint;
            List<Figure> figureNow = new List<Figure>();
            foreach (Figure figure in NotArtist.Figures)
            {
                figureNow.Add(figure.Clone());
            }
            NotArtist.Figures.Clear();
            foreach (Figure figure in figureNow)
            {
                if (figure.Select == true)
                {
                    for (var i = 0; i < figure.coordinates.Count; i++)
                    {
                        figure.coordinates[i] = Point.Add(figure.coordinates[i], Point.Subtract(LastPoint, StartPoint));
                    }

                    for (var i = 0; i < 2; i++)
                    {
                        figure.SelectRect.coordinates[i] = Point.Add(figure.SelectRect.coordinates[i], Point.Subtract(LastPoint, StartPoint));
                    }
                }
            }
            NotArtist.Figures = new List<Figure>();
            foreach (Figure figure in figureNow)
            {
                NotArtist.Figures.Add(figure.Clone());
            }
            StartPoint = LastPoint;
        }
    }
}
