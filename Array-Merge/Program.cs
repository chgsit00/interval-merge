using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Array_Merge
{
    public class Program
    {
        static void Main()
        {
            var test = new List<int[]>
            {
                new []{4,8},
                new []{12,15},
                new []{9,10},
                new []{15,20},
                new []{3,6},
                new []{1,2},
                new []{1,5} // Not an valid interval, but the program will ignore the 4 and just look at 1 and 5
            };
            try
            {
                Merge(test);
            }
            catch (WrongInputException e)
            {
                Console.WriteLine("\n"+e.Message);
            }
            Console.ReadKey();
        }

        //----------------------------------------------------------------------------------------
        // Performs the Sorting and Merging as well as the console outputs
        //----------------------------------------------------------------------------------------
        public static void Merge(List<int[]> intervals)
        {
            Console.WriteLine("\nInput:");
            PrintIntervals(intervals);
            var result = MergeIntervals(intervals);
            Console.WriteLine("\nMerged Intervals:");
            PrintIntervals(result);
        }

        //----------------------------------------------------------------------------------------
        // Merges Intervals represented by a list of one-dimensional arrays
        //----------------------------------------------------------------------------------------
        public static List<int[]> MergeIntervals(List<int[]> intervals)
        {
            // No need to merge anything, if there are not enough intervals
            if (intervals.Count < 2)
                throw new WrongInputException("Not enough intervals provided to perform a merge");
            // First the intervals are sorted
            intervals = SortIntervals(intervals);
            Console.WriteLine("\nSorted Input Intervals:");
            PrintIntervals(intervals);
            var buffer = new Stack<int[]>();
            // put the first interval from the sorted List into the buffer for later comparison if valid
            CheckForValidInterval(intervals.First());
            PrintMemorySize(intervals.First());
            buffer.Push(intervals.First());
            // Start with the second interval in the sorted List
            for (var i = 1; i < intervals.Count; i++)
            {
                CheckForValidInterval(intervals[i]);
                // Check for an overlap between the interval in the buffer and the interval currently looked at.
                if (OverlapExists(buffer.Peek(), intervals[i]))
                {
                    // If there is an overlap, both intervals get merged and put back into the buffer
                    var temp = buffer.Pop();
                    var temp2 = new[] { temp.First(), temp.Last() };
                    temp2[1] = Math.Max(temp.Last(), intervals[i].Last());
                    buffer.Push(temp2);
                } else
                {
                    // Otherwise we put the current interval into the buffer for comparison to the next interval
                    buffer.Push(intervals[i]);
                }
            }
            PrintMemorySize(intervals);
            PrintMemorySize(buffer);
            // The Stack ist printed in descending order, so it has to be reversed
            return buffer.Reverse().ToList();
        }

        private static void PrintMemorySize(object o)
        {
            long size = 0;
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, o);
                size = s.Length;
            }
            Console.WriteLine("\n\n Size of object ${0}: ${1} bytes", o, size);
        }

        //----------------------------------------------------------------------------------------
        // If one of the arrays in the list has a number at position [0] that ist bigger than the
        // number at the last position, this array is not a valid interval
        //----------------------------------------------------------------------------------------
        private static void CheckForValidInterval(int[] interval)
        {
            if (interval.Last() < interval.First())
                throw new WrongInputException(
                    $"One of the given intervals is in a wrong Order:\n[{interval.First()}, {interval.Last()}]");
        }

        //----------------------------------------------------------------------------------------
        // Sort the given intervals by their smallest members.
        // For example [5,7] [0,3] [2,4] -> [0,3] [2,4] [5,7]
        //----------------------------------------------------------------------------------------
        private static List<int[]> SortIntervals(List<int[]> intervals)
        {
            var comparer = new IntervalComparer();
            intervals.Sort(comparer);
            return intervals;
        }

        //----------------------------------------------------------------------------------------
        // If the end of the first interval is a higher of the same number than the start of the
        // second interval, we have an overlap
        //----------------------------------------------------------------------------------------
        private static bool OverlapExists(int[] first, int[] second)
        {
            var start = Math.Max(first.First(), second.First());
            var end = Math.Min(first.Last(), second.Last());
            return end - start >= 0;
        }

        private static void PrintIntervals(List<int[]> intervals)
        {
            intervals.ForEach(x => Console.Write("[" + x.First() + "," + x.Last() + "]"));
        }
    }

    //--------------------------------------------------------------------------------------------
    // This Comparer is used for sorting the List of intervals
    //--------------------------------------------------------------------------------------------
    public class IntervalComparer : Comparer<int[]>{

        //----------------------------------------------------------------------------------------
        // Compares the given intervals by their first member
        //----------------------------------------------------------------------------------------
        public override int Compare(int[] x, int[] y)
        {
            // Avoid Errors during Integer Shift
            if (x[0] == int.MinValue && y[0] > int.MinValue)
            {
                return -1;
            }
            if (y[0] == int.MinValue && x[0] > int.MinValue)
            {
                return 1;
            }
            if (y[0] == int.MinValue && x[0] == int.MinValue)
            {
                return 0;
            }
            return x[0] - y[0];
        }
    }
}
