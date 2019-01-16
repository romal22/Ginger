using System;
using System.Runtime.InteropServices;

namespace GingerUtils
{
    public class UserSystemInfo
    {
        public static string GetUserProfileDirectory()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return userFolder;
        }

        public static string GetHomeDirectoryUsingEnvVariable()
        {
            var envHome = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "HOMEPATH" : "HOME";
            var home = Environment.GetEnvironmentVariable(envHome);
            return home;
        }

        public static string GetSystemProfileDirectory()
        {
            return Environment.SystemDirectory;
        }

    }
}

