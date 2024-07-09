
namespace Problem_Shared_Stack
{
    internal class NumberReader
    {
        private readonly Stack<int> _buffer;

        public NumberReader(Stack<int> buffer)
        {
            _buffer = buffer;
        }
        public Thread Run()
        {
            var t1 = new Thread(Reader);
            t1.Start();
            return t1;
        }

        private void Reader()
        {
            while (true)
            {
                 _buffer.TryPop(out int nr);
                if (nr < 0)
                {
                    Console.WriteLine($"\t\t\t\t\t\t{nr}");
                }
                else
                {
                    Console.WriteLine($"{nr}");
                }
            }
        }
    }
}
