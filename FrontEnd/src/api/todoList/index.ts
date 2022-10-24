import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';

import {
    ITodo,
    IAddTodo,
    IUpdateTodoItem,
    IUpdateTodoItemResponse
  } from '@/api/todoList/interface';

class TodoListClient {
    private readonly client: AxiosInstance;

    constructor() {
      const baseURL = process.env.VUE_APP_TODOLIST_API_URL;
      this.client = axios.create({
        baseURL
      });    
    }

    public getAllTodos = () =>
    this.get<ITodo[]>({
        url: '/todoList/getAllTodo',
    });

    public addTodo = (addTodoData: IAddTodo) =>
    this.post<ITodo>({
        url: '/todoList/addTodo',
        data: addTodoData
    });

    public updateTodoItem = (id: number, updateTodoItem: IUpdateTodoItem) =>
    this.patch<IUpdateTodoItemResponse>({
        url: '/todoList/' + id,
        data: updateTodoItem
    });

    protected get<T>(config: AxiosRequestConfig) {
        return this.makeRequest<T>({
          ...config,
          method: 'get',
        });
      }
    protected post<T>(config: AxiosRequestConfig) {
        return this.makeRequest<T>({
          ...config,
          method: 'post',
        });
      }

    protected patch<T>(config: AxiosRequestConfig) {
        return this.makeRequest<T>({
          ...config,
          method: 'patch',
        });
      }
    private makeRequest = <T>(config: AxiosRequestConfig) =>
        this.client.request<T>(config).then((x) => x.data);
}

export default new TodoListClient();