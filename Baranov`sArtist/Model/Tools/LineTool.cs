using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Baranov_sArtist.Model.DifferentFigures;

namespace Baranov_sArtist.Model.Tools
{
    class LineTool : Tool
    {

        public override void MouseDown(Point aPoint) => NotArtist.Figures.Add(new Line(aPoint));

        public override void MouseMove(Point aPoint) => NotArtist.Figures[NotArtist.Figures.Count - 1].ChangeCoord(aPoint);

    }
}
