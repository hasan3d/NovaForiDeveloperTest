namespace TodoList.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoItemRepository TodoItemRepository { get; }
        Task<int> Complete();
    }
}
