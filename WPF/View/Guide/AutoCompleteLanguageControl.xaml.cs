using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for AutoCompleteLanguageControl.xaml
    /// </summary>
    public partial class AutoCompleteLanguageControl : UserControl
    {
        private TextBox NumTouristsTextBox;

        public AutoCompleteLanguageControl()
        {
            InitializeComponent();
            foreach (Language language in _languageService.GetAll())
            {
                _suggestions.Add(language.ToReadableString(), language);
            }
            Loaded += OnLoaded;

        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var location in _languageService.GetAll())
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
                NumTouristsTextBox = page.FindName("NumTouristsTextBox") as TextBox;
            }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(AutoCompleteLanguageControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.Register(nameof(KeyDownCommand), typeof(ICommand), typeof(AutoCompleteLanguageControl));

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
            DependencyProperty.Register(nameof(IsControlEnabled), typeof(bool?), typeof(AutoCompleteLanguageControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsControlEnabledChanged));

        public bool? IsControlEnabled
        {
            get => (bool)GetValue(IsControlEnabledProperty);
            set => SetValue(IsControlEnabledProperty, value);
        }

        private static void OnIsControlEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteLanguageControl control = (AutoCompleteLanguageControl)d;
            control.OnIsControlEnabledChanged((bool)e.NewValue);
        }

        protected virtual void OnIsControlEnabledChanged(bool newValue)
        {
            AutoCompleteTextBox.IsEnabled = (bool)IsControlEnabled;
        }


        public static readonly DependencyProperty SelectedLanguageProperty =
            DependencyProperty.Register(nameof(SelectedLanguage), typeof(Language), typeof(AutoCompleteLanguageControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedLanguageChanged));

        private static void OnSelectedLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteLanguageControl control = (AutoCompleteLanguageControl)d;
            control.OnSelectedLanguageChanged((Language)e.NewValue);
        }

        protected virtual void OnSelectedLanguageChanged(Language newValue)
        {
            AutoCompleteTextBox.Text = SelectedLanguage == null ? "" : SelectedLanguage.ToReadableString();
        }

        public Language? SelectedLanguage
        {
            get => (Language)GetValue(SelectedLanguageProperty);
            set => SetValue(SelectedLanguageProperty, value);
        }

        private readonly Dictionary<string, Language> _suggestions = new Dictionary<string, Language>();
        private readonly LanguageService _languageService = new LanguageService();


        private void AutoCompleteTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            string text = AutoCompleteTextBox.Text.ToLower();

            List<string> filteredSuggestions = new List<string>();

            foreach (string key in _suggestions.Keys)
            {
                if (key.ToLower().Contains(text.ToLower()))
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

            if (string.IsNullOrEmpty(text)) { SelectedLanguage = null; }
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
        private void Select()
        {
            string? text = SuggestionsListBox.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                AutoCompleteTextBox.Text = text;
                SelectedLanguage = _suggestions[text];
                AutoCompletePopup.IsOpen = false;
                AutoCompleteTextBox.CaretIndex = AutoCompleteTextBox.Text.Length;
                NumTouristsTextBox.Focus();
            }
        }


    }
}
