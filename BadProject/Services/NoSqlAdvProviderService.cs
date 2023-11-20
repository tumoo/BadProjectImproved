using BadProject.Cache.Contracts;
using BadProject.Services.Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Services
{
    public class NoSqlAdvProviderService : INoSqlAdvProviderService
    {
        private readonly ILogger _logger;
        private readonly ICacheProvider _cacheProvider;
        private readonly NoSqlAdvProvider _thirdPartyNoSqlProvider;
        private const int MaxRetryCount = 3;
        public NoSqlAdvProviderService(ICacheProvider cacheProvider, ILogger logger)
        {
            _logger = logger;
            _cacheProvider = cacheProvider;
            _thirdPartyNoSqlProvider = new NoSqlAdvProvider();
        }

        public Advertisement GetAdv(string webId,Queue<DateTime> errors)
        {
            Advertisement adv = null;
            int retry = 0;
            do
            {
                retry++;
                try
                {
                    // Delegate call to the third party class
                    adv = _thirdPartyNoSqlProvider.GetAdv(webId);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error retrieving data from NoSQLAdvProvider");
                    Thread.Sleep(1000);
                    errors.Enqueue(DateTime.Now);
                }
            } while (adv == null && retry < MaxRetryCount);

            if (adv != null)
            {
                _cacheProvider.SetAdvertisementCache(webId,adv);
            }

            return adv;
        }
    }
}
