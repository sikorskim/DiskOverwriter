using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiskOverwriter
{
    public static class Overwriter
    {
        public static async Task write(string path, int passes)
        {
            byte[] data = new byte[getMegaByte()];
            int i = 0;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            while (true)
            {
                data = getRandom1MByte();
                string filename = Path.Combine(path, i.ToString());

                try
                {
                    await File.WriteAllBytesAsync(filename, data);
                }
                catch (Exception ex)
                {
                    if (IsDiskFull(ex))
                    {
                        Console.WriteLine("Disk is full.");
                        Console.WriteLine("Cleaning temp directory...");

                        removeFilesFromTempDirectory(path);

                        Console.WriteLine("Cleanup complete.");
                        passes--;

                        if (passes > 0)
                        {
                            await write(path, passes);
                        }
                        else
                        {
                            Console.WriteLine("Task complete.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (i % 1024 == 100)
                {
                    Console.WriteLine("{0} MB written to disk", i);
                }

                i++;
            }
        }

        static void removeFilesFromTempDirectory(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
            {
                    file.Delete();
            }
        }

        static int getMegaByte()
        {
            return (int)Math.Pow(2, 20);
        }

        static byte[] getRandom1MByte()
        {
           
            byte[] random1MByte = new byte[getMegaByte()];
            Random random = new Random();
            random.NextBytes(random1MByte);

            return random1MByte;
        }

        static byte[] getRandom256bytes()
        {
            byte[] random256bytes = new byte[256];
            Random random = new Random();
            random.NextBytes(random256bytes);

            return random256bytes;
        }

        static bool IsDiskFull(Exception ex)
        {
            const int HR_ERROR_HANDLE_DISK_FULL = unchecked((int)0x80070027);
            const int HR_ERROR_DISK_FULL = unchecked((int)0x80070070);

            return ex.HResult == HR_ERROR_HANDLE_DISK_FULL
                    || ex.HResult == HR_ERROR_DISK_FULL;
        }
    }
}
