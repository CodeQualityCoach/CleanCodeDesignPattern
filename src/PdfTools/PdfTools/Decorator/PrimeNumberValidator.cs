using System.Linq;
using PdfTools.Logging.NLog.SuperMath;

namespace PdfTools.Decorator
{
    public class PrimeNumberValidator : IValidatorStrategy
    {
        public bool IsValid(int value)
        {
            return value > 1 && Enumerable.Range(2, value - 2).All(i => value % i != 0);
        }
    }

    public class SuperFastPrimeNumberValidatorAdapter : IValidatorStrategy
    {
        public bool IsValid(int value)
        {
            var mathLib = new SuperPrime();
            var result = mathLib.IsPrimeButFast(value);
            return result;
        }
    }
}