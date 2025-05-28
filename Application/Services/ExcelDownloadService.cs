using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Cenace.API.Application.Settings;

namespace Cenace.API.Application.Services
{
    public class ExcelDownloadService
    {
        private readonly string _downloadPath;
        private readonly string _url;

        public ExcelDownloadService(IOptions<CenaceSettings> settings)
        {
            _url = settings.Value.CapacidadDemandadaUrl;
            _downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");

            if (!Directory.Exists(_downloadPath))
                Directory.CreateDirectory(_downloadPath);
        }

        public async Task DownloadExcelsAsync()
        {
            try
            {
                var existingFiles = Directory.GetFiles(_downloadPath);
                foreach (var file in existingFiles)
                {
                    try { File.Delete(file); }
                    catch (Exception ex) { Console.WriteLine($"Error al eliminar el archivo '{file}': {ex.Message}"); }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al limpiar la carpeta de descargas: {ex.Message}");
            }

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", _downloadPath);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            using var driver = new ChromeDriver(chromeOptions);

            try
            {
                driver.Navigate().GoToUrl(_url);
                await Task.Delay(5000);

                var links = driver.FindElements(By.XPath("//a[contains(@href, '.xlsx')]")).ToList();
                foreach (var link in links)
                {
                    string fileUrl = link.GetAttribute("href");
                    driver.ExecuteScript("window.open(arguments[0]);", fileUrl);
                    await Task.Delay(4000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la descarga de archivos: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }
        }

        public string GetDownloadPath() => _downloadPath;
    }
}
