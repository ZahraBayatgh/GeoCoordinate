using MagicOnion;

namespace Opeqe.Identity.API.Grpc1
{
    public interface IIdentityService : IService<IIdentityService>
    {
        // Return type must be `UnaryResult<T>` or `Task<UnaryResult<T>>`.
        // If you can use C# 7.0 or newer, recommend to use `UnaryResult<T>`.
        UnaryResult<int> LoginAsync(string model);
    }
}