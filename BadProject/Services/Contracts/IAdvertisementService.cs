using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.AdvertisementService.Contracts
{
    public interface IAdvertisementService
    {
        Advertisement GetAdvertisement(string id);
    }
}
