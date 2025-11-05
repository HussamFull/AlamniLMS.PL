using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlamniLMS.DAL.Utils
{
    public interface ISeedData
    {
        Task DataSeedingAsync();

        Task IdentityDataSeedingAsync();
    }
}
