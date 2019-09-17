namespace Infrastructure.Dal.Abstract
{
    public enum HowCreateDb
    {
        None,                 //Не создавать
        Migrate,              //С помощью последней миграции 
        EnsureCreated         //Принудительно
    }
}