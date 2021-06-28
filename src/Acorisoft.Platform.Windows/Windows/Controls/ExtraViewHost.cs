using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Acorisoft.Platform.Windows.Threadings;

namespace Acorisoft.Platform.Windows.Controls
{
    public static class IxContentHostCommands 
    {
        
    }
    
    public enum SwipeDirection
    {
        ToLeft,
        ToRight,
        ToTop,

        /// <summary>
        /// 从上到下
        /// </summary>
        ToBottom
    }
    public delegate void SwipeStartingHandler(SwipeDirection direction);
    public delegate void SwipeProcessingHandler(double delta, SwipeDirection direction);

    public class SwipeRecognitor
    {

        class PointQueue
        {
            private Point _start,_last,_current;
            private int _time;

            public void Prepare(Point point)
            {
                _start = point;
                _current = point;
                _time++;
            }

            public void Set(Point point)
            {
                _last = _current;
                _current = point;
                _time++;
            }

            public void Unset()
            {
                _time = 0;
            }

            public double DeltaX => _current.X - _last.X;
            public double DeltaY => _current.Y - _last.Y;
            public double StartDeltaX => _current.X - _start.X;
            public double StartDeltaY => _current.Y - _start.Y;
            public bool IsEnable => _time > 5;
            public bool IsPrepare => _time < 1;
            public bool IsDetect => _time == 6;
        }


        private DispatcherTimer _timer;
        private double _delta = 0;
        private SwipeDirection _direction;
        private readonly PointQueue _queue;
        private readonly FrameworkElement _element;

        public SwipeRecognitor(FrameworkElement element)
        {
            _queue = new PointQueue();
            _element = element ?? throw new ArgumentNullException(nameof(element));
            Threshould = 160;
        }

        void Sampling(object sender, EventArgs e)
        {
            if (!IsEnable)
            {
                return;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                var position = Mouse.GetPosition(_element);

                if (_queue.IsPrepare)
                {
                    _queue.Prepare(position);
                }
                else
                {
                    _queue.Set(position);
                }
                if (_queue.IsEnable)
                {
                    Detect();
                }
            }
            else if (_queue.IsEnable)
            {
                _delta = 0;
                //
                // Fire SwipeCompleted Event
                SwipeCompleted?.Invoke(this, new EventArgs());
                _queue.Unset();
            }
        }

        void Detect()
        {
            var deltaX = _queue.DeltaX;
            var deltaY = _queue.DeltaY;

            if (_queue.IsDetect)
            {
                _direction = GetSwipeDirection(_queue.StartDeltaX, _queue.StartDeltaY);
                SwipeStarting?.Invoke(_direction);
                // Fire SwipeStarting

                SwipeStarted?.Invoke(0, _direction);

            }
            else
            {

                _delta = Math.Clamp(_delta + GetDelta(_direction, deltaX, deltaY), 0d, 1d);

                // Fire SwipeProcessing
                SwipeProcessing?.Invoke(_delta, _direction);
            }
        }

        static SwipeDirection GetSwipeDirection(double x, double y)
        {
            var angle = GetAngle(x , y);

            // -pi          -180 180
            // -3/4 pi      -135 225
            // -1/2 pi      -90  270
            // -1/4 pi      -45  315

            if (Range(angle, 0, 48) || Range(angle, -45, 0))
            {
                return SwipeDirection.ToRight;
            }

            if (Range(angle, -180, -135) || Range(angle, 135, 180))
            {
                return SwipeDirection.ToLeft;
            }

            if (Range(angle, 45, 135))
            {
                return SwipeDirection.ToTop;
            }

            if (Range(angle, -135, -45))
            {
                return SwipeDirection.ToBottom;
            }

            return SwipeDirection.ToBottom;
        }

        static bool Range(double value, double min, double max)
        {
            return min <= value && value < max;
        }

        double GetDelta(SwipeDirection direction, double x, double y)
        {
            switch (direction)
            {
                default:
                case SwipeDirection.ToBottom:
                    return y / Threshould;
                case SwipeDirection.ToLeft:
                    return -x / Threshould;
                case SwipeDirection.ToRight:
                    return x / Threshould;
                case SwipeDirection.ToTop:
                    return -y / Threshould;
            }
        }

