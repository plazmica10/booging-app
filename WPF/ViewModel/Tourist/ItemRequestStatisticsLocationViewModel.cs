using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemRequestStatisticsLocationViewModel : ViewModelBase
    {
        // hidden
        public Location ActualLocation { get;set; }

        // display
        public string Location => ActualLocation.ToSmallString();
        public int Counter { get; set; }

        public ItemRequestStatisticsLocationViewModel(Location location, int counter)
        {
            ActualLocation = location;
            Counter = counter;
        }
    }
}
