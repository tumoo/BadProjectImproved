using BadProject.Cache.Contracts;
using BadProject.Services.Contracts;
using NLog;
using System;
using ThirdParty;

namespace BadProject.Services
{
    public class SQLAdvProviderService : ISQLAdvProviderService
    {
        private readonly ILogger _logger;
        private readonly ICacheProvider _cacheProvider;
        public SQLAdvProviderService(ICacheProvider cacheProvider, ILogger logger)
        {
            _logger = logger;
            _cacheProvider = cacheProvider;
        }
        public Advertisement GetAdv(string webId)
        {

            Advertisement adv = null;
            try
            {
                adv = SQLAdvProvider.GetAdv(webId);

                if (adv != null)
                {
                    _cacheProvider.SetAdvertisementCache(webId,adv);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving data from SQLAdvProvider");
            }

            return adv;
        }
    }
}
