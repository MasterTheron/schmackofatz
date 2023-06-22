<template>
  <q-card class="text-center q-pa-md gap flex items-center flex-center">
    <q-btn
      round
      color="primary"
      @click="emit('update:model-value', modelValue - 1)"
      size="lg"
      icon="remove"
    />
    <span>{{ modelValue }}</span>
    <q-btn
      round
      color="primary"
      @click="emit('update:model-value', modelValue + 1)"
      size="lg"
      icon="add"
    />
  </q-card>
</template>
<script setup>
const props = defineProps({
  modelValue: Number,
});
const emit = defineEmits("update:model-value");
</script>
