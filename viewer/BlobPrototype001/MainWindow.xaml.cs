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
using System.Windows.Media.Animation;
using System.Windows.Markup;
using System.Windows.Ink;
using System.IO;
using System.Reflection;
using System.Xml;

namespace BlobPrototype001
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean noTranslate;
        private Boolean noRotate;
        private Boolean noScale;
        private Boolean rotateIconAdded;
        private Boolean zoomIconAdded;
        private Boolean panIconAdded;
        private Boolean soundIconAdded;
        private Boolean soundPlaying;

        private MediaElement soundFile;       

        MenuItem rotateIconMenuItem = new MenuItem();
        MenuItem zoomIconMenuItem = new MenuItem();
        MenuItem panIconMenuItem = new MenuItem();
        MenuItem soundIconMenuItem = new MenuItem();

        String XAMLPath;
        String applicationPath;
        String tempPath;
        String sceneDir;
        String sceneName;

        Boolean sceneLoaded;
        Boolean playback;
        Viewbox mtViewbox;

        String recordingFilename;
        Double DecelerationTranslation;
        Double DecelerationRotation;
        Double DecelerationExpansion;

        public MainWindow()
        {
            InitializeComponent();

            sceneLoaded = false;

            noTranslate = false;
            noRotate = false;
            noScale = false;
            rotateIconAdded = false;
            zoomIconAdded = false;
            panIconAdded = false;
            soundIconAdded = false;
            soundPlaying = false;

            DecelerationExpansion = Convert.ToDouble(initExpandDeceleration.Text);
            DecelerationRotation = Convert.ToDouble(initRotateDeceleration.Text);
            DecelerationTranslation = Convert.ToDouble(initTranslateDeceleration.Text);

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

            //Setup Icons

            //Rotate icon
            Image rotateIcon = new Image();
            BitmapImage rotateIconSource = new BitmapImage();
            rotateIconSource.BeginInit();
            rotateIconSource.UriSource = new Uri("icons/rotateIcon.png", UriKind.Relative);
            rotateIconSource.EndInit();
            rotateIcon.Source = rotateIconSource;
            rotateIcon.Width = 50;
            rotateIcon.Height = 50;
            rotateIconMenuItem.Icon = rotateIcon;

            //Zoom icon
            Image zoomIcon = new Image();
            BitmapImage zoomIconSource = new BitmapImage();
            zoomIconSource.BeginInit();
            zoomIconSource.UriSource = new Uri("icons/zoomIcon.png", UriKind.Relative);
            zoomIconSource.EndInit();
            zoomIcon.Source = zoomIconSource;
            zoomIcon.Width = 50;
            zoomIcon.Height = 50;
            zoomIconMenuItem.Icon = zoomIcon;

            //Pan icon
            Image panIcon = new Image();
            BitmapImage panIconSource = new BitmapImage();
            panIconSource.BeginInit();
            panIconSource.UriSource = new Uri("icons/panIcon.png", UriKind.Relative);
            panIconSource.EndInit();
            panIcon.Source = panIconSource;
            panIcon.Width = 50;
            panIcon.Height = 50;
            panIconMenuItem.Icon = panIcon;

            //Sound icon
            Image soundIcon = new Image();
            BitmapImage soundIconSource = new BitmapImage();
            soundIconSource.BeginInit();
            soundIconSource.UriSource = new Uri("icons/play.png", UriKind.Relative);
            soundIconSource.EndInit();
            soundIcon.Source = soundIconSource;
            soundIcon.Width = 50;
            soundIcon.Height = 50;
            soundIconMenuItem.Icon = soundIcon;

            mnuSave.Visibility = Visibility.Hidden;

            recordingFilename = "session.xml";
            XmlDocument xmlDoc = new XmlDocument();

            XmlTextWriter xmlWriter = new XmlTextWriter(recordingFilename, System.Text.Encoding.UTF8);
            xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            xmlWriter.WriteStartElement("Session");
            xmlWriter.Close();
            xmlDoc.Load(recordingFilename);

            XmlElement root = xmlDoc.DocumentElement;
            XmlElement sceneNode = xmlDoc.CreateElement("Scene");
            root.AppendChild(sceneNode);
            sceneNode.SetAttribute("FilePath", sceneDir + "//" + sceneName + ".xaml");

            XmlElement eventsNode = xmlDoc.CreateElement("Events");
            sceneNode.AppendChild(eventsNode);

            xmlDoc.Save(recordingFilename);

            playback = false;
        }

        public void LoadXAML(String fileLoc)
        {
            
            XAMLPath = System.IO.Path.GetDirectoryName(fileLoc);
            string sceneFile = System.IO.Path.GetFileName(fileLoc);

            sceneDir = XAMLPath;
            sceneName = sceneFile.Substring(0, sceneFile.LastIndexOf("."));

            //copy entire directory to temp path
            string[] files = System.IO.Directory.GetFiles(XAMLPath);
            string fileName;
            string targetFile;

            if (!sceneLoaded)
            {
                //Copy files and overwrite destination files if they exist
                foreach (string file in files)
                {
                    fileName = System.IO.Path.GetFileName(file);

                    if (fileName != sceneName)
                    {
                        targetFile = System.IO.Path.Combine(tempPath, fileName);
                        if (!System.IO.File.Exists(targetFile))
                        {
                            System.IO.File.Copy(file, targetFile, true);
                        }
                    }
                }

                //copy images into temp path (if it exists)
                string imagePath = XAMLPath + "\\images";

                if (System.IO.Directory.Exists(imagePath))
                {
                    files = System.IO.Directory.GetFiles(imagePath);

                    if (files.Length > 0 && !System.IO.Directory.Exists(tempPath + "\\images"))
                    {
                        System.IO.Directory.CreateDirectory(tempPath + "\\images");
                    }

                    //Copy files and overwrite destination files if they exist
                    foreach (string file in files)
                    {
                        fileName = System.IO.Path.GetFileName(file);
                        targetFile = System.IO.Path.Combine(tempPath + "\\images\\", fileName);
                        if (!System.IO.File.Exists(targetFile))
                        {
                            System.IO.File.Copy(file, targetFile, true);
                        }
                    }
                }

                //copy video files into temp path (if it exists)
                string videoPath = XAMLPath + "\\video";

                if (System.IO.Directory.Exists(videoPath))
                {
                    files = System.IO.Directory.GetFiles(videoPath);

                    if (files.Length > 0 && !System.IO.Directory.Exists(tempPath + "\\video"))
                    {
                        System.IO.Directory.CreateDirectory(tempPath + "\\video");
                    }

                    //Copy files and overwrite destination files if they exist
                    foreach (string file in files)
                    {
                        fileName = System.IO.Path.GetFileName(file);
                        targetFile = System.IO.Path.Combine(tempPath + "\\video\\", fileName);
                        if (!System.IO.File.Exists(targetFile))
                        {
                            System.IO.File.Copy(file, targetFile, true);
                        }
                    }
                }

                //copy audio files into temp path (if it exists)
                string audioPath = XAMLPath + "\\audio";

                if (System.IO.Directory.Exists(audioPath))
                {
                    files = System.IO.Directory.GetFiles(audioPath);

                    if (files.Length > 0 && !System.IO.Directory.Exists(tempPath + "\\audio"))
                    {
                        System.IO.Directory.CreateDirectory(tempPath + "\\audio");
                    }

                    //Copy files and overwrite destination files if they exist
                    foreach (string file in files)
                    {
                        fileName = System.IO.Path.GetFileName(file);
                        targetFile = System.IO.Path.Combine(tempPath + "\\audio\\", fileName);
                        if (!System.IO.File.Exists(targetFile))
                        {
                            System.IO.File.Copy(file, targetFile, true);
                        }
                    }
                }

                sceneLoaded = true;
            }

            String scenePath = System.IO.Path.Combine(tempPath, sceneFile);

            StreamReader mysr = new StreamReader(scenePath);

            Page rootObject = XamlReader.Load(mysr.BaseStream) as Page;
            //Page rootObjectCopy = new Page();
            //ICloneable rootObjectContent = rootObject.Content as ICloneable;

            //rootObjectCopy.Content = ((ICloneable)rootObject.Content).Clone();

            InkCanvas newScene = LogicalTreeHelper.FindLogicalNode(rootObject, "scene") as InkCanvas;
            mtViewbox = LogicalTreeHelper.FindLogicalNode(rootObject, "viewboxScene") as Viewbox;

            initTranslateDeceleration = LogicalTreeHelper.FindLogicalNode(rootObject, "initTranslateDeceleration") as TextBlock;
            initRotateDeceleration = LogicalTreeHelper.FindLogicalNode(rootObject, "initRotateDeceleration") as TextBlock;
            initExpandDeceleration = LogicalTreeHelper.FindLogicalNode(rootObject, "initExpandDeceleration") as TextBlock;

            rootObject.Content = null;

            windowGrid.Children.Remove(viewBox);
            windowGrid.Children.Add(mtViewbox);
            
            scene = newScene;
            viewBox = mtViewbox;

            setupObjects();

            //reset ink menu options
            mnuSave.Visibility = Visibility.Visible;
            mnuInk.Header = "Start Ink";
            mnuErase.Header = "Eraser";

            //setup friction variables
            if (initTranslateDeceleration != null)
            {
                DecelerationTranslation = Convert.ToDouble(initTranslateDeceleration.Text);
            }

            Console.WriteLine(DecelerationTranslation);

            if (initExpandDeceleration != null)
            {
                DecelerationExpansion = Convert.ToDouble(initExpandDeceleration.Text);
            }

            if (initRotateDeceleration != null)
            {
                DecelerationRotation = Convert.ToDouble(initRotateDeceleration.Text);
            }

            if (!playback)
            {
                loadInk();
            }

            newScene.StrokeCollected += new InkCanvasStrokeCollectedEventHandler(inkCanvas_StrokeCollected);

            //rootObject.Content = newViewbox;
        }

        public void setupObjects()
        {
            //remove sound icon if it is loaded
            if (soundIconAdded)
            {
                iconMenu.Items.Remove(soundIconMenuItem);
                soundFile = null;
                soundIconAdded = false;
                soundPlaying = false;
            }

            //add inertia to the scene
            scene.ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(scene_ManipulationInertiaStarting);

            //Get children of scene
            foreach (Object obj in LogicalTreeHelper.GetChildren(scene))
            {
                String objectType = obj.GetType().ToString();
                DependencyObject depObj = obj as DependencyObject;
                UIElement currControl = new UIElement();

                String objectName = "";

                if (objectType == "System.Windows.Controls.Image")
                {
                    Image currImage = obj as Image;
                    objectName = currImage.Name;
                    currControl = currImage;
                }
                else if (objectType == "System.Windows.Shapes.Ellipse"
                            || objectType == "System.Windows.Shapes.Rectangle")
                {
                    Shape currShape = obj as Shape;
                    objectName = currShape.Name;
                    currControl = currShape;
                }
                else if (objectType == "System.Windows.Controls.StackPanel")
                {
                    StackPanel panel = obj as StackPanel;
                    MediaElement currVideo = panel.Children[0] as MediaElement;

                    if (panel.Children.Count > 1)
                    {
                        StackPanel buttons = panel.Children[1] as StackPanel;

                        Image playButton = buttons.Children[0] as Image;
                        Image pauseButton = buttons.Children[1] as Image;
                        Image stopButton = buttons.Children[2] as Image;

                        playButton.MouseUp += startMedia;
                        pauseButton.MouseUp += pauseMedia;
                        stopButton.MouseUp += stopMedia;

                        objectName = currVideo.Name;
                        currControl = currVideo;
                    }

                    //MediaElement currVideo = panel.Children[0] as MediaElement;
                    objectName = panel.Name;
                    currControl = panel;
                }
                else if (objectType == "System.Windows.Controls.TextBlock")
                {
                    TextBlock currText = obj as TextBlock;
                    objectName = currText.Name;
                    currControl = currText;
                }
                else if (objectType == "System.Windows.Controls.MediaElement")
                {
                    DependencyObject parent = LogicalTreeHelper.GetParent((DependencyObject)obj);
                    if (parent.ToString() == "System.Windows.Controls.Canvas")
                    {
                        soundFile = obj as MediaElement;

                        if (!soundIconAdded)
                        {
                            iconMenu.Items.Add(soundIconMenuItem);
                            soundIconMenuItem.Click += new RoutedEventHandler(soundIconMenuItem_Click);
                            soundIconAdded = true;
                        }
                    }
                }

                //attach neccessary event  handlers
                if (!objectName.Contains("None") || !objectName.Contains("Click"))
                {
                    currControl.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(initManipulationDelta);
                    currControl.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(initManipulationCompleted);
                }

                if (objectName.Contains("All"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationAll);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addRotateIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeRotateIcon);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addZoomIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeZoomIcon);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addPanIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removePanIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("Rotate"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationRotation);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addRotateIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeRotateIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("Translate"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationTranslation);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addPanIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removePanIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("Scale"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationScale);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addZoomIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeZoomIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("RotateTranslate"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationRotationTranslation);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addRotateIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeRotateIcon);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addPanIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removePanIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("RotateScale"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationRotationScale);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addZoomIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeZoomIcon);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addRotateIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeRotateIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("TranslateScale"))
                {
                    currControl.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(initManipulationTranslationScale);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addZoomIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removeZoomIcon);
                    currControl.TouchDown += new EventHandler<TouchEventArgs>(addPanIcon);
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(removePanIcon);

                    currControl.TouchDown += new EventHandler<TouchEventArgs>(bringToFront);
                }

                if (objectName.Contains("Click"))
                {
                    currControl.TouchUp += new EventHandler<TouchEventArgs>(initTouchUp);
                }
            }
        }

        private String resolveSourceURL(String oldSource)
        {
            return oldSource;
        }

        //Event handlers for different manipulation types:
        private void addRotateIcon(object sender, TouchEventArgs e)
        {
            rotateIconMenuItem.SetValue(MarginProperty, new Thickness(0, 0, 0, 0));

            if (!rotateIconAdded)
            {
                iconMenu.Items.Add(rotateIconMenuItem);
                rotateIconAdded = true;
            }
        }

        private void removeRotateIcon(object sender, TouchEventArgs e)
        {
            if (rotateIconAdded)
            {
                iconMenu.Items.Remove(rotateIconMenuItem);
                rotateIconAdded = false;
            }
        }
        private void addZoomIcon(object sender, TouchEventArgs e)
        {
            zoomIconMenuItem.SetValue(MarginProperty, new Thickness(0, 0, 0, 0));

            if (!zoomIconAdded)
            {
                iconMenu.Items.Add(zoomIconMenuItem);
                zoomIconAdded = true;
            }
        }

        private void removeZoomIcon(object sender, TouchEventArgs e)
        {
            if (zoomIconAdded)
            {
                iconMenu.Items.Remove(zoomIconMenuItem);
                zoomIconAdded = false;
            }
        }

        private void addPanIcon(object sender, TouchEventArgs e)
        {
            if (!panIconAdded)
            {
                iconMenu.Items.Add(panIconMenuItem);
                panIconAdded = true;
            }
        }

        private void removePanIcon(object sender, TouchEventArgs e)
        {
            if (panIconAdded)
            {
                iconMenu.Items.Remove(panIconMenuItem);
                panIconAdded = false;
            }
        }

        private void bringToFront(object sender, TouchEventArgs e)
        {
            UIElement element = (UIElement)sender;

            bringElementToFront(element);
        }

        private void bringElementToFront(UIElement element)
        {
            int maxZIndex = 0;

            //find highest z-index in the scene
            foreach (UIElement obj in LogicalTreeHelper.GetChildren(scene))
            {
                if (maxZIndex < Canvas.GetZIndex(obj))
                {
                    maxZIndex = Canvas.GetZIndex(obj);
                }
            }

            Canvas.SetZIndex(element, (maxZIndex + 1));
        }

        private void soundIconMenuItem_Click(object sender, EventArgs e)
        {
            if (!soundPlaying)
            {
                soundFile.Play();

                Image soundIcon = new Image();
                BitmapImage soundIconSource = new BitmapImage();
                soundIconSource.BeginInit();
                soundIconSource.UriSource = new Uri("icons/stop.png", UriKind.Relative);
                soundIconSource.EndInit();
                soundIcon.Source = soundIconSource;
                soundIcon.Width = 50;
                soundIcon.Height = 50;
                soundIconMenuItem.Icon = soundIcon;

                soundPlaying = true;
            }
            else
            {
                soundFile.Stop();
                
                Image soundIcon = new Image();
                BitmapImage soundIconSource = new BitmapImage();
                soundIconSource.BeginInit();
                soundIconSource.UriSource = new Uri("icons/play.png", UriKind.Relative);
                soundIconSource.EndInit();
                soundIcon.Source = soundIconSource;
                soundIcon.Width = 50;
                soundIcon.Height = 50;
                soundIconMenuItem.Icon = soundIcon;

                soundPlaying = false;
            }
        }

        //All gestures supported
        private void initManipulationAll(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.All;

            e.Handled = true;
        }

        private void initTouchUp(object sender, TouchEventArgs e)
        {
            FrameworkElement itemTouched = e.Source as FrameworkElement;

            TextBox sceneToLoadContainer = LogicalTreeHelper.FindLogicalNode((DependencyObject)scene, (itemTouched.Name + "_Scene")) as TextBox;

            LoadXAML(sceneToLoadContainer.Text);
        }

        //Panning supported
        private void initManipulationTranslation(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.Translate;

            e.Handled = true;
        }

        //Rotation supported
        private void initManipulationRotation(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.Rotate;
            e.Handled = true;
        }

        //Zoom supported
        private void initManipulationScale(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.Scale;
            e.Handled = true;
        }

        //No manipulations supported
        private void initManipulationNone(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.None;
            e.Handled = true;
        }

        //Rotate and Zoom supported
        private void initManipulationRotationScale(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.All;

            noTranslate = true; 

            e.Handled = true;
        }

        //Rotate and Translate supported
        private void initManipulationRotationTranslation(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.All;

            noScale = true; 

            e.Handled = true;
        }


        //Translate and Zoom supported
        private void initManipulationTranslationScale(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = scene;
            e.Mode = ManipulationModes.All;

            noRotate = true; 

            e.Handled = true;
        }

        private void IncreaseFriction(object sender, EventArgs e)
        {
            //increase deceleration
            DecelerationTranslation = DecelerationTranslation + DecelerationTranslation * 0.1;

            DecelerationExpansion = DecelerationExpansion + DecelerationExpansion * 0.1;

            DecelerationRotation = DecelerationRotation + DecelerationRotation * 0.1;

        }

        private void DecreaseFriction(object sender, EventArgs e)
        {
            //decrease deceleration
            DecelerationTranslation = DecelerationTranslation - DecelerationTranslation * 0.1;

            DecelerationExpansion = DecelerationExpansion - DecelerationExpansion * 0.1;

            DecelerationRotation = DecelerationRotation - DecelerationRotation * 0.1;
        }
        
        void scene_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = DecelerationTranslation * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = DecelerationExpansion * 96 / (1000.0 * 1000.0);
            
            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = DecelerationRotation * 360 / (1000.0 * 1000.0);

            e.Handled = true;
        }
        
        private void initManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            FrameworkElement element = e.Source as FrameworkElement;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(recordingFilename);
            XmlNode root = xmlDoc.DocumentElement;

            XmlNode eventsNode = xmlDoc.GetElementsByTagName("Events").Item(0);
            XmlElement scaleNode = xmlDoc.CreateElement("Scale");
            XmlElement rotateNode = xmlDoc.CreateElement("Rotate");
            XmlElement translateNode = xmlDoc.CreateElement("Translate");

            if (element != null)
            {                
                ManipulationDelta deltaManipulation = e.DeltaManipulation;

                Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;

                Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);

                center = matrix.Transform(center);

                //Zoom
                if (!noScale)
                {
                    if (deltaManipulation.Scale.X != 1 && deltaManipulation.Scale.Y != 1)
                    {
                        eventsNode.AppendChild(scaleNode);
                        scaleNode.SetAttribute("Element", element.Name);
                        scaleNode.SetAttribute("X", deltaManipulation.Scale.X.ToString());
                        scaleNode.SetAttribute("Y", deltaManipulation.Scale.Y.ToString());
                        scaleNode.SetAttribute("CenterX", center.X.ToString());
                        scaleNode.SetAttribute("CenterY", center.Y.ToString());
                    }

                    matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                }

                // Rotate
                if (!noRotate)
                {
                    if (deltaManipulation.Rotation != 0)
                    {
                        eventsNode.AppendChild(rotateNode);
                        rotateNode.SetAttribute("Element", element.Name);
                        rotateNode.SetAttribute("Rotation", deltaManipulation.Rotation.ToString());
                        rotateNode.SetAttribute("CenterX", center.X.ToString());
                        rotateNode.SetAttribute("CenterY", center.Y.ToString());
                    }

                    matrix.RotateAt(deltaManipulation.Rotation, center.X, center.Y);
                }

                //Pan
                if (!noTranslate)
                {
                    if (deltaManipulation.Translation.X != 0 && deltaManipulation.Translation.Y != 0)
                    {
                        eventsNode.AppendChild(translateNode);
                        translateNode.SetAttribute("Element", element.Name);
                        translateNode.SetAttribute("X", deltaManipulation.Translation.X.ToString());
                        translateNode.SetAttribute("Y", deltaManipulation.Translation.Y.ToString());
                    }
                    
                    matrix.Translate(deltaManipulation.Translation.X, deltaManipulation.Translation.Y);
                }

                xmlDoc.Save(recordingFilename);

                //((MatrixTransform)element.RenderTransform).Matrix = matrix;
                element.RenderTransform = new MatrixTransform(matrix);

                if (e.IsInertial)
                {
                    Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);
                    Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));

                    if (!containingRect.Contains(shapeBounds))
                    {
                        //let us know if we go outside the boundaries
                        e.ReportBoundaryFeedback(e.DeltaManipulation);

                        e.Complete();
                    }
                }

                e.Handled = true;
            }
        }

        private void inkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(recordingFilename);

            XmlNodeList sceneNodes = xmlDoc.GetElementsByTagName("Scene");
            XmlNode currentSceneNode = sceneNodes.Item(0);

            foreach(XmlNode sceneNode in sceneNodes)
            {
                String sceneFile = sceneNode.Attributes.Item(0).Value.ToString();

                if (sceneFile == sceneDir + "//" + sceneName + ".xaml")
                {
                    currentSceneNode = sceneNode;
                    break;
                }
            }

            XmlNode eventsNode = currentSceneNode.ChildNodes.Item(0);
            XmlElement inkNode = xmlDoc.CreateElement("InkStroke");
            eventsNode.AppendChild(inkNode);
            
            foreach (StylusPoint point in e.Stroke.StylusPoints)
            {

                XmlElement pointNode = xmlDoc.CreateElement("Point");
                inkNode.AppendChild(pointNode);

                pointNode.SetAttribute("X", point.X.ToString());
                pointNode.SetAttribute("Y", point.Y.ToString());
            }

            xmlDoc.Save(recordingFilename);
        }

        private void initManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            noTranslate = false; 
            noRotate = false;
            noScale = false;
        }

        private void startMedia(object sender, MouseButtonEventArgs e)
        {
            //DependencyObject senderElement = sender as DependencyObject;
            DependencyObject parent = LogicalTreeHelper.GetParent((DependencyObject)sender);
            DependencyObject panel = LogicalTreeHelper.GetParent((DependencyObject)parent);
            StackPanel buttonsPanel = ((StackPanel)panel).Children[1] as StackPanel;

            MediaElement video = ((StackPanel)panel).Children[0] as MediaElement;
            video.Play();

            Image playButton = e.Source as Image;
            playButton.Visibility = Visibility.Collapsed;
            Image pauseButton = buttonsPanel.Children[1] as Image;
            pauseButton.Visibility = Visibility.Visible;
        }

        private void stopMedia(object sender, MouseButtonEventArgs e)
        {
            //DependencyObject senderElement = sender as DependencyObject;
            DependencyObject parent = LogicalTreeHelper.GetParent((DependencyObject)sender);
            DependencyObject panel = LogicalTreeHelper.GetParent((DependencyObject)parent);
            StackPanel buttonsPanel = ((StackPanel)panel).Children[1] as StackPanel;

            MediaElement video = ((StackPanel)panel).Children[0] as MediaElement;
            video.Stop();

            Image playButton = buttonsPanel.Children[0] as Image;
            Image pauseButton = buttonsPanel.Children[1] as Image;
            pauseButton.Visibility = Visibility.Collapsed;
            playButton.Visibility = Visibility.Visible;
        }

        private void pauseMedia(object sender, MouseButtonEventArgs e)
        {
            //DependencyObject senderElement = sender as DependencyObject;
            DependencyObject parent = LogicalTreeHelper.GetParent((DependencyObject)sender);
            DependencyObject panel = LogicalTreeHelper.GetParent((DependencyObject)parent);
            StackPanel buttonsPanel = ((StackPanel)panel).Children[1] as StackPanel;

            MediaElement video = ((StackPanel)panel).Children[0] as MediaElement;
            video.Pause();

            Image playButton = buttonsPanel.Children[0] as Image;
            Image pauseButton = e.Source as Image;
            pauseButton.Visibility = Visibility.Collapsed;
            playButton.Visibility = Visibility.Visible;
        }

        private void StartInk(object sender, RoutedEventArgs e)
        {
            if (scene.EditingMode == InkCanvasEditingMode.None)
            {
                scene.EditingMode = InkCanvasEditingMode.Ink;
                mnuInk.Header = "Stop Ink";
            }
            else
            {
                scene.EditingMode = InkCanvasEditingMode.None;
                mnuInk.Header = "Start Ink";
            }
        }

        private void ClearInk(object sender, RoutedEventArgs e)
        {
            scene.Strokes.Clear();
        }

        private void EraseInk(object sender, RoutedEventArgs e)
        {
            if (scene.EditingMode == InkCanvasEditingMode.EraseByPoint)
            {
                scene.EditingMode = InkCanvasEditingMode.Ink;
                mnuErase.Header = "Eraser";
            }
            else
            {
                scene.EditingMode = InkCanvasEditingMode.EraseByPoint;
                mnuErase.Header = "Pen";
            }
        }

        private void SaveInk(object sender, RoutedEventArgs e)
        {
            String inkFile = sceneName + ".ink";
            StreamWriter writer = new StreamWriter(sceneDir + "\\" + inkFile);

            for (int i = 0; i < scene.Strokes.Count; i++)
            {
                for (int j = 0; j < scene.Strokes[i].StylusPoints.Count; j++)
                {
                    writer.WriteLine(scene.Strokes[i].StylusPoints[j].X);
                    writer.WriteLine(scene.Strokes[i].StylusPoints[j].Y);
                }
                writer.WriteLine("---");
            }

            writer.Close();
        }

        private void loadInk()
        {
            String inkFile = sceneName + ".ink";
            if(File.Exists(sceneDir + "\\" + inkFile))
            {
                StreamReader reader = new StreamReader(sceneDir + "\\" + inkFile);

                double x = -1;
                double y = -1;
                StylusPointCollection points = new StylusPointCollection();

                String s = reader.ReadLine();

                while (s != null)
                {
                    if (s != "")
                    {
                        x = Convert.ToDouble(s);
                    }

                    s = reader.ReadLine();

                    if (s != null && s != "")
                    {
                        y = Convert.ToDouble(s);
                    }

                    if (x != -1 && y != -1)
                    {
                        points.Add(new StylusPoint(x, y));
                    }

                    s = reader.ReadLine();

                    if (s == "---")
                    {
                        scene.Strokes.Add(new Stroke(points));
                        points = new StylusPointCollection();

                        s = reader.ReadLine();
                    }
                }

                reader.Close();

            }
        }

        private void OpenPresentationDialog(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogBox = new Microsoft.Win32.OpenFileDialog();

            dialogBox.DefaultExt = ".xaml";

            Nullable<bool> result = dialogBox.ShowDialog();

            String filename;

            if (result == true)
            {
                filename = dialogBox.FileName;

                if (File.Exists(filename))
                {
                    sceneLoaded = false;
                    LoadXAML(filename);
                }
            }
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void RecordSession(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveSession = new Microsoft.Win32.SaveFileDialog();
            saveSession.AddExtension = true;
            saveSession.DefaultExt = ".xml";

            Nullable<bool> result = saveSession.ShowDialog();

            if (result == true)
            {
                recordingFilename = saveSession.FileName;
                XmlDocument xmlDoc = new XmlDocument();

                XmlTextWriter xmlWriter = new XmlTextWriter(recordingFilename, System.Text.Encoding.UTF8);
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("Session");
                xmlWriter.Close();
                xmlDoc.Load(recordingFilename);

                XmlElement root = xmlDoc.DocumentElement;
                XmlElement sceneNode = xmlDoc.CreateElement("Scene");
                root.AppendChild(sceneNode);
                sceneNode.SetAttribute("FilePath", sceneDir + "//" + sceneName + ".xaml");

                XmlElement eventsNode = xmlDoc.CreateElement("Events");
                sceneNode.AppendChild(eventsNode);

                xmlDoc.Save(recordingFilename); 
            }
        }

        private void PlaybackSession(object sender, RoutedEventArgs e)
        {
            playback = true;

            Microsoft.Win32.OpenFileDialog dialogBox = new Microsoft.Win32.OpenFileDialog();

            dialogBox.DefaultExt = ".xml";

            Nullable<bool> result = dialogBox.ShowDialog();

            String filename;

            if (result == true)
            {
                filename = dialogBox.FileName;

                if (File.Exists(filename))
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    //load xml file
                    xmlDoc.Load(filename);

                    XmlElement session = xmlDoc.DocumentElement;
                    XmlNodeList sceneNodes = session.GetElementsByTagName("Scene");

                    //begin playback
                    playback = true;

                    //load scenes listed in xml file
                    foreach (XmlNode currScene in sceneNodes)
                    {
                        XmlElement sceneNode = currScene as XmlElement;
                        String scenePath = currScene.Attributes.GetNamedItem("FilePath").Value;
                        StreamReader mysr = new StreamReader(scenePath);

                        LoadXAML(scenePath);

                        //InkCanvas sessionScene = LogicalTreeHelper.FindLogicalNode(rootObject, "scene") as InkCanvas;
                        
                        //perform events on scene
                        XmlNode eventNode = sceneNode.GetElementsByTagName("Events").Item(0);

                        XmlNodeList events;

                        if (eventNode.HasChildNodes)
                        {
                            events = eventNode.ChildNodes;
                        }
                        else
                        {
                            continue;
                        }

                        foreach (XmlNode mtEvent in events)
                        {
                            String mtAction = mtEvent.Name;

                            if (mtAction == "InkStroke")
                            {
                                //get points
                                XmlNodeList mtInkPoints = mtEvent.ChildNodes;
                                StylusPoint defaultPoint;
                                StylusPointCollection defaultPoints = new StylusPointCollection();
                                Stroke mtStroke;

                                XmlElement inkPoint = mtInkPoints[0] as XmlElement;

                                String xVal = inkPoint.Attributes.GetNamedItem("X").Value;
                                String yVal = inkPoint.Attributes.GetNamedItem("Y").Value;

                                Double X = Convert.ToDouble(xVal);
                                Double Y = Convert.ToDouble(yVal);

                                defaultPoint = new StylusPoint(X, Y);
                                defaultPoints.Add(defaultPoint);
                                mtStroke = new Stroke(defaultPoints);

                                scene.Strokes.Add(mtStroke);
                                
                                for (int i = 1; i < mtInkPoints.Count; i++)
                                {
                                    inkPoint = mtInkPoints[i] as XmlElement;

                                    xVal = inkPoint.Attributes.GetNamedItem("X").Value;
                                    yVal = inkPoint.Attributes.GetNamedItem("Y").Value;

                                    X = Convert.ToDouble(xVal);
                                    Y = Convert.ToDouble(yVal);

                                    //draw ink stroke one section at a time
                                    mtStroke.StylusPoints.Add(new StylusPoint(X, Y));

                                    //scene.Strokes.Remove(mtStroke);
                                    //scene.Strokes.Add(mtStroke);

                                    Refresh(scene);
                                }
                            }
                            else
                            {
                                XmlAttributeCollection attributes = mtEvent.Attributes;
                                Double X, Y, CenterX, CenterY, Rotation;
                                String elementName = attributes.GetNamedItem("Element").Value;

                                // objects = LogicalTreeHelper.GetChildren(scene);
                                Object mtElement = LogicalTreeHelper.FindLogicalNode(mtViewbox, elementName);
                                String objectType = mtElement.GetType().ToString();
                                UIElement currControl = new UIElement();

                                switch (objectType)
                                {
                                    case "System.Windows.Controls.Image":
                                        currControl = mtElement as Image;
                                        break;
                                    case "System.Windows.Controls.StackPanel":
                                        currControl = mtElement as StackPanel;
                                        break;
                                    case "System.Windows.Controls.MediaElement":
                                        currControl = mtElement as MediaElement;
                                        break;
                                    case "System.Windows.Controls.TextBlock":
                                        currControl = mtElement as TextBlock;
                                        break;
                                    case "System.Windows.Shapes.Ellipse":
                                        currControl = mtElement as Ellipse;
                                        break;
                                    case "System.Windows.Shapes.Rectangle":
                                        currControl = mtElement as Rectangle;
                                        break;
                                }

                                Matrix matrix = ((MatrixTransform)currControl.RenderTransform).Matrix;

                                switch (mtAction)
                                {
                                    case "Rotate":
                                        Rotation = Convert.ToDouble(attributes.GetNamedItem("Rotation").Value);
                                        CenterX = Convert.ToDouble(attributes.GetNamedItem("CenterX").Value);
                                        CenterY = Convert.ToDouble(attributes.GetNamedItem("CenterY").Value);

                                        matrix.RotateAt(Rotation, CenterX, CenterY);
                                        break;
                                    case "Scale":
                                        CenterX = Convert.ToDouble(attributes.GetNamedItem("CenterX").Value);
                                        CenterY = Convert.ToDouble(attributes.GetNamedItem("CenterY").Value);
                                        X = Convert.ToDouble(attributes.GetNamedItem("X").Value);
                                        Y = Convert.ToDouble(attributes.GetNamedItem("Y").Value);

                                        matrix.ScaleAt(X, Y, CenterX, CenterY);
                                        break;
                                    case "Translate":
                                        X = Convert.ToDouble(attributes.GetNamedItem("X").Value);
                                        Y = Convert.ToDouble(attributes.GetNamedItem("Y").Value);

                                        matrix.Translate(X, Y);
                                        break;
                                }
                                currControl.RenderTransform = new MatrixTransform(matrix);
                                bringElementToFront(currControl);
                                //currControl

                                Refresh(currControl);
                            }

                            System.Threading.Thread.Sleep(50);

                        }

                    }

                    //apply each action to the scene
                }
            }
        }
        private delegate void NoArgDelegate();
        public static void Refresh(DependencyObject obj)
        {
            obj.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Loaded,
                (NoArgDelegate)delegate { });
        }
    }

}
