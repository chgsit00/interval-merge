using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Array_Merge;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestMergeIntervals_WithNormalInput()
        {
            var test = new List<int[]>
            {
                new[] {4, 8},
                new[] {12, 15},
                new[] {9, 10},
                new[] {15, 20},
                new[] {3, 6},
                new[] {1, 2},
            };
            var expectedResult = new List<int[]>
            {
                new[] {1, 2},
                new[] {3, 8},
                new[] {9, 10},
                new[] {12, 20}
            };
            var result = Program.MergeIntervals(test);
            CollectionAssert.AreEqual(expectedResult, result, StructuralComparisons.StructuralComparer);
        }

        [TestMethod]
        public void TestMergeIntervals_WithBigNumbers()
        {
            var test = new List<int[]>
            {
                new[] {int.MinValue, 8},
                new[] {12, 15},
                new[] {9, 10},
                new[] {15, 20},
                new[] {3, 6},
                new[] {1, int.MaxValue}
            };
            var expectedResult = new List<int[]>
            {
                new[] {int.MinValue, int.MaxValue}
            };
            var result = Program.MergeIntervals(test);
            CollectionAssert.AreEqual(expectedResult, result, StructuralComparisons.StructuralComparer);
        }

        [TestMethod]
        public void TestMergeIntervals_WithWrongInput()
        {
            var test = new List<int[]>
            {
                new[] {1, 2, 5}, // This is not a valid Interval
                new[] {3, 8},
            };
            var expectedResult = new List<int[]>
            {
                new[] {1, 8} // But the program will handle it anyway
            };
            var result = Program.MergeIntervals(test);
            CollectionAssert.AreEqual(expectedResult, result, StructuralComparisons.StructuralComparer);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongInputException))]
        public void TestMergeIntervals_WithNotEnoughIntervals()
        {
            var test = new List<int[]>
            {
                new[] {1, 5} // Just one Interval is not enough
            };
            Program.MergeIntervals(test);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongInputException))]
        public void TestMergeIntervals_WithWrongInputOrder()
        {
            var test = new List<int[]>
            {
                new[] {1, 5},
                new[] {9, 4}, // This is not a valid Interval
            };
            Program.MergeIntervals(test);
        }

        [TestMethod]
        public void TestIntervalComparer()
        {
            var test = new List<int[]>
            {
                new[] {3, 9},
                new[] {1, 4},
                new[] {7, 12},
                new[] {0, 2},
            };
            var expectedResult = new List<int[]>
            {
                new[] {0, 2},
                new[] {1, 4},
                new[] {3, 9},
                new[] {7, 12},
            };
            var comparer = new IntervalComparer();
            test.Sort(comparer);
            // test should now be equal to the sorted interval-list
            CollectionAssert.AreEqual(expectedResult, test, StructuralComparisons.StructuralComparer);
        }

        [TestMethod]
        public void TestMergeIntervals_ManyIntervals()
        {
            const int numberOfIntervals = 10000;
            var test = generateTestIntervals(numberOfIntervals);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = Program.MergeIntervals(test);
            stopwatch.Stop();
            Console.WriteLine("\n Original number of intervals was: " + numberOfIntervals);
            Console.WriteLine("Number of merged intervals is: " + result.Count);
            Console.WriteLine("Execution Time was: " + stopwatch.ElapsedMilliseconds + " ms");
        }

        private List<int[]> generateTestIntervals(int numberOfIntervals)
        {
            var min = int.MinValue;
            var max = int.MaxValue - 100;
            var intervals = new List<int[]>();
            for (var i = 0; i < numberOfIntervals; i++)
            {
                var random = new Random();
                var randomNumber = random.Next(min, max);
                var distance = random.Next(0, 100);
                var interval = new[] {randomNumber, randomNumber + distance};
                intervals.Add(interval);
            }

            return intervals;
        }
    }
}
