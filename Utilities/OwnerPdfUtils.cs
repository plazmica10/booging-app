using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.DTO.Owner;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookingApp.Utilities
{
    public static class OwnerPdfUtils
    {
        public static void ExportToPDF(User user, List<YearStatisticsDto> yearStatistics, AccommodationDTO accommodation)
        {
            string pdfPath = @"resources\data\AccommodationStatistics.pdf";
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
                        .Text($"Date: {DateTime.Now.ToShortDateString()} Location: Novi Sad, Serbia",
                            TextStyle.Default.Size(14));

                    page.Content()
                        .Background(Colors.Grey.Lighten2)
                        .PaddingVertical(1, Unit.Centimetre)
                        .Stack(stack =>
                        {
                            stack.Item().Text("Accommodation statistics").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium).AlignCenter();
                            stack.Item().Row(row =>
                            {
                                row.RelativeColumn().Padding(10).Stack(column =>
                                {
                                    column.Spacing(10);
                                    column.Item().Text($"Accommodation: {accommodation.Name}");
                                    column.Item().Text($"Location: {accommodation.Location}").FontSize(14);
                                    column.Item().Text($"Type: {accommodation.Type}").FontSize(14);
                                    column.Item().Text($"Max guests: {accommodation.MaxGuests}").FontSize(14);
                                    column.Item().Text($"Min days: {accommodation.MinDays}").FontSize(14);
                                    column.Item().Text($"Refund period: {accommodation.RefundPeriod}").FontSize(14);

                                });

                                row.RelativeColumn().Padding(10).Stack(column =>
                                {
                                    column.Item().Image(accommodation.Photos[0]);
                                });
                            });
                            stack.Item().Padding(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                });


                                table.Header(header =>
                                {
                                    header.Cell().Border(1).PaddingLeft(2).Text("Year").FontSize(14);
                                    header.Cell().Border(1).PaddingLeft(2).Text("Reservations").FontSize(14);
                                    header.Cell().Border(1).PaddingLeft(2).Text("Cancellations").FontSize(14);
                                    header.Cell().Border(1).PaddingLeft(2).Text("Rescheduling count").FontSize(14);
                                    header.Cell().Border(1).PaddingLeft(2).Text("Renovation count").FontSize(14);
                                });


                                foreach (var year in yearStatistics)
                                {
                                    table.Cell().Border(1).AlignCenter().Text($"{year.Year}");
                                    table.Cell().Border(1).AlignCenter().Text($"{year.Reservations}");
                                    table.Cell().Border(1).AlignCenter().Text($"{year.Cancellations}");
                                    table.Cell().Border(1).AlignCenter().Text($"{year.ReschedulingCount}");
                                    table.Cell().Border(1).AlignCenter().Text($"{year.RenovationCount}");
                                }
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
