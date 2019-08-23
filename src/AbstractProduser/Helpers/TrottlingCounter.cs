namespace AbstractProduser.Helpers
{
    /// <summary>
    /// Счетчик до наступления троттлинга
    /// </summary>
    public class TrottlingCounter
    {
        #region field

        private readonly int _quantityBeforeThrottling;
        private int _count;

        #endregion



        #region prop

        public bool IsTrottle => (_count >= _quantityBeforeThrottling);

        #endregion



        #region ctor

        public TrottlingCounter(int quantityBeforeThrottling)
        {
            _quantityBeforeThrottling = quantityBeforeThrottling;
        }

        #endregion




        #region Operator

        public static TrottlingCounter operator++ (TrottlingCounter instance)
        {         
            instance._count++;
            return instance;
        }

        public static TrottlingCounter operator --(TrottlingCounter instance)
        {
            instance._count--;
            return instance;
        }

        #endregion

    }
}