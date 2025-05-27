using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Utils
{
    public static class CollectionHelper
    {
        public static List<ulong> GetDifference(IEnumerable<ulong> longerList, IEnumerable<ulong> shorterList)
        {
            return longerList.Except(shorterList).ToList();
        }

        public static List<TResult> MapTo<T, TResult>(IEnumerable<T> collection, Func<T, TResult> selector)
        {
            return collection.Select(selector).ToList();
        }
    }
}
