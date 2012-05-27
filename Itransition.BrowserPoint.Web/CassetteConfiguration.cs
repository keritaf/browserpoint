using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Itransition.BrowserPoint.Web
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        // Please read http://getcassette.net/documentation/configuration
        public void Configure(BundleCollection bundles)
        {
            bundles.Add<StylesheetBundle>("Content/less");
            bundles.AddPerIndividualFile<StylesheetBundle>("Content/custom");
            bundles.AddPerIndividualFile<ScriptBundle>("Scripts");
        }
    }
}