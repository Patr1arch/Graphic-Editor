using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;
using System.Windows;

namespace Baranov_sArtist.Model.Tools
{
    class RoundRectangleTool : Tool
    {
        public override void MouseDown(Point aPoint) => NotArtist.Figures.Add(new RoundRectangle(aPoint));

        public override void MouseMove(Point aPoint) => NotArtist.Figures[NotArtist.Figures.Count - 1].ChangeCoord(aPoint);
    }
}