        static int GetAngle(double x, double y)
        {
            return (int)(Math.Atan2(-y, x) * 180 / Math.PI);
        }

        public void SetSampler(DispatcherTimer sampler)
        {
            if (_timer != null)
            {
                _timer.Tick -= Sampling;
                _timer.Stop();
            }

            _timer = sampler ?? throw new ArgumentNullException(nameof(sampler));
            _timer.Tick += Sampling;
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            // _Timer.Start();
        }

        public event EventHandler SwipeCompleted;
        public event SwipeStartingHandler SwipeStarting;
        public event SwipeProcessingHandler SwipeStarted;
        public event SwipeProcessingHandler SwipeProcessing;

        /// <summary>
        /// 获取或设置一个值，该值用于指示全局 <see cref="SwipeRecognitor"/> 实例，跳过手势识别。
        /// </summary>
        public static bool IsEnable { get; set; } = false;
        public int Threshould { get; set; }
    }
    
    public class ExtraViewHost : ContentControl
    {
        private readonly Point _zero = new Point(0, 0);
         public const string IxLeftName      = "PART_IxLeft";
        public const string IxRightName     = "PART_IxRight";
        public const string IxUpName        = "PART_IxUp";
        public const string IxDownName      = "PART_IxDown";

        static ExtraViewHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtraViewHost), new FrameworkPropertyMetadata(typeof(ExtraViewHost)));
        }

        private readonly DispatcherTimer _sampler;
        private readonly SwipeRecognitor _recognitor;

        private ExtraViewContentControl _ixLeft;
        private ExtraViewContentControl _ixRight;
        private ExtraViewContentControl _ixUp;
        private ExtraViewContentControl _ixDown;

        public ExtraViewHost()
        {
            _sampler = DispatcherTimerFactory.Create(DispatcherPriority.Normal, Dispatcher);
            _recognitor = new SwipeRecognitor(this);
            _recognitor.SetSampler(_sampler);
            _recognitor.SwipeStarting += OnSwipeStarting;
            _recognitor.SwipeProcessing += OnSwipeProcessing;
            _recognitor.SwipeCompleted += OnSwipeCompleted;

            //
            //
            CommandBindings.Add(new CommandBinding(PlatformCommands.ToggleIxLeft, ToggleIxLeft , AlwaysCanExecute));
            CommandBindings.Add(new CommandBinding(PlatformCommands.ToggleIxRight, ToggleIxRight, AlwaysCanExecute));
            CommandBindings.Add(new CommandBinding(PlatformCommands.ToggleIxUp, ToggleIxUp, AlwaysCanExecute));
            CommandBindings.Add(new CommandBinding(PlatformCommands.ToggleIxDown, ToggleIxDown, AlwaysCanExecute));
            this.Unloaded += OnUnloaded;
        }


        private void OnSwipeStarting(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.ToRight:
                    (_ixRight.Delta > .65 ? _ixRight : _ixLeft).IxState = InteractiveContentState.Dragging;
                    break;
                case SwipeDirection.ToBottom:
                    (_ixDown.Delta > .65 ? _ixDown : _ixUp).IxState = InteractiveContentState.Dragging;
                    break;
                case SwipeDirection.ToLeft:
                    (_ixLeft.Delta > .65 ? _ixLeft : _ixRight).IxState = InteractiveContentState.Dragging;
                    break;
                case SwipeDirection.ToTop:
                    (_ixUp.Delta > .65 ? _ixUp : _ixDown).IxState = InteractiveContentState.Dragging;
                    break;
            }
        }

        public void SetContentState(InteractiveContentState state)
        {
            _ixLeft.IxState =
                _ixUp.IxState =
                    _ixRight.IxState =
                        _ixDown.IxState = state;
        }

        private void OnSwipeCompleted(object sender, EventArgs e)
        {
            SetContentState(InteractiveContentState.Idle);
        }

        private void OnSwipeProcessing(double delta, SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.ToRight:
                    if (_ixRight.Delta > 0)
                    {

                        _ixRight.Delta = 1 - delta;
                    }
                    else
                    {
                        _ixLeft.Delta = delta;
                    }
                    break;
                case SwipeDirection.ToBottom:
                    if (_ixDown.Delta > 0)
                    {

                        _ixDown.Delta = 1 - delta;
                    }
                    else
                    {
                        _ixUp.Delta = delta;
                    }
                    break;
                case SwipeDirection.ToLeft:
                    if (_ixLeft.Delta > 0)
                    {

                        _ixLeft.Delta = 1 - delta;
                    }
                    else
                    {
                        _ixRight.Delta = delta;
                    }
                    break;
                case SwipeDirection.ToTop:
                    if(_ixUp.Delta > 0)
                    {

                        _ixUp.Delta = 1 - delta;
                    }
                    else
                    {
                        _ixDown.Delta = delta;
                    }
                    break;
            }
        }

        #region Toggle IxContentOpenState

        void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        void ToggleIxRight(object sender, ExecutedRoutedEventArgs e)
        {
            _ixRight.IsOpen = !_ixRight.IsOpen;
        }

        void ToggleIxUp(object sender, ExecutedRoutedEventArgs e)
        {
            _ixUp.IsOpen = !_ixUp.IsOpen;
        }
        void ToggleIxDown(object sender, ExecutedRoutedEventArgs e)
        {
            _ixDown.IsOpen = !_ixDown.IsOpen;
        }
        void ToggleIxLeft(object sender, ExecutedRoutedEventArgs e)
        {
            _ixLeft.IsOpen = !_ixLeft.IsOpen;
        }
        void ToggleSwipe(object sender, ExecutedRoutedEventArgs e)
        {
            SwipeRecognitor.IsEnable = !SwipeRecognitor.IsEnable;
        }

        #endregion Toggle IxContentOpenState

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _sampler.Stop();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _ixDown.Arrange(new Rect(_zero, arrangeBounds));
            _ixRight.Arrange(new Rect(_zero, arrangeBounds));
            _ixLeft.Arrange(new Rect(_zero, arrangeBounds));
            _ixUp.Arrange(new Rect(_zero, arrangeBounds));
            return base.ArrangeOverride(arrangeBounds);
        }

        public override void OnApplyTemplate()
        {
            _ixLeft = (ExtraViewContentControl)GetTemplateChild(IxLeftName);
            _ixRight = (ExtraViewContentControl)GetTemplateChild(IxRightName);
            _ixUp = (ExtraViewContentControl)GetTemplateChild(IxUpName);
            _ixDown = (ExtraViewContentControl)GetTemplateChild(IxDownName);

            _ixLeft.Sampler =
            _ixRight.Sampler =
            _ixUp.Sampler =
            _ixDown.Sampler = _sampler;

            _ixLeft.ParentElement =
            _ixRight.ParentElement =
            _ixUp.ParentElement =
            _ixDown.ParentElement = this;

            _sampler.Start();
        }
        
        public static readonly DependencyProperty QuickViewStringFormatProperty = DependencyProperty.Register(
            "QuickViewStringFormat",
            typeof(string),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty QuickViewTemplateSelectorProperty = DependencyProperty.Register(
            "QuickViewTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty QuickViewTemplateProperty = DependencyProperty.Register(
            "QuickViewTemplate",
            typeof(DataTemplate),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ExtraViewStringFormatProperty = DependencyProperty.Register(
            "ExtraViewStringFormat",
            typeof(string),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ExtraViewTemplateSelectorProperty = DependencyProperty.Register(
            "ExtraViewTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ExtraViewTemplateProperty = DependencyProperty.Register(
            "ExtraViewTemplate",
            typeof(DataTemplate),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ExtraViewProperty = DependencyProperty.Register(
            "ExtraView",
            typeof(object),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContextViewProperty = DependencyProperty.Register(
            "ContextView",
            typeof(object),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContextViewStringFormatProperty = DependencyProperty.Register(
            "ContextViewStringFormat",
            typeof(string),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContextViewTemplateSelectorProperty = DependencyProperty.Register(
            "ContextViewTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContextViewTemplateProperty = DependencyProperty.Register(
            "ContextViewTemplate",
            typeof(DataTemplate),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty QuickViewProperty = DependencyProperty.Register(
            "QuickView",
            typeof(object),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ToolViewStringFormatProperty = DependencyProperty.Register(
            "ToolViewStringFormat",
            typeof(string),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ToolViewTemplateSelectorProperty = DependencyProperty.Register(
            "ToolViewTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ToolViewTemplateProperty = DependencyProperty.Register(
            "ToolViewTemplate",
            typeof(DataTemplate),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ToolViewProperty = DependencyProperty.Register(
            "ToolView",
            typeof(object),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));



        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Brush),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));
        public static readonly DependencyProperty TitleBarStringFormatProperty = DependencyProperty.Register(
            "TitleBarStringFormat",
            typeof(string),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarTemplateSelectorProperty = DependencyProperty.Register(
            "TitleBarTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarTemplateProperty = DependencyProperty.Register(
            "TitleBarTemplate",
            typeof(DataTemplate),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(
            "TitleBar",
            typeof(object),
            typeof(ExtraViewHost),
            new PropertyMetadata(null));
        
        public object QuickView
        {
            get => (object)GetValue(QuickViewProperty);
            set => SetValue(QuickViewProperty, value);
        }

        public DataTemplate QuickViewTemplate
        {
            get => (DataTemplate)GetValue(QuickViewTemplateProperty);
            set => SetValue(QuickViewTemplateProperty, value);
        }

        public DataTemplateSelector QuickViewTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(QuickViewTemplateSelectorProperty);
            set => SetValue(QuickViewTemplateSelectorProperty, value);
        }

        public string QuickViewStringFormat
        {
            get => (string)GetValue(QuickViewStringFormatProperty);
            set => SetValue(QuickViewStringFormatProperty, value);
        }

        public object ExtraView
        {
            get => (object)GetValue(ExtraViewProperty);
            set => SetValue(ExtraViewProperty, value);
        }

        public DataTemplate ExtraViewTemplate
        {
            get => (DataTemplate)GetValue(ExtraViewTemplateProperty);
            set => SetValue(ExtraViewTemplateProperty, value);
        }

        public DataTemplateSelector ExtraViewTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ExtraViewTemplateSelectorProperty);
            set => SetValue(ExtraViewTemplateSelectorProperty, value);
        }

        public string ExtraViewStringFormat
        {
            get => (string)GetValue(ExtraViewStringFormatProperty);
            set => SetValue(ExtraViewStringFormatProperty, value);
        }

        public object ContextView
        {
            get => (object)GetValue(ContextViewProperty);
            set => SetValue(ContextViewProperty, value);
        }

        public DataTemplate ContextViewTemplate
        {
            get => (DataTemplate)GetValue(ContextViewTemplateProperty);
            set => SetValue(ContextViewTemplateProperty, value);
        }

        public DataTemplateSelector ContextViewTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ContextViewTemplateSelectorProperty);
            set => SetValue(ContextViewTemplateSelectorProperty, value);
        }

        public string ContextViewStringFormat
        {
            get => (string)GetValue(ContextViewStringFormatProperty);
            set => SetValue(ContextViewStringFormatProperty, value);
        }

        public object ToolView
        {
            get => (object)GetValue(ToolViewProperty);
            set => SetValue(ToolViewProperty, value);
        }

        public DataTemplate ToolViewTemplate
        {
            get => (DataTemplate)GetValue(ToolViewTemplateProperty);
            set => SetValue(ToolViewTemplateProperty, value);
        }

        public DataTemplateSelector ToolViewTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ToolViewTemplateSelectorProperty);
            set => SetValue(ToolViewTemplateSelectorProperty, value);
        }

        public string ToolViewStringFormat
        {
            get => (string)GetValue(ToolViewStringFormatProperty);
            set => SetValue(ToolViewStringFormatProperty, value);
        }


        public Brush Color
        {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public object TitleBar
        {
            get => (object)GetValue(TitleBarProperty);
            set => SetValue(TitleBarProperty, value);
        }

        public DataTemplate TitleBarTemplate
        {
            get => (DataTemplate)GetValue(TitleBarTemplateProperty);
            set => SetValue(TitleBarTemplateProperty, value);
        }

        public DataTemplateSelector TitleBarTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(TitleBarTemplateSelectorProperty);
            set => SetValue(TitleBarTemplateSelectorProperty, value);
        }

        public string TitleBarStringFormat
        {
            get => (string)GetValue(TitleBarStringFormatProperty);
            set => SetValue(TitleBarStringFormatProperty, value);
        }
    }

    #region ExtraViewContentControl

    

    
    public enum InteractiveContentState
    {
        Idle,
        Dragging,
        Expanding,
        Collapsing
    }

    public interface IExtraViewContentControl
    {
        /// <summary>
        /// 
        /// </summary>
        ExpandDirection Direction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        InteractiveContentState IxState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DispatcherTimer Sampler { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值表示当前的 <see cref="IExtraViewContentControl"/> 是否单独使用。
        /// </summary>
        bool IsStandalone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        EasingFunctionBase EasingFunction { get; set; }
    }
    
    public class ExtraViewContentControl : ContentControl
    {
         #region Transformers

        protected abstract class TranslateTransformer
        {
            public abstract void Transform(FrameworkElement parent, FrameworkElement element, double delta);
        }
        protected sealed class Top2BottomTransformer : TranslateTransformer
        {
            public override void Transform(FrameworkElement parent, FrameworkElement element, double delta)
            {
                var height = element.ActualHeight;
                var transform = new TranslateTransform(0, -height + height * delta);
                element.RenderTransform = transform;
            }
        }
        protected sealed class Bottom2TopTransformer : TranslateTransformer
        {
            public override void Transform(FrameworkElement parent, FrameworkElement element, double delta)
            {
                var height = element.ActualHeight;
                var transform = new TranslateTransform(0, height - height * delta);
                element.RenderTransform = transform;
            }
        }
        protected sealed class Left2RightTransformer : TranslateTransformer
        {
            public override void Transform(FrameworkElement parent, FrameworkElement element, double delta)
            {
                var width = element.ActualWidth;
                var transform = new TranslateTransform(-width + delta * width,0);
                element.RenderTransform = transform;
            }
        }
        protected sealed class Right2LeftTransformer : TranslateTransformer
        {
            public override void Transform(FrameworkElement parent, FrameworkElement element, double delta)
            {
                var width = element.ActualWidth;
                var transform = new TranslateTransform(parent.ActualWidth - width * delta, 0);
                element.RenderTransform = transform;
            }
        }


        #endregion Transformers
        
         public static readonly DependencyProperty SamplerProperty = DependencyProperty.Register(
            "Sampler",
            typeof(DispatcherTimer),
            typeof(ExtraViewContentControl),
            new PropertyMetadata(null, OnSamplerChanged));


        public static readonly DependencyProperty IsStandaloneProperty = DependencyProperty.Register(
            "IsStandalone",
            typeof(bool),
            typeof(ExtraViewContentControl),
            new PropertyMetadata(Xaml.Box(false), OnIsStandableChanged));


        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
            "EasingFunction",
            typeof(EasingFunctionBase),
            typeof(ExtraViewContentControl),
            new PropertyMetadata(null));


        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            "Direction",
            typeof(ExpandDirection),
            typeof(ExtraViewContentControl),
            new PropertyMetadata(ExpandDirection.Down, OnDirectionChanged));


        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen",
            typeof(bool),
            typeof(ExtraViewContentControl),
            new PropertyMetadata(Xaml.Box(false), OnIsOpenChanged));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ixContent = (ExtraViewContentControl) d;
            if ((bool) e.NewValue)
            {
                if (ixContent._delta < 1)
                {
                    ixContent.IxState = InteractiveContentState.Expanding;
                }
            }
            else
            {
                if (ixContent._delta > 0)
                {
                    ixContent.IxState = InteractiveContentState.Collapsing;
                }
            }
        }

        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var direction = (ExpandDirection) e.NewValue;
            var tcc = (ExtraViewContentControl) d;
            switch (direction)
            {
                case ExpandDirection.Down:
                    tcc._transformer = new Top2BottomTransformer();
                    break;
                case ExpandDirection.Up:
                    tcc._transformer = new Bottom2TopTransformer();
                    break;
                case ExpandDirection.Left:
                    tcc._transformer = new Right2LeftTransformer();
                    break;
                case ExpandDirection.Right:
                    tcc._transformer = new Left2RightTransformer();
                    break;
            }
        }

        private static void OnSamplerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DispatcherTimer)
            {
                var ixContent = (ExtraViewContentControl) d;
                ixContent.OnSamplerChanged(e.NewValue as DispatcherTimer, e.OldValue as DispatcherTimer);
            }
        }

        private static void OnIsStandableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                d.SetValue(SamplerProperty, new DispatcherTimer(DispatcherPriority.Render, d.Dispatcher));
            }
        }

        private TranslateTransformer    _transformer;
        private double                  _delta;
        private ContentPresenter        _presenter;
        private InteractiveContentState _state;

        public ExtraViewContentControl()
        {
            _transformer = new Top2BottomTransformer();
            this.SizeChanged += OnSizeChanged;
        }

        void Sampling(object sender, EventArgs e)
        {
            switch (_state)
            {
                case InteractiveContentState.Idle:
                    if(_delta == 0)
                    {
                        return;
                    }
                    if(_delta > .65)
                    {
                        IxState = InteractiveContentState.Expanding;
                    }
                    else if(_delta < .65)
                    {
                        IxState = InteractiveContentState.Collapsing;
                    }
                    return;
                case InteractiveContentState.Collapsing:
                    _delta = Math.Clamp(_delta - 0.023, 0, 1);
                    OnPerformancePosition(_delta);
                    if (_delta == 0)
                    {
                        IxState = InteractiveContentState.Idle;
                    }
                    break;
                case InteractiveContentState.Expanding:
                    _delta = Math.Clamp(_delta + 0.06, 0, 1);
                    OnPerformancePosition(_delta);
                    if(_delta == 1)
                    {
                        IxState = InteractiveContentState.Idle;
                    }
                    break;
            }
        }

        #region OnSizeChanged / OnChildDesiredSizeChanged / OnContentChanged


        protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_presenter != null)
            {
                _transformer?.Transform(this, _presenter, Delta);
            }
        }

        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            if (_presenter != null)
            {
                _transformer?.Transform(this, _presenter, Delta);
            }
            base.OnChildDesiredSizeChanged(child);
        }
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (_presenter != null)
            {
                _transformer?.Transform(this, _presenter, Delta);
            }
            base.OnContentChanged(oldContent, newContent);
        }


        #endregion OnSizeChanged / OnChildDesiredSizeChanged / OnContentChanged

        public override void OnApplyTemplate()
        {
            var count = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < count; i++)
            {
                if (VisualTreeHelper.GetChild(this, i) is ContentPresenter presenter)
                {
                    _presenter = presenter;
                    _transformer?.Transform(this, presenter, Delta);
                }
            }

            Sampler.Start();
            base.OnApplyTemplate();
        }

        protected void OnPerformancePosition(double delta)
        {
            if(_transformer != null && _presenter != null)
            {
                // 
                // _Transformer.Transform(ParentElement ?? this, _Presenter, EasingFunction?.Ease(delta) ?? delta);
                _transformer.Transform(ParentElement ?? this, _presenter, delta);
            }
        }

        protected void OnSamplerChanged(DispatcherTimer newSampler, DispatcherTimer oldSampler)
        {
            newSampler.Tick += Sampling;

            if (oldSampler != null)
            {
                oldSampler.Stop();
                oldSampler.Tick -= Sampling;
                newSampler.Start();
            }
        }
        
        public bool IsStandalone
        {
            get => (bool)GetValue(IsStandaloneProperty);
            set => SetValue(IsStandaloneProperty, value);
        }

        public DispatcherTimer Sampler
        {
            get => (DispatcherTimer)GetValue(SamplerProperty);
            set => SetValue(SamplerProperty, value);
        }


        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }


        /// <summary>
        /// 获取或设置当前状态。
        /// </summary>
        public InteractiveContentState IxState { get => _state; set => _state = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 只有当 <see cref="IxState"/>的状态为 <see cref="InteractiveContentState.Dragging"/> 的情况下才能直接修改当前属性的值。
        /// </remarks>
        public double Delta
        {
            get => _delta;
            set
            {
                if(IxState == InteractiveContentState.Dragging)
                {
                    _delta = Math.Clamp(value, 0, 1);
                    OnPerformancePosition(_delta);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExpandDirection Direction
        {
            get => (ExpandDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public EasingFunctionBase EasingFunction
        {
            get => (EasingFunctionBase)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement ParentElement { get; set; }
    }
    
    
    #endregion
}