<template>
    <q-input v-model="message" label="Message" />
    <q-btn class="q-my-md" label="Send" icon="send" @click="SendMessage" />
</template>

<script setup lang="ts">
import { ref, defineEmits, defineExpose } from 'vue';

const message = ref('');
const emits = defineEmits(['SendMessage'])
defineExpose({
    Clear
})

async function SendMessage(): Promise<void> {
    emits('SendMessage', message.value)
}

function Clear() {
    message.value = '';
}
</script>
