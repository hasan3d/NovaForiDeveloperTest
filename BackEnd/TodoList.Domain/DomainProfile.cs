using AutoMapper;
using TodoList.Domain.DTO;
using TodoList.Domain.Entities;

namespace TodoList.Domain
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<TodoItem, TodoItemDto>()
                .ReverseMap();

            CreateMap<TodoItem, AddTodoItemDto>()
                .ReverseMap();

            CreateMap<TodoItem, UpdateTodoItemResponseDto>()
                .ReverseMap();
        }
    }
}
