using TodoList.DataAccess.EFCore.Repositories;
using TodoList.Domain.Interfaces;

namespace TodoList.DataAccess.EFCore.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoListContext _context;
        public UnitOfWork(TodoListContext context)
        {
            _context = context;
            TodoItemRepository = new TodoItemRepository(_context);
        }
        public ITodoItemRepository TodoItemRepository { get; private set; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}
