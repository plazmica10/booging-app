using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for AutoCompleteBox.xaml
    /// </summary>
    public partial class AutoCompleteLocation : UserControl
    {

        public static readonly DependencyProperty SelectedLocationProperty =
            DependencyProperty.Register(nameof(SelectedLocation), typeof(Location), typeof(AutoCompleteLocation), new PropertyMetadata(null));

        public Location? SelectedLocation
        {
            get => (Location)GetValue(SelectedLocationProperty);
            set => SetValue(SelectedLocationProperty, value);
        }

        private readonly Dictionary<string, Location> _suggestions = new Dictionary<string, Location>();
        private readonly LocationService _locationService = new LocationService();

        public AutoCompleteLocation()
        {
            InitializeComponent();

            foreach (Location location in _locationService.GetAll())
            {
                _suggestions.TryAdd(location.ToReadableString(), location);
            }
        }
        
        private void AutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void Select()
        {
            string? text = SuggestionsListBox.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                AutoCompleteTextBox.Text = text;
                SelectedLocation = _suggestions[text];
                AutoCompletePopup.IsOpen = false;
                AutoCompleteTextBox.CaretIndex = AutoCompleteTextBox.Text.Length;
            }
        }
    }
}
