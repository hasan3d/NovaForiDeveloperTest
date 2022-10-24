<template>
  <table class="table table-bordered mt-5">
      <thead>
        <tr>
          <th scope="col">Task</th>
          <th scope="col" style="width: 120px">Status</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="todo in todos" :key="todo.id">
          <td>
            <span :class="{ 'line-through': todo.status.toLowerCase() === 'completed' }">
              {{ todo.description }}
            </span>
          </td>
          <td>
            <span
              class="pointer noselect"
              @click="updateTodo(todo)"
              :class="{
                'text-success': todo.status.toLowerCase() === 'completed',
                'text-warning': todo.status.toLowerCase() === 'pending',
              }"
            >
              {{ capitalizeFirstChar(todo.status) }}
            </span>
          </td>
        </tr>
      </tbody>
    </table>
</template>
<script lang="ts">
import { defineComponent,PropType } from 'vue'
import { ITodo } from '../api/todoList/interface'

export default defineComponent({

  props: {
    todos: {
            required: true,
            type: Array as PropType<ITodo[]>
        }
    },

    setup(props, context) {
      const updateTodo = async (todo: ITodo) =>  {
        if (todo.status.toLowerCase() == "completed") {
            todo.status = "pending"
        } else {
            todo.status = "completed"
        }
      
         // Send up to parent
         context.emit('update-todo', todo);
    }

      return { updateTodo };
    },
    methods: {
      
    capitalizeFirstChar(status: string) {
      return status.charAt(0).toUpperCase() + status.slice(1);
    }
    }
})
</script>
<style scoped>
.pointer {
  cursor: pointer;
}

.noselect {
  -webkit-touch-callout: none;
  /* iOS Safari */
  -webkit-user-select: none;
  /* Safari */
  -khtml-user-select: none;
  /* Konqueror HTML */
  -moz-user-select: none;
  /* Old versions of Firefox */
  -ms-user-select: none;
  /* Internet Explorer/Edge */
  user-select: none;
  /* Non-prefixed version, currently supported by Chrome, Edge, Opera and Firefox */
}

.line-through {
  text-decoration: line-through;
}
</style>