using System;
using System.Linq;

namespace Core.Utils
{
    public static class StringHelper
    {
        public static string ManyToOneLine(this string stringWithManyLines)
        {
            if (string.IsNullOrWhiteSpace(stringWithManyLines))
                return string.Empty;

            var oneLineString = stringWithManyLines
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
            return string.Join(' ', oneLineString);
        }
    }
}
