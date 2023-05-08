using System;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace lab_2_3
{
    class MyThread
    {
        public Thread thrd;
        public int[] mas;
        public EventWaitHandle handle = new AutoResetEvent(false);
        public MyThread(int[] nums)
        {
            mas = nums;
            thrd = new Thread(this.Run);
            thrd.Start();
        }
        public void Bubble_Sort(int[] mas)
        {
            for(int i=0; i<mas.Length; i++)
            {
                for(int j=0; j<mas.Length-1-i; j++)
                {
                    if (mas[j] > mas[j + 1])
                    {
                        Swap(ref mas[j], ref mas[j + 1]);
                    }
                }
            }
        }
        public void Print(int[] mas)
        {
            for(int i=0; i<mas.Length; i++)
            {
                Console.Write(mas[i] + ' ');
            }
        }
        public void Swap(ref int first, ref int second)
        {
            int tmp = first;
            first = second;
            second = tmp;
        }
        void Run()
        {
            Bubble_Sort(mas);
            handle.Set();
        }
    }
    class Program
    {
        static int[] mas;
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Введите количество элементов массива: ");
            int n = Convert.ToInt32(Console.ReadLine());
            var rand = new Random();
            mas = new int[n];
            for(int i = 0; i < n; i++)
            {
            mas[i] = rand.Next(1000);
            }
            int k = n / 10;
            for (int i = 0; i < k; i++)
            {
                mas[i] = 1000;
            }
            for (int i = k; i < n; i++)
            {
                mas[i] = 1;
            }
            Console.WriteLine("Введите количество потоков: ");
            int p = Convert.ToInt32(Console.ReadLine());
            int len = n / p;
            sw.Start();
            MyThread[] threads = new MyThread[p];
            var list = mas.ToList();
            for(int i=0; i<p-1; i++)
            {
                var range_list = mas.Skip(i * len).Take(len);
                int[] range = range_list.ToArray();
                Console.WriteLine("Количество элементов для потока " + i + " = " + range.Length);
                threads[i] = new MyThread(range);
            }
            var result = mas.Skip((p - 1) * len);
            var range_end = result.ToArray();
            Console.WriteLine("Количество элементов для потока " + (p - 1) + " = " + range_end.Length);
            threads[p - 1] = new MyThread(range_end);
            for(int i = 0; i < p; i++)
            {
                threads[i].handle.WaitOne();
            }
            int[] index = new int[len];
            int[] answer = new int[n];
            for (int i = 0; i < p; i++)
            {
                index[i] = 0;
            }
            int elem, ind;
            int min_ind = 0;
            for(int i = 0; i < n; i++)
            {
                int min = 1000;
                for(int j = 0; j < p; j++)
                {
                    ind = index[j];
                    elem = threads[j].mas[ind];
                    if (elem < min)
                    {
                        min = elem;
                        min_ind = j;
                    }
                }
                answer[i] = min;
                if (index[min_ind] < threads[min_ind].mas.Length - 1)
                {
                    index[min_ind]++;
                }
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine("time = " + time);

        }
    }
}
