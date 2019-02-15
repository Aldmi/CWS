using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helpers
{
    public static class HelpersException
    {
        public static void ThrowIfNotFind<T>(T obj, string message) where T : class
        {
            if (obj == null)
                throw new KeyNotFoundException(message);
        }

    }
}
