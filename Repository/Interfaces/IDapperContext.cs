using System.Data;
using System;

namespace MyPSG.API.Repository.Interfaces
{
    public interface IDapperContext : IDisposable
    {
        IDbConnection Db { get; }
		IDbTransaction Transaction { get; }
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        string GetGUID();
    }
}