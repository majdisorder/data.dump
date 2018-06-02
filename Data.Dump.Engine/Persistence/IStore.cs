using System.Data;

namespace Data.Dump.Persistence
{
    public interface IStore
    {
        IDbConnection OpenSession();
    }
}