using System.ComponentModel;
using System.Text.Json.Serialization;
using TodoList.Domain.Enum;

namespace TodoList.Domain.DTO
{
    public class UpdateTodoItemResponseDto
    {
        public long Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [DefaultValue(ToDoItemStatus.Pending)]
        public ToDoItemStatus Status { get; set; }
    }
}
