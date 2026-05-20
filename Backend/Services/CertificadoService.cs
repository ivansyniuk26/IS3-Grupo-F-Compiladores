using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Backend.Services
{
    public class CertificadoService
    {
        public byte[] GenerarCertificado(string nombreParticipante, string nombreEvento)
        {
            // QuestPDF dibuja el documento utilizando una sintaxis fluida
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Certificado de Asistencia")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Darken2);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(x =>
                    {
                        x.Spacing(20);
                        x.Item().Text("Por la presente se certifica que:").FontSize(20);
                        x.Item().Text(nombreParticipante).SemiBold().FontSize(28);
                        x.Item().Text($"Ha participado con éxito en el evento: {nombreEvento}");
                        x.Item().Text("Emitido en la sede de Montecarlo.").FontSize(16).Italic();
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generado el: ");
                        x.Span($"{DateTime.Now:dd/MM/yyyy}");
                    });
                });
            });

            // Retorna el archivo crudo en formato de bytes
            return document.GeneratePdf();
        }
    }
}