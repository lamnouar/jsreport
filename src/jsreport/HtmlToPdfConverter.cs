﻿using System.Runtime.InteropServices;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;

namespace jsreport;

public interface IHtmlToPdfConverter
{
    Task<byte[]> ConvertAsync(string html, IGenerationConfiguration configuration);
}

public class HtmlToPdfConverter : IHtmlToPdfConverter
{

    public HtmlToPdfConverter()
    {

    }

    public async Task<byte[]> ConvertAsync(string html, IGenerationConfiguration configuration)
    {
        var rs = new LocalReporting()
            .Configure(config => { return config; })
            .RunInDirectory(Path.Combine(Path.GetTempPath(), "jsreports"))
            .UseBinary(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? JsReportBinary.GetBinary() : Binary.Linux.JsReportBinary.GetBinary())
            .AsUtility()
            .Create();

        var report = await rs.RenderAsync(new RenderRequest()
        {
            Template = new Template()
            {
                Recipe = Recipe.ChromePdf,
                Engine = Engine.None,
                Content = html,
                Chrome = new Chrome()
                {
                    DisplayHeaderFooter = true,
                    FooterTemplate = GetPdfFooter(configuration.TemplateVariables),
                    HeaderTemplate = "<span>test</span>",
                    Landscape = false,
                    MarginLeft = "1in",
                    MarginBottom = "1in",
                    MarginRight = "1in"
                },
            }
        });

        using var memoryStream = new MemoryStream();
        await report.Content.CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    private string GetPdfFooter(IDictionary<string, string> templateVars)
    {
        var fontSize = templateVars.ContainsKey("footerTextFontSize") ? $"font-size: {templateVars["footerTextFontSize"]};" : string.Empty;
        var border = templateVars.ContainsKey("tableBorder") ? $"border-top: {templateVars["tableBorder"]};" : string.Empty;
        var fontFamily = templateVars.ContainsKey("fontFamily") ? $"font-family: {templateVars["fontFamily"]};" : string.Empty;
        var color = templateVars.ContainsKey("textColor") ? $"color: {templateVars["textColor"]};" : string.Empty;

        return @$"<table style='margin: 30px 70px; {border} {fontFamily}
                           {color} text-align:right; {fontSize} width:100%;'>
                        <td>Page <span class='pageNumber'></span>&nbsp;|&nbsp;<span class='totalPages'></span></td></table>";
    }
}

