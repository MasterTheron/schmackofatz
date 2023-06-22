<template>
  <div class="row q-pa-md row">
    <div class="col-12">
      <q-input v-model="searchTerm" outlined label="Search...">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
    </div>
  </div>
  <div class="q-pa-md row items-start q-col-gutter-sm">
    <RouterLink
      v-for="recipe in filteredRecipes"
      :key="recipe.id"
      :to="'/recipe/' + recipe.id"
      style="text-decoration: none"
      class="col-12 col-md-6"
    >
      <q-card class="q-pa-md">
        <q-img :src="getImage(recipe.id)">
          <div class="absolute-bottom">
            <q-card-section class="text-center text-subtitle2">
              {{ recipe.title }}
            </q-card-section>
            <q-section>
              <!-- <TagChips :tags="recipe.tags" /> -->
            </q-section>
          </div>
        </q-img>
      </q-card>
    </RouterLink>
  </div>
  <q-page-sticky
    v-if="authStore.user"
    position="bottom-right"
    :offset="[18, 18]"
  >
    <q-btn fab icon="add" color="primary" to="/recipe/new/edit" />
  </q-page-sticky>
</template>

<script setup>
import { computed, reactive, ref, onMounted } from "vue";
import { recipeService, imageService } from "src/services";
import { useAuthStore } from "src/stores";
const recipes = ref([]);
const images = ref(null);
const searchTerm = ref("");
const authStore = useAuthStore();
onMounted(async () => {
  recipes.value = await recipeService.getAll();
  images.value = await imageService.getAll();
  const hasChanged = await recipeService.sync();
  if (hasChanged) {
    recipes.value = await recipeService.getAll();
    images.value = await imageService.getAll();
  }
});

function getImage(recipeId) {
  if (images.value && recipeId in images.value) {
    return images.value[recipeId];
  }
}

const filteredRecipes = computed(() => {
  if (recipes.value) {
    if (searchTerm.value != "") {
      return recipes.value.filter(isValid);
    }
    return recipes.value;
  }
  return null;
});

function isValid(recipe) {
  const lowerCaseSearch = searchTerm.value.toLowerCase();
  return (
    titleIncludes(recipe, lowerCaseSearch) ||
    hasTag(recipe, lowerCaseSearch) ||
    hasIngredient(recipe, lowerCaseSearch)
  );
}

function hasTag(recipe, searchTerm) {
  return recipe.tags?.some((tag) =>
    tag.value.toLowerCase().includes(searchTerm)
  );
}

function hasIngredient(recipe, searchTerm) {
  return recipe.ingredients.some((ingredient) =>
    ingredient.name.toLowerCase().includes(searchTerm)
  );
}

function titleIncludes(recipe, searchTerm) {
  return recipe.title.toLowerCase().includes(searchTerm);
}
</script>

<style>
.q-img__content > div {
  background: rgba(0, 0, 0, 0.87);
}
</style>
