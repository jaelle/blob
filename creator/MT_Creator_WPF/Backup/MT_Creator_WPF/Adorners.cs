using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace MT_Creator_WPF
{
    class Adorners: Adorner
    {
        Thumb tl, tr, bl, br;
        VisualCollection visualChildren;
        Window1 w_Cur;
        enum TYPE { TEXT, IMAGE, VIDEO, AUDIO, UNKNOWN };

        public Adorners(UIElement adornedElement, Window1 w)
            : base(adornedElement)
        {
            w_Cur = w;
            visualChildren = new VisualCollection(this);

            MakeAdorner(ref tl, Cursors.SizeNWSE);
            MakeAdorner(ref tr, Cursors.SizeNESW);
            MakeAdorner(ref bl, Cursors.SizeNESW);
            MakeAdorner(ref br, Cursors.SizeNWSE);

            bl.DragDelta += new DragDeltaEventHandler(HandleBL);
            br.DragDelta += new DragDeltaEventHandler(HandleBR);
            tl.DragDelta += new DragDeltaEventHandler(HandleTL);
            tr.DragDelta += new DragDeltaEventHandler(HandleTR);
        }

        void MakeAdorner(ref Thumb tmb, Cursor customizedCursor)
        {
            if (tmb != null) return;

            tmb = new Thumb();
            
            // Decorate the adorner.
            tmb.Cursor = customizedCursor;
            tmb.Height = tmb.Width = 15;
            tmb.Opacity = .8;
            tmb.Background = new SolidColorBrush(Colors.Orange);

            visualChildren.Add(tmb);
        }

        //Make sure that the element can't be bigger than the parent or NaN.
        void SizeChecker(FrameworkElement elm)
        {
            if (elm.Width.Equals(Double.NaN))
                elm.Width = elm.DesiredSize.Width;
            if (elm.Height.Equals(Double.NaN))
                elm.Height = elm.DesiredSize.Height;

            FrameworkElement parent = elm.Parent as FrameworkElement;
            if (parent != null)
            {
                elm.MaxHeight = parent.ActualHeight;
                elm.MaxWidth = parent.ActualWidth;
            }
        }

        // Adorner Handlers.
        void HandleBR(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;
            if (adornedElement == null || hitThumb == null) return;
            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;
            SizeChecker(adornedElement);
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
            UpdateElm((int)adornedElement.Height, (int)adornedElement.Width, (int)Canvas.GetLeft(adornedElement), (int)Canvas.GetTop(adornedElement));
        }

        // Top Right handler.
        void HandleTR(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;
            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            SizeChecker(adornedElement);
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;            
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
            UpdateElm((int)height_new, (int)adornedElement.Width, (int)(Canvas.GetTop(adornedElement)), (int)(top_old - (height_new - height_old)));
        }

        // Handler for resizing from the top-left.
        void HandleTL(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            SizeChecker(adornedElement);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;
            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));

            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
            UpdateElm((int)height_new, (int)adornedElement.Width, (int)Canvas.GetTop(adornedElement), (int)(top_old - (height_new - height_old)));
        }            

        // Bottom-left
        void HandleBL(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            SizeChecker(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            //adornedElement.Width = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;
            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));
            UpdateElm((int)adornedElement.Height, (int)width_new, (int)(left_old - (width_new - width_old)), (int)Canvas.GetTop(adornedElement));
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            tl.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            tr.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bl.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            br.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }

        private void UpdateElm(int height, int width, int x, int y)
        {
            int loc = 0;
            Point myPoint = new Point(x, y);
            if(w_Cur.selectedElement.GetType().Equals(typeof(Image)))
            {
                loc = w_Cur.findElement((int)TYPE.IMAGE);
            }
            if(w_Cur.selectedElement.GetType().Equals(typeof(MediaElement)))
            {
                loc = w_Cur.findElement((int)TYPE.VIDEO);
            }
            if(w_Cur.selectedElement.GetType().Equals(typeof(TextBlock)))
            {
                loc = w_Cur.findElement((int)TYPE.TEXT);
            }

            w_Cur.myScene[loc].width = width;
            w_Cur.myScene[loc].height = height;
            w_Cur.myScene[loc].coords = myPoint;
        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

    }


}
