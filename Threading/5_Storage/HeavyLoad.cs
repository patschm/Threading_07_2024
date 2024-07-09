using System.Threading.Tasks;

namespace M5_Storage
{
    internal class HeavyLoad
    {
        static int counter = 0;

        public int Counter { get { return counter; } }

        public HeavyLoad()
        {
            Task.Delay(3000).Wait();
            counter++;
        }
    }
}
