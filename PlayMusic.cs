using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace EmadAdel.Redemption_Team.Music
{
    public static class PlayMusic
    {
        private static MediaPlayer player;

        public static void Play(string resourceName, bool loop = true, double volume = 0.5)
        {
            if (player == null)
                player = new MediaPlayer();

            string tempPath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                resourceName
            );

            if (!File.Exists(tempPath))
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "EmadAdel.Redemption_Team.Assets." + resourceName))
                {
                    if (stream == null)
                        throw new Exception("Resource not found: " + resourceName);

                    using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fs);
                    }
                }
            }

            player.Open(new Uri(tempPath, UriKind.Absolute));
            player.Volume = Clamp(volume);

            if (loop)
            {
                player.MediaEnded -= Loop;
                player.MediaEnded += Loop;
            }

            player.Play();
        }

        private static void Loop(object sender, EventArgs e)
        {
            player.Position = TimeSpan.Zero;
            player.Play();
        }

        private static double Clamp(double v) => Math.Max(0, Math.Min(1, v));
    }
}