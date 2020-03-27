using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.BLL.DTO;
using TaskList.Models;

namespace TaskList.ViewModels.Helpers
{
    public static class MapperHelpers
    {

        public static IMapper CreateAutoMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TodoDTO, TodoModel>()
                    .ForMember(x => x.ContentTodo, x => x.MapFrom(m => m.Content));
                cfg.CreateMap<TodoModel, TodoDTO>()
                    .ForMember(x => x.Content, x => x.MapFrom(m => m.ContentTodo));

                cfg.CreateMap<PriorityTypeDTO, PriorityModel>()
                    .ForMember(x => x.PriorityContent, x => x.MapFrom(m => m.NamePriority))
                    .ForMember(x => x.PriorityId, x => x.MapFrom(m => m.PriorityTypeId));
                cfg.CreateMap<PriorityModel, PriorityTypeDTO>()
                    .ForMember(x => x.NamePriority, x => x.MapFrom(m => m.PriorityContent))
                    .ForMember(x => x.PriorityTypeId, x => x.MapFrom(m => m.PriorityId));

                cfg.CreateMap<UserDTO, UserModel>().ForMember(x => x.Role, x => x.MapFrom(m => m.Role));
                cfg.CreateMap<UserModel, UserDTO>();
            }).CreateMapper();
        }
    }
}
