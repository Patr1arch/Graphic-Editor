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

namespace Baranov_sArtist
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        bool ClickOnCanvas = false;

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
            NotArtist.SelectedTool = NotArtist.ToolsList[(sender as System.Windows.Controls.Button).Name.ToString()];
            if ((sender as System.Windows.Controls.Button).Name.ToString() == "RoundRectangle")
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
                NotArtist.SelectedColor = NotArtist.TransformColor[(sender as System.Windows.Controls.Button).Name.ToString()];
                NotArtist.ColorStringNow = (sender as System.Windows.Controls.Button).Name.ToString();
                if ((sender as System.Windows.Controls.Button).Background == null) { button_firstColor.Background = Brushes.Gray; }
                else { button_firstColor.Background = (sender as System.Windows.Controls.Button).Background; }

            }
            else
            {
                NotArtist.BrushNow = NotArtist.TransformColor[(sender as System.Windows.Controls.Button).Name.ToString()];
                NotArtist.BrushStringNow = (sender as System.Windows.Controls.Button).Name.ToString();
                if ((sender as System.Windows.Controls.Button).Background == null) { button_secondColor.Background = Brushes.Gray; }
                else { button_secondColor.Background = (sender as System.Windows.Controls.Button).Background; }
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
                sfd.Filter = "Files(*.bin)|*.bin";
                sfd.ShowDialog();
                if (sfd.FileName != "")
                {
                    FileStream file = (FileStream)sfd.OpenFile();
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(file, NotArtist.Figures);
                    file.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Нарисуйте что-нибудь...");
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
        }

        //Change property figure function

        public void changeRoundX(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundX(e.NewValue);
                }
            }
            Invalidate();
        }

        public void changeRoundY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundY(e.NewValue);
                }
            }
            Invalidate();
        }

        public void ChangeStrokeColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(NotArtist.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()], (sender as System.Windows.Controls.Button).Tag.ToString());
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void ChangeBrushColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(NotArtist.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()], (sender as System.Windows.Controls.Button).Tag.ToString(), new bool());
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void ChangeDash(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(NotArtist.TransformDashProp[(sender as System.Windows.Controls.Button).Content.ToString()], (sender as System.Windows.Controls.Button).Content.ToString());
                }
            }
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void ClearSelectedFigure(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in NotArtist.Figures.ToArray())
            {
                if (figure.Select == true)
                {
                    NotArtist.Figures.Remove(figure);
                }
            }
            PropToolBarPanel.Children.Clear();
            NotArtist.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void RowThicnessChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in NotArtist.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(e.NewValue);
                }
            }
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
        }

    }
}
