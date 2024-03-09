<template>
  <q-page class="column items-center">
    <div class="col-4">
      <template v-if="!authenticatedUserStore.isAuthenticated">
        <q-input v-model="username" label="Username" />
        <q-input v-model="chatroom" label="Chatroom" :readonly="connection != null" />
        <q-btn class="q-my-md" label="Join Chatroom" @click="JoinSpecificChatRoom(username, chatroom)" />
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

const $q = useQuasar()
const authenticatedUserStore = useAuthenticatedUserStore();

const connection = ref<HubConnection | null>(null);
const username = ref('Arman');
const chatroom = ref('backenders-room');
const message = ref('');

const messages = ref<Message[]>([]);

async function JoinSpecificChatRoom(username: string, chatroom: string) {
  try {
    connection.value = new HubConnectionBuilder()
      .withUrl('https://localhost:1501/chat')
      .configureLogging(LogLevel.Information)
      .build();

    connection.value.on('JoinSpecificChatRoom', (serverUsername, serverMessage) => {
      if (authenticatedUserStore.isAuthenticated && serverUsername !== authenticatedUserStore.current.username) {
        $q.notify({
          message: serverMessage,
          caption: 'Just now',
          icon: 'announcement',
          color: 'primary',
          position: 'bottom-right'
        })
      }
    });

    connection.value.on('SendMessage', (serverUsername, serverMessage) => {
      messages.value.push({ sender: serverUsername, content: serverMessage });
      message.value = '';
    });

    await connection.value.start()
      .then(() => {
        authenticatedUserStore.login(username, chatroom);

        $q.notify({
          message: `Welcome to the ${chatroom}, ${username}!`,
          icon: 'check',
          color: 'secondary',
          position: 'bottom'
        })
      })
    connection.value.onreconnected(() => {
      authenticatedUserStore.login(username, chatroom);
      console.log('Connection re-established');
    });
    connection.value.onreconnecting(() => {
      authenticatedUserStore.logout();
      console.log('Connection lost, reconnecting...');
    });
    connection.value.onclose(() => {
      authenticatedUserStore.logout();
      console.log('Connection closed');
    });

    await connection.value.invoke('JoinSpecificChatRoom', { username, chatroom });
  } catch (error) {
    console.log(error);
  }
}

async function SendMessage(message: string) {
  if (connection.value === null) return;

  try {
    await connection.value.invoke('SendMessage', message);
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
