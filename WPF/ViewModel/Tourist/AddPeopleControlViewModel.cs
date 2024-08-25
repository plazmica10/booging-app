using BookingApp.Command;
using BookingApp.Domain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class AddPeopleControlViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public User CurrentUser;

        private Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public bool HasErrors => Errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : Enumerable.Empty<string>();
        }

        public void Validate(string propertyName, object propertyValue)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

            Errors.Remove(propertyName);
            if (results.Any())
            {
                var res = results.Select(r => r.ErrorMessage).ToList();
                res.Reverse();
                Errors[propertyName] = res;
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            AddCommand.RaiseCanExecuteChanged();
        }

        private string _firstName;
        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[a-zA-Z]+(?:[-'\s][a-zA-Z]+)*$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                Validate(nameof(FirstName), value);
            }
        }
        
        private string _lastName;
        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[a-zA-Z]+(?:[-'\s][a-zA-Z]+)*$", ErrorMessage = "Last name can only contain letters.")]
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                Validate(nameof(LastName), value);
            }
        }
        
        private string _email;
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                Validate(nameof(Email), value);
            }
        }

        private string _birthYear;
        [Required(ErrorMessage = "Birth year is required.")]
        public string BirthYear
        {
            get => _birthYear;
            set
            {
                _birthYear = value;
                OnPropertyChanged(nameof(BirthYear));

                int currentYear = DateTime.Now.Year;

                Errors.Remove(nameof(BirthYear));
                if (!int.TryParse(BirthYear, out int birthYear))
                {
                    Errors[nameof(BirthYear)] = new List<string> { "Invalid birth year." };
                }
                else if (birthYear < 1800)
                {
                    Errors[nameof(BirthYear)] = new List<string> { "Birth year can't be less than 1800." };
                }
                else if (birthYear > currentYear)
                {
                    Errors[nameof(BirthYear)] = new List<string> { $"Birth year can't be greater than the current year ({currentYear})." };
                }

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(BirthYear));
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<ItemReservationViewModel> Reservations { get; set; } = new();
        public RelayCommand AddCommand { get; }
        public ICommand RemoveCommand => new RelayCommand<ItemReservationViewModel>(RemoveAction);
        public bool CanExecuteAddAction(object? argument)
        {
            if (string.IsNullOrEmpty(_firstName)
             || string.IsNullOrEmpty(_lastName)
             || string.IsNullOrEmpty(_email)
             || string.IsNullOrEmpty(_birthYear)

            ) { return false; }

            return !HasErrors;
        }
        public void AddAction(object? argument)
        {
            // add
            Reservations.Add(new ItemReservationViewModel(FirstName, LastName, Email, int.Parse(BirthYear)));

            // clear inputs
            _firstName = ""; OnPropertyChanged(nameof(FirstName));
            _lastName = ""; OnPropertyChanged(nameof(LastName));
            _email = ""; OnPropertyChanged(nameof(Email));
            _birthYear = ""; OnPropertyChanged(nameof(BirthYear));
            AddCommand.RaiseCanExecuteChanged();
        }
        public void RemoveAction(ItemReservationViewModel item) { Reservations.Remove(item); }
        public AddPeopleControlViewModel(User currentUser)
        {
            CurrentUser = currentUser;

            // add people control
            _firstName = CurrentUser.FirstName;
            _lastName = CurrentUser.LastName;
            _email = CurrentUser.Email;
            _birthYear = "2002";
            AddCommand = new RelayCommand(AddAction, CanExecuteAddAction);
        }
    }
}
