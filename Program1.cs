using System;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace bubble_sort
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
            for (int i = 0; i < mas.Length; i++)
            {
                for (int j = 0; j < mas.Length - 1 - i; j++)
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
            for (int i = 0; i < mas.Length; i++)
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
            int k = n / 10;
            for(int i=0; i<k; i++)
            {
                mas[i] = 1000;
            }
            for (int i = k; i < n; i++)
            {
                mas[i] = 1;
            }
            for (int i = 0; i < n; i++)
            {
            mas[i] = rand.Next(1000);
            }
            int mas_min, mas_max;
            mas_min = mas.Min();
            mas_max = mas.Max();
            int lenght = mas_max - mas_min;
            Console.WriteLine("Введите количество потоков: ");
            int p = Convert.ToInt32(Console.ReadLine());
            int len = lenght / p;
            sw.Start();
            MyThread[] threads = new MyThread[p];
            int[] ranges = new int[p+1];
            for(int i=0; i<p; i++)
            {
                ranges[i] = i * len;
            }
            ranges[p] = mas_max;
            if (p == 1)
            {
                threads[0] = new MyThread(mas);
            }
            for (int i = 0; i < p-1; i++)
            {
                List<int> list = new List<int>();
                for (int j=0; j<n; j++)
                {
                    if (mas[j] >= ranges[i] && mas[j] < ranges[i + 1])
                    {
                        list.Add(mas[j]);
                    }
                }
                var range = list.ToArray();
                Console.WriteLine("Количество элементов для потока " + i + " = " + range.Length);
                threads[i] = new MyThread(range);
            }
            List<int> list_end = new List<int>();
            if (p >= 2)
            {
                for (int j = 0; j < n; j++)
                {
                    if (mas[j] >= ranges[p - 1] && mas[j] <= ranges[p])
                    {
                        list_end.Add(mas[j]);
                    }
                }
                var range_end = list_end.ToArray();
                Console.WriteLine("Количество элементов для потока " + (p-1) + " = " + range_end.Length);
                threads[p - 1] = new MyThread(range_end);
            }
            for (int i = 0; i < p; i++)
            {
                threads[i].handle.WaitOne();
            }
            int[] ans = threads[0].mas;
            for(int i=1; i<p; i++)
            {
                ans = ans.Concat(threads[i].mas).ToArray();
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine("time = " + time);
        }
    }
}
