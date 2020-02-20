using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services.Contracts
{
    public interface ISmsSender
    {
        #region BaseClass

        Task SendSmsAsync(string number, string message);

        #endregion

        #region CustomMethods

        #endregion
    }
}