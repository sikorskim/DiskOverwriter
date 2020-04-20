using System;
using System.Threading.Tasks;

namespace DiskOverwriter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string tmpPath = "tmp";
            int passes = 3;

            await Overwriter.write(tmpPath, passes);
        }
    }
}
