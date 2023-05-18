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
            var htmlFilePath = Path.Combine($@"{AppContext.BaseDirectory}", "AppData", "Commission_Report.txt");
            var htmlFileAsString = await File.ReadAllTextAsync(htmlFilePath);

            var configuration = new GenerationConfiguration
            {
                DocumentName = "Commission_Report.PDF",
                TemplateVariables = new Dictionary<string, string>()
            };
            configuration.TemplateVariables.Add("footerTextFontSize", "12px");
            var convertedPdf = await _converter.ConvertAsync(htmlFileAsString, configuration);
            File.WriteAllBytes(Path.Combine(@"data", "test.PDF"), convertedPdf);
        }
    }
}
