using System;
using AutoMapper;
using OkanDemir.Dto;
using OkanDemir.Model;

namespace OkanDemir.Business.Mapper
{
    internal class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<ArchiveCategoryDto, ArchiveCategory>().ReverseMap();
            CreateMap<ArchiveDto, Archive>().ReverseMap();
            CreateMap<IncomeTypeDto, IncomeType>().ReverseMap();
            CreateMap<IncomeDto, Income>().ReverseMap();
            CreateMap<InvoiceTypeDto, InvoiceType>().ReverseMap();
            CreateMap<InvoiceDto, Invoice>().ReverseMap();
            CreateMap<CodeCategoryDto, CodeCategory>().ReverseMap();
            CreateMap<CodeNoteDto, CodeNote>().ReverseMap();
            CreateMap<SubscriptionDto, Subscription>().ReverseMap();
            CreateMap<SubscriptionTypeDto, SubscriptionType>().ReverseMap();
            CreateMap<HashtagDto, Hashtag>().ReverseMap();
            CreateMap<MetionDto, Metion>().ReverseMap();
            CreateMap<NoteDto, Note>().ReverseMap();
            CreateMap<RegisterRequestDto, UserDto>().ReverseMap();
            CreateMap<RegisterRequestDto, User>().ReverseMap();
            CreateMap<TodoProject, TodoProjectDto>().ReverseMap();
        }
    }
}
