using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemVoucherViewModel : ViewModelBase
    {
        private readonly TourVoucher _voucher;

        // hidden
        public int Id => _voucher.Id;

        // display
        public string Index => _voucher.Index.ToString();
        public string ReceivedAt => $"{_voucher.ReceivedAt:yyyy-MM-dd HH:mm:ss}";
        public string Expiration => $"{_voucher.Expiration:yyyy-MM-dd HH:mm:ss}";
        public string Hash => _voucher.Hash;
        public ItemVoucherViewModel(TourVoucher voucher) { _voucher = voucher; }
    }
}
