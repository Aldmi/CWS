namespace Infrastructure.Dal.Abstract
{
    public enum HowCreateDb : byte
    {
        None,                 //Не создавать
        Migrate,              //С помощью последней миграции 
        EnsureCreated         //Принудительно
    }
}