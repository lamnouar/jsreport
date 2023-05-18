using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace jsreport.tests;
public class HtmlToPdfConverterTests
{
    private readonly ILogger<HtmlToPdfConverter> _mockLogger = Substitute.For<ILogger<HtmlToPdfConverter>>();
    private readonly HtmlToPdfConverter _converter;

    public HtmlToPdfConverterTests()
    {
        _converter = new HtmlToPdfConverter();
    }

    [Fact]
    public async void GivenAnHtmlString_WhenConvertToPdf_ThenReturnThePdf()
    {
        //arrange   
        var configuration = new GenerationConfiguration
        {
            DocumentName = "Commission_Report.PDF",
            TemplateVariables = new Dictionary<string, string>()
        };
        configuration.TemplateVariables.Add("footerTextFontSize", "12px"); ;

        var htmlFilePath = Path.Combine($@"{AppContext.BaseDirectory}", "AppData", "Commission_Report.txt");
        var htmlFileAsString = await File.ReadAllTextAsync(htmlFilePath);

        var expectedPdfFilePath = Path.Combine($@"{AppContext.BaseDirectory}", "AppData", "Commission_Report.PDF");
        var expectedPdfAsBytes = await File.ReadAllBytesAsync(expectedPdfFilePath);

        //act
        var convertedPdf = await _converter.ConvertAsync(htmlFileAsString, configuration);
        File.WriteAllBytes(Path.Combine("data", "Commission_Report.PDF"), convertedPdf);
        //assert   
        convertedPdf.Count().Should().Be(expectedPdfAsBytes.Count());
        

        //Note: This assertion is ignoring the file metadata 
        var convertedPdfBase64WithoutMetadata = Convert.ToBase64String(convertedPdf).Split('+').Skip(1);
        var expectedPdfBase64WithoutMetadata = Convert.ToBase64String(expectedPdfAsBytes).Split('+').Skip(1);
        convertedPdfBase64WithoutMetadata.Should().BeEquivalentTo(expectedPdfBase64WithoutMetadata);
    }
}
