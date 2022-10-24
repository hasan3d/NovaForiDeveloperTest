using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoList.Domain.DTO;
using ToDoList.Domain.DTO;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly ILogger<TodoListController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoListController(ILogger<TodoListController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllTodo")]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAllTodoItems()
        {
            var allTodoItems = await _unitOfWork.TodoItemRepository.GetAllAsync();

            var allTodoItemsData = _mapper.Map<IEnumerable<TodoItemDto>>(allTodoItems);

            if (!allTodoItemsData.Any()) return NotFound();

            return Ok(allTodoItemsData);
        }


        [HttpPost]
        [Route("AddTodo")]
        public async Task<ActionResult<TodoItemDto>> AddTodoItem([FromBody] AddTodoItemDto addTodoItemDto)
        {
            if (addTodoItemDto == null || string.IsNullOrEmpty(addTodoItemDto.Description)) return BadRequest();

            var todoItemData = _mapper.Map<TodoItem>(addTodoItemDto);

            _unitOfWork.TodoItemRepository.Add(todoItemData);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                _logger.LogError("An error has occurred. {e}", e);
                throw;
            }

            var addedTodoItemData = _mapper.Map<TodoItemDto>(todoItemData);

            return Ok(addedTodoItemData);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UpdateTodoItemResponseDto>> UpdateTodoItem(long id, [FromBody] UpdateTodoItemRequestDto updateTodoItemRequest)
        {
            if (updateTodoItemRequest == null) return BadRequest();

            var existingTodoItem = _unitOfWork.TodoItemRepository.GetById(id);

            if (existingTodoItem == null) return NotFound();

            existingTodoItem.Status = updateTodoItemRequest.Status;

            _unitOfWork.TodoItemRepository.Update(existingTodoItem);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                _logger.LogError("An error has occurred. {e}", e);
                throw;
            }

            var updatedTodoItem = _mapper.Map<UpdateTodoItemResponseDto>(existingTodoItem);

            return Ok(updatedTodoItem);
        }
    }
}