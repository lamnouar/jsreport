using System.Collections.Generic;

namespace jsreport;

public interface IGenerationConfiguration
{
    IDictionary<string, string> TemplateVariables { get; }
    string DocumentName { get; }
    public string Filename { get; }
}

public class GenerationConfiguration : IGenerationConfiguration
{
    public IDictionary<string, string> TemplateVariables { get; set; }
    public string DocumentName { get; set; }
    public string Filename { get; set; }
}
