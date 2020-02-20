using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Data.Context
{
    public partial class MongoContextBase
    {
        public readonly IMongoDatabase _database = null;
        public IMongoClient _client;

        #region Ctors
        public MongoContextBase(string mongoConnectionString, string mongoDatabase)
        {
            _client = new MongoClient(mongoConnectionString);
            if (_client != null)
            {
                _database = _client.GetDatabase(mongoDatabase);
            }
        }
        public MongoContextBase(IMongoClient client, IMongoDatabase mongodatabase)
        {
            _database = mongodatabase;
            _client = client;
        }
        #endregion

        #region Methods
        public IMongoClient Client()
        {
            return _client;
        }

        public IMongoDatabase Database()
        {
            return _database;
        }

        public TResult RunCommand<TResult>(string command)
        {
            return _database.RunCommand<TResult>(command);
        }

        public TResult RunCommand<TResult>(string command, ReadPreference readpreference)
        {
            return _database.RunCommand<TResult>(command, readpreference);
        }

        public BsonValue RunScript(string command, CancellationToken cancellationToken)
        {
            var script = new BsonJavaScript(command);
            var operation = new EvalOperation(_database.DatabaseNamespace, script, null);
            var writeBinding = new WritableServerBinding(_client.Cluster, NoCoreSession.NewHandle());
            return operation.Execute(writeBinding, CancellationToken.None);
        }

        public Task<BsonValue> RunScriptAsync(string command, CancellationToken cancellationToken)
        {
            var script = new BsonJavaScript(command);
            var operation = new EvalOperation(_database.DatabaseNamespace, script, null);
            var writeBinding = new WritableServerBinding(_client.Cluster, NoCoreSession.NewHandle());
            return operation.ExecuteAsync(writeBinding, CancellationToken.None);

        }
        private IClientSessionHandle _session;
        private DateTime _transactionStartDateTime;
        /// <summary>
        /// Start Transaction
        /// </summary>
        public async Task StartTransaction()
        {
            _transactionStartDateTime = DateTime.Now;

            _session = await _client.StartSessionAsync();
            _session.StartTransaction();
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public async Task CommitTransaction()
        {
            await _session.CommitTransactionAsync();
        }

        /// <summary>
        /// Abort Transaction
        /// </summary>
        public async Task AbortTransaction()
        {
            await _session.AbortTransactionAsync();
        }
        #endregion
    }
}
