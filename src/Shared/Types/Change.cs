namespace Shared.Types
{

    public class Change<T>
    {
        public Change(T startVal, T finishVal)
        {
            StartVal = startVal;
            FinishVal = finishVal;
        }

        public T StartVal { get; }
        public T FinishVal { get; }
    }
}