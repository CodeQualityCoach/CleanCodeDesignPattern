using System;
using System.Collections.Generic;

namespace PdfTools.Decorator
{
    public class CachingValidatorDecorator : IValidatorStrategy
    {
        private readonly IValidatorStrategy _validator;
        private readonly Dictionary<int, bool> _cache;

        public CachingValidatorDecorator(IValidatorStrategy validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _cache = new Dictionary<int, bool>();
        }

        public bool IsValid(int value)
        {
            if (_cache.ContainsKey(value)) return _cache[value];

            var result = _validator.IsValid(value);
            _cache[value] = result;
            return result;
        }
    }
}