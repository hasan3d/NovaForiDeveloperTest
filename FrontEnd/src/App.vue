<template>
<LayoutHeader />
<div class="container" style="max-width: 600px">
  <div class="d-flex justify-content-center mt-5">
    <AddTodo v-on:add-todo="addTodo" />
  </div>
    <AllTodos v-on:update-todo="updateTodo" :todos="todos" />
</div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted  } from 'vue';
import LayoutHeader from './components/layout/LayoutHeader.vue'
import AddTodo from './components/AddTodo.vue'
import AllTodos from './components/AllTodos.vue'
import todoListApi from './api/todoList'
import { IAddTodo, ITodo } from './api/todoList/interface'
export default defineComponent({
  name: 'App',
  components: {
    LayoutHeader,
    AddTodo,
    AllTodos
  },
  setup() {
    const todos = ref<ITodo[]>([]);

    const addTodo = async (newTodo: IAddTodo) => {
      const response = await todoListApi.addTodo(newTodo)
      todos.value.push(response)
    }

    const updateTodo = async (todo: ITodo) => {
      const updatedTodoItemResponse = await todoListApi.updateTodoItem(todo.id, todo)
      const updatedTodoItemIndex = todos.value.findIndex(x => x.id == updatedTodoItemResponse.id)
      todos.value[updatedTodoItemIndex] = todo
    }


    const getAllTodos = async () => {
      const response = await todoListApi.getAllTodos()
      todos.value = response
    }

    onMounted(() => {
      getAllTodos();
    })

    return {
      todos,
      addTodo,
      updateTodo
    }
  },
  methods: {
  
  }
});
</script>
