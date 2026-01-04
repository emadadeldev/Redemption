// =======================================================
// Developer: Emad Adel
// Source Code https://github.com/emadadeldev/Redemption
// =======================================================
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;

namespace EmadAdel.Redemption_Team
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadContributors();
        }

        public async void LoadContributors()
        {
            try
            {
                string url = "https://raw.githubusercontent.com/emadadeldev/Redemption/refs/heads/main/CONTRIBUTOR.md";

                using (HttpClient client = new HttpClient())
                {
                    string content = await client.GetStringAsync(url);

                    var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> contributors = new List<string>();
                    foreach (var line in lines)
                    {
                        string trimmed = line.Trim();
                        if (!string.IsNullOrWhiteSpace(trimmed))
                            contributors.Add(trimmed);
                    }

                    ContributorsList.ItemsSource = contributors;
                }

                Contributor_Text.Visibility = Visibility.Collapsed;
                Nextbtn.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Contributor_Text.Text = "❌ فشل تحميل المساهمين";
            }
        }

        private void Button_Options_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Visible;
        }

        private void CloseBtn(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
                "هل أنت متأكد من الخروج؟",
                "تأكيد",                 
                MessageBoxButton.YesNo,    
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
