using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WMS.Domain.Entities;

namespace WMS.Application.Reports;

public static class TimesheetReportGenerator
{
    public static byte[] GeneratePdf(Employee? employee, int empId, int month, int year, List<Attendance> records)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var totalHours = records.Sum(r => r.TotalHours ?? 0);

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.Header().Text("WMS Timesheet Report (Crystal Reports Format)").Bold().FontSize(18);
                page.Content().Column(col =>
                {
                    col.Item().Text($"Employee: {employee?.FirstName} {employee?.LastName} (ID: {empId})");
                    col.Item().Text($"Period: {month}/{year}");
                    col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC").FontSize(9);
                    col.Item().PaddingVertical(10).LineHorizontal(1);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(1);
                            c.RelativeColumn(1);
                        });
                        table.Header(h =>
                        {
                            h.Cell().Text("Date").Bold();
                            h.Cell().Text("Check-In").Bold();
                            h.Cell().Text("Check-Out").Bold();
                            h.Cell().Text("Hours").Bold();
                            h.Cell().Text("Mode").Bold();
                        });
                        foreach (var r in records)
                        {
                            table.Cell().Text(r.AttendanceDate.ToString("yyyy-MM-dd"));
                            table.Cell().Text(r.CheckIn.ToString("HH:mm"));
                            table.Cell().Text(r.CheckOut?.ToString("HH:mm") ?? "N/A");
                            table.Cell().Text((r.TotalHours?.ToString("F2") ?? "0"));
                            table.Cell().Text(r.WorkMode ?? "-");
                        }
                    });
                    col.Item().PaddingTop(10).Text($"Total Hours: {totalHours:F2}").Bold();
                });
                page.Footer().AlignCenter().Text("Workforce Management System - Confidential");
            });
        }).GeneratePdf();
    }
}
