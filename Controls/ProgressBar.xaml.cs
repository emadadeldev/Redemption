// =======================================================
// Developer: Emad Adel
// Source Code https://github.com/emadadeldev/Redemption
// =======================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.IO.Compression;


namespace EmadAdel.Redemption_Team.Controls
{
    public partial class ProgressBar : UserControl
    {
        private WebClient webClient;
        private bool isDownloading = false;
        private string zipFilePath;

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

            string url = "https://github.com/emadadeldev/RDR2AR/archive/refs/heads/main.zip";

            string fileName = System.IO.Path.GetFileName(new Uri(url).LocalPath);

            zipFilePath = System.IO.Path.Combine(savePath, fileName);

            try
            {
                isDownloading = true;
                prog.Value = 0;

                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                    await webClient.DownloadFileTaskAsync(new Uri(url), zipFilePath);
                }
            }
            catch (Exception ex)
            {
                isDownloading = false;
                MessageBox.Show(ex.Message);
            }
        }
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double percentage = e.ProgressPercentage;

            prog.Value = percentage;
        }

        private void ExtractZipWithOverride(string zipPath, string extractPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string relativePath = entry.FullName;

                    int index = relativePath.IndexOf('/');
                    if (index >= 0)
                        relativePath = relativePath.Substring(index + 1);

                    if (string.IsNullOrEmpty(relativePath))
                        continue;

                    string fullPath = System.IO.Path.Combine(extractPath, relativePath);

                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        Directory.CreateDirectory(fullPath);
                        continue;
                    }

                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                    entry.ExtractToFile(fullPath, true);
                }
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            isDownloading = false;

            if (e.Error != null)
            {
                loadingText.Text = "❌ خطأ في التحميل";
                return;
            }

            try
            {
                prog.Value = 100;
                loadingText.Text = "...جاري تثبيت التعريب";

                string extractPath = System.IO.Path.GetDirectoryName(zipFilePath);

                ExtractZipWithOverride(zipFilePath, extractPath);

                loadingText.Text = ".تم التثبيت بنجاح";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

