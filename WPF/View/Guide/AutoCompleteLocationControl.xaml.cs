using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for AutoCompleteLocationControl.xaml
    /// </summary>
    public partial class AutoCompleteLocationControl : UserControl
    {
        public AutoCompleteLocationControl()
        {
            InitializeComponent();
            foreach (Location location in _locationService.GetAll())
            {
                _suggestions.TryAdd(location.ToReadableString(), location);
            }
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var location in _locationService.GetAll())
            {
                _suggestions.TryAdd(location.ToReadableString(), location);
            }

            // Assuming DescriptionTextBox is defined in the parent view
            var parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is Page page)
            {
                DescriptionTextBox = page.FindName("DescriptionTextBox") as TextBox;
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(AutoCompleteLocationControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.Register(nameof(KeyDownCommand), typeof(ICommand), typeof(AutoCompleteLocationControl));

        public ICommand KeyDownCommand
        {
            get => (ICommand)GetValue(KeyDownCommandProperty);
            set => SetValue(KeyDownCommandProperty, value);
        }

        private void AutoCompleteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownCommand?.CanExecute(e) ?? false)
            {
                KeyDownCommand.Execute(e);
            }
        }
     
        private void AutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = AutoCompleteTextBox.Text;
        }
        public static readonly DependencyProperty IsControlEnabledProperty =
            DependencyProperty.Register(nameof(IsControlEnabled), typeof(bool?), typeof(AutoCompleteLocationControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsControlEnabledChanged));

        public bool? IsControlEnabled
        {
            get => (bool)GetValue(IsControlEnabledProperty);
            set => SetValue(IsControlEnabledProperty, value);
        }

        private static void OnIsControlEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteLocationControl control = (AutoCompleteLocationControl)d;
            control.OnIsControlEnabledChanged((bool)e.NewValue);
        }

        protected virtual void OnIsControlEnabledChanged(bool newValue)
        {
            AutoCompleteTextBox.IsEnabled = (bool)IsControlEnabled;
        }


        public static readonly DependencyProperty SelectedLocationProperty =
            DependencyProperty.Register(nameof(SelectedLocation), typeof(Location), typeof(AutoCompleteLocationControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedLocationChanged));

        private static void OnSelectedLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteLocationControl control = (AutoCompleteLocationControl)d;
            control.OnSelectedLocationChanged((Location)e.NewValue);
        }

        protected virtual void OnSelectedLocationChanged(Location newValue)
        {
            AutoCompleteTextBox.Text = SelectedLocation == null ? "" : SelectedLocation.ToReadableString();
        }

        public Location? SelectedLocation
        {
            get => (Location)GetValue(SelectedLocationProperty);
            set => SetValue(SelectedLocationProperty, value);
        }

        private readonly Dictionary<string, Location> _suggestions = new Dictionary<string, Location>();
        private readonly LocationService _locationService = new LocationService();


        private void AutoCompleteTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            string text = AutoCompleteTextBox.Text.ToLower();

            List<string> filteredSuggestions = new List<string>();

            foreach (string key in _suggestions.Keys)
            {
                if (key.ToLower().Contains(text.ToLower()) && filteredSuggestions.Count <= 20) // show only 20
                {
                    filteredSuggestions.Add(key);
                }
            }

            if (filteredSuggestions.Any())
            {
                SuggestionsListBox.ItemsSource = filteredSuggestions;
                AutoCompletePopup.IsOpen = true;
            }
            else
            {
                AutoCompletePopup.IsOpen = false;
            }

            if (string.IsNullOrEmpty(text)) { SelectedLocation = null; }
        }

     

        private void AutoCompleteTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && SuggestionsListBox.Items.Count > 0)
            {
                SuggestionsListBox.Focus();
                SuggestionsListBox.SelectedIndex = 0;
            }
        }

        private void SuggestionsListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Select();
        }
        private void SuggestionsListBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: Select(); break;
                case Key.Escape: AutoCompletePopup.IsOpen = false; break;
                case Key.Back: AutoCompletePopup.IsOpen = false; e.Handled = true; AutoCompleteTextBox.Focus(); break;
            }
        }
        private TextBox DescriptionTextBox;

        private void Select()
        {
            string? text = SuggestionsListBox.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                AutoCompleteTextBox.Text = text;
                SelectedLocation = _suggestions[text];
                AutoCompletePopup.IsOpen = false;
                AutoCompleteTextBox.CaretIndex = AutoCompleteTextBox.Text.Length;

                // Find the parent window
                DescriptionTextBox.Focus();
            }
        }
        }

        
    }

