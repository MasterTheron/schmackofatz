<template>
  <q-list>
    <q-item v-for="ingredient in ingredients" :key="ingredient.type"
      ><q-item-section class="text-right"
        >{{ ingredient.amount }} {{ ingredient.unit }}</q-item-section
      >
      <q-item-section>
        <q-input outlined :model-value="ingredient.name" />
      </q-item-section>
    </q-item>
  </q-list>
</template>

<script setup>
import { ref, defineProps } from "vue";
const props = defineProps({
  ingredients: Array,
});
</script>

<style></style>
