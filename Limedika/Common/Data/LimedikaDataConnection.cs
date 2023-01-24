using Common.Entities;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Common.Data
{
    public class LimedikaDataConnection : DataConnection
    {
        public LimedikaDataConnection(LinqToDBConnectionOptions<LimedikaDataConnection> options) 
            : base(options) { }

        public ITable<Client> Clients => this.GetTable<Client>();
        public ITable<Log> Logs => this.GetTable<Log>();
    }
}
