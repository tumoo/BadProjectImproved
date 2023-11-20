using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Cache.Contracts
{
    public interface ICacheProvider
    {
        Advertisement GetAdvertisementFromCache(string id);
        void SetAdvertisementCache(string id, Advertisement adv);
    }
}
