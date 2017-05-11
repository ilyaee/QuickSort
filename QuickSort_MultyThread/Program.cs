using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuickSort_MultyThread
{
	class Program
	{
		static void Main()
		{
			int n = 20000000;
			int[] a = new int[n];
			var rng = new Random(937525);

			for (int i = 0; i < n; ++i)
				a[i] = rng.Next();

			var sw = new Stopwatch();

			sw.Restart();
			QuicksortParallelOptimised(a, 0, a.Length - 1);
			Console.WriteLine("Time: " + sw.Elapsed);
			Console.ReadKey();
		}

		static void QuicksortSequential<T>(T[] arr, int left, int right)
		where T : IComparable<T>
		{
			if (right > left)
			{
				int pivot = Partition(arr, left, right);
				QuicksortSequential(arr, left, pivot - 1);
				QuicksortSequential(arr, pivot + 1, right);
			}
		}

		static void QuicksortParallelOptimised<T>(T[] arr, int left, int right)
		where T : IComparable<T>
		{
			const int end = 4096;
			if (right > left)
			{
				if (right - left < end)
				{
					QuicksortSequential(arr, left, right);
				}
				else
				{
					int pivot = Partition(arr, left, right);
					Parallel.Invoke(
						() => QuicksortParallelOptimised(arr, left, pivot - 1),
						() => QuicksortParallelOptimised(arr, pivot + 1, right));
				}
			}
		}

		static int Partition<T>(T[] arr, int low, int high) where T : IComparable<T>
		{
			int pivotPos = (high + low) / 2;
			T pivot = arr[pivotPos];
			Swap(arr, low, pivotPos);

			int left = low;
			for (int i = low + 1; i <= high; i++)
			{
				if (arr[i].CompareTo(pivot) < 0)
				{
					left++;
					Swap(arr, i, left);
				}
			}

			Swap(arr, low, left);
			return left;
		}

		static void Swap<T>(T[] arr, int i, int j)
		{
			T tmp = arr[i];
			arr[i] = arr[j];
			arr[j] = tmp;
		}
	}
}
