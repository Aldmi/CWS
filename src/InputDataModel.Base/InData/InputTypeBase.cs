namespace Domain.InputDataModel.Base.InData
{
    public abstract class InputTypeBase
    {
        public int Id { get; private set; }

        protected InputTypeBase(int id)
        {
            Id = id;
        }
    }
}