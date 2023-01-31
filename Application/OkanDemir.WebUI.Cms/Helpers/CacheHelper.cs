using OkanDemir.Infrastructure.Interfaces;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public class CacheHelper
    {
        ICache cache;

        public CacheHelper(ICache cache)
        {
            this.cache = cache;
        }

        public bool Clear(string name)
        {
            cache.Remove(name);

            return true;
        }

        public T Get<T>(string cacheKey) where T : class
        {
            object cookies;

            if (!cache.TryGetValue(cacheKey, out cookies))
                return null;

            return cookies as T;
        }

        public void Set(string cacheKey, object value)
        {
            cache.Set(cacheKey, value, 180);
        }
    }
}
