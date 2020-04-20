using System;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public class StringInsertModelExt : IDisposable
    {
         public int Id { get; set; }
         public string VarName { get; set; }
         public string Format { get; set; }
         public BorderSubString BorderSubString { get; set; }

        //TODO: сделать конструктор и убрать get доступ в prop





        public void Dispose()
        {
        }
    }
}