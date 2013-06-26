using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Reflection;

namespace MT_Creator_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    public partial class Window1 : Window
    {
        Image DraggableImage;
        MediaElement DraggableVideo;
        public TextBlock DraggableText;
        MediaElement MySound;

        //Background Color.
        public SolidColorBrush bgColor;

        //Fonts
        public int fontSize;
        public FontFamily ff;
        public SolidColorBrush fontColor;

        bool IsMouseDown = false;
        bool isMouseDragging = false;
        Point startPoint;
        double oLeft;
        double oTop;
        bool selected = false;
        public UIElement selectedElement = null;
        AdornerLayer a;
        string tempPath;

        enum TYPE { TEXT, IMAGE, VIDEO, AUDIO, UNKNOWN };
        public int curType;

        public class sElements
        {
            public string fileName;
            public Point coords;
            public int angle;
            public int media;
            public Image curImg;
            public MediaElement curVid;
            public string text;
            public int height;
            public int width;
            //Array is stored isMovable, isRotatable, isScalable
            public bool[] gesturesAllowed = new bool[3];
            public string linksTo;
            public FontFamily ff;
            public int fontSize;
            public SolidColorBrush fontColor;
        };

        public List<sElements> myScene = new List<sElements>();

        public Window1()
        {
            InitializeComponent();
            //Temp folder.
            string applicationPath;
            applicationPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (applicationPath.Substring(0, 6) == "file:\\")
            {
                applicationPath = applicationPath.Remove(0, 6);
            }
            tempPath = applicationPath + "\\tmp";

            if (!System.IO.Directory.Exists(tempPath))
            {
                System.IO.Directory.CreateDirectory(tempPath);
            }

            //Initialize the background color.
            bgColor = (SolidColorBrush)this.Background;
            selectedElement = null;
            selected = false;
            ff = new FontFamily("Arial");
            fontColor = (SolidColorBrush)(Brushes.Black);
            fontSize = 10;
            scene.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(scene_PreviewMouseLeftButtonDown);
            scene.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
        }

        protected void OnStartup(StartupEventArgs e)
        {
            String applicationPath, tempPath;

            applicationPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (applicationPath.Substring(0, 6) == "file:\\")
            {
                applicationPath = applicationPath.Remove(0, 6);
            }
            tempPath = applicationPath + "\\tmp";

            if (System.IO.Directory.Exists(tempPath))
            {
                //remove everything in this directory
                string[] files;

                files = System.IO.Directory.GetFiles(tempPath);
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }


                string[] directories = System.IO.Directory.GetDirectories(tempPath);

                foreach (string directory in directories)
                {
                    files = System.IO.Directory.GetFiles(directory);

                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }

                    System.IO.Directory.Delete(directory);
                }
            }
        }

        public void BG_Color(Object sender, RoutedEventArgs e)
        {
            colorChooser cc = new colorChooser(this);
            cc.bg = true;
            cc.ShowDialog();
        }

        public void Draggables_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    a.Remove(a.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
        }

        void Draggables_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            int loc = 0;
            Point newCoords = new Point(e.GetPosition(null).X, e.GetPosition(null).Y);
            if (selectedElement != null)
            {
                newCoords.X = (int)(Canvas.GetLeft(selectedElement));
                newCoords.Y = (int)(Canvas.GetTop(selectedElement));

                if (selectedElement.GetType().Equals(typeof(Image)))
                {
                    loc = findElement((int)TYPE.IMAGE);
                    curType = (int)TYPE.IMAGE;
                }
                if (selectedElement.GetType().Equals(typeof(TextBlock)))
                {
                    loc = findElement((int)TYPE.TEXT);
                    curType = (int)TYPE.TEXT;
                }
                if (selectedElement.GetType().Equals(typeof(MediaElement)))
                {
                    loc = findElement((int)TYPE.VIDEO);
                    curType = (int)TYPE.VIDEO;
                }

                (myScene[loc]).coords = newCoords;
            }
            e.Handled = true;
        }

        public void Draggables_MouseMove(Object sender, MouseEventArgs e)
        {
            if (e.Source.GetType().Equals(typeof(Image)))
            {
                if (DraggableImage != null)
                {
                    if (IsMouseDown)
                    {
                        if ((isMouseDragging == false) &&
                            ((Math.Abs(e.GetPosition(scene).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                            (Math.Abs(e.GetPosition(scene).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                            isMouseDragging = true;

                        if (isMouseDragging)
                        {
                            Point position = Mouse.GetPosition(scene);
                            Canvas.SetTop(DraggableImage, position.Y - (startPoint.Y - oTop));
                            Canvas.SetLeft(DraggableImage, position.X - (startPoint.X - oLeft));                            
                        }
                    }
                }
            }
            if (e.Source.GetType().Equals(typeof(MediaElement)))
            {
                if (DraggableVideo != null)
                {
                    if (IsMouseDown)
                    {
                        if ((isMouseDragging == false) &&
                            ((Math.Abs(e.GetPosition(scene).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                            (Math.Abs(e.GetPosition(scene).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                            isMouseDragging = true;

                        if (isMouseDragging)
                        {
                            Point position = Mouse.GetPosition(scene);
                            Canvas.SetTop(DraggableVideo, position.Y - (startPoint.Y - oTop));
                            Canvas.SetLeft(DraggableVideo, position.X - (startPoint.X - oLeft));
                        }
                    }
                }
            }
            if (e.Source.GetType().Equals(typeof(TextBlock)))
            {
                if (DraggableText != null)
                {
                    if (IsMouseDown)
                    {
                        if ((isMouseDragging == false) &&
                            ((Math.Abs(e.GetPosition(scene).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                            (Math.Abs(e.GetPosition(scene).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                            isMouseDragging = true;

                        if (isMouseDragging)
                        {
                            Point position = Mouse.GetPosition(scene);
                            Canvas.SetTop(DraggableText, position.Y - (startPoint.Y - oTop));
                            Canvas.SetLeft(DraggableText, position.X - (startPoint.X - oLeft));
                        }
                    }
                }
            }
        }
        void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            int loc = 0;
            Point newCoords = new Point(e.GetPosition(null).X, e.GetPosition(null).Y);
            if (selectedElement != null)
            {
                newCoords.X = (int)(Canvas.GetLeft(selectedElement));
                newCoords.Y = (int)(Canvas.GetTop(selectedElement));
                if (selectedElement.GetType().Equals(typeof(Image)))
                {
                    loc = findElement((int)TYPE.IMAGE);
                    curType = (int)TYPE.IMAGE;
                }
                if (selectedElement.GetType().Equals(typeof(TextBlock)))
                {
                    loc = findElement((int)TYPE.TEXT);
                    curType = (int)TYPE.TEXT;
                }
                if (selectedElement.GetType().Equals(typeof(MediaElement)))
                {
                    loc = findElement((int)TYPE.VIDEO);
                    curType = (int)TYPE.VIDEO;
                }

                (myScene[loc]).coords = newCoords;
            }
            e.Handled = true;
        }

        private void StopDragging()
        {
            if (IsMouseDown)
            {
                IsMouseDown = false;
                isMouseDragging = false;
            }
        }

        void scene_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    a.Remove(a.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
            if (e.Source != scene)
            {
                IsMouseDown = true;
                startPoint = e.GetPosition(scene);

                selectedElement = e.Source as UIElement;
                oLeft = Canvas.GetLeft(selectedElement);
                oTop = Canvas.GetTop(selectedElement);
                
                a = AdornerLayer.GetAdornerLayer(selectedElement);
                a.Add(new Adorners(selectedElement, this));
                selected = true;

                if (selectedElement.GetType().Equals(typeof(Image)))
                {
                    DraggableImage = (Image)selectedElement;
                    curType = (int)TYPE.IMAGE;
                }
                if (selectedElement.GetType().Equals(typeof(MediaElement)))
                {
                    DraggableVideo = (MediaElement)selectedElement;
                    curType = (int)TYPE.VIDEO;
                }
                if (selectedElement.GetType().Equals(typeof(TextBlock)))
                {
                    DraggableText = (TextBlock)selectedElement;
                    curType = (int)TYPE.TEXT;
                }

                e.Handled = true;
            }
        }
        
        public void RefreshScene(string filename, int type, int height, int width, bool []allowedGestures, string linksTo)
        {
            sElements newElement = new sElements();
            newElement.angle = 0;
            newElement.coords = new Point(0, 0);
            newElement.fileName = filename;
            newElement.media = type;
            newElement.curVid = null;
            newElement.curImg = null;
            newElement.text = null;
            newElement.height = height;
            newElement.width = width;
            newElement.linksTo = linksTo;
            newElement.gesturesAllowed = allowedGestures;
            curType = type;
            newElement.ff = null;
            newElement.fontSize = 1;
            newElement.fontColor = null;
            string targetFile;
            string name;

            if (type == (int)TYPE.IMAGE)
            {
                //Add to temp directory.
                name = System.IO.Path.GetFileName(filename);
                targetFile = tempPath + '/' + name;
                if(!File.Exists(targetFile))
                {
                    System.IO.File.Copy(newElement.fileName, targetFile, true);
                }

                Image newImg = new Image();
                newImg.Stretch = Stretch.Fill;
                newImg.MouseLeftButtonDown += new MouseButtonEventHandler(Draggables_MouseLeftButtonDown);
                newImg.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
                newImg.MouseMove += new MouseEventHandler(Draggables_MouseMove);
                newImg.MouseLeave += new MouseEventHandler(Draggables_MouseLeave);
                newImg.Source = new BitmapImage(new Uri(targetFile));
                scene.Children.Add((UIElement)newImg);
                newElement.curImg = newImg;
                myScene.Add(newElement);
                DraggableImage = newImg;
                scene.UpdateLayout();
                if (height == -1)
                {
                    newElement.height = (int)DraggableImage.ActualHeight;
                }
                else
                {
                    DraggableImage.Height = height;
                }
                if (width == -1)
                {
                    newElement.width = (int)DraggableImage.ActualWidth;
                }
                else
                {
                    DraggableImage.Width = width;
                }
                Canvas.SetTop(DraggableImage, 1);
                Canvas.SetLeft(DraggableImage, 1);
            }
            else if (type == (int)TYPE.VIDEO)
            {
                //Add to temp directory.
                name = System.IO.Path.GetFileName(filename);
                targetFile = tempPath + '/' + name;
                if (!File.Exists(targetFile))
                {
                    System.IO.File.Copy(newElement.fileName, targetFile, true);
                }
                MediaElement newVid = new MediaElement();
                newVid.Stretch = Stretch.Fill;
                newVid.MouseLeftButtonDown += new MouseButtonEventHandler(Draggables_MouseLeftButtonDown);
                newVid.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
                newVid.MouseMove += new MouseEventHandler(Draggables_MouseMove);
                newVid.MouseLeave += new MouseEventHandler(Draggables_MouseLeave);
                newVid.Source = new Uri(targetFile);
                newVid.LoadedBehavior = MediaState.Stop;
                scene.Children.Add(newVid);
                newElement.curVid = newVid;
                myScene.Add(newElement);
                DraggableVideo = newVid;
                scene.UpdateLayout();
                if (height == -1)
                {
                    newElement.height = (int)DraggableVideo.ActualHeight;
                }
                else
                {
                    DraggableVideo.Height = height;
                }
                if (height == -1)
                {
                    newElement.width = (int)DraggableVideo.ActualWidth;
                }
                else
                {
                    DraggableVideo.Width = width;
                }

                Canvas.SetTop(DraggableVideo, 1);
                Canvas.SetLeft(DraggableVideo, 1);
            }
            else if (type == (int)TYPE.AUDIO)
            {
                //Add to temp directory.
                name = System.IO.Path.GetFileName(filename);
                targetFile = tempPath + '/' + name;
                if (!File.Exists(targetFile))
                {
                    System.IO.File.Copy(newElement.fileName, targetFile, true);
                }
                MediaElement newSound = new MediaElement();
                newSound.Source = new Uri(targetFile);
                newSound.LoadedBehavior = MediaState.Stop;
                scene.Children.Add(newSound);
                MySound = newSound;
                DraggableVideo = newSound;
                newElement.curVid = newSound;
                myScene.Add(newElement);
            }
            else if (type == (int)TYPE.TEXT)
            {
                TextBlock newText = new TextBlock();
                newText.MouseLeftButtonDown += new MouseButtonEventHandler(Draggables_MouseLeftButtonDown);
                newText.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
                newText.MouseMove += new MouseEventHandler(Draggables_MouseMove);
                newText.MouseLeave += new MouseEventHandler(Draggables_MouseLeave);
                newText.Text = filename;
                newElement.fontColor = fontColor;
                newElement.fontSize = fontSize;
                newElement.ff = ff;
                newText.TextWrapping = TextWrapping.Wrap;
                scene.Children.Add(newText);
                DraggableText = newText;
                DraggableText.FontFamily = ff;
                DraggableText.FontSize = fontSize;
                DraggableText.Foreground = fontColor;
                scene.UpdateLayout();
                if (height == -1)
                {
                    newElement.height = (int)DraggableText.ActualHeight;
                }
                if (width == -1)
                {
                    newElement.width = (int)DraggableText.ActualWidth;
                }
                newElement.text = filename;
                DraggableText.Text = filename;
                Canvas.SetTop(DraggableText, 1);
                Canvas.SetLeft(DraggableText, 1);
                myScene.Add(newElement);
            }
        }

        public int findElement(int type)
        {
            int it = 0;

            if (type == (int)TYPE.TEXT)
            {
                for (it = 0; it < myScene.Count; it++)
                {
                    if (myScene[it].text == DraggableText.Text)
                    {
                        return it;
                    }
                }
            }
            if (type == (int)TYPE.IMAGE)
            {
                for (it = 0; it < myScene.Count; it++)
                {
                    if (myScene[it].curImg == DraggableImage)
                    {
                        return it;
                    }
                }
            }
            if (type == (int)TYPE.AUDIO)
            {
                for (it = 0; it < myScene.Count; it++)
                {
                    if (myScene[it].curVid == MySound)
                    {
                        return it;
                    }
                }
            }
            if (type == (int)TYPE.VIDEO)
            {
                for (it = 0; it < myScene.Count; it++)
                {
                    if (myScene[it].curVid == DraggableVideo)
                    {
                        return it;
                    }
                }
            }

            return 0;
        }

        private void Click_Open(Object sender, RoutedEventArgs e)
        {
            //Open a new project.
        }

        private void Click_Load(Object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XAML files(.xaml)|*.xaml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                //Clear the scene.
                scene.Children.Clear();
                myScene.Clear();
                selectedElement = null;
                
                // Open document
                string filename = dlg.FileName;
                string dirname = System.IO.Path.GetDirectoryName(filename);
                StreamReader lfile = new StreamReader(filename);
                string line;
                string bg;
                int index1, index2;
                while ((line = lfile.ReadLine()) != null)
                {
                    if(line.Contains("Background=\""))
                    {
                        index1 = line.IndexOf("Background=\"");
                        index2 = line.IndexOf('"', index1+12);
                        bg = line.Substring(index1+12, index2-index1-12);
                        BrushConverter fromHex = new BrushConverter();
                        bgColor = (SolidColorBrush)(fromHex.ConvertFrom(bg));
                        scene.Background = bgColor;
                        
                    }
                    if (line.Contains("Image Name"))
                    {
                        processImage(line, dirname);
                        curType = (int)TYPE.IMAGE;
                    }
                    if (line.Contains("MediaElement Name"))
                    {
                        processMedia(line, dirname);
                        curType = (int)TYPE.VIDEO;
                    }
                    if (line.Contains("TextBlock Name"))
                    {
                        processText(line);
                        curType = (int)TYPE.TEXT;
                    }
                }
            }
        }
        private void savedGestures(string name, ref bool[] savedGestures)
        {
            savedGestures[0] = false;
            savedGestures[1] = false;
            savedGestures[2] = false;

            if(name == "All")
            {
                savedGestures[0] = true;
                savedGestures[1] = true;
                savedGestures[2] = true;
            }
            if(name == "Translate")
            {
                savedGestures[0] = true;
            }
            if(name == "RotateTranslate")
            {
                savedGestures[0] = true;
                savedGestures[1] = true;
            }
            if(name == "TranslateScale")
            {
                savedGestures[0] = true;
                savedGestures[2] = true;
            }
            if(name == "RotateScale")
            {
                savedGestures[1] = true;
                savedGestures[2] = true;
            }
            if(name == "Rotate")
            {
                savedGestures[1] = true;
            }
            if(name == "Scale")
            {
                savedGestures[2] = true;
            }
        }

        public void processText(string line)
        {
            int index1, index2;
            int top, left, length, modIndex, height, width;
            bool[] gesturesAllowed = new bool[3];
            string text, name;
            string linksTo = null;
            //Parse name.
            index2 = line.IndexOf("Name");
            index1 = line.IndexOf('_', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            name = line.Substring(index1 + 1, index2 - index1 - 1);
            savedGestures(name, ref gesturesAllowed);
            //Parse text.
            index1 = line.IndexOf(">");
            index2 = line.IndexOf("<", index1 + 1);
            text = line.Substring(index1+1, index2-index1-1);
            //Parse width.
            index2 = line.IndexOf("Width");
            index1 = line.IndexOf('"', index2+1);
            index2=line.IndexOf('"', index1+1);
            width = Int32.Parse(line.Substring(index1 + 1, index2 - index1 - 1));
            //Parse height.
            index2 = line.IndexOf("Height");
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1+1);
            height = Int32.Parse(line.Substring(index1 + 1, index2 - index1 - 1));
            //Parse font family.
            index2 = line.IndexOf("FontFamily");
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            ff = new FontFamily (line.Substring(index1+1, index2-index1-1));
            //Parse font size.
            index2 = line.IndexOf("FontSize");
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            fontSize = Int32.Parse(line.Substring(index1+1, index2-index1-1));
            //Parse font color.
            index2= line.IndexOf("Foreground");
            index1=line.IndexOf('"', index2+1);
            index2 = line.IndexOf('"', index1+1);
            BrushConverter fromHex = new BrushConverter();
            fontColor = (SolidColorBrush)(fromHex.ConvertFrom(line.Substring(index1+1, index2-index1-1)));
            //Parse left.
            index2 = line.IndexOf("Matrix OffsetX");
            index2 += 14;
            index1 = line.IndexOf('"', index2);
            Console.WriteLine(index1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            left = Int32.Parse(line.Substring(index1 + 1, length));
            //Parse top.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            top = Int32.Parse(line.Substring(index1 + 1, length));
            if (line.Contains("_Click_Scene"))
            {
                index1 = line.IndexOf("_Click_Scene\" Text=\"");
                index2 = line.IndexOf('"', index1 + 20);
                linksTo = line.Substring(index1 + 20, index2 - index1 - 20);
            }

            RefreshScene(text, (int)TYPE.TEXT, height, width, gesturesAllowed, linksTo);
            modIndex = myScene.Count - 1;
            myScene[modIndex].coords = new Point(left, top);
            Canvas.SetTop(DraggableText, top);
            Canvas.SetLeft(DraggableText, left);
        }

        public void processMedia(string line, string dirname)
        {
            int width;
            int height;
            int left;
            int top;
            string filename;
            int length;
            string fullpath;
            string name;
            bool[] gesturesAllowed = new bool[3];
            int modIndex;

            int index1 = 0;
            int index2 = 0;
            //Parse name.
            index1 = line.IndexOf('"');
            index2 = line.IndexOf('"', index1 + 1);
            if (line.Substring(index1 + 1, index2 - index1 - 1) == "Audio")
            {
                processAudio(line, dirname);
                return;
            }
            index2 = line.IndexOf("MediaElement Name");
            index2 += 17;
            //Parse name.
            index2 = line.IndexOf("Name");
            index1 = line.IndexOf('_', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            name = line.Substring(index1 + 1, index2 - index1 - 1);
            savedGestures(name, ref gesturesAllowed);  
            //Parse filename.
            index1 = line.IndexOf("Source", index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            index1 = line.IndexOf('"', index2 + 1);
            length = index1 - index2 - 1;
            filename = line.Substring(index2 + 24, length-23);
            fullpath = dirname + filename;
            //parse width.
            index1 = line.IndexOf("Width", index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            index1 = line.IndexOf('"', index2 + 1);
            length = index1 - index2 - 1;
            width = Int32.Parse(line.Substring(index2 + 1, length));
            //parse height.
            index1 = line.IndexOf("Height", index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            index1 = line.IndexOf('"', index2 + 1);
            length = index1 - index2 - 1;
            height = Int32.Parse(line.Substring(index2 + 1, length));
            //Parse left.
            index2 = line.IndexOf("Matrix OffsetX");
            index2 += 14;
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            left = Int32.Parse(line.Substring(index1 + 1, length));
            //Parse top.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            top = Int32.Parse(line.Substring(index1 + 1, length));

            RefreshScene(fullpath, (int)TYPE.VIDEO, height, width, gesturesAllowed, null);
            modIndex = myScene.Count - 1;
            myScene[modIndex].coords = new Point(left, top);
            Canvas.SetTop(DraggableVideo, top);
            Canvas.SetLeft(DraggableVideo, left);
        }

        public void processAudio(string line, string dirname)
        {
            int index1;
            int index2;
            int length;
            string fullpath;
            string filename;
            bool[] gesturesAllowed = new bool[3];

            //Parse name.
            index1 = line.IndexOf('"');
            index2 = line.IndexOf('"', index1 + 1);
            //Parse filename.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            filename = line.Substring(index1 + 28, length - 27);
            fullpath = dirname + filename;
            RefreshScene(fullpath, (int)TYPE.AUDIO, 0, 0, gesturesAllowed, null);
        }
        public void processImage(string line, string dirname)
        {
            string filename;
            string fullpath;
            string name, temp;
            int height;
            int width;
            int length;
            int modIndex;
            int top, left;
            bool[] gesturesAllowed = new bool[3];
            string linksTo = null;

            int index1 = 0;
            int index2 = 0;
            //Parse name.
            index2 = line.IndexOf("Name");
            index1 = line.IndexOf('_', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            name = line.Substring(index1 + 1, index2 - index1 - 1);
            savedGestures(name, ref gesturesAllowed);
            //Parse filename.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            filename = line.Substring(index1 + 28, length - 27);
            fullpath = dirname + filename;
            //parse width.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            temp = line.Substring(index1 + 1, length);
            width = Int32.Parse(temp);
            //parse height.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            height = Int32.Parse(line.Substring(index1 + 1, length));
            //Parse left.
            index2 = line.IndexOf("Matrix OffsetX");
            index2 += 14;
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            left = Int32.Parse(line.Substring(index1 + 1, length));
            //Parse top.
            index1 = line.IndexOf('"', index2 + 1);
            index2 = line.IndexOf('"', index1 + 1);
            length = index2 - index1 - 1;
            top = Int32.Parse(line.Substring(index1 + 1, length));
            //Parse Link.
            if (line.Contains("_Click_Scene"))
            {
                index1 = line.IndexOf("_Click_Scene\" Text=\"");
                index2 = line.IndexOf('"', index1 + 20);
                linksTo = line.Substring(index1 + 20, index2 - index1 - 20);
            }
            RefreshScene(fullpath, (int)TYPE.IMAGE, height, width, gesturesAllowed, linksTo);
            modIndex = myScene.Count - 1;
            myScene[modIndex].coords = new Point(left, top);
            Canvas.SetTop(DraggableImage, top);
            Canvas.SetLeft(DraggableImage, left);
        }

        private void Click_Close(Object sender, RoutedEventArgs e)
        {
            //Close the window.
            this.Close();
        }

        private void Add_Image(Object sender, RoutedEventArgs e)
        {
            addImage ai = new addImage(this);
            ai.ShowDialog();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            int loc = 0;

            Console.WriteLine(myScene.Count);

            for (int i = 0; i < myScene.Count; i++)
            {
                Console.WriteLine(myScene[i].fileName);
            }
            if (curType == (int)TYPE.IMAGE)
            {
                loc = findElement((int)TYPE.IMAGE);
                scene.Children.Remove(DraggableImage);
            }
            if (curType == (int)TYPE.TEXT)
            {
                loc = findElement((int)TYPE.TEXT);
                scene.Children.Remove(DraggableText);
            }
            if (curType == (int)TYPE.AUDIO)
            {
                loc = findElement((int)TYPE.AUDIO);
                scene.Children.Remove(MySound);
            }
            if (curType == (int)TYPE.VIDEO)
            {
                loc = findElement((int)TYPE.VIDEO);
                scene.Children.Remove(DraggableVideo);
            }

            if (myScene.Count != 0)
            {
                myScene.RemoveAt(loc);
            }
            selectedElement = null;
            selected = false;
        }

        private void Add_Vid(object sender, RoutedEventArgs e)
        {
            addVid av = new addVid(this);
            av.ShowDialog();
        }

        private void Add_Text(object sender, RoutedEventArgs e)
        {
            addText at = new addText(this);
            at.ShowDialog();
        }

        private void Add_Sound(object sender, RoutedEventArgs e)
        {
            addSound ads = new addSound(this);
            ads.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            DraggableVideo.LoadedBehavior = MediaState.Manual;

            DraggableVideo.Stop();
        }

        private void SelectSound(object sender, RoutedEventArgs e)
        {
            DraggableVideo = MySound;
        }

        private void Click_Play(object sender, RoutedEventArgs e)
        {
            if (DraggableVideo != null)
            {
                DraggableVideo.LoadedBehavior = MediaState.Manual;

                if (DraggableVideo.Position == DraggableVideo.NaturalDuration)
                {
                    DraggableVideo.Stop();
                }
                DraggableVideo.Play();
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (DraggableVideo != null)
            {
                DraggableVideo.LoadedBehavior = MediaState.Manual;
                DraggableVideo.Pause();
            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Presentation";
            dlg.DefaultExt = ".xaml";
            dlg.Filter = "XAML Files (.xaml)|*.xaml";
            string exportFile;
            string targetPath;
            string sourceName;
            string targetFile;

            string imageTargetPath;
            string videoTargetPath;
            string audioTargetPath;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                exportFile = dlg.FileName;

                //get the file path
                targetPath = System.IO.Path.GetDirectoryName(exportFile);
                imageTargetPath = targetPath + "/images/";
                videoTargetPath = targetPath + "/video/";
                audioTargetPath = targetPath + "/audio/";
            }

            else
            {
                return;
            }

            int imageNum = 1;
            int videoNum = 1;
            int textNum = 1;
            String elementName;
            String gestureName;

            StreamWriter writer = new StreamWriter(exportFile);
            
            writer.WriteLine("<Page");
            writer.WriteLine("xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            writer.WriteLine("xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            writer.WriteLine("xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"");
            writer.WriteLine("xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"");
            writer.WriteLine("mc:Ignorable=\"d\"");
            writer.WriteLine("d:DesignHeight=\"" + this.Height + "\" d:DesignWidth=\"" + this.Width + "\"");
            writer.WriteLine("Title=\"Presentation\">");
            writer.WriteLine("<InkCanvas Name=\"scene\" Background=\"" + bgColor.ToString() + "\" EditingMode=\"None\">");

            foreach (sElements elm in myScene)
            {
                Boolean isManipulationEnabled = true;
                if (elm.media == (int)TYPE.IMAGE)
                {
                    //get source name
                    sourceName = elm.fileName;
                    if (File.Exists(elm.fileName.ToString()))
                    {
                        sourceName = System.IO.Path.GetFileName(elm.fileName).ToString();
                    }
                    else
                    {
                        Console.WriteLine("File does not exist!");
                    }

                    if (imageNum < 10)
                    {
                        elementName = "Image00";
                    }
                    else if (imageNum < 100)
                    {
                        elementName = "Image0";
                    }
                    else
                    {
                        elementName = "Image";
                    }

                    //set gestures
                    //see what gestures are assigned to the object and set the name
                    //if there are no gestures assigned, set the name to None, and
                    //      isManipulationEnabled to false

                    gestureName = gestureWriter(elm.gesturesAllowed, elm.linksTo);
                    if (gestureName == "_None" || gestureName == "_Click")
                    {
                        isManipulationEnabled = false;
                    }

                    writer.Write("<Image Name=\"");
                    writer.Write(elementName + imageNum + gestureName);

                    imageNum++;

                    writer.Write("\" ");
                    writer.Write("Source=\"pack://siteoforigin:,,,/tmp/images/");
                    writer.Write(sourceName);
                    writer.Write("\" ");
                    writer.Write("Width=\"" + elm.width + 
                        "\" Height=\"" + elm.height + "\"");
                    writer.Write(" IsManipulationEnabled=\"" + isManipulationEnabled + "\">");

                    //Add matrix manipulation structure
                    writer.Write("<Image.RenderTransform>");
                    writer.Write("<MatrixTransform>");
                    writer.Write("<MatrixTransform.Matrix>");
                    writer.Write("<Matrix OffsetX=\"" + elm.coords.X + "\" OffsetY=\"" + elm.coords.Y + "\" M11=\"1\" M22=\"1\" />");
                    writer.Write("</MatrixTransform.Matrix>");
                    writer.Write("</MatrixTransform>");
                    writer.Write("</Image.RenderTransform>");

                    writer.Write("</Image>");

                    if (gestureName == "_Click")
                    {
                        writer.Write("<TextBox Name=\"" + elementName + imageNum + gestureName + "_Scene" + "\" Text=\"" + elm.linksTo + "\" Visibility=\"Hidden\" />");
                    }

                    writer.Write("\n");

                    //copy image to presentation directory
                    if (!System.IO.Directory.Exists(imageTargetPath))
                    {
                        System.IO.Directory.CreateDirectory(imageTargetPath);
                    }

                    targetFile = System.IO.Path.Combine(imageTargetPath, sourceName).ToString();
                    Console.WriteLine(elm.fileName);
                    if (File.Exists(elm.fileName))
                    {
                        System.IO.File.Copy(elm.fileName, targetFile, true);
                    }

                }
                if (elm.media == (int)TYPE.VIDEO)
                {
                    Console.WriteLine(elm.coords.ToString());
                    //get source name
                    sourceName = System.IO.Path.GetFileName(elm.fileName).ToString();

                    if (videoNum < 10)
                    {
                        elementName = "Video00";
                    }
                    else if (videoNum < 100)
                    {
                        elementName = "Video0";
                    }
                    else
                    {
                        elementName = "Video";
                    }

                    //set gestures
                    //see what gestures are assigned to the object and set the name
                    //if there are no gestures assigned, set the name to None, and
                    //      isManipulationEnabled to false
                    //if the gesture is _Click, set the isManipulationEnabled variable
                    //      to false
                    //_All, _Rotate, _Translate, _Scale, _RotateTranslate, _RotateScale, _TranslateScale, _Click

                    gestureName = gestureWriter(elm.gesturesAllowed, elm.linksTo);
                    if (gestureName == "_Click" || gestureName == "_None")
                    {
                        isManipulationEnabled = false;
                    }

                    writer.Write("<StackPanel Name=\"" + elementName + videoNum + "Container" + gestureName + "\" Background=\"Black\" Width=\"" + elm.width + "\" Height=\"" + (elm.height + 30) + "\" IsManipulationEnabled=\"" + isManipulationEnabled + "\">");

                    writer.Write("<MediaElement Name=\"");
                    writer.Write(elementName + videoNum + gestureName);
                    videoNum++;

                    writer.Write("\" ");
                    writer.Write("Source=\"pack://siteoforigin:,,,/tmp/video/");
                    writer.Write(sourceName);
                    writer.Write("\" ");
                    writer.Write("Width=\"" + elm.width + "\" Height=\"" + elm.height + "\"");
                    writer.Write("LoadedBehavior=\"Manual\" UnloadedBehavior=\"Stop\" ScrubbingEnabled=\"True\" />");

                    writer.Write("<StackPanel Orientation=\"Horizontal\" Background=\"Gray\">");
                    writer.Write("<Image Source=\"icons\\play.png\" Width=\"30\" Height=\"30\" HorizontalAlignment=\"Left\" />");
                    writer.Write("<Image Source=\"icons\\pause.png\" Width=\"30\" Height=\"30\" HorizontalAlignment=\"Left\" Visibility=\"Collapsed\" />");
                    writer.Write("<Image Source=\"icons\\stop.png\" Width=\"30\" Height=\"30\" HorizontalAlignment=\"Left\" />");
                    writer.Write("</StackPanel>");

                    writer.Write("<StackPanel.RenderTransform>");
                    writer.Write("<MatrixTransform>");
                    writer.Write("<MatrixTransform.Matrix>");
                    writer.Write("<Matrix OffsetX=\"" + elm.coords.X + "\" OffsetY=\"" + elm.coords.Y + "\" M11=\"1\" M22=\"1\" />");
                    writer.Write("</MatrixTransform.Matrix>");
                    writer.Write("</MatrixTransform>");
                    writer.Write("</StackPanel.RenderTransform>");
                    writer.Write("</StackPanel>\n");

                    //copy video to presentation directory
                    if (!System.IO.Directory.Exists(videoTargetPath))
                    {
                        System.IO.Directory.CreateDirectory(videoTargetPath);
                    }

                    targetFile = System.IO.Path.Combine(videoTargetPath, sourceName).ToString();

                    if (File.Exists(elm.fileName))
                    {
                        System.IO.File.Copy(elm.fileName, targetFile, true);
                    }
                }
                if (elm.media == (int)TYPE.TEXT)
                {
                    if (textNum < 10)
                    {
                        elementName = "Text00";
                    }
                    else if (textNum < 100)
                    {
                        elementName = "Text0";
                    }
                    else
                    {
                        elementName = "Text";
                    }


                    //set gestures
                    //TODO: see what gestures are assigned to the object and set the name
                    //TODO: if there are no gestures assigned, set the name to None, and
                    //      isManipulationEnabled to false
                    gestureName = gestureWriter(elm.gesturesAllowed, elm.linksTo);
                    if (gestureName == "_Click" || gestureName == "_None")
                    {
                        isManipulationEnabled = false;
                    }

                    writer.Write("<TextBlock Name=\"");
                    writer.Write(elementName + textNum + gestureName);
                    writer.Write("\" ");
                    writer.Write("IsManipulationEnabled=\"" + isManipulationEnabled + "\" TextWrapping=\"Wrap\" Margin=\"0,0,0,20\"");
                    writer.Write(" Width=\"" + elm.width + 
                        "\" Height=\"" + elm.height + "\"" +  
                        " FontFamily=\"" + elm.ff.ToString() +
                        "\"" + " FontSize=\"" + elm.fontSize + "\"" +
                        " Foreground=\"" + (Color)elm.fontColor.Color + "\">");
                    writer.Write(elm.fileName);
                    textNum++;

                    //matrix manipulation stuff
                    writer.Write("<TextBlock.RenderTransform>");
                    writer.Write("<MatrixTransform>");
                    writer.Write("<MatrixTransform.Matrix>");
                    writer.Write("<Matrix OffsetX =\"" + elm.coords.X + "\" OffsetY=\"" + elm.coords.Y + "\" M11=\"1\" M22=\"1\" />");
                    writer.Write("</MatrixTransform.Matrix>");
                    writer.Write("</MatrixTransform>");
                    writer.Write("</TextBlock.RenderTransform>");
                    writer.Write("</TextBlock>");


                    if (gestureName == "_Click")
                    {
                        writer.Write("<TextBox Name=\"" + elementName + textNum + gestureName + "_Scene" + "\" Text=\"" + elm.linksTo + "\" Visibility=\"Hidden\" />");
                    }

                    writer.Write("\n");
                }
                if (elm.media == (int)TYPE.AUDIO)
                {
                    sourceName = System.IO.Path.GetFileName(elm.fileName).ToString();

                    writer.Write("<MediaElement Name=\"");
                    writer.Write("Audio");
                    writer.Write("\" ");
                    writer.Write("Source=\"pack://siteoforigin:,,,/tmp/audio/");
                    writer.Write(sourceName);
                    writer.Write("\" LoadedBehavior=\"Manual\" UnloadedBehavior=\"Stop\" />\n");

                    //copy audio to presentation directory
                    if (!System.IO.Directory.Exists(audioTargetPath))
                    {
                        System.IO.Directory.CreateDirectory(audioTargetPath);
                    }

                    targetFile = System.IO.Path.Combine(audioTargetPath, sourceName).ToString();
                    if (File.Exists(elm.fileName))
                    {
                        System.IO.File.Copy(elm.fileName, targetFile, true);
                    }
                }
            }
            writer.WriteLine("</InkCanvas>");
            writer.WriteLine("</Page>");
            writer.Close();
        }
        private string gestureWriter(bool[] gestures, string linksTo)
        {
            if (linksTo != null)
            {
                return "_Click";
            }
            if (gestures[0] == true && gestures[1] == false && gestures[2] == false)
            {
                return "_Translate";
            }
            if (gestures[0] == true && gestures[1] == true && gestures[2] == false)
            {
                return "_TranslateRotate";
            }
            if (gestures[0] == true && gestures[1] == false && gestures[2] == true)
            {
                return "_TranslateScale";
            }
            if (gestures[0] == false && gestures[1] == true && gestures[2] == false)
            {
                return "_Rotate";
            }
            if (gestures[0] == false && gestures[1] == true && gestures[2] == true)
            {
                return "_RotateScale";
            }
            if (gestures[0] == false && gestures[1] == false && gestures[2] == true)
            {
                return "_Scale";
            }
            if (gestures[0] == true && gestures[1] == true && gestures[2] == true)
            {
                return "_All";
            }
            if (gestures[0] == false && gestures[1] == false && gestures[2] == false)
            {
                return "_None";
            }
            else return "ERROR";
        }


        private void Properties_Click(object sender, RoutedEventArgs e)
        {
            int loc;
            //populate the windows.
            if (curType == (int)TYPE.TEXT)
            {
                loc = findElement((int)TYPE.TEXT);
                updateText uTxt = new updateText(this);
                uTxt.WidthT.Text = myScene[loc].width.ToString();
                uTxt.HeightT.Text = myScene[loc].height.ToString();
                uTxt.textBox1.Text = myScene[loc].fileName.ToString();
                if (myScene[loc].linksTo != null)
                {
                    uTxt.textBox2.Text = myScene[loc].linksTo.ToString();
                }
                if (myScene[loc].gesturesAllowed[0])
                {
                    uTxt.checkBox1.IsChecked = true;
                }
                if (myScene[loc].gesturesAllowed[1])
                {
                    uTxt.checkBox2.IsChecked = true;
                }
                if (myScene[loc].gesturesAllowed[2])
                {
                    uTxt.checkBox3.IsChecked = true;
                }
                uTxt.ShowDialog();
            }
            if (curType == (int)TYPE.IMAGE)
            {
                loc = findElement((int)TYPE.IMAGE);
                updateImage uImg = new updateImage(this);
                uImg.WidthT.Text = myScene[loc].width.ToString();
                uImg.HeightT.Text = myScene[loc].height.ToString();
                if (myScene[loc].linksTo != null)
                {
                    uImg.textBox1.Text = myScene[loc].linksTo.ToString();
                }
                if (myScene[loc].gesturesAllowed[0])
                {
                    uImg.checkBox1.IsChecked = true;
                }
                if (myScene[loc].gesturesAllowed[1])
                {
                    uImg.checkBox2.IsChecked = true;
                }
                if (myScene[loc].gesturesAllowed[2])
                {
                    uImg.checkBox3.IsChecked = true;
                }
                uImg.ShowDialog();
            }
            if (curType == (int)TYPE.VIDEO)
            {
                loc = findElement((int)TYPE.VIDEO);
                updateImage uImg = new updateImage(this);
                uImg.WidthT.Text = myScene[loc].width.ToString();
                uImg.HeightT.Text = myScene[loc].height.ToString();
                if (myScene[loc].linksTo != null)
                {
                    uImg.textBox1.Text = myScene[loc].linksTo.ToString();
                }
                if (myScene[loc].gesturesAllowed[0])
                {
                    uImg.checkBox1.IsChecked = true;
                }
                if (myScene[loc].gesturesAllowed[1])
                {
                    uImg.checkBox2.IsChecked = true;
                }
                if (myScene[loc].gesturesAllowed[2])
                {
                    uImg.checkBox3.IsChecked = true;
                }
                uImg.ShowDialog();
            }
            if (curType == (int)TYPE.AUDIO)
            {
                loc = findElement((int)TYPE.AUDIO);
            }
        }
        public void UpdateScene(string text, int type, int height, int width, bool[] allowedGestures, string linksTo)
        {
            int loc = 0;
            if (type == (int)TYPE.TEXT)
            {
                loc = findElement((int)TYPE.TEXT);
                DraggableText.Width = width;
                DraggableText.Height = height;
                DraggableText.Text = text;
            }
            if (type == (int)TYPE.IMAGE)
            {
                loc = findElement((int)TYPE.IMAGE);
                DraggableImage.Height = height;
                DraggableImage.Width = width;
            }
            if (type == (int)TYPE.VIDEO)
            {
                loc = findElement((int)TYPE.VIDEO);
                DraggableVideo.Height = height;
                DraggableVideo.Width = width;
            }
            if (type == (int)TYPE.AUDIO)
            {
                loc = findElement((int)TYPE.AUDIO);
            }

            if (text != null && text != "")
            {
                myScene[loc].fileName = text;
            }
            myScene[loc].height = height;
            myScene[loc].width = width;
            myScene[loc].linksTo = linksTo;
            myScene[loc].gesturesAllowed[0] = allowedGestures[0];
            myScene[loc].gesturesAllowed[1] = allowedGestures[1];
            myScene[loc].gesturesAllowed[2] = allowedGestures[2];
        }
    }
}
