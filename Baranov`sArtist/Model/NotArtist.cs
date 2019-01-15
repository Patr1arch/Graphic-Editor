using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Baranov_sArtist.Model.Tools;
using Baranov_sArtist.Model;
using Baranov_sArtist.Model.DifferentFigures;

namespace Baranov_sArtist.Model
{
    class NotArtist
    {
        public static List<Figure> Figures = new List<Figure>();

        public static FigureHost FigureHost = new FigureHost();

        public static SolidColorBrush BrushNow = null;

        public static SolidColorBrush SelectedColor = Brushes.Black;

        public static double ScaleRateX = 1;

        public static double ScaleRateY = 1;

        public static double DistanceToPointX;

        public static double DistanceToPointY;

        public static double HandScrollX;

        public static double HandScrollY;

        public static bool FirstPress = true;

        public static bool SecondPress = false;

        public static double CanvasWidth;

        public static double CanvasHeigth;

        public static Tool SelectedTool = new PencilTool();

        public static readonly Dictionary<string, Tool> ToolsList = new Dictionary<string, Tool>()
        {
            { "Line", new LineTool() },
            { "Rectangle", new RectangleTool() },
            { "Ellipse", new EllipseTool() },
            { "RoundRectangle", new RoundRectangleTool() },
            { "Pencil", new PencilTool() },
            { "Hand", new HandTool() },
            { "ZoomTool", new ZoomTool() },
        };
        public static readonly Dictionary<string, SolidColorBrush> TransformColor = new Dictionary<string, SolidColorBrush>()
        {
            { "Black", Brushes.Black },
            { "Red", Brushes.Red },
            { "Gray", Brushes.Gray },
            { "Orange", Brushes.Orange },
            { "Yellow", Brushes.Yellow },
            { "Blue", Brushes.Blue },
            { "Purple", Brushes.Purple },
            { "Coral", Brushes.Coral },
            { "White", Brushes.White },
        };
    }
}
