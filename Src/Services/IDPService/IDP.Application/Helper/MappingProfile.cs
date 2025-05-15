using AutoMapper;
using IDP.Application.Command.Auth;
using IDP.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Add as many of these lines as you need to map your objects
            CreateMap<AuthCommand,User>().ReverseMap();
        }
    }
}
