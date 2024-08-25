using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemSelectVoucherViewModel : ViewModelBase
    {
        private readonly TourVoucher _voucher;

        public int Id => _voucher.Id;
        public string DisplayName => "\ud83c\udfab" +
                                     " - " +
                                     _voucher.Index +
                                     " -- " +
                                     _voucher.ReceivedAt.ToString("yyyy-MM-dd HH:mm:ss") +
                                     " --- " +
                                     _voucher.Expiration.ToString("yyyy-MM-dd HH:mm:ss");

        public ItemSelectVoucherViewModel(TourVoucher voucher) { _voucher = voucher; }

        public override string ToString() => DisplayName;
    }
}
