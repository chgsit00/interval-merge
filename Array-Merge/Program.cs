using System;
using System.Collections.Generic;
using System.Linq;

namespace Array_Merge
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new List<int[]>
            {
                new []{4,8},
                new []{12,15},
                new []{9,10},
                new []{15,20},
                new []{3,6},
                new []{1,2},
                new []{1,4,5} // Not an valid interval, but the program will ignore the 4 and just look at 1 and 5
            };
            // No need to merge anything, if there are not enough intervals
            if (test.Count > 2)
            {
                Console.WriteLine("\nInput:");
                PrintIntervals(test);
                var sortedIntervals = SortIntervals(test);
                Console.WriteLine("\nSorted Input Intervals:");
                PrintIntervals(sortedIntervals);
                Console.WriteLine("\nMerged Intervals:");
                var result = MergeIntervals(sortedIntervals);
                PrintIntervals(result);
            }
            else
            {
                Console.WriteLine("Not enough intervals provided to perform a merge");
                PrintIntervals(test);
            }
            Console.ReadKey();
        }

        //----------------------------------------------------------------------------------------
        // Merges Intervals represented by a list of one-dimensional arrays with two members each
        //----------------------------------------------------------------------------------------
        private static List<int[]> MergeIntervals(IReadOnlyList<int[]> intervals)
        {
            var buffer = new Stack<int[]>();
            // put the first interval from the sorted List into the buffer for later comparison
            buffer.Push(intervals.First());
            // Start with the second interval in the sorted List
            for (var i = 1; i < intervals.Count; i++)
            {
                // Check for an overlap between the interval in the buffer and the interval currently looked at.
                if (OverlapExists(buffer.Peek(), intervals[i]))
                {
                    // If there is an overlap, both intervals get merged and put back into the buffer
                    var temp = buffer.Pop();
                    temp[temp.Length-1] = Math.Max(temp.Last(), intervals[i].Last());
                    buffer.Push(temp);
                } else
                {
                    // Otherwise we put the current interval into the buffer for comparison to the next interval
                    buffer.Push(intervals[i]);
                }
            }
            // The Stack ist printed in descending order, so it has to be reversed
            return buffer.Reverse().ToList();
        }

        //----------------------------------------------------------------------------------------
        // Sort the given intervals by their smallest members.
        // For example [5,7] [0,3] [2,4] -> [0,3] [2,4] [5,7]
        //----------------------------------------------------------------------------------------
        public static List<int[]> SortIntervals(List<int[]> intervals)
        {
            var comparer = new IntervalComparer();
            intervals.Sort(comparer);
            return intervals;
        }

        //----------------------------------------------------------------------------------------
        // If the end of the first interval is a higher of the same number than the start of the
        // second interval, we have an overlap
        //----------------------------------------------------------------------------------------
        public static bool OverlapExists(int[] first, int[] second)
        {
            var start = Math.Max(first.First(), second.First());
            var end = Math.Min(first.Last(), second.Last());
            return end - start >= 0;
        }

        public static void PrintIntervals(List<int[]> intervals)
        {
            intervals.ForEach(x => Console.Write("[" + x.First() + "," + x.Last() + "]"));
        }
    }

    //--------------------------------------------------------------------------------------------
    // This Comparer is used for sorting the List of intervals
    //--------------------------------------------------------------------------------------------
    class IntervalComparer : Comparer<int[]>{

        //----------------------------------------------------------------------------------------
        // Compares the given intervals by their first member
        //----------------------------------------------------------------------------------------
        public override int Compare(int[] x, int[] y)
        {
            return x[0] - y[0];
        }
    }
}
