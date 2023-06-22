<template>
  <div class="q-pa-md row q-col-gutter-md">
    <div class="col-md-6 col-12">
      <q-img :src="imgSrc"
        ><div class="absolute-bottom flex justify-between">
          <div class="text-h5">{{ recipe.title }}</div>
          <div><TagChips :tags="recipe.tags" /></div>
        </div>
      </q-img>
      <div></div>
      <PortionSelector class="q-my-sm" v-model="portions" />
      <q-card class="col-12">
        <IngredientList :ingredients="portionedIngredients" />
      </q-card>
    </div>

    <div class="col-12 col-md-6 items-start">
      <RecipeStep
        v-for="(step, i) in recipe.steps"
        :key="i"
        :step="step"
        :index="i"
      />
    </div>
  </div>
  <q-page-sticky
    v-if="authStore.user"
    position="bottom-right"
    :offset="[18, 18]"
  >
    <q-btn fab icon="edit" color="primary" :to="id + '/edit'" />
  </q-page-sticky>
</template>

<script setup>
import IngredientList from "src/components/IngredientList.vue";
import { computed, reactive, ref, onMounted } from "vue";
import { recipeService, imageService } from "src/services";
import TagChips from "src/components/TagChips.vue";
import PortionSelector from "src/components/PortionSelector.vue";
import RecipeStep from "src/components/RecipeStep.vue";
import { useAuthStore } from "src/stores";
const authStore = useAuthStore();

const props = defineProps({
  id: String,
});
const recipe = ref({});
const imgSrc = ref(null);
onMounted(async () => {
  recipe.value = await recipeService.getById(props.id);
  imgSrc.value = await imageService.getImage(props.id);
});
const portions = ref(2);
const portionedIngredients = computed(() => {
  if (recipe.value.ingredients) {
    return recipe.value.ingredients.map((ingredient) => {
      return {
        name: ingredient.name,
        unit: ingredient.unit,
        amount: ingredient.amount * portions.value,
      };
    });
  }
  return null;
});
</script>

<style></style>
