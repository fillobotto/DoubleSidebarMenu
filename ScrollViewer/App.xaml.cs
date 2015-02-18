using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Il modello di applicazione vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=391641

namespace App1
{
    /// <summary>
    /// Fornisci un comportamento specifico dell'applicazione in supplemento alla classe Application predefinita.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Inizializza l'oggetto Application singleton. Si tratta della prima riga del codice creato
        /// eseguita e, come tale, corrisponde all'equivalente logico di main() o WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Richiamato quando l'applicazione viene avviata normalmente dall'utente.  All'avvio dell'applicazione
        /// verranno utilizzati altri punti di ingresso per aprire un file specifico, per visualizzare
        /// risultati di ricerche e così via.
        /// </summary>
        /// <param name="e">Dettagli sulla richiesta e il processo di avvio.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Non ripetere l'inizializzazione dell'applicazione se la finestra già dispone di contenuto,
            // assicurarsi solo che la finestra sia attiva
            if (rootFrame == null)
            {
                // Creare un frame che agisca da contesto di navigazione e passare alla prima pagina
                rootFrame = new Frame();

                // TODO: modificare questo valore su una dimensione di cache appropriata per l'applicazione
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Caricare lo stato dall'applicazione sospesa in precedenza
                }

                // Posizionare il frame nella finestra corrente
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Rimuove l'avvio della navigazione turnstile.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // Quando lo stack di navigazione non viene ripristinato, esegui la navigazione alla prima pagina,
                // configurando la nuova pagina per passare le informazioni richieste come parametro di
                // navigazione
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Assicurarsi che la finestra corrente sia attiva
            Window.Current.Activate();
        }

        /// <summary>
        /// Ripristina le transizioni del contenuto dopo l'avvio dell'applicazione.
        /// </summary>
        /// <param name="sender">Oggetto a cui è associato il gestore.</param>
        /// <param name="e">Dettagli sull'evento di navigazione.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Richiamato quando l'esecuzione dell'applicazione viene sospesa.  Lo stato dell'applicazione viene salvato
        /// senza che sia noto se l'applicazione verrà terminata o ripresa con il contenuto
        /// della memoria ancora integro.
        /// </summary>
        /// <param name="sender">Origine della richiesta di sospensione.</param>
        /// <param name="e">Dettagli relativi alla richiesta di sospensione.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Salvare lo stato dell'applicazione e interrompere qualsiasi attività in background
            deferral.Complete();
        }
    }

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
            //if (Children.Count < 2 || Children.Count > 2)
            //{
            //    return base.MeasureOverride(availableSize);
            //}

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
            //if (Children.Count < 2 || Children.Count > 2)
            //{
            //    return base.ArrangeOverride(finalSize);
            //}

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
                //if (Position == TestHamburger.Position.Near)
                //{
                return new float[] { 0.0f, (float)Viewport, (float)(Viewport + Window.Current.Bounds.Width), (float)(Viewport * 2 + Window.Current.Bounds.Width) };
                //}
                //else
                //{
                //    return new float[] { 0.0f, (float)Window.Current.Bounds.Width };
                //}
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