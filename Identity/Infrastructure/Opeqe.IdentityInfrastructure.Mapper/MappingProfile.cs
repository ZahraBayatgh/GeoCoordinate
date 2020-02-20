using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.ViewModels;

namespace Opeqe.IdentityInfrastructure.Mapper
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            #region AppLoginData
            CreateMap<AppLoginData, AppLoginDataViewModel>();
            CreateMap<AppLoginDataViewModel, AppLoginData>();
            #endregion
        }
    }
}
