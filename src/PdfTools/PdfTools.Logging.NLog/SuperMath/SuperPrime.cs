using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.Logging.NLog.SuperMath
{
    public class SuperPrime
    {
        public int GetRandomPrime()
        {
            return 2;
        }

        public bool IsPrimeButFast(int prime)
        {
            if (prime == 2) return true;

            for (int i = 3; i < Math.Sqrt(prime); i = i + 2)
                if (prime % i == 0) return false;

            return true;
        }
    }
}
