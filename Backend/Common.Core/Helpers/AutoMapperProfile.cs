using AutoMapper;
using Common.Core.Models.Accounts;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Helpers
{
    public class AutoMapperProfile : Profile
    {
        // mappings between model and entity objects
        public AutoMapperProfile()
        {
            CreateMap<Account, AuthenticateResponse>();
        }
    }
}
