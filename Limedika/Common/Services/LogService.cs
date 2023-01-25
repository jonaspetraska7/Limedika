using Common.Data;
using Common.Entities;
using Common.Interfaces;
using LinqToDB;

namespace Common.Services
{
    public class LogService : ILogService
    {
        private readonly LimedikaDataConnection _connection;
        public LogService(LimedikaDataConnection connection)
        {
            _connection = connection;
        }

        public Task<List<Log>> GetLogs()
        {
            return _connection.Logs.ToListAsync();
        }

        public Task<int> InsertLog(Log log)
        {
            return _connection.InsertAsync(log);
        }
    }
}
