<template>
  <q-page class="column items-center">
    <div class="col-4">
      <JoinRoomComponent @JoinRoom="JoinRoom" v-if="!authenticatedUserStore.isAuthenticated" />

      <template v-if="authenticatedUserStore.isAuthenticated">
        <MessageBox v-for="(item, idx) in messages" :message="item" :key="idx"
          :sent="item.sender !== authenticatedUserStore.current.username" />
        <SendMessageComponent ref="sendMessageComponent" @SendMessage="SendMessage" />
      </template>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import MessageBox from 'src/components/MessageBox.vue';
import JoinRoomComponent from 'src/components/JoinRoom.vue';
import SendMessageComponent from 'src/components/SendMessage.vue';
import { Message } from 'src/components/Messages.model';
import { useAuthenticatedUserStore } from 'src/stores/authenticated-user-store';
import { useQuasar } from 'quasar'
import { JoinRoomResponse } from 'src/hubs/responses/JoinRoom.responses';
import { ResponseUtilities } from 'src/hubs/Common.responses';
import { SendMessageResponse } from 'src/hubs/responses/SendMessage.responses';

const $q = useQuasar()
const authenticatedUserStore = useAuthenticatedUserStore();

const connection = ref<HubConnection>(new HubConnectionBuilder()
  .withUrl('https://localhost:1501/chat')
  .configureLogging(LogLevel.Information)
  .build());

const messages = ref<Message[]>([]);
const sendMessageComponent = ref();

async function JoinRoom(username: string, room: string) {
  await connection.value.invoke(JoinRoom.name, { username, room });
}

async function SendMessage(message: string) {
  await connection.value.invoke('SendMessage', { message });
}

async function Start() {
  try {
    connection.value.on(JoinRoom.name, (response: JoinRoomResponse) => {
      $q.notify({
        message: response.message,
        caption: 'Just now',
        icon: 'announcement',
        color: ResponseUtilities.GetColor(response),
        position: 'bottom'
      })

      if (ResponseUtilities.IsSuccessful(response)) {
        authenticatedUserStore.login(response.username, response.room);
      }
    });

    connection.value.on(SendMessage.name, (response: SendMessageResponse) => {
      messages.value.push({ sender: response.username, content: response.message });
      sendMessageComponent.value.Clear();
    });

    await connection.value.start();

    connection.value.onreconnected(() => {
      console.log('Connection re-established');
    });

    connection.value.onreconnecting(() => {
      console.log('Connection lost, reconnecting...');
    });

    connection.value.onclose(() => {
      $q.notify({
        message: 'You have been disconnected from the server. Please try again later.',
        caption: 'Connection closed',
        icon: 'announcement',
        color: 'negative',
        position: 'bottom'
      })
    });

  } catch (error) {
    console.log(error);
  }
}

await Start();

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
