﻿using AutoMapper;
using SkyStorage.Application.FileDetails.Dtos;
using SkyStorage.Domain.Entities;

namespace SkyStorage.Application.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<FileDetail, FileDetailDto>().ReverseMap();
    }
}