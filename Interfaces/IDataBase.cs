namespace Oop.Client.Interfaces
{
    public interface IInitialize
    {
        Task Initialize();
    }
    public interface IDataBase
    {
    }
    public interface IIndexDBService : IDataBase, IAsyncDisposable
    {

    }

}
