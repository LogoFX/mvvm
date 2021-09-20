using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.View.Controls
{
    /// <summary>
    /// Content control with header which is displayed in popup.
    /// </summary>
    /// <seealso cref="HeaderedContentControl" />
    /// <seealso cref="IUpdateVisualState" />
    [TemplatePart(Name = "Popup",Type = typeof(Popup))]
    [TemplatePart(Name = "ClickHandler", Type = typeof(FrameworkElement))]
    public class PopupHeaderedContentControl : HeaderedContentControl, IUpdateVisualState
    {
        private PopupHelper InternalPopup = null;

        /// <summary>
        /// Occurs when drop down is opening.
        /// </summary>
        public event RoutedPropertyChangingEventHandler<bool> DropDownOpening;

        /// <summary>
        /// Occurs when drop down is opened.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<bool> DropDownOpened;

        /// <summary>
        /// Occurs when drop down is closing.
        /// </summary>
        public event RoutedPropertyChangingEventHandler<bool> DropDownClosing;

        /// <summary>
        /// Occurs when drop down is closed.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<bool> DropDownClosed;

#if NET || NETCORE || NETFRAMEWORK
        static PopupHeaderedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupHeaderedContentControl), new FrameworkPropertyMetadata(typeof(PopupHeaderedContentControl)));
        }
#endif        
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupHeaderedContentControl"/> class.
        /// </summary>
        public PopupHeaderedContentControl()
        {
#if WINDOWS_UWP || NETFXCORE
            this.DefaultStyleKey = typeof(PopupHeaderedContentControl);
#endif
            Interaction = new InteractionHelper(this);
        }

        #region PopupHorizontalOffset dependency property

        /// <summary>
        /// Gets or sets the popup horizontal offset.
        /// </summary>
        /// <value>
        /// The popup horizontal offset.
        /// </value>
        public double PopupHorizontalOffset
        {
            get { return (double)GetValue(PopupHorizontalOffsetProperty); }
            set { SetValue(PopupHorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// The popup horizontal offset property
        /// </summary>
        public static readonly DependencyProperty PopupHorizontalOffsetProperty =
            DependencyProperty.Register("PopupHorizontalOffset", typeof (double), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(double), OnPopupOffsetChanged));

        private static void OnPopupOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {}

        #endregion

        #region PopupVerticalOffset dependency property

        /// <summary>
        /// Gets or sets the popup vertical offset.
        /// </summary>
        /// <value>
        /// The popup vertical offset.
        /// </value>
        public double PopupVerticalOffset
        {
            get { return (double)GetValue(PopupVerticalOffsetProperty); }
            set { SetValue(PopupVerticalOffsetProperty, value); }
        }

        /// <summary>
        /// The popup vertical offset property
        /// </summary>
        public static readonly DependencyProperty PopupVerticalOffsetProperty =
            DependencyProperty.Register("PopupVerticalOffset", typeof (double), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(double), OnPopupVerticalOffsetChanged));

        private static void OnPopupVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {}

        #endregion

        #region PopupPlacement dependency property

        /// <summary>
        /// Gets or sets the popup placement.
        /// </summary>
        /// <value>
        /// The popup placement.
        /// </value>
        public PopupPlacement PopupPlacement
        {
            get { return (PopupPlacement)GetValue(PopupPlacementProperty); }
            set { SetValue(PopupPlacementProperty, value); }
        }

        /// <summary>
        /// The popup placement property
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement", typeof (PopupPlacement), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(PopupPlacement), OnPopupPlacementChanged));

        private static void OnPopupPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {}

        #endregion

        #region IsPopupOpen dependency property

        /// <summary>
        /// Gets or sets a value indicating whether this instance is popup open.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is popup open; otherwise, <c>false</c>.
        /// </value>
        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        /// <summary>
        /// The is popup open property
        /// </summary>
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register("IsPopupOpen", typeof (bool), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(bool), OnIsPopupOpenChanged));

        private bool _popupHasOpened;
        private bool _ignorePropertyChange;
        private InteractionHelper Interaction;
        private FrameworkElement ClickHandler;

        private static void OnIsPopupOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PopupHeaderedContentControl source = d as PopupHeaderedContentControl;

            // Ignore the change if requested
            if (source._ignorePropertyChange)
            {
                source._ignorePropertyChange = false;
                return;
            }

            bool oldValue = (bool)e.OldValue;
            bool newValue = (bool)e.NewValue;

            if (newValue)
            {
                source.OpeningDropDown(false);
                if (source.InternalPopup != null)
                {
                    source.InternalPopup.Arrange();
                }
            }
            else
            {
                source.ClosingDropDown(oldValue);
            }

            source.UpdateVisualState(true);            
        }

        #endregion

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (InternalPopup != null)
            {
                InternalPopup.Closed -= DropDownPopup_Closed;
               
                InternalPopup.FocusChanged -= OnDropDownFocusChanged;
                InternalPopup.UpdateVisualStates -= OnDropDownPopupUpdateVisualStates;
                InternalPopup.BeforeOnApplyTemplate();
                InternalPopup = null;
            }

            base.OnApplyTemplate();

            // Set the template parts. Individual part setters remove and add 
            // any event handlers.
            Popup popup = GetTemplateChild("Popup") as Popup;
            if (popup != null)
            {
                InternalPopup = new PopupHelper(this, popup);
                //todo
                InternalPopup.MaxDropDownHeight = 300;
                InternalPopup.AfterOnApplyTemplate();
                InternalPopup.Closed += DropDownPopup_Closed;
                InternalPopup.FocusChanged += OnDropDownFocusChanged;
                InternalPopup.UpdateVisualStates += OnDropDownPopupUpdateVisualStates;
            }

            ClickHandler = GetTemplateChild("ClickHandler") as FrameworkElement;
            if (ClickHandler != null) 
                ClickHandler.MouseLeftButtonDown += ClickHandler_MouseLeftButtonDown;

            Interaction.OnApplyTemplateBase();

            // If the drop down property indicates that the popup is open,
            // flip its value to invoke the changed handler.
            if (IsPopupOpen && InternalPopup != null && !InternalPopup.IsOpen)
            {
                OpeningDropDown(false);
            }
        }

        private void OnDropDownFocusChanged(object sender, EventArgs e)
        {
            if (IsPopupOpen)
                ClosingDropDown(true); 
        }

        void ClickHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!IsPopupOpen)
                OpeningDropDown(false);
            else
                ClosingDropDown(true);
            e.Handled = false;
        }

        private void OpeningDropDown(bool oldValue)
        {
            RoutedPropertyChangingEventArgs<bool> args = new RoutedPropertyChangingEventArgs<bool>(IsPopupOpenProperty, oldValue, true, true);

            // Opening
            OnDropDownOpening(args);

            if (args.Cancel)
            {
                _ignorePropertyChange = true;
                SetValue(IsPopupOpenProperty, oldValue);
            }
            else
            {
                //RaiseExpandCollapseAutomationEvent(oldValue, true);
                OpenDropDown(oldValue, true);
            }
            UpdateVisualState(true);
        }

        private void OpenDropDown(bool oldValue, bool newValue)
        {
            IsPopupOpen = true;
            if (InternalPopup != null)
            {
                InternalPopup.IsOpen = true;
            }
            _popupHasOpened = true;
            OnDropDownOpened(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue));
        }

        /// <summary>
        /// Raises the <see cref="DropDownOpening" /> event.
        /// </summary>
        /// <param name="e">The <see cref="bool"/> instance containing the event data.</param>
        protected virtual void OnDropDownOpening(RoutedPropertyChangingEventArgs<bool> e)
        {
            RoutedPropertyChangingEventHandler<bool> handler = DropDownOpening;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DropDownOpened"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDropDownOpened(RoutedPropertyChangedEventArgs<bool> e)
        {
            RoutedPropertyChangedEventHandler<bool> handler = DropDownOpened;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DropDownClosing" /> event.
        /// </summary>
        /// <param name="e">The <see cref="bool"/> instance containing the event data.</param>
        protected virtual void OnDropDownClosing(RoutedPropertyChangingEventArgs<bool> e)
        {
            RoutedPropertyChangingEventHandler<bool> handler = DropDownClosing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DropDownClosed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="bool"/> instance containing the event data.</param>
        protected virtual void OnDropDownClosed(RoutedPropertyChangedEventArgs<bool> e)
        {
            RoutedPropertyChangedEventHandler<bool> handler = DropDownClosed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void DropDownPopup_Closed(object sender, EventArgs e)
        {
            // Force the drop down dependency property to be false.
            if (IsPopupOpen)
            {
                IsPopupOpen = false;
            }

            // Fire the DropDownClosed event
            if (_popupHasOpened)
            {
                OnDropDownClosed(new RoutedPropertyChangedEventArgs<bool>(true, false));
            }
        }

        internal virtual void UpdateVisualState(bool useTransitions)
        {
            // Popup
            VisualStateManager.GoToState(this, IsPopupOpen ? VisualStates.StatePopupOpened : VisualStates.StatePopupClosed, useTransitions);

            // Handle the Common and Focused states
            Interaction.UpdateVisualStateBase(useTransitions);
        }

        private void OnDropDownPopupUpdateVisualStates(object sender, EventArgs e)
        {
            UpdateVisualState(true);
        }

        private void ClosingDropDown(bool oldValue)
        {
            bool delayedClosingVisual = false;
            if (InternalPopup != null)
            {
                delayedClosingVisual = InternalPopup.UsesClosingVisualState;
            }

            RoutedPropertyChangingEventArgs<bool> args = new RoutedPropertyChangingEventArgs<bool>(IsPopupOpenProperty, oldValue, false, true);

            OnDropDownClosing(args);

            if (args.Cancel)
            {
                _ignorePropertyChange = true;
                SetValue(IsPopupOpenProperty, oldValue);
            }
            else
            {
                // Immediately close the drop down window:
                // When a popup closed visual state is present, the code path is 
                // slightly different and the actual call to CloseDropDown will 
                // be called only after the visual state's transition is done
                //RaiseExpandCollapseAutomationEvent(oldValue, false);
                if (!delayedClosingVisual)
                {
                    CloseDropDown(oldValue, false);
                }
            }
            UpdateVisualState(true);
        }

        private void CloseDropDown(bool oldValue, bool newValue)
        {
            if (_popupHasOpened)
            {
                if (InternalPopup != null)
                {
                    InternalPopup.IsOpen = false;
                }
                OnDropDownClosed(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue));
            }
        }

        void IUpdateVisualState.UpdateVisualState(bool useTransitions)
        {
            UpdateVisualState(useTransitions);
        }
    }
}
