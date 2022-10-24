using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using TodoList.Domain;
using TodoList.Domain.DTO;
using TodoList.Domain.Entities;
using TodoList.Domain.Enum;
using TodoList.Domain.Interfaces;
using TodoList.WebApi.Controllers;
using ToDoList.Domain.DTO;

namespace TodoList.Tests
{
    public class TodoListControllerTests
    {
        private Mock<ILogger<TodoListController>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private IMapper _mapper;
        private MapperConfiguration _mockMapper;
        private Mock<ITodoItemRepository> _todoItemRepository;
        private TodoListController _sut;


        [SetUp]
        public void Setup()
        {
            _mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });

            _logger = new Mock<ILogger<TodoListController>>();

            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = _mockMapper.CreateMapper();
            _todoItemRepository = new Mock<ITodoItemRepository>();
            _sut = new TodoListController(_logger.Object, _unitOfWork.Object, _mapper);
        }

        [Test]
        public void TodoListController_Constructor_Should_Throw_ArgumentNullException_When_UnitOfWork_Is_Null()
        {
            // Act 

            var ex = Assert.Throws<ArgumentNullException>(() => new TodoListController(_logger.Object, null, _mapper));

            // Assert

            ex?.Message.Should()
                .BeEquivalentTo(
                    "Value cannot be null. (Parameter 'unitOfWork')");
        }


        [Test]
        public async Task TodoListController_GetAllTodo_Should_Return_All_TodoItems()
        {
            // Arrange

            var tesTodoItems = GetTestTodoItemsList().ToList();

            _todoItemRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(tesTodoItems);

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.GetAllTodoItems();


            // Assert

            var result = response.Result as OkObjectResult;

            result?.StatusCode.Should().Be(200);

            result?.Value.Should().BeAssignableTo<IEnumerable<TodoItemDto>>();

            var resultData = result?.Value as IEnumerable<TodoItemDto>;

            resultData?.Count().Should().Be(tesTodoItems.Count);

            _todoItemRepository.Verify(x => x.GetAllAsync(), Times.Once());

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Once());

        }

        [Test]
        public async Task TodoListController_GetAllTodo_Return_NotFoundResult_When_No_TodoItems_Found()
        {
            // Arrange

            _todoItemRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<TodoItem>());

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.GetAllTodoItems();

            // Assert

            var result = response.Result as NotFoundResult;

            result?.StatusCode.Should().Be(404);

            _todoItemRepository.Verify(x => x.GetAllAsync(), Times.Once());

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Once());
        }

        [Test]
        public async Task TodoListController_AddTodo_Should_AddTodoItem_Given_Input_AddTodoItemDto()
        {
            // Arrange

            var addTodoItemDto = new AddTodoItemDto()
            {
                Description = "Demo Task 1",
                Status = ToDoItemStatus.Pending
            };

            _todoItemRepository.Setup(x => x.Add(It.IsAny<TodoItem>())).Verifiable();

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.AddTodoItem(addTodoItemDto);


            // Assert

            var result = response.Result as OkObjectResult;

            result?.StatusCode.Should().Be(200);

            result?.Value.Should().BeAssignableTo<TodoItemDto>();

            var resultData = result?.Value as TodoItemDto;

            resultData?.Status.Should().Be(addTodoItemDto.Status);

            resultData?.Description.Should().Be(addTodoItemDto.Description);

            _todoItemRepository.Verify(x => x.Add(It.IsAny<TodoItem>()), Times.Once());

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Once());
        }

        [Test]
        public async Task TodoListController_AddTodo_Return_BadRequest_When_Input_TodoItemDto_Object_Is_Null()
        {
            // Arrange

            _todoItemRepository.Setup(x => x.Add(It.IsAny<TodoItem>())).Verifiable();

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.AddTodoItem(null);


            // Assert

            var result = response.Result as BadRequestResult;

            result?.StatusCode.Should().Be(400);

            _todoItemRepository.Verify(x => x.Add(It.IsAny<TodoItem>()), Times.Never);

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Never);
        }

        [Test]
        public async Task TodoListController_AddTodo_Return_BadRequest_When_Input_TodoItem_Description_Is_NullOrEmpty()
        {
            // Arrange

            _todoItemRepository.Setup(x => x.Add(It.IsAny<TodoItem>())).Verifiable();

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.AddTodoItem(new AddTodoItemDto()
            {
                Status = ToDoItemStatus.Pending
            });


            // Assert

            var result = response.Result as BadRequestResult;

            result?.StatusCode.Should().Be(400);

            _todoItemRepository.Verify(x => x.Add(It.IsAny<TodoItem>()), Times.Never);

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Never);
        }

        [Test]
        public async Task TodoListController_UpdateTodoItem_Should_UpdateTodoItem_With_Given_Input_UpdateTodoItemRequestDto()
        {
            // Arrange

            var resourceId = 1;

            var updateTodoItemRequest = new UpdateTodoItemRequestDto()
            {
                Status = ToDoItemStatus.Completed
            };

            var existingTodoItem = new TodoItem()
            {
                Id = 1,
                Description = "Demo Task 1",
                Status = ToDoItemStatus.Pending
            };

            _todoItemRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(existingTodoItem);

            _todoItemRepository.Setup(x => x.Update(It.IsAny<TodoItem>())).Verifiable();

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.UpdateTodoItem(resourceId, updateTodoItemRequest);


            // Assert

            var result = response.Result as OkObjectResult;

            result?.StatusCode.Should().Be(200);

            result?.Value.Should().BeAssignableTo<UpdateTodoItemResponseDto>();

            var resultData = result?.Value as UpdateTodoItemResponseDto;

            resultData?.Status.Should().Be(updateTodoItemRequest.Status);

            resultData?.Id.Should().Be(resourceId);

            _todoItemRepository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());

            _todoItemRepository.Verify(x => x.Update(It.IsAny<TodoItem>()), Times.Once);

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Exactly(2));
        }


        [Test]
        public async Task TodoListController_UpdateTodoItem_Return_BadRequest_When_Input_UpdateTodoItemRequestDto_Is_Null()
        {
            // Arrange

            var resourceId = 1;


            var existingTodoItem = new TodoItem()
            {
                Id = 1,
                Description = "Demo Task 1",
                Status = ToDoItemStatus.Pending
            };

            _todoItemRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(existingTodoItem);

            _todoItemRepository.Setup(x => x.Update(It.IsAny<TodoItem>())).Verifiable();

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.UpdateTodoItem(resourceId, null);


            // Assert

            var result = response.Result as BadRequestResult;

            result?.StatusCode.Should().Be(400);

            _todoItemRepository.Verify(x => x.GetById(It.IsAny<long>()), Times.Never);

            _todoItemRepository.Verify(x => x.Update(It.IsAny<TodoItem>()), Times.Never);

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Exactly(0));
        }

        [Test]
        public async Task TodoListController_UpdateTodoItem_Return_NotFound_When_TodoItem_Is_NotFound_With_Given_Id()
        {
            // Arrange

            var resourceId = 0;

            var updateTodoItemRequest = new UpdateTodoItemRequestDto()
            {
                Status = ToDoItemStatus.Completed
            };

            _todoItemRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(() => null);

            _todoItemRepository.Setup(x => x.Update(It.IsAny<TodoItem>())).Verifiable();

            _unitOfWork.Setup(x => x.TodoItemRepository).Returns(_todoItemRepository.Object);

            // Act
            var response = await _sut.UpdateTodoItem(resourceId, updateTodoItemRequest);


            // Assert

            var result = response.Result as NotFoundResult;

            result?.StatusCode.Should().Be(404);

            _todoItemRepository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);

            _todoItemRepository.Verify(x => x.Update(It.IsAny<TodoItem>()), Times.Never);

            _unitOfWork.Verify(x => x.TodoItemRepository, Times.Exactly(1));
        }

        private IEnumerable<TodoItem> GetTestTodoItemsList()
        {
            var tesTodoItems = new List<TodoItem>()
            {
                new TodoItem()
                {
                    Id = 1,
                    Description = "Demo task 1",
                    Status = ToDoItemStatus.Pending
                },
                new TodoItem()
                {
                    Id = 2,
                    Description = "Demo task 2",
                    Status = ToDoItemStatus.Pending
                },
                new TodoItem()
                {
                    Id = 3,
                    Description = "Demo task 3",
                    Status = ToDoItemStatus.Completed
                }


            };

            return tesTodoItems;
        }
    }
}