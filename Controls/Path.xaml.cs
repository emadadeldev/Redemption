// =======================================================
// Developer: Emad Adel
// Source Code https://github.com/emadadeldev/Redemption
// =======================================================
using System.Windows;
using System;
using System.Windows.Controls;
using EmadAdel.Redemption_Team.Controls;

namespace EmadAdel.Redemption_Team.Controls
{
    public partial class Path : UserControl
    {
        public Path()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "اختر مسار تثبيت التعريب";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedPath = dialog.SelectedPath;

                    if (!selectedPath.EndsWith("Red Dead Redemption 2", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Windows.MessageBox.Show(
                            "يرجى اختيار المسار الصحيح",
                            "خطأ في المسار",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }

                    InstallPathTextBox.Text = selectedPath;
                }
            }
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            PathContent.Visibility = Visibility.Collapsed;
            installBtn.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Visible;


            DownloadProgressBar.StartDownload(InstallPathTextBox.Text);
        }
    }
}