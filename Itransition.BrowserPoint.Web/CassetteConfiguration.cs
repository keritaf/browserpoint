using Cassette.Configuration;
using Cassette.Stylesheets;

namespace Itransition.BrowserPoint.Web
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            bundles.AddUrlWithLocalAssets(@"//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js",
                new LocalAssetSettings
                    {
                        Path = "scripts/jquery/jquery.js",
                        FallbackCondition = "!window.jQuery"
                    });

            bundles.AddPerIndividualFile<StylesheetBundle>(@"Content");
        }
    }
}