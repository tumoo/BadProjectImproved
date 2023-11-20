using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty;

namespace BadProject.Services.Contracts
{
    public interface INoSqlAdvProviderService
    {
        Advertisement GetAdv(string webId, Queue<DateTime> errors);
    }
}
