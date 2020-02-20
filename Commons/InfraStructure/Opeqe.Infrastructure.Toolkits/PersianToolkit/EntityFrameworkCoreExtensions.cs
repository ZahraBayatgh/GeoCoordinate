using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Toolkits.PersianToolkit
{
    public class PersianYeKeCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            return result;
        }

        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(result);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result)
        {
            return result;
        }

        public override Task<InterceptionResult<int>> NonQueryExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(result);
        }

        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result)
        {
            return result;
        }

        public override Task<InterceptionResult<object>> ScalarExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(result);
        }

    }

}
