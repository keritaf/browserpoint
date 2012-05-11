using Cassette.Configuration;
using Cassette.Stylesheets;

namespace Itransition.BrowserPoint.Web
{
    using Cassette.Scripts;

    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            bundles.AddPerIndividualFile<ScriptBundle>(@"Scripts");
            bundles.AddPerIndividualFile<StylesheetBundle>(@"Content");
        }
    }
}