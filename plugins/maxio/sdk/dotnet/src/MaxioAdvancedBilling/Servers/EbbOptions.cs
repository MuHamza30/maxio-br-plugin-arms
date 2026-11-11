using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Servers;

public class EbbOptions
{
    public UsOptions Us { get; set; } = new();
    public EuOptions Eu { get; set; } = new();

    internal UrlTemplate Resolve(ServerEnvironment environment, string path) =>
        environment.Match(
            () => new UrlTemplate(Us.BaseUrl, path, [TemplateParam.ForServer("site", Us.Site)]),
            () => new UrlTemplate(Eu.BaseUrl, path, [TemplateParam.ForServer("site", Eu.Site)]));

    public class UsOptions
    {
        public string BaseUrl { get; set; } = "https://events.chargify.com/{site}";
        public string Site { get; set; } = "subdomain";
    }

    public class EuOptions
    {
        public string BaseUrl { get; set; } = "https://events.chargify.com/{site}";
        public string Site { get; set; } = "subdomain";
    }
}
