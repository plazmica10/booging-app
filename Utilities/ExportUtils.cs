using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookingApp.Utilities
{
    public static class ExportUtils
    {
        public static void ExportToPDF(User user, ReservationsDto reservation)
        {
            string pdfPath = @"resources\data\ReservationSummary.pdf";
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                            .Background(Colors.Grey.Lighten1)
                            .Height(60)
                            .Stack(stack =>
                            {
                                stack.Item().Text("BoogingApp").FontSize(20).Bold();
                                stack.Item().Text("Location: Novi Sad, Serbia", TextStyle.Default.Size(14));
                                stack.Item().Text($"Date: {DateTime.Now.ToShortDateString()}", TextStyle.Default.Size(14));
                            });

                        page.Content()
                            .Background(Colors.Grey.Lighten2)
                            .PaddingVertical(1, Unit.Centimetre)
                            .Stack(stack =>
                            {
                                stack.Item().Text("Reservation Summary").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium).AlignCenter();
                                stack.Item().Row(row =>
                                {
                                    row.RelativeColumn().Padding(10).Stack(column =>
                                    {
                                        column.Spacing(10);
                                        column.Item().Text($"{reservation.AccommodationName}:");
                                        column.Item().Text($"Location: {reservation.City}, {reservation.Country}").FontSize(14);
                                        column.Item().Text($"Check-in: {reservation.CheckIn.ToShortDateString()}").FontSize(14);
                                        column.Item().Text($"Check-out: {reservation.CheckOut.ToShortDateString()}").FontSize(14);
                                        column.Item().Text($"Number of guests: {reservation.GuestCount}").FontSize(14);
                                        column.Item().Text($"Owner: {reservation.Owner.FirstName} {reservation.Owner.LastName}").FontSize(14);

                                    });

                                    row.RelativeColumn().Padding(10).Stack(column =>
                                    {
                                        column.Spacing(10);
                                        column.Item().Text($"Your information:");
                                        column.Item().Text($"Name: {user.FirstName} {user.LastName}").FontSize(14);
                                        column.Item().Text($"Email: {user.Email}").FontSize(14);
                                        column.Item().Text($"Phone: {user.Phone}").FontSize(14);

                                    });
                                });
                            });

                        page.Footer()
                            .Background(Colors.Grey.Lighten1)
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                })
                .GeneratePdf(pdfPath);
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("An error occurred while trying to open the PDF file. Please check if you have a PDF reader installed.");
            }
        }
    }
}
