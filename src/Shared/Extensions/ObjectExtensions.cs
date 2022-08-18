using CSharpFunctionalExtensions;
using KellermanSoftware.CompareNetObjects;

namespace Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static Result<bool> Compare<T>(this T obj1, T obj2)
        {
            var compareLogic = new CompareLogic { Config = { MaxMillisecondsDateDifference = 1000 } };
            ComparisonResult result = compareLogic.Compare(obj1, obj2);
            return result.AreEqual ? Result.Ok(true) : Result.Failure<bool>(result.DifferencesString);
        }
    }
}