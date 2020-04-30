using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart
{
    public static class DependentInseartsServiceFactory
    {
        /// <summary>
        /// фабрика 
        /// </summary>
        public static DependentInseartService Create(string str, IReadOnlyDictionary<string, StringInsertModelExt> extDict)
        {
            var insertModels= StringInsertModelFactory.CreateList(str, extDict);
            var insHandlers = CreateListDependentInseartHandlers(insertModels);
            var service = new DependentInseartService(insHandlers.ToArray());
            return service;
        }
        
        /// <summary>
        ///  Порядок выполнения обработчиков СТРОГО задан и не определяется порядком нахождения replacement в строке.
        /// </summary>
        private static List<BaseDepInsH> CreateListDependentInseartHandlers(IEnumerable<StringInsertModel> insertModels)
        {
            var array = insertModels as StringInsertModel[] ?? insertModels.ToArray();
            //1. Вставки NumberOfCharacters
            var handlers = (from model in array where model.VarName == "NumberOfCharacters" select new NumberOfCharactersDepInsH(model)).Cast<BaseDepInsH>().ToList();
            //2. Вставки NbyteFull и Nchar
            handlers.AddRange(from model in array
                where model.VarName == "Nchar"
                let crcModel = array.FirstOrDefault(m => m.VarName.Contains("CRC")) //передать доп модель. содержащую CRC
                select new NcharDepInsH(model, crcModel));
            handlers.AddRange(from model in array
                where model.VarName == "NbyteFull"
                let crcModel = array.FirstOrDefault(m => m.VarName.Contains("CRC")) //передать доп модель. содержащую CRC
                select new NbyteFullDepInsH(model, crcModel));
            //3. Вставки CRC
            handlers.AddRange(from model in array where model.VarName == "CRCXor" select new CrcXorDepInsH(model));
            handlers.AddRange(from model in array where model.VarName == "CRCXorInverse" select new CrcXorInverseDepInsH(model));
            handlers.AddRange(from model in array where model.VarName == "CRCMod256" select new CrcMod256DepInsH(model));
            handlers.AddRange(from model in array where model.VarName == "CRCMod256At" select new CrcMod256AlphaTimeDepInsH(model));
            handlers.AddRange(from model in array where model.VarName == "CRC8Bit" select new Crc8BitDepInsH(model));
            handlers.AddRange(from model in array where model.VarName == "CRCCcitt" select new Crc16CcittDepInsH(model));

            return handlers;
        }
    }
}