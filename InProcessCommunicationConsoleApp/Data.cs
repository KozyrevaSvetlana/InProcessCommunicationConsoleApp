namespace InProcessCommunicationConsoleApp
{
    public class Data
    {
        public int[] numbers_100_000;
        public int[] numbers_1000_000;
        public int[] numbers_10_000_000;

        public Data()
        {
            numbers_100_000 = setNumbers(100_000);
            numbers_1000_000 = setNumbers(1_000_000);
            numbers_10_000_000 = setNumbers(10_000_000);
        }
        private int[] setNumbers(int count)
        {
            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(1);
            }
            return list.ToArray();
        }
    }
}
