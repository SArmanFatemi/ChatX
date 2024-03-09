<template>
  <q-page class="column items-center">
    <div class="col-4">
      <template v-if="!authenticatedUserStore.isAuthenticated">
        <q-input v-model="username" label="Username" />
        <q-input v-model="room" label="Room" :readonly="connection != null" />
        <q-btn class="q-my-md" label="Join Room" @click="JoinRoom(username, room)" />
      </template>

      <template v-if="authenticatedUserStore.isAuthenticated">
        <MessageBox v-for="(item, idx) in messages" :message="item" :key="idx"
          :sent="item.sender !== authenticatedUserStore.current.username" />
        <q-input v-model="message" label="Message" />
        <q-btn class="q-my-md" label="Send" icon="send" @click="SendMessage(message)" />
      </template>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import MessageBox from 'src/components/MessageBox.vue';
import { Message } from 'src/components/Messages.model';
import { useAuthenticatedUserStore } from 'src/stores/authenticated-user-store';
import { useQuasar } from 'quasar'
import { JoinRoomResponse } from 'src/hubs/responses/JoinRoom.responses';
import { ResponseUtilities } from 'src/hubs/Common.responses';
import { SendMessageResponse } from 'src/hubs/responses/SendMessage.responses';

const $q = useQuasar()
const authenticatedUserStore = useAuthenticatedUserStore();

const connection = ref<HubConnection | null>(null);
const username = ref('Arman');
const room = ref('backenders-room');
const message = ref('');

const messages = ref<Message[]>([]);

async function JoinRoom(username: string, room: string) {
  try {
    connection.value = new HubConnectionBuilder()
      .withUrl('https://localhost:1501/chat')
      .configureLogging(LogLevel.Information)
      .build();

    connection.value.on(JoinRoom.name, (response: JoinRoomResponse) => {
      $q.notify({
        message: response.message,
        caption: 'Just now',
        icon: 'announcement',
        color: ResponseUtilities.GetColor(response),
        position: 'bottom'
      })

      if (ResponseUtilities.IsSuccessful(response)) {
        authenticatedUserStore.login(username, room);
      }
    });

    connection.value.on('SendMessage', (response: SendMessageResponse) => {
      messages.value.push({ sender: response.username, content: response.message });
      message.value = '';
    });

    await connection.value.start();

    connection.value.onreconnected(() => {
      authenticatedUserStore.login(username, room);
      console.log('Connection re-established');
    });

    connection.value.onreconnecting(() => {
      authenticatedUserStore.logout();
      console.log('Connection lost, reconnecting...');
    });

    connection.value.onclose(() => {
      authenticatedUserStore.logout();

      $q.notify({
        message: 'You have been disconnected from the server. Please try again later.',
        caption: 'Connection closed',
        icon: 'announcement',
        color: 'negative',
        position: 'bottom'
      })
    });

    await connection.value.invoke(JoinRoom.name, { username, room });
  } catch (error) {
    console.log(error);
  }
}

async function SendMessage(message: string) {
  if (connection.value === null) return;

  try {
    await connection.value.invoke('SendMessage', { message });
  } catch (error) {
    console.log(error);
  }
}

async function Disconnect() {
  if (connection.value === null) return;

  try {
    await connection.value.stop();
    authenticatedUserStore.logout();
  } catch (error) {
    console.log(error);
  }
}
</script>
