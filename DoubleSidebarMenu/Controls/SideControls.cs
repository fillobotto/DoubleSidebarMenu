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
    public sealed class SideMenu : ContentControl
    {
        #region ScrollViewerProperty

        private ScrollViewer ScrollViewer;

        #endregion

        public void toggleLeft()
        {
            if (Math.Round(ScrollViewer.HorizontalOffset, 0) == Viewport)
            {
                var Succes = ScrollViewer.ChangeView(0.0d, null, null);

            }
            else if (Math.Round(ScrollViewer.HorizontalOffset, 0) == 0)
            {
                var Succes = ScrollViewer.ChangeView(Viewport, null, null);
            }

        }

        public void toggleCenter()
        {
            ScrollViewer.ChangeView(Viewport, null, null);
        }

        public void toggleRight()
        {
            if (Math.Round(ScrollViewer.HorizontalOffset, 0) == Viewport * 2)
            {
                var period = TimeSpan.FromMilliseconds(0);
                Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        var Succes = ScrollViewer.ChangeView(Viewport, null, null);
                    });
                }, period);
            }
            else if (Math.Round(ScrollViewer.HorizontalOffset, 0) == Viewport)
            {
                var period = TimeSpan.FromMilliseconds(0);
                Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        var Succes = ScrollViewer.ChangeView(Viewport * 2, null, null);
                    });
                }, period);
                
            }

        }

        public SideMenu()
        {
            this.DefaultStyleKey = typeof(SideMenu);
            //this.Loaded += OnLoaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var period = TimeSpan.FromMilliseconds(0);
            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var Succes = ScrollViewer.ChangeView(Viewport, null, null, true);
                });
            }, period);
        }

        protected override void OnApplyTemplate()
        {
            ScrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
            //ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            base.OnApplyTemplate();
        }

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(SideMenu), new PropertyMetadata(null));
        #endregion

        #region LeftContent
        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register("LeftContent", typeof(object), typeof(SideMenu), new PropertyMetadata(null));
        #endregion

        #region RightContent
        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(SideMenu), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(SideMenu), new PropertyMetadata(null));
        #endregion

        #region Position
        public SnapPointsAlignment Position
        {
            get { return (SnapPointsAlignment)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(SnapPointsAlignment), typeof(SideMenu), new PropertyMetadata(SnapPointsAlignment.Near));
        #endregion

        #region Viewport
        public double Viewport
        {
            get { return (double)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register("Viewport", typeof(double), typeof(SideMenu), new PropertyMetadata(300d));
        #endregion
    }

    public class SidePanel : Panel, IScrollSnapPointsInfo
    {

        #region Position
        public SnapPointsAlignment Position
        {
            get { return (SnapPointsAlignment)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(SnapPointsAlignment), typeof(SidePanel), new PropertyMetadata(SnapPointsAlignment.Near));
        #endregion

        #region Viewport
        public double Viewport
        {
            get { return (double)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register("Viewport", typeof(double), typeof(SidePanel), new PropertyMetadata(300d));
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
            get { return false; }
        }

        public IReadOnlyList<float> GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment)
        {

            //return new float[] { 0.0f, (float)Viewport, (float)(Viewport + Window.Current.Bounds.Width), (float)(Viewport * 2 + Window.Current.Bounds.Width) };
            return new float[] { 0.0f, (float)Viewport, (float)(Viewport * 2)};


        }

        public float GetRegularSnapPoints(Orientation orientation, SnapPointsAlignment alignment, out float offset)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<object> HorizontalSnapPointsChanged;

        public event EventHandler<object> VerticalSnapPointsChanged;
    }
}
