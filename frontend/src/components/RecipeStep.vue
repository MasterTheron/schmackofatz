<template>
  <q-card class="q-my-sm">
    <q-card-section v-if="step.ingredients">
      <span v-for="ingredient in step.ingredients" :key="ingredient"
        >{{ ingredient.amount }} {{ ingredient.unit }} {{ ingredient.name }} -
      </span>
    </q-card-section>
    <q-card-section>
      <span class="text-bold text-h5 q-ma-md">{{ index + 1 }}.</span
      >{{ step.text }}
    </q-card-section>
  </q-card>
</template>

<script setup>
const props = defineProps({
  step: Object,
  index: Number,
});
</script>
