using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApp.Command;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class testViewModel : ViewModelBase
    {
        #region Bindings

        private Location? _selectedLocation;

        public Location? SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        #endregion

        public ICommand Click => new FunctionCommand(() => MessageBox.Show($"{SelectedLocation.City}, {SelectedLocation.Country}"));

        public testViewModel()
        {
            // Initialize the view model
        }
    }
}
