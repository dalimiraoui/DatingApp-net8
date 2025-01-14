using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles :Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemeberDTO>()
           .ForMember( d => d.Age, o=> o.MapFrom( s => s.DateOfBirth.CalculateAge()))
           .ForMember(d => d.PhotoUrl, o =>
               o.MapFrom( s=>s.Photos.FirstOrDefault( x=>x.IsMain)!.Url));
        CreateMap<Photo, PhotoDTO>();

        CreateMap<MemberUpdateDTO, AppUser>();
        CreateMap<AppUser, MemberUpdateDTO>();
        CreateMap<RegisterDTO, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
    }

}
