using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace Chromed
{
    public static class ChromeBundler
    {
        public static string UnzipTo(string targetDir, 
            string zipBaseDir = null, 
            OSPlatform? platform = null)
        {
            zipBaseDir ??= Environment.CurrentDirectory;
            string zipFileRelativePath = GetZipRelativePath(platform);
            var zipFilePath = Path.Combine(zipBaseDir, zipFileRelativePath);
            if (IsOSPlatform(OSPlatform.OSX))
            {
                NativeExtractToDirectory(zipFilePath, targetDir);
            }
            else
            {
                ZipFile.ExtractToDirectory(zipFilePath, targetDir);
            }
        }

        private static void NativeExtractToDirectory(string zipPath, string folderPath)
        {
            using var process = new Process();
            process.StartInfo.FileName = "unzip";
            process.StartInfo.Arguments = $"\"{zipPath}\" -d \"{folderPath}\"";
            process.Start();
            process.WaitForExit();
        }

        private static string GetZipRelativePath(OSPlatform? platform = null)
        {
            if (platform == OSPlatform.Windows || IsOSPlatform(OSPlatform.Windows))
            {
                return "runtimes/win-x64/native/chrome-win.zip";
            }
            else if (platform == OSPlatform.OSX || IsOSPlatform(OSPlatform.OSX))
            {
                return "runtimes/osx-x64/native/chrome-mac.zip";
            }
            else if (platform == OSPlatform.Linux || IsOSPlatform(OSPlatform.Linux))
            {
                return "runtimes/linux-x64/native/chrome-linux.zip";
            }
            else
            {
                throw new InvalidOperationException("Chromed only supports Windows, Mac and Linux");
            }
        }
    }
}
