using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace AlamniLMS.BLL.Services.Documents
{
    public class CertificateDocument : IDocument
    {
        private readonly string _studentName;
        private readonly string _courseTitle;
        private readonly string _certificateNumber;

        public CertificateDocument(string studentName, string courseTitle, string certificateNumber)
        {
            _studentName = studentName;
            _courseTitle = courseTitle;
            _certificateNumber = certificateNumber;
        }

        public DocumentMetadata GetMetadata() => new DocumentMetadata();

        // Replace the incorrect usage of Border and BorderColor on DecorationDescriptor
        // with the correct usage on IContainer, by moving these calls to the .Before() or .Content() containers.

        public void Compose(IDocumentContainer container)
        {
            var PrimaryColor = Colors.Blue.Darken2;
            var AccentColor = Colors.Yellow.Darken2;

            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(0);
                page.Background(Colors.White);

                page.Content().Decoration(decoration =>
                {
                    // Apply the outer border using .Before()
                    decoration.Before(before =>
                    {
                        before
                            .Border(30, Unit.Point)
                            .BorderColor(AccentColor);
                    });

                    // Apply the inner border and content using .Content()
                    decoration.Content(inner =>
                    {
                        inner
                            .Padding(30, Unit.Point)
                            .Border(10, Unit.Point)
                            .BorderColor(PrimaryColor)
                            .Background(Colors.White)
                            .Padding(60, Unit.Point)
                            .Column(col =>
                            {
                                col.Spacing(25);

                                col.Item().PaddingTop(20).Column(header =>
                                {
                                    header.Spacing(10);
                                    header.Item().AlignCenter().Text("ALAMNI LEARNING MANAGEMENT SYSTEM").FontSize(20).FontColor(PrimaryColor).SemiBold();
                                    header.Item().AlignCenter().Text("شهادة إتمام").FontSize(20).SemiBold();
                                    header.Item().AlignCenter().Text("تُمنَح هذه الشهادة بتقدير إلى:").FontSize(14).FontColor(Colors.Grey.Medium);
                                });

                                col.Item().PaddingVertical(20).AlignCenter().Text(_studentName)
                                    .FontSize(42)
                                    .FontColor(PrimaryColor)
                                    .ExtraBold();

                                col.Item().PaddingBottom(20).AlignCenter().Text(text =>
                                {
                                    text.Span(" لإتمامه بنجاح الدورة التدريبية ").FontSize(18);
                                    text.Span(_courseTitle).FontSize(20).Bold().FontColor(AccentColor);
                                });

                                col.Item().PaddingTop(40).Row(row =>
                                {
                                    row.RelativeColumn().Column(sig =>
                                    {
                                        sig.Item().PaddingBottom(5).Text("____________").FontSize(14);
                                        sig.Item().Text("المدير التنفيذي").FontSize(16).SemiBold();
                                        sig.Item().Text("CEO / General Manager").FontSize(12).FontColor(Colors.Grey.Medium);
                                    });

                                    row.RelativeColumn().AlignRight().Column(sig =>
                                    {
                                        sig.Item().Text($"التاريخ: {DateTime.UtcNow:dd MMMM, yyyy}").FontSize(16).SemiBold();
                                        sig.Item().PaddingTop(10).Text(text =>
                                        {
                                            text.Span("رقم الشهادة: ").FontSize(14);
                                            text.Span(_certificateNumber).FontSize(14).SemiBold().FontColor(Colors.Grey.Darken2);
                                        });
                                    });
                                });

                                col.Item().ExtendVertical();
                            });
                    });
                });
            });
        }
    }
}
