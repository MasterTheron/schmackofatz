<template>
  <q-chip square color="primary" v-for="tag in tags" :key="tag.value">{{
    tag.value
  }}</q-chip>
</template>

<script setup>
const props = defineProps({
  tags: {
    type: Array,
    default: () => [],
  },
});
</script>
