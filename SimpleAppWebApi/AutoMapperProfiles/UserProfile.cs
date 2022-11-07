﻿using AutoMapper;
using SimpleApp.Core.Models.Entity;
using SimpleApp.WebApi.DTO;

namespace SimpleApp.WebApi.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RegisterDto>()
                .ReverseMap()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
