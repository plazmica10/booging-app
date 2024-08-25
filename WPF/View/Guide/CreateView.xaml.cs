using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class CreateView : Page
    {
        
        public CreateView(User currentUser,NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new CreateViewModel(currentUser, navigationService);
            NameTextBox.Focus();

            CreateViewModel.RequestFocusNameTextBox += OnRequestFocusNameTextBox;
            Unloaded += CreateView_Unloaded;
        }
        private void CreateView_Unloaded(object sender, RoutedEventArgs e)
        {
            CreateViewModel.RequestFocusNameTextBox -= OnRequestFocusNameTextBox;
        }

        private void HourTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Ensure the entered text is a digit and the total length is not more than 2
            e.Handled = !IsNumericKey(e.Text) || (sender as TextBox).Text.Length >= 2;
        }

        private void MinuteTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Ensure the entered text is a digit and the total length is not more than 2
            e.Handled = !IsNumericKey(e.Text) || (sender as TextBox).Text.Length >= 2;
        }

        private void SecondTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Ensure the entered text is a digit and the total length is not more than 2
            e.Handled = !IsNumericKey(e.Text) || (sender as TextBox).Text.Length >= 2;
        }
        private void HourTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.Length == 2)
            {
                MinuteTextBox.Focus();
            }
        }

        private void MinuteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.Length == 2)
            {
                SecondTextBox.Focus();
            }
        }

        private void SecondTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.Length == 2)
            {
                // Move focus to the next control if needed, or keep focus
            }
        }
        private bool IsNumericKey(string text)
        {
            // Check if the entered text is numeric
            return int.TryParse(text, out _);
        }
        private void OnRequestFocusNameTextBox()
        {
            NameTextBox.Focus();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F10)
            {
                NameTextBox.Focus();
                e.Handled = true;
            }
        }
        private bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        private bool IsValidLocation(string location) => !string.IsNullOrWhiteSpace(location);


        private bool IsValidDescription(string description)
        {
            return !string.IsNullOrEmpty(description);
        }

        private bool IsValidLanguage(string language) => !string.IsNullOrWhiteSpace(language);


        private bool IsValidNumTourists(string numTourists)
        {
            int result;
            return int.TryParse(numTourists, out result) && result > 0;
        }

        private bool IsValidDuration(string duration)
        {
            int result;
            return int.TryParse(duration, out result) && result > 0;
        }

      

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (IsValidName(NameTextBox.Text))
                {
                    selectedLocation.Focus();
                }
                else
                {
                    NameTextBox.Focus();
                }
            }
        }



        private void LocationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (IsValidLocation(selectedLocation.Text)) // Assuming selectedLocation is a custom control with a Text property
                {
                    DescriptionTextBox.Focus();
                }
                else
                {
                    selectedLocation.Focus();
                }
            }
        }

        private void DescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (IsValidDescription(DescriptionTextBox.Text))
                {
                    selectedLanguage.Focus();
                }
                else
                {
                    DescriptionTextBox.Focus();
                }
            }
        }

        private void LanguageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (IsValidLanguage(selectedLanguage.Text)) // Assuming selectedLanguage is a custom control with a Text property
                {
                    NumTouristsTextBox.Focus();
                }
                else
                {
                    selectedLanguage.Focus();
                }
            }
        }

        private void NumTouristsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsNumericKey(e.Key) && e.Key != Key.Enter)
            {
                e.Handled = true; // Prevent the key event from being processed further
            }

            if (e.Key == Key.Enter)
            {
                if (IsValidNumTourists(NumTouristsTextBox.Text))
                {
                    DurationTextBox.Focus();
                }
                else
                {
                    NumTouristsTextBox.Focus();
                }
            }
        }

        private void DurationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsNumericKey(e.Key) && e.Key != Key.Enter)
            {
                e.Handled = true; 
            }

            if (e.Key == Key.Enter)
            {
                if (IsValidDuration(DurationTextBox.Text))
                {
                    DatePicker.Focus();
                    OpenDatePicker();
                }
                else
                {
                    DurationTextBox.Focus();
                }
            }
        }
        private bool IsNumericKey(Key key)
        {
            if ((key >= Key.D0 && key <= Key.D9) ||
                (key >= Key.NumPad0 && key <= Key.NumPad9) ||
                key == Key.Back || key == Key.Delete ||
                key == Key.Tab || key == Key.Enter ||
                key == Key.Left || key == Key.Right ||
                key == Key.Up || key == Key.Down)
            {
                return true;
            }
            return false;
        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OpenDatePicker();
            }
        }

        private bool isAddDateButtonFocused = false;

      
        private void PeakPointTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddPeakPointButton.Focus();
            }
        }


        private void OpenDatePicker()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                DatePicker.IsDropDownOpen = true;
            }));
        }

    }
}
