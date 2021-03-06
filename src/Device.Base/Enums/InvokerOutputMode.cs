﻿namespace Domain.Device.Enums
{
    public enum InvokerOutputMode : byte
    {
        /// <summary>
        /// Обработка сразу.
        /// </summary>
        Instantly,
        /// <summary>
        /// Обработка по внутреннему таймеру.
        /// </summary>
        ByTimer,
        /// <summary>
        /// Ожидание выставления флага обратной связи (выставляется внешним кодом).
        /// </summary>
        FeedBackWaiting
    }
}
