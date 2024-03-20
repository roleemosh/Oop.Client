using Oop.Client.Interfaces;
using System.Runtime.Versioning;

namespace Oop.Client.Services
{
    [SupportedOSPlatform("browser")]
    public partial class IndexDBService : IIndexDBService
    {

        //[JSImport("getBlazorCulture", nameof(LanguageService))]
        //public static partial string GetBlazorCulture();

        //[JSImport("getBrowserLocale", nameof(LanguageService))]
        //public static partial string GetBrowserLocale();

        //[JSImport("setBrowserLanguage", nameof(LanguageService))]
        //public static partial string SetBrowserLanguage(string lang);

        //[JSImport("setBlazorCulture", nameof(LanguageService))]
        //public static partial void SetBlazorCulture(string lang);
        public async ValueTask DisposeAsync()
        {
           
        }
    }
}
