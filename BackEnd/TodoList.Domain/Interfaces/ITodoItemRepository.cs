using TodoList.Domain.Entities;

namespace TodoList.Domain.Interfaces
{
    public interface ITodoItemRepository : IGenericRepository<TodoItem>
    {
    }
}
