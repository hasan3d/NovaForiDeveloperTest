using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.DataAccess.EFCore.Repositories
{
    public class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(TodoListContext context) : base(context)
        {
        }
    }

}
