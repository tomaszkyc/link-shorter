using System;
using System.Linq;
using System.Text;

namespace LinkShorter.Models
{
    public class ShortUrlGenerator
    {
        private const int MaxUrlLength = 6;
        private long AdId { get; set; }
        public string GeneratedShortUrl { get; set; }

        public ShortUrlGenerator(long _adId)
        {
            AdId = _adId;
            GeneratedShortUrl = EncodeInt32AsString(AdId, MaxUrlLength);
        }

        /// <summary>
        /// Encodes the 'input' parameter into a string of characters defined by the allowed list (0-9, A-Z) 
        /// </summary>
        /// <param name="input">Integer that is to be encoded as a string</param>
        /// <param name="maxLength">If zero, the string is returned as-is. If non-zero, the string is truncated to this length</param>
        /// <returns></returns>
        private static String EncodeInt32AsString(long input, Int32 maxLength = 0)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(maxLength)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();
            return id;
        }
    }
}
