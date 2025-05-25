using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashidsNet;

namespace Web_Shop.Application.Utils
{
    public static class HashUtils
    {
        public static string EncodeHashId(this ulong id, IHashids hashIds)
        {
            return hashIds.EncodeLong((long)id);
        }

        public static string EncodeHashId(this ulong? id, IHashids hashIds)
        {
            ulong hashedId = id ?? default;
            return hashedId.EncodeHashId(hashIds);
        }

        public static ulong DecodeHashId(this string hashid, IHashids hashIds)
        {
            return (ulong)hashIds.DecodeSingleLong(hashid);
        }

        public static IEnumerable<ulong> DecodeHashIds(this IEnumerable<string> iDs, IHashids hashIds)
        {
            foreach (var id in iDs)
            {
                yield return id.DecodeHashId(hashIds);
            }
        }
    }
}
