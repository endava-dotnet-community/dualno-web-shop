using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
    [MemoryDiagnoser]
    
    public class LongestPalindromicSubstring : ISolution
    {
        [Params("aaaaaaa", "ratar", "radio", "babad", "cbbd")]
        public string palindrom;


        [Benchmark]
        public void DavidIlic()
        {

        }
        [Benchmark]
        public void FilipMandic()
        {

        }
        [Benchmark]
        public void IlijaPavlovic()
        {

        }
        [Benchmark]
        public void LaraTrivunovic()
        {

        }
        [Benchmark]
        public void LazarMilinkovic()
        {
        }
        [Benchmark]
        public void LukaNikolic()
        {

        }
        [Benchmark]
        public void MihajloIvanovic()
        {

        }
        [Benchmark]
        public void MladenDencic()
        {

        }
        [Benchmark]
        public void NikolaStojkovic()
        { 
        }
        [Benchmark]
        public void PetarVincic()
        {

        }
        [Benchmark]
        public void VeljkoStojanovic()
        {

        }
    }
}
