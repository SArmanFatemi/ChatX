import { defineStore } from 'pinia';

export const useAuthenticatedUserStore = defineStore('authenticated-user-store', {
  state: () => ({
    username: '',
    room: '',
  }),
  getters: {
    current: (state) => {
      return {
        username: state.username,
        room: state.room
      }
    },
    isAuthenticated: (state) => !!state.username,
  },
  actions: {
    login(username: string, room: string) {
      this.username = username;
      this.room = room;
    },
    logout() {
      this.username = '';
      this.room = '';
    },
  },
});
