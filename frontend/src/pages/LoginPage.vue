<script setup>
import { useAuthStore } from "../stores";
import { ref } from "vue";
const username = ref("");
const password = ref("");
async function onSubmit() {
  const authStore = useAuthStore();
  try {
    return await authStore.login(username.value, password.value);
  } catch (error) {
    console.log(error);
  }
}
</script>

<template>
  <div class="login-page q-pa-md">
    <div class="row">
      <div class="col-md-8 offset-md-2">
        <h2>Login</h2>
      </div>
    </div>

    <form @submit.prevent="onSubmit" class="row q-gutter-md">
      <q-input
        class="col-12 col-md-8 offset-md-2"
        v-model="username"
        outlined
        label="Username"
      >
        <template v-slot:append>
          <q-icon name="person" />
        </template>
      </q-input>
      <q-input
        class="col-12 col-md-8 offset-md-2"
        v-model="password"
        type="password"
        outlined
        label="Password"
      >
        <template v-slot:append>
          <q-icon name="password" />
        </template>
      </q-input>
      <div class="col-12 col-md-8 offset-md-2">
        <q-btn type="submit">Login</q-btn>
      </div>
    </form>
  </div>
</template>
