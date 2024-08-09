using System.Diagnostics;

namespace InProcessCommunicationConsoleApp
{
    internal class Program
    {
        private static int totalSum = 0;
        private static readonly object lockObj = new object();
        static void Main(string[] args)
        {
            var data = new Data();

            Console.WriteLine("Информация о системе:");
            Console.WriteLine($"Архитектура операционной системы: {(Environment.Is64BitOperatingSystem ? "64-bit" : "32 - bit")}");
            Console.WriteLine($"Архитектура процессора: {(Environment.Is64BitProcess ? "64-bit" : "32 - bit")}");
            Console.WriteLine($"Версия операционной системы: {Environment.OSVersion}");
            Console.WriteLine();
            //Обычное
            CalculateSum(data.numbers_100_000);
            CalculateSum(data.numbers_1000_000);
            CalculateSum(data.numbers_10_000_000);
            Console.WriteLine();

            //Параллельное(для реализации использовать Thread, например List)
            CalculateSumByThread(data.numbers_100_000, 5);
            CalculateSumByThread(data.numbers_1000_000, 5);
            CalculateSumByThread(data.numbers_10_000_000, 5);
            Console.WriteLine();

            //Параллельное с помощью LINQ
            CalculateSumByParallelLink(data.numbers_100_000, 5);
            CalculateSumByParallelLink(data.numbers_1000_000, 5);
            CalculateSumByParallelLink(data.numbers_10_000_000, 5);
            Console.ReadLine();

            //Пришлите в чат с преподавателем помимо ссылки на репозиторий номера своих строк в таблице.
        }

        private static void CalculateSum(int[] numbers)
        {
            var timer = new Stopwatch();
            timer.Start();
            totalSum = numbers.Select(i => i).Sum();
            timer.Stop();
            var timeTaken = timer.Elapsed;
            Console.WriteLine($"Время выполнения Обычное перечисление для {numbers.Length.ToString("N0")} - " + timeTaken.ToString(@"ss\.fff"));
        }

        private static void CalculateSumByThread(int[] numbers, int numberOfThreads)
        {
            var timer = new Stopwatch();
            timer.Start();
            var threads = new Thread[numberOfThreads];
            int chunkSize = numbers.Length / numberOfThreads;

            for (int i = 0; i < numberOfThreads; i++)
            {
                // Определяем начальный индекс и размер части
                int start = i * chunkSize;
                int end = (i == numberOfThreads - 1) ? numbers.Length : start + chunkSize;
                // Создаем и запускаем поток
                threads[i] = new Thread(() => Sum(numbers, start, end));
                threads[i].Start();
            }

            // Дожидаемся завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            timer.Stop();
            var timeTaken = timer.Elapsed;
            Console.WriteLine($"Время выполнения Параллельное перечисление для {numbers.Length.ToString("N0")} - " + timeTaken.ToString(@"ss\.fff"));
        }

        static void Sum(int[] numbers, int start, int end)
        {
            int sum = 0;
            for (int i = start; i < end; i++)
            {
                sum += numbers[i];
            }
            lock (lockObj)
            {
                totalSum += sum;
            }
        }

        private static void CalculateSumByParallelLink(int[] numbers, int degreeOfParallelism)
        {
            var timer = new Stopwatch();
            timer.Start();
            totalSum = numbers.AsParallel().WithDegreeOfParallelism(degreeOfParallelism).Sum();
            timer.Stop();
            var timeTaken = timer.Elapsed;
            Console.WriteLine($"Время выполнения Обычное перечисление для {numbers.Length.ToString("N0")} - " + timeTaken.ToString(@"ss\.fff"));
        }
    }
}
