namespace BookingApp.WPF.ViewModel.Guide
{
    public class ItemPeakPointViewModel : ViewModelBase
    {
        public string Name { get; set; }
        private int _peopleArrived;
        public int PeopleArrived { get => _peopleArrived; set { _peopleArrived = value; OnPropertyChanged(nameof(PeopleArrived)); } }
        public ItemPeakPointViewModel(string name, int peopleArrived)
        {
            Name = name;
            _peopleArrived = peopleArrived;
        }
    }
}
