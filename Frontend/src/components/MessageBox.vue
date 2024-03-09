<template>
    <q-chat-message v-if="props.message" :name="props.message.sender" :text="[props.message.content]" />
</template>

<script setup lang="ts">
import { Message } from './Messages.model';

const props = defineProps<{
    message: Message;
}>();
</script>