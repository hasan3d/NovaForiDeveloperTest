import TodoStatus from '../../types/TodoStatus'

export interface ITodo extends IAddTodo {
    id: number
}

export interface IAddTodo {
    description: string,
    status: TodoStatus
}

export interface IUpdateTodoItem {
    status: TodoStatus
}

export interface IUpdateTodoItemResponse {
    id: number,
    status: TodoStatus
}