using BadProject.AdvertisementService.Contracts;
using BadProject.Cache.Contracts;
using BadProject.Services.Contracts;
using BadProject.Services.Helper;
using System;
using System.Collections.Generic;
using ThirdParty;

namespace Adv
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly INoSqlAdvProviderService _noSqlProvider;
        private readonly ISQLAdvProviderService _sqlProvider;
        private readonly ICacheProvider _cacheProvider;
        private static Queue<DateTime> errors = new Queue<DateTime>();

        private readonly object lockObj = new object();

        public AdvertisementService(INoSqlAdvProviderService noSqlProvider,
         ISQLAdvProviderService sqlProvider,
         ICacheProvider cacheProvider)
        {
            _noSqlProvider = noSqlProvider ?? throw new ArgumentNullException(nameof(noSqlProvider));
            _sqlProvider = sqlProvider ?? throw new ArgumentNullException(nameof(sqlProvider));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        public Advertisement GetAdvertisement(string id)
        {
            Advertisement adv = null;

            lock (lockObj)
            {
                adv = _cacheProvider.GetAdvertisementFromCache(id);

                if (adv == null)
                {
                    int errorCount = ErrorQueueHelper.CountErrorsInLastHour(errors);

                    if (errorCount < 10)
                    {
                        adv = _noSqlProvider.GetAdv(id,errors);
                    }
                }
                if (adv == null)
                {
                    adv = _sqlProvider.GetAdv(id);
                }
            }
            return adv;
        }
    }
}