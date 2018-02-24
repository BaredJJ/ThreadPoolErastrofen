using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolErastrofen
{
    public class Program
    {
        static object _locked = new object( );

        public static List<int> baseArray = new List<int>();

        static List<int> primeList = new List<int>( );

        public static int GetInt<T>(T t)
        {
            int n;
            if (!int.TryParse(t.ToString( ), out n))
                throw new FormatException("Incorrect data type");
            return n;
        }

        public static List<int> PrimeList(int size)
        {
            List<int> list = new List<int>();
            for(int i = 0; i < size; i++)
            {
                list.Add(i + 2);
            }
            return list;
        }

        public static List<int> GetIntList()
        {
            Console.Write("Please enter to size array: ");
            int size = GetInt(Console.ReadLine( ));
            return PrimeList(size);
        }

        public static bool IsPrimeNumber(int number)
        {
            for (int i = 2; i <= number / i; i++)
                if (number % i == 0)
                    return false;
            return true;
        }

        public static int[] CopyList(List<int> list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i];

            return array;
        }

        public static List<int> GetBasePrime(List<int> array)
        {
            List<int> primeList = new List<int>( );

            for (int i = 0; i < Math.Sqrt(array.Count); i++)
                if (IsPrimeNumber(array[i]))
                    primeList.Add(array[i]);

            return primeList;
        }

        public static void Show(List<int> list)
        {
            foreach (int i in list)
                Console.Write(i + " ");
            Console.WriteLine( );
            Console.WriteLine(new string('-', 50));
            Console.WriteLine( );
        }

        public static void Run(object o)
        {
            int prime = (int)((object[]) o)[0];
            ManualResetEvent thiseEvent = (ManualResetEvent)((object[])o)[1];

            lock (_locked)
            {
                for (int i = 0; i < baseArray.Count;)
                    if ((baseArray[i] != prime) && (baseArray[i] % prime == 0))
                        baseArray.RemoveAt(i);
                    else i++;
                thiseEvent.Set( );
            }
            
        }

        static void Main()
        {
            baseArray = GetIntList( );
            Show(baseArray);
            List<int> basePrime = GetBasePrime(baseArray);
            Show(basePrime);
            ManualResetEvent[] arrayReset = new ManualResetEvent[basePrime.Count];

            for (int i = 0; i < basePrime.Count; i++)
            {
                arrayReset[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(Run, new object[] { basePrime[i], arrayReset[i] });
            }

            WaitHandle.WaitAll(arrayReset);

            Show(baseArray);
            Console.ReadKey( );
        }
    }
}
