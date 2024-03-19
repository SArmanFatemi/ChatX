<template>
    <q-input v-model="message" label="Message" />
    <q-btn class="q-my-md" label="Send" icon="send" @click="SendMessage" />
</template>

<script setup lang="ts">
import { ref, defineEmits, defineExpose } from 'vue';

const message = ref('');
const emits = defineEmits<{
    SendMessage: [message: string]
}>();

function SendMessage(): void {
    emits('SendMessage', message.value)
}

defineExpose({
    Clear
})
function Clear() {
    message.value = '';
}
</script>
