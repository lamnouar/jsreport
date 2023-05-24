using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsreport
{
    public class program
    {
        static async Task Main(string[] args)
        {
            HtmlToPdfConverter _converter = new HtmlToPdfConverter();
            var htmlFilePath = Path.Combine($@"{AppContext.BaseDirectory}", "AppData", "htmlFile.html");
            var htmlFileAsString = await File.ReadAllTextAsync(htmlFilePath);

            var configuration = new GenerationConfiguration
            {
                DocumentName = "test.PDF",
                TemplateVariables = new Dictionary<string, string>()
            };
            configuration.TemplateVariables.Add("footerTextFontSize", "12px");
            var convertedPdf = await _converter.ConvertAsync(htmlFileAsString, configuration);
            File.WriteAllBytes("test.PDF", convertedPdf);
        }
    }
}
