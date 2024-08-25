namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemReservationViewModel : ViewModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int BirthYear { get; set; }

        public ItemReservationViewModel(string firstName, string lastName, string email, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthYear = birthYear;
        }
    }
}
