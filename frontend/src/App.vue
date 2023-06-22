<template>
  <router-view />
</template>

<script>
import { defineComponent } from "vue";
import { useQuasar } from "quasar";

export default defineComponent({
  name: "App",
  setup() {
    const $q = useQuasar();

    $q.dark.set(true);
  },
});
</script>
<style>
.q-card--dark {
  box-shadow: 0 1px 5px rgba(0, 0, 0, 0.2), 0 2px 2px rgba(0, 0, 0, 0.14),
    0 3px 1px -2px rgba(0, 0, 0, 0.12);
  border-radius: 14px;
}
</style>
