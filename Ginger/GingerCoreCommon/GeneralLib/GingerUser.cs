using System.IO;

namespace Amdocs.Ginger.Common
{
    public class GingerUser
    {
        public static string GetUserGingerDirectory()
        {
            string userGingerFolder = Path.Combine(GingerUtils.UserSystemInfo.GetUserProfileDirectory(), "Ginger");
            return userGingerFolder;
        }

    }
}
