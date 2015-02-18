using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace DoubleSidebarMenu
{
    public sealed class HamburgerMenu : ContentControl
    {
        #region ScrollViewerProperty

        private ScrollViewer ScrollViewer;

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (ScrollViewer.HorizontalOffset < Viewport)
                ScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Near;
            else if (ScrollViewer.HorizontalOffset > Viewport)
                ScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Far;
        }

        #endregion

        public HamburgerMenu()
        {
            this.DefaultStyleKey = typeof(HamburgerMenu);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ChangeView(Viewport, null, null);
        }

        protected override void OnApplyTemplate()
        {
            ScrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
            ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            base.OnApplyTemplate();
        }

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));
        #endregion

        #region LeftContent
        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register("LeftContent", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));
        #endregion

        #region RightContent
        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));
        #endregion

        #region Position
        public SnapPointsAlignment Position
        {
            get { return (SnapPointsAlignment)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(SnapPointsAlignment), typeof(HamburgerMenu), new PropertyMetadata(SnapPointsAlignment.Near));
        #endregion

        #region Viewport
        public double Viewport
        {
            get { return (double)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register("Viewport", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(300d));
        #endregion
    }

    public class HamburgerPanel : Panel, IScrollSnapPointsInfo
    {

        #region Position
        public SnapPointsAlignment Position
        {
            get { return (SnapPointsAlignment)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(SnapPointsAlignment), typeof(HamburgerPanel), new PropertyMetadata(SnapPointsAlignment.Near));
        #endregion

        #region Viewport
        public double Viewport
        {
            get { return (double)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register("Viewport", typeof(double), typeof(HamburgerPanel), new PropertyMetadata(300d));
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            var sidebar = Children[0];
            var content = Children[1];
            var sidebar2 = Children[2];

            sidebar.Measure(new Size(Viewport, availableSize.Height));
            content.Measure(new Size(Window.Current.Bounds.Width, availableSize.Height));
            sidebar2.Measure(new Size(Viewport, availableSize.Height));

            return new Size(Viewport * 2 + Window.Current.Bounds.Width, availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var sidebar = Children[0];
            var content = Children[1];
            var sidebar2 = Children[2];

            sidebar.Arrange(new Rect(0, 0, Viewport, finalSize.Height));
            content.Arrange(new Rect(Viewport, 0, Window.Current.Bounds.Width, finalSize.Height));
            sidebar2.Arrange(new Rect(Viewport + Window.Current.Bounds.Width, 0, Viewport, finalSize.Height));

            return finalSize;
        }

        public bool AreHorizontalSnapPointsRegular
        {
            get { return false; }
        }

        public bool AreVerticalSnapPointsRegular
        {
            get { throw new NotImplementedException(); }
        }

        public IReadOnlyList<float> GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment)
        {
            if (orientation == Orientation.Horizontal)
            {
                return new float[] { 0.0f, (float)Viewport, (float)(Viewport + Window.Current.Bounds.Width), (float)(Viewport * 2 + Window.Current.Bounds.Width) };
            }

            throw new NotImplementedException();
        }

        public float GetRegularSnapPoints(Orientation orientation, SnapPointsAlignment alignment, out float offset)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<object> HorizontalSnapPointsChanged;

        public event EventHandler<object> VerticalSnapPointsChanged;
    }
}
