using System.ComponentModel.DataAnnotations;
using TodoList.Domain.Enum;

namespace TodoList.Domain.Entities
{
    public class TodoItem
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public ToDoItemStatus Status { get; set; }

    }
}