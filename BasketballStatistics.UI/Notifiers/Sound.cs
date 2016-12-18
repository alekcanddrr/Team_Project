using System.Media;

namespace BasketballStatistics.UI.Notifiers
{
    static class Sound
    {
        public static void TimeIsOver()
        {
            SystemSounds.Asterisk.Play();
        }
    }
    
}
