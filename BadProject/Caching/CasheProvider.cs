using BadProject.Cache.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Cache
{
    public class CasheProvider : ICacheProvider
    {
        private readonly ObjectCache _cache;

        public CasheProvider()
        {
            _cache = MemoryCache.Default;
        }

        public Advertisement GetAdvertisementFromCache(string id)
        {
            //return _cache.Get<Advertisement>($"AdvKey_{id}");
            return (Advertisement)_cache.Get($"AdvKey_{id}");
        }

        public void SetAdvertisementCache(string id,Advertisement adv)
        {
            _cache.Set($"AdvKey_{id}", adv, DateTimeOffset.Now.AddMinutes(5));
        }
    }
}
