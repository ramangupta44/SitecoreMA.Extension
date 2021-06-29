using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA.CustomMarketingApps.Model
{
    public interface IListSubscriptionsStore<T> where T : new()
    {
        IEnumerable<T> GetSubscribers(Guid listId, string searchFilter, int pageIndex, int pageSize);
    }
}
