using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Baranov_sArtist.Model;
using Figure = Baranov_sArtist.Model.DifferentFigures.Figure;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace Baranov_sArtist
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        List<Figure> BufList = new List<Figure>();

        bool ClickOnCanvas = false;

        int saveConditionNumber = -1;

        bool isSave = false;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MyCanvas.Children.Add(NotArtist.FigureHost);
            ButtonsGenerations.Generation();
            NotArtist.AddCondition();
        }

        private void Invalidate()
        {
            NotArtist.FigureHost.Children.Clear();
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            foreach (var figure in NotArtist.Figures)
            {
                figure.Draw(drawingContext);
                if (figure.SelectRect != null)
                {
                    figure.SelectRect.Draw(drawingContext);
                }
            }
            drawingContext.Close();
            NotArtist.FigureHost.Children.Add(drawingVisual);
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                NotArtist.SelectedTool.MouseDown(e.GetPosition(MyCanvas));
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                NotArtist.tempBrush = NotArtist.BrushNow;
                NotArtist.BrushNow = NotArtist.SelectedColor;
                NotArtist.SelectedColor = NotArtist.tempBrush;
                NotArtist.tempStringBrush = NotArtist.BrushStringNow;
                NotArtist.BrushStringNow = NotArtist.ColorStringNow;
                NotArtist.ColorStringNow = NotArtist.tempStringBrush;
                NotArtist.SelectedTool.MouseDown(e.GetPosition(MyCanvas));
                NotArtist.tempBrush = NotArtist.BrushNow;
                NotArtist.BrushNow = NotArtist.SelectedColor;
                NotArtist.SelectedColor = NotArtist.tempBrush;
                NotArtist.tempStringBrush = NotArtist.BrushStringNow;
                NotArtist.BrushStringNow = NotArtist.ColorStringNow;
                NotArtist.ColorStringNow = NotArtist.tempStringBrush;
            }
            ClickOnCanvas = true;
            Invalidate();
        }

        private void MyCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ClickOnCanvas)
            {
                NotArtist.SelectedTool.MouseMove(e.GetPosition(MyCanvas));
                if (NotArtist.SelectedTool == NotArtist.ToolsList["Hand"])
                {
                    ScrollViewerCanvas.ScrollToVerticalOffset(NotArtist.HandScrollY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(NotArtist.HandScrollX);
                }
                Invalidate();
            }
        }
        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickOnCanvas)
            {
                NotArtist.SelectedTool.MouseUp(e.GetPosition(MyCanvas));

                if (NotArtist.SelectedTool != NotArtist.ToolsList["Allotment"] & NotArtist.SelectedTool != NotArtist.ToolsList["Zoom"] & NotArtist.SelectedTool != NotArtist.ToolsList["Hand"])
                {
                    NotArtist.AddCondition();
                    gotoPastCondition.IsEnabled = true;
                    gotoSecondCondition.IsEnabled = false;
                    SaveButton.Content = "Save*";
                }
                if (NotArtist.SelectedTool == NotArtist.ToolsList["Zoom"])
                {
                    MyCanvas.LayoutTransform = new ScaleTransform(NotArtist.ScaleRateX, NotArtist.ScaleRateY);
                    ScrollViewerCanvas.ScrollToVerticalOffset(NotArtist.DistanceToPointY * NotArtist.ScaleRateY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(NotArtist.DistanceToPointX * NotArtist.ScaleRateX);
                }
                if (NotArtist.SelectedTool == NotArtist.HandTool)
                {
                    NotArtist.SelectedTool = NotArtist.ToolsList["Allotment"];
                }
                ClickOnCanvas = false;
                Invalidate();
            }
        }

        private void MyCanvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ClickOnCanvas)
            {
                NotArtist.SelectedTool.MouseUp(e.GetPosition(MyCanvas));

                if (NotArtist.SelectedTool != NotArtist.ToolsList["Allotment"] & NotArtist.SelectedTool != NotArtist.ToolsList["Zoom"] & NotArtist.SelectedTool != NotArtist.ToolsList["Hand"] & NotArtist.SelectedTool != NotArtist.HandTool)
                {
                    NotArtist.AddCondition();
                    gotoPastCondition.IsEnabled = true;
                    gotoSecondCondition.IsEnabled = false;
                    SaveButton.Content = "Save*";
                }
                if (NotArtist.SelectedTool == NotArtist.ToolsList["Zoom"])
                {
                    MyCanvas.LayoutTransform = new ScaleTransform(NotArtist.ScaleRateX, NotArtist.ScaleRateY);
                    ScrollViewerCanvas.ScrollToVerticalOffset(NotArtist.DistanceToPointY * NotArtist.ScaleRateY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(NotArtist.DistanceToPointX * NotArtist.ScaleRateX);
                }
                ClickOnCanvas = false;
                Invalidate();
            }
        }

        public void ButtonChangeTool(object sender, RoutedEventArgs e)
        {
            NotArtist.SelectedTool = NotArtist.ToolsList[(sender as Button).Name.ToString()];
            if ((sender as Button).Name.ToString() == "RoundRectangle")
            {
                textBoxRoundRectX.IsEnabled = true;
                textBoxRoundRectY.IsEnabled = true;
            }
            else
            {
                textBoxRoundRectX.IsEnabled = false;
                textBoxRoundRectY.IsEnabled = false;
            }
            foreach (Figure figure in NotArtist.Figures)
            {
                figure.UnSelected();
            }
            Invalidate();
            PropToolBarPanel.Children.Clear();
        }

        public void ButtonChangeColor(object sender, RoutedEventArgs e)
        {
            if (NotArtist.FirstPress == true)
            {
                NotArtist.SelectedColor = NotArtist.TransformColor[(sender as Button).Name.ToString()];
                NotArtist.ColorStringNow = (sender as Button).Name.ToString();
                //if ((sender as Button).Background == null) { button_firstColor.Background = Brushes.Gray; }
                //else { button_firstColor.Background = (sender as Button).Background; }

            }
            else
            {
                NotArtist.BrushNow = NotArtist.TransformColor[(sender as Button).Name.ToString()];
                NotArtist.BrushStringNow = (sender as Button).Name.ToString();
                //if ((sender as Button).Background == null) { button_secondColor.Background = Brushes.Gray; }
                //else { button_secondColor.Background = (sender as Button).Background; }
            }
        }

        private void ThiknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NotArtist.ThicnessNow = ThiknessSlider.Value;
        }

        private void FirstColor(object sender, RoutedEventArgs e)
        {
            NotArtist.FirstPress = true;
            NotArtist.SecondPress = false;
            button_firstColor.BorderThickness = new Thickness(5);
            button_secondColor.BorderThickness = new Thickness(0);
        }

        private void SecondColor(object sender, RoutedEventArgs e)
        {
            NotArtist.FirstPress = false;
            NotArtist.SecondPress = true;
            button_secondColor.BorderThickness = new Thickness(5);
            button_firstColor.BorderThickness = new Thickness(0);
        }

        private void MyCanvasLoaded(object sender, RoutedEventArgs e)
        {
            NotArtist.CanvasHeigth = MyCanvas.Height;
            NotArtist.CanvasWidth = MyCanvas.Width;
        }

        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotArtist.CanvasHeigth = MyCanvas.Height;
            NotArtist.CanvasWidth = MyCanvas.Width;
        }

        public void CleanMyCanvas(object sender, RoutedEventArgs e)
        {
            NotArtist.FigureHost.Children.Clear();
            NotArtist.Figures.Clear();
            NotArtist.ConditionNumber = 0;
            NotArtist.ConditionsCanvas.Clear();
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = false;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save";
        }

        public void MinusZoomMyCanvas(object sender, RoutedEventArgs e)
        {
            MyCanvas.LayoutTransform = new ScaleTransform(1, 1);
            ScrollViewerCanvas.ScrollToVerticalOffset(0);
            ScrollViewerCanvas.ScrollToHorizontalOffset(0);
        }


        private void ChangeSelectionDash(object sender, SelectionChangedEventArgs e)
        {
            NotArtist.DashNow = NotArtist.TransformDash[comboBoxDash.SelectedIndex.ToString()];
            if (comboBoxDash.SelectedIndex.ToString() == "0")
            {
                NotArtist.DashStringhNow = "―――――";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "1")
            {
                NotArtist.DashStringhNow = "— — — — — —";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "2")
            {
                NotArtist.DashStringhNow = "— ∙ — ∙ — ∙ — ∙ —";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "3")
            {
                NotArtist.DashStringhNow = "— ∙ ∙ — ∙ ∙ — ∙ ∙ — ";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "4")
            {
                NotArtist.DashStringhNow = "∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙";
            }
        }

        private void textBoxRoundRectX_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotArtist.RoundXNow = Convert.ToDouble(textBoxRoundRectX.Text);
        }

        private void textBoxRoundRectY_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotArtist.RoundYNow = Convert.ToDouble(textBoxRoundRectY.Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotArtist.Figures.Count != 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Сохранить как";
                sfd.OverwritePrompt = true;
                sfd.CheckPathExists = true;
                sfd.Filter = "BIN Files (*.bin)|*.bin|Image (.PNG)|*.PNG|SVG Files|*.html"; //Files(*.bin)|*.bin
                sfd.ShowDialog();
                if (sfd.FileName != "")
                {
                    Regex regex = new Regex(@"\w*.bin$");
                    MatchCollection matches = regex.Matches(sfd.FileName);
                    if (matches.Count > 0)
                    {
                        FileStream file = (FileStream)sfd.OpenFile();
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(file, NotArtist.Figures);
                        file.Close();
                    }
                    else 
                    {
                        regex = new Regex(@"\w*.html$");
                        matches = regex.Matches(sfd.FileName);
                        if (matches.Count > 0)
                        {
                            ToSVGSource(MyCanvas, sfd.FileName);
                        }
                        else
                        {
                            ToImageSource(MyCanvas, sfd.FileName);
                        }
                    }

                }
                isSave = true;
                saveConditionNumber = NotArtist.ConditionNumber;
                SaveButton.Content = "Save";
            }
            else
            {
                MessageBox.Show("Нарисуйте что-нибудь...");
            }
            PropToolBarPanel.Children.Clear();
        }

        public static void ToSVGSource(Canvas canvas, string filename)
        {
            var svg = "<svg width=" + canvas.Width.ToString() + " height=" + canvas.Height.ToString() + ">\n" + Environment.NewLine;
            foreach (Figure figure in NotArtist.Figures)
            {
                svg += " " + figure.ConvertToSVG() + Environment.NewLine;
            }
            svg += "</svg>";
            File.WriteAllText(filename, svg);
        }

        public static void ToImageSource(Canvas canvas, string filename)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(
            (int)canvas.Width, (int)canvas.Height, 96d, 96d, PixelFormats.Pbgra32);
            canvas.Measure(new Size((int)canvas.Width, (int)canvas.Height));
            canvas.Arrange(new Rect(new Size((int)canvas.Width, (int)canvas.Height)));
            bmp.Render(canvas);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

            NotArtist.Figures.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Files(*.bin)|*.bin";
            ofd.Title = "Открыть";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                Stream file = (FileStream)ofd.OpenFile();
                BinaryFormatter deserializer = new BinaryFormatter();
                NotArtist.Figures = (List<Figure>)deserializer.Deserialize(file);
                file.Close();
                Invalidate();
            }
            NotArtist.ConditionsCanvas.Clear();
            NotArtist.ConditionNumber = 0;
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = false;
            gotoSecondCondition.IsEnabled = false;
            saveConditionNumber = NotArtist.ConditionNumber;
            isSave = true;
            SaveButton.Content = "Save";
            PropToolBarPanel.Children.Clear();
        }

        private void gotoPastCondition_Click(object sender, RoutedEventArgs e)
        {
            NotArtist.gotoPastCondition();
            if (NotArtist.ConditionNumber == 1)
            {
                gotoPastCondition.IsEnabled = false;
            }
            gotoSecondCondition.IsEnabled = true;
            Invalidate();
            if (isSave && (saveConditionNumber == NotArtist.ConditionNumber) || (NotArtist.Figures.Count == 0))
            {
                SaveButton.Content = "Save";
            }
            else
            {
                SaveButton.Content = "Save*";
            }
        }

        private void gotoSecondCondition_Click(object sender, RoutedEventArgs e)
        {
            NotArtist.gotoSecondCondition();
            if (NotArtist.ConditionNumber == NotArtist.ConditionsCanvas.Count)
            {
                gotoSecondCondition.IsEnabled = false;
            }
            gotoPastCondition.IsEnabled = true;
            Invalidate();
            if (isSave && (saveConditionNumber == NotArtist.ConditionNumber))
            {
                SaveButton.Content = "Save";
            }
            else
            {
                SaveButton.Content = "Save*";
            }
        }

        public void changeRoundX(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundX(e.NewValue);
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void changeRoundY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select)
                {
                    figure.ChangeRoundY(e.NewValue);
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void ChangeStrokeColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select)
                {
                    figure.ChangePen(NotArtist.TransformColor[(sender as Button).Tag.ToString()], (sender as System.Windows.Controls.Button).Tag.ToString());
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void ChangeBrushColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select)
                {
                    figure.ChangePen(NotArtist.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()], (sender as System.Windows.Controls.Button).Tag.ToString(), new bool());
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void ChangeDash(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select)
                {
                    figure.ChangePen(NotArtist.TransformDashProp[(sender as Button).Content.ToString()], (sender as System.Windows.Controls.Button).Content.ToString());
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void ClearSelectedFigure(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures.ToArray())
            {
                if (figure.Select)
                {
                    NotArtist.Figures.Remove(figure);
                }
            }
            PropToolBarPanel.Children.Clear();
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void RowThicnessChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select)
                {
                    figure.ChangePen(e.NewValue);
                }
            }
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public void HandForSelectedFigure(object sender, RoutedEventArgs e)
        {
            NotArtist.SelectedTool = NotArtist.HandTool;
        }

        public void SldMouseUp(object sender, MouseButtonEventArgs e)
        {
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
        }


        public void IncreaseZIndex(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (Figure figure in NotArtist.Figures.ToArray())
            {
                if ((!figure.Equals(NotArtist.Figures[NotArtist.Figures.Count - 1])) && (figure.Select))
                {
                    var buf = NotArtist.Figures[i];
                    NotArtist.Figures[i] = NotArtist.Figures[i + 1];
                    NotArtist.Figures[i + 1] = buf;
                    //NotArtist.Figures.Remove(figure);
                    //NotArtist.Figures.Add(figure);
                    Invalidate();
                }
                i++;
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }


        public void DecZIndex(object sender, RoutedEventArgs e)
        {
            var i = 0;
            foreach (Figure figure in NotArtist.Figures.ToArray())
            {
                if ((!figure.Equals(NotArtist.Figures[0])) && (figure.Select))
                {
                    var buf = NotArtist.Figures[i];
                    NotArtist.Figures[i] = NotArtist.Figures[i - 1];
                    NotArtist.Figures[i - 1] = buf;
                }
                i++;
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            SaveButton.Content = "Save*";
            Invalidate();
        }

        public static ImageSource ToImageSource(FrameworkElement obj)
        {
            // Save current canvas transform
            Transform transform = obj.LayoutTransform;
            obj.LayoutTransform = null;
            
            // fix margin offset as well
            Thickness margin = obj.Margin;
            obj.Margin = new Thickness(0, 0,
                 margin.Right - margin.Left, margin.Bottom - margin.Top);
 
            // Get the size of canvas
            Size size = new Size(obj.Width, obj.Height);
            
            // force control to Update
            obj.Measure(size);
            obj.Arrange(new Rect(size));
 
            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)obj.Width, (int)obj.Height, 96, 96, PixelFormats.Pbgra32);
            
            bmp.Render(obj);
 
            // return values as they were before
            obj.LayoutTransform = transform;
            obj.Margin = margin;
            return bmp;
        }

        
        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.C)
            {
                BufList.Clear();
                for (int i = 0, k = NotArtist.Figures.Count; i < k; i++)
                {

                    if (NotArtist.Figures[i].Select)
                    {
                        
                        BufList.Add(NotArtist.Figures[i]);
                    }
                }
            }

            if (e.Key == Key.V)
            {
                int pointsCount = 0;
                var v = new Vector();
                foreach (var figure in BufList)
                {
                    foreach (var point in figure.coordinates)
                    {
                        pointsCount++;
                        v.X += point.X;
                        v.Y += point.Y;
                    }
                }
                v.X = v.X / pointsCount;
                v.Y = v.Y / pointsCount;
                v -= (Vector)Mouse.GetPosition(MyCanvas);

                for (var i = 0; i < BufList.Count; i++)
                {
                    var cloneObj = BufList[i].Clone();
                    for (var j = 0; j < cloneObj.coordinates.Count; j++)
                    {
                        cloneObj.coordinates[j] -= v;
                    }
                    NotArtist.Figures.Add(cloneObj);
                }

                for (int i = 0, k = NotArtist.Figures.Count; i < k; i++)
                {

                    if (NotArtist.Figures[i].Select)
                    {
                        NotArtist.Figures[i].UnSelected();
                    }
                }
                PropToolBarPanel.Children.Clear();
                NotArtist.AddCondition();
                gotoPastCondition.IsEnabled = true;
                gotoSecondCondition.IsEnabled = false;
                SaveButton.Content = "Save*";
                Invalidate();
            }           
        }
    }
}
