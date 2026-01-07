using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace EmadAdel.Redemption_Team
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SplashScreen splash = new SplashScreen();
            splash.Show();

            _ = LoadContributorsAndShowMainAsync(splash);
        }

        private async Task LoadContributorsAndShowMainAsync(SplashScreen splash)
        {
            List<string> contributors = new List<string>();

            try
            {
                string url = "https://raw.githubusercontent.com/emadadeldev/Redemption/refs/heads/main/CONTRIBUTOR.md";

                using (HttpClient client = new HttpClient())
                {
                    string content = await client.GetStringAsync(url);

                    var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        string trimmed = line.Trim();
                        if (!string.IsNullOrWhiteSpace(trimmed))
                            contributors.Add(trimmed);
                    }
                }
            }
            catch
            {
                splash.LoadingText.Text = "❌ فشل تحميل البيانات";
            }

            splash.Close();

            MainWindow main = new MainWindow();
            main.SetContributors(contributors);
            main.Show();
        }
    }
}
