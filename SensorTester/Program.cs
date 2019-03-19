using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var ps = new probeAPI.SpaceProbeClient();
            var dSpace = ps.GetDriveInfo();
            for (int i = 0; i< dSpace.Count() ; i++)
            {
                Console.WriteLine("{0} : {1}", dSpace[i].driveLetter, dSpace[i].freeSpace.ToString());

            }
            Console.WriteLine(dSpace);
            Console.ReadKey();


        }
    }

    public class DriveSpaces
    {
        public string driveLetter { get; set; }
        public long freeSpace { get; set; }
    }
}
