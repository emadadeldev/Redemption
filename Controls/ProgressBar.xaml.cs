using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;


namespace EmadAdel.Redemption_Team.Controls
{
    public partial class ProgressBar : UserControl
    {
        private WebClient webClient;
        private bool isDownloading = false;

        public ProgressBar()
        {
            InitializeComponent();
            InitializeProgressBar();
        }

        private void InitializeProgressBar()
        {
            prog.Minimum = 0;
            prog.Maximum = 100;
            prog.Value = 0;
        }

        public async void DownloadFile(string savePath)
        {
            if (isDownloading) return;

            string url = "http://127.0.0.1:5500/local.zip";

            string fileName = System.IO.Path.GetFileName(new Uri(url).LocalPath);

            string downloadPath = System.IO.Path.Combine(savePath, fileName);


            try
            {
                isDownloading = true;

                prog.Value = 0;

                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                    await webClient.DownloadFileTaskAsync(new Uri(url), downloadPath);
                }
            }
            catch (Exception ex)
            {
                isDownloading = false;
                MessageBox.Show($"❌ حدث خطأ: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double percentage = e.ProgressPercentage;

            prog.Value = percentage;
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            isDownloading = false;

            if (e.Error == null)
            {
                prog.Value = 100;

                loadingText.Text = "اكتمل التثبيت و التحميل بنجاح";
            }
            else
            {
                loadingText.Text = "حدث خطأ في التحميل:";
            }
        }

        public void StartDownload(string save)
        {
            loadingText.Text = "..جاري تنزيل احدث نسخة من التعريب";
            DownloadFile(save);
        }

        public void StopDownload()
        {
            if (webClient != null && webClient.IsBusy)
            {
                webClient.CancelAsync();
                isDownloading = false;
            }
        }

        public void StopLoading()
        {
            StopDownload();
        }

        public void GoToValue(double value)
        {
            if (value >= 0 && value <= 100)
            {
                prog.Value = value;
            }
        }

        private void prog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //StartDownload();
        }
    }
}