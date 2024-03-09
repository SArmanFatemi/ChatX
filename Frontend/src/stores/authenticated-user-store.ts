import { defineStore } from 'pinia';

export const useAuthenticatedUserStore = defineStore('authenticated-user-store', {
  state: () => ({
    username: '',
    chatroom: '',
  }),
  getters: {
    current: (state) => {
      return {
        username: state.username,
        chatroom: state.chatroom
      }
    },
    isAuthenticated: (state) => !!state.username,
  },
  actions: {
    login(username: string, chatroom: string) {
      this.username = username;
      this.chatroom = chatroom;
    },
    logout() {
      this.username = '';
      this.chatroom = '';
    },
  },
});
