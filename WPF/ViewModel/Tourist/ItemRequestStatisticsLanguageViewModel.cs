using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemRequestStatisticsLanguageViewModel : ViewModelBase
    {
        // hidden
        public Language ActualLanguage { get;set; }

        // display
        public string Language => ActualLanguage.ToReadableString();
        public int Counter { get; set; }

        public ItemRequestStatisticsLanguageViewModel(Language language, int counter)
        {
            ActualLanguage = language;
            Counter = counter;
        }
    }
}
