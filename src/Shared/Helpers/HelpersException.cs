using System.Collections.Generic;

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
