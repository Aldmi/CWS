using Domain.InputDataModel.Base.InData;

namespace Domain.InputDataModel.OpcServer.Model
{
    public class OpcInputType : InputTypeBase
    {
        #region prop
        public string NameProp { get; private set; }                    //имя свойства
        public double Voltage { get; private set; }                     //напряжение
        public int Temperature { get; private set; }                 //температура
        #endregion


        #region ctor
        public OpcInputType(int id, string nameProp, double voltage) : base(id)
        {
            NameProp = nameProp;
            Voltage = voltage;
        }

        /// <summary>
        /// для дефолтного создания объекта DefaultItemJson
        /// </summary>
        public OpcInputType() : base(0)
        {
        }
        #endregion
    }
}