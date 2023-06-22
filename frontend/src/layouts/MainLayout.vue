<template>
  <q-layout view="lHh Lpr lFf">
    <q-header>
      <q-toolbar class="toolbar q-py-sm">
        <!-- <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        /> -->
        <q-toolbar-title>Schmackofatz</q-toolbar-title>
        <q-btn flat icon="home" to="/" />
        <q-btn
          v-if="authStore.user"
          flat
          icon="logout"
          @click="authStore.logout"
        />
      </q-toolbar>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent, ref } from "vue";
import { useAuthStore } from "src/stores";

export default defineComponent({
  name: "MainLayout",
  setup() {
    const leftDrawerOpen = ref(false);
    const authStore = useAuthStore();
    return {
      authStore,
      leftDrawerOpen,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value;
      },
    };
  },
});
</script>
<style lang="scss" scoped>
.q-header {
  display: flex;
  justify-content: space-between;
}
</style>
