namespace DatPhongNhanh.Application.Common.Caching
{
    public class CacheKeyPolicy
    {
        private readonly string _prefix;
        private readonly Dictionary<string, Func<object, string>> _keyGenerators;

        public CacheKeyPolicy(string prefix)
        {
            _prefix = prefix;
            _keyGenerators = [];
        }

        public CacheKeyPolicy AddKeyGenerator(string name, Func<object, string> generator)
        {
            _keyGenerators[name] = generator;
            return this;
        }

        public bool TryGetKey(out string? key, string generatorName, object value)
        {
            if (!_keyGenerators.TryGetValue(generatorName, out var generator))
            {
                key = null;
                return false;
            }

            key =  $"{_prefix}:{generator(value)}";
            return true;
        }
    }
}
