using System.ComponentModel;
using TodoList.Domain.Enum;

namespace TodoList.Domain.DTO
{
    public class AddTodoItemDto
    {
        public string Description { get; set; }

        [DefaultValue(ToDoItemStatus.Pending)]
        public ToDoItemStatus Status { get; set; }
    }
}
