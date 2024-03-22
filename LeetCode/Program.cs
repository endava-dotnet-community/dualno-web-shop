using BenchmarkDotNet.Running;

namespace LeetCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<LongestPalindromicSubstring>();
        }
    }
}
