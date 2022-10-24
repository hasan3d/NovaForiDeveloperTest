<template>
<form @submit.prevent="addTodo">
      <input type="text" v-model="task" name="task" placeholder="Add Todo...">
      <input type="submit" value="Submit" :disabled="disabled" class="btn btn-success">
</form>    
</template>

<script lang="ts">
import { defineComponent, ref, watch } from 'vue'
import { IAddTodo } from '../api/todoList/interface'

export default defineComponent({
    setup(props, context) {
        const task = ref('')
        const disabled = ref<boolean>(true)
        const newTodo = ref<IAddTodo>()
        const addTodo = () => {
            if (task.value.trim() !== '' && !disabled.value) {
                disabled.value = true
                newTodo.value = {
                description: task.value,
                status: 'pending'
            }
            // Send up to parent
             context.emit('add-todo', newTodo.value);
             task.value = '';
            }
        }

        watch(task, () => {
          if(task.value.trim() !== '') {
            disabled.value = false
          } else {
            disabled.value = true
          }
        })
        return {
            task,
            addTodo,
            disabled
        }
    },
})
</script>
<style scoped>
form {
    display: flex;
  }
  input[type="text"] {
    flex: 10;
    padding: 5px;
    width:25rem;
  }
  input[type="submit"] {
    flex: 2;
  }
</style>
