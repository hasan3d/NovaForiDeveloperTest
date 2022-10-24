using TodoList.Domain.Enum;

namespace ToDoList.Domain.DTO
{
    public class UpdateTodoItemRequestDto
    {
        public ToDoItemStatus Status { get; set; }
    }
}
