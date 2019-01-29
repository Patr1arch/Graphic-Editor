using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace Baranov_sArtist.Model.DifferentFigures
{
    [Serializable]

    class Ellipse : Figure
    {
        public Ellipse() { }

        public Ellipse(Point aPoint)
        {
            coordinates = new List<Point> { aPoint, aPoint };
            Color = NotArtist.SelectedColor;
            ColorString = NotArtist.ColorStringNow;
            BrushColor = NotArtist.BrushNow;
            BrushColorString = NotArtist.BrushStringNow;
            PenThikness = NotArtist.ThicnessNow;
            Dash = NotArtist.DashNow;
            DashString = NotArtist.DashStringhNow;
            Pen = new Pen(Color, PenThikness) { DashStyle = Dash };
            Select = false;
            SelectRect = null;
            Type = "Ellipse";
        }

        public override void Draw(DrawingContext drawingContext)
        {
            RoundX = (coordinates[1].X - coordinates[0].X) / 2;
            RoundY = (coordinates[1].Y - coordinates[0].Y) / 2;
            var diagonal = Point.Subtract(coordinates[0], coordinates[1]);
            drawingContext.DrawRoundedRectangle(BrushColor, Pen, new Rect(coordinates[1], diagonal), RoundX, RoundY);

        }

        public override void ChangeCoord(Point aPoint) => coordinates[1] = aPoint;

        public override void Selected()
        {
            if (!Select)
            {
                Point pForRect3 = new Point();
                pForRect3.X = Math.Min(coordinates[0].X, coordinates[1].X);
                pForRect3.Y = Math.Min(coordinates[0].Y, coordinates[1].Y);
                Point pForRect4 = new Point();
                pForRect4.X = Math.Max(coordinates[0].X, coordinates[1].X);
                pForRect4.Y = Math.Max(coordinates[0].Y, coordinates[1].Y);
                SelectRect = new ZoomRectangle(new Point(pForRect3.X - 15, pForRect3.Y - 15), new Point(pForRect4.X + 15, pForRect4.Y + 15));
                var drawingVisual = new DrawingVisual();
                var drawingContext = drawingVisual.RenderOpen();
                SelectRect.Draw(drawingContext);
                drawingContext.Close();
                NotArtist.FigureHost.Children.Add(drawingVisual);
                Select = true;
            }
        }

        public override void UnSelected()
        {
            if (Select)
            {
                Select = false;
                SelectRect = null;
            }
        }

        public override void ChangePen(Brush color, string str)
        {
            Pen = new Pen(color, PenThikness) { DashStyle = Dash };
            Color = color;
            ColorString = str;
        }

        public override void ChangePen(Brush color, string str, bool check)
        {
            BrushColor = color;
            BrushColorString = str;
        }

        public override void ChangePen(DashStyle dash, string str)
        {
            Pen = new Pen(Color, PenThikness) { DashStyle = dash };
            Dash = dash;
            DashString = str;
        }

        public override void ChangePen(double thikness)
        {
            Pen = new Pen(Color, thikness) { DashStyle = Dash };
            PenThikness = thikness;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("coordinates", coordinates);
            info.AddValue("PenThikness", PenThikness);
            info.AddValue("Color", ColorString);
            info.AddValue("BrushColor", BrushColorString);
            info.AddValue("Dash", DashString);
            info.AddValue("Type", Type);
        }

        public Ellipse(SerializationInfo info, StreamingContext context)
        {
            coordinates = (List<Point>)info.GetValue("coordinates", typeof(List<Point>));
            PenThikness = (double)info.GetValue("PenThikness", typeof(double));
            ColorString = (string)info.GetValue("Color", typeof(string));
            BrushColorString = (string)info.GetValue("BrushColor", typeof(string));
            DashString = (string)info.GetValue("Dash", typeof(string));
            Type = (string)info.GetValue("Type", typeof(string));
            Color = NotArtist.TransformColor[ColorString];
            BrushColor = NotArtist.TransformColor[BrushColorString];
            Dash = NotArtist.TransformDashProp[DashString];
            Pen = new Pen(Color, PenThikness) { DashStyle = Dash };
        }

        public override Figure Clone()
        {
            return new Ellipse
            {
                BrushColor = this.BrushColor,
                BrushColorString = this.BrushColorString,
                Color = this.Color,
                ColorString = this.ColorString,
                coordinates = new List<Point>(coordinates),
                Dash = this.Dash,
                DashString = this.DashString,
                Pen = this.Pen,
                PenThikness = this.PenThikness,
                RoundX = this.RoundX,
                RoundY = this.RoundY,
                Select = this.Select,
                SelectRect = this.SelectRect,
                Type = this.Type
            };
        }

        public override string ConvertToSVG()
        {

            var size = Point.Subtract(coordinates[1], coordinates[0]);
            var point0 = Point.Subtract(coordinates[1], size / 2);
            var opacity = ((SolidColorBrush)BrushColor).Color.A / 255.0;
            var fill = ((SolidColorBrush)BrushColor).Color.ToString().Remove(1, 2);
            var stroke = ((SolidColorBrush)Pen.Brush).Color.ToString().Remove(1, 2);
            return "<ellipse cx=" + Math.Floor(point0.X).ToString() + " cy=" + Math.Floor(point0.Y).ToString() + " fill-opacity=" + opacity.ToString() + " rx=" + Math.Round(size.X / 2).ToString() + " ry=" + Math.Round(size.Y / 2).ToString() + " style=\"fill:" + fill + " ;stroke:" + stroke + " ;stroke-width:" + Math.Round(Pen.Thickness).ToString() + "\"/>";
        }
    }
}