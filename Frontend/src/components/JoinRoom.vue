<template>
    <q-input v-model="username" label="Username" />
    <q-input v-model="room" label="Room" />
    <q-btn class="q-my-md" label="Join Room" @click="JoinRoom" />
</template>

<script setup lang="ts">
import { ref, defineEmits } from 'vue';

const username = ref('Arman');
const room = ref('backenders-room');

const emits = defineEmits(['JoinRoom'])

async function JoinRoom(): Promise<void> {
    emits('JoinRoom', username.value, room.value)
}
</script>