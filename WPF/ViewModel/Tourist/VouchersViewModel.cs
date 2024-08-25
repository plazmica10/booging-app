using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class VouchersViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourInfoService _tourInfoService = new();
        public ObservableCollection<ItemVoucherViewModel> Vouchers { get; set; } = new();

        public ICommand ExportCommand { get; }

        public bool ExportCommandCanExecute(object? argument)
        {
            return Vouchers.Count > 0;
        }

        public void ExportAction(object? argument)
        {
            try
            {
                ExportVouchers();
            }
            catch (Exception) {}
        }

        public void ExportVouchers()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            if (Vouchers.Count > 0)
            {
                // Show a file save dialog to get the save location
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Vouchers.pdf",
                    Filter = "PDF Files (*.pdf)|*.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Create a new PDF document using QuestPDF
                    var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            //page.Margin(2, Unit.Centimetre);

                            page.Content().Column(column =>
                            {
                                column.Spacing(2);

                                int i = 0;
                                foreach (var voucher in Vouchers)
                                {
                                    i++;
                                    column.Item().Text($"\ud83c\udfab Voucher #{voucher.Index}").FontSize(25);
                                    column.Item().Text($"\ud83d\udcc5 Received At: {voucher.ReceivedAt}").FontSize(20);
                                    column.Item().Text($"\ud83d\udcc5 Expiration: {voucher.Expiration}").FontSize(20);
                                    column.Item().Text($"UUID: {voucher.Hash}").FontSize(12);
                                    column.Item().Text("").FontSize(12);
                                    column.Item().LineHorizontal(3, Unit.Point).LineColor(Color.FromHex("#000000"));

                                    if (i % 8 == 0) { column.Item().PageBreak(); }
                                }
                            });
                        });
                    });

                    // Save the PDF document
                    document.GeneratePdf(saveFileDialog.FileName);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = saveFileDialog.FileName,
                        UseShellExecute = true
                    });
                }
            }
        }

        public VouchersViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            ExportCommand = new RelayCommand(ExportAction, ExportCommandCanExecute);

            foreach (TourVoucher voucher in _tourInfoService.GetTouristVouchers(CurrentUser.Id))
            {
                Vouchers.Add(new ItemVoucherViewModel(voucher));
            }
        }
    }
}
