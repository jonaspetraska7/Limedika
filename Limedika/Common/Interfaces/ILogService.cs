using Common.Entities;
using LinqToDB.Data;

namespace Common.Interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Method for getting a list of logs
        /// </summary>
        /// <returns>A list of log objects</returns>
        Task<List<Log>> GetLogs();
        /// <summary>
        /// Method for inserting a log to the database
        /// </summary>
        /// <param name="log">A log object to insert</param>
        /// <returns>A status</returns>
        Task<int> InsertLog(Log log);
        /// <summary>
        /// Method for bulk inserting logs to the database
        /// </summary>
        /// <param name="logs">A list of logs</param>
        /// <returns>Info about the rows copied</returns>
        Task<BulkCopyRowsCopied> BulkInsertLogs(List<Log> logs);
    }
}
