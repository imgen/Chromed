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
        public static void UnzipTo(string targetDir, string zipBaseDir = null)
        {
            zipBaseDir ??= Environment.CurrentDirectory;
            string zipFileRelativePath = GetZipRelativePath();
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

        private static string GetZipRelativePath()
        {
            if (IsOSPlatform(OSPlatform.Windows))
            {
                return "runtimes/win-x64/native/chrome-win.zip";
            }
            else if (IsOSPlatform(OSPlatform.OSX))
            {
                return "runtimes/osx-x64/native/chrome-mac.zip";
            }
            else if (IsOSPlatform(OSPlatform.Linux))
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
