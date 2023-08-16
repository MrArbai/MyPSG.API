using System;
using System.Data;
using System.Data.Common;
using MyPSG.API.Repository.Interfaces;
using Npgsql;

namespace MyPSG.API.Repository.Implements
{
    public class DapperContext : IDapperContext
    {
        private IDbConnection _db;
        private readonly string _connectionString;
        
        public DapperContext()
        {
            var server = "localhost";
            var port = "5432";
            var dbName = "sgaccounting";
            var appName = "SambuAF";
            var userId = "postgres";
            var userPassword = "sambuacc@2022";

  
            _connectionString = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};ApplicationName={5};Timeout=600", server, port, userId, userPassword, dbName, appName);

            if(_db == null){
                _db = GetOpenConnection( _connectionString);
            }
        }

        private IDbConnection GetOpenConnection(string connectionString)
        {
            DbConnection conn;
            try
            {
                NpgsqlFactory provider = NpgsqlFactory.Instance;
                conn = provider.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
            }
            catch(SystemException e)
            {
                throw new Exception(e.Message);
            }

            return conn;
        }

        public IDbConnection db {
            get { return _db ?? (_db = GetOpenConnection( _connectionString)); }
        }

        public IDbTransaction transaction { get; private set; }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (transaction == null)
                transaction = _db.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }  
        }

        public void Dispose()
        {
            if (_db != null)
            {
                try
                {
                    if (_db.State != ConnectionState.Closed)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }

                        _db.Close();
                    }                        
                }
                finally
                {
                    _db.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }

        public string GetGUID()
        {
            string result;
            try
            {
                result = Guid.NewGuid().ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }    
        }
    }
}
