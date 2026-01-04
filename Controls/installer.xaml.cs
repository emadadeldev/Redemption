// =======================================================
// Developer: Emad Adel
// Source Code https://github.com/emadadeldev/Redemption
// =======================================================
using System.Windows;
using System;
using System.Windows.Controls;

namespace Emadadel.Redemption_Team.path
{
    public partial class InstallerControl : UserControl
    {
        public InstallerControl()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "اختر مسار تثبيت التعريب";
                dialog.ShowNewFolderButton = true;

                //dialog.SelectedPath = InstallPathTextBox.Text;

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

    }
}
