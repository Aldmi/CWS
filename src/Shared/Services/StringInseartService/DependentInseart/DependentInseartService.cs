using System.Text;
using CSharpFunctionalExtensions;
using Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers;

namespace Shared.Services.StringInseartService.DependentInseart
{
    public class DependentInseartService
    {
        private readonly BaseDepInsH[] _depInsHArray;
        public DependentInseartService(params BaseDepInsH[] depInsHArray)
        {
            _depInsHArray = depInsHArray;
        }
        
        /// <summary>
        /// Выполнить ЗАВИСИМУЮ вставку элементов в строку.
        /// Каждый обработчик берет строку для выполнения вставки (и возможного преобразования стоки).
        /// все изменения непосредственно мутируют строку типа StringBuilder.
        /// Если обработчик из списка не найдет "свое применние к строке", то обработчик верент Result.Failure и дальнейшая обработка прекратится. 
        /// </summary>
        public Result<StringBuilder> ExecuteInsearts(StringBuilder sb, string format)
        {
            if (_depInsHArray == null || sb == null)
                return Result.Failure<StringBuilder>("_depInsHArray == null || sb == null");

            foreach (var handler in _depInsHArray)
            {
                //Вычислить результат ОБРАБОТКИ
                var (_, isFailure, _, error) = handler.CalcInsert(sb, format);
                if (isFailure)
                {
                    return Result.Failure<StringBuilder>(error);
                }
            }
            return Result.Ok(sb);
        }
    }
}