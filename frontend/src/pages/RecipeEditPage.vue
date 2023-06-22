<template>
  <div class="row q-pa-md">
    <q-img :src="imgSrc"
      ><div class="absolute-bottom flex justify-between">
        <q-input class="col" outlined v-model="recipe.title" label="Titel" />
      </div>
    </q-img>
  </div>
  <div class="q-pa-md row">
    <div class="coi-12 flex">
      <q-file v-model="file" label="Bild" filled style="max-width: 300px" />
      <q-btn icon="upload" @click="uploadImage"></q-btn>
    </div>
    <div class="col-12">
      <q-chip
        v-for="(tag, index) in tags"
        :key="tag"
        removable
        @remove="removeTag(index)"
        >{{ tag }}</q-chip
      >
    </div>
    <div class="col-12">
      <q-select
        label="Tags"
        outlined
        use-input
        input-debounce="0"
        @update:model-value="addTag"
        @new-value="addTag"
        :options="filterOptions"
      />
    </div>
  </div>
  <div class="q-pa-md row q-col-gutter-md">
    <div class="col-md-6 col-12">
      <PortionSelector class="q-my-sm" v-model="portions" />
      <q-card
        class="q-pa-md q-my-sm"
        v-for="(ingredient, i) in recipe.ingredients"
        :key="i"
      >
        <q-section class="row q-mb-sm">
          <div class="col-10">
            <q-input outlined v-model="ingredient.name" label="Zutat" />
          </div>

          <div class="col-2 q-pa-sm">
            <q-btn
              round
              color="secondary"
              @click="recipe.ingredients.splice(i, i + 1)"
              icon="delete"
            />
          </div>
        </q-section>
        <q-section>
          <div class="row small-gap">
            <q-input
              class="col"
              outlined
              v-model="ingredient.amount"
              label="Menge"
            />
            <q-select
              class="col"
              v-model="ingredient.unit"
              label="Einheit"
              outlined
              use-input
              input-debounce="0"
              :options="unitList"
              @filter="filterUnits"
            />
          </div>
        </q-section>
      </q-card>
      <q-card
        class="bg-secondary col-12 q-py-lg flex items-center flex-center text-h3"
        @click="addIngredient"
      >
        +
      </q-card>
    </div>
    <div class="col-md-6 col-12">
      <q-card
        v-for="(step, i) in recipe.steps"
        :key="i.toString()"
        class="q-my-sm"
      >
        <q-card-section class="row">
          <q-btn round @click="recipe.steps.splice(i, i + 1)" icon="delete" />
        </q-card-section>
        <q-card-section class="row">
          <q-input
            class="col-12"
            v-model="step.text"
            filled
            type="textarea"
            :label="i.toString() + '.'"
          />
        </q-card-section>
      </q-card>
      <q-card
        class="col-12 q-py-lg flex items-center flex-center text-h3"
        @click="addStep"
      >
        +
      </q-card>
    </div>
  </div>

  <q-page-sticky
    v-if="authStore.user"
    position="bottom-right"
    :offset="[18, 18]"
  >
    <q-fab color="primary" icon="keyboard_arrow_left" direction="left">
      <q-fab-action
        color="primary"
        text-color="black"
        @click="save"
        icon="save"
      />
      <q-fab-action
        color="primary"
        text-color="black"
        @click="deleteRecipe"
        v-if="id != 'new'"
        icon="delete"
      />
    </q-fab>
  </q-page-sticky>
</template>

<script setup>
import { computed, reactive, ref, onMounted } from "vue";
import { recipeService, imageService } from "src/services";
import { useRouter } from "vue-router";
import { useAuthStore } from "src/stores";

import RecipePage from "./RecipePage.vue";
import PortionSelector from "src/components/PortionSelector.vue";
const imgSrc = ref(null);
const authStore = useAuthStore();

const router = useRouter();
const props = defineProps({
  id: String,
});
const recipe = ref({
  title: "",
  tags: [],
  ingredients: [],
  steps: [],
});
const stringUnitList = ["g", "kg", "TL", "EL", "ml", "l", "Stück", "Prise", ""];
const unitList = ref(stringUnitList);
function filterUnits(val, update, abort) {
  update(() => {
    const needle = val.toLowerCase();
    unitList.value = stringUnitList.filter(
      (v) => v.toLowerCase().indexOf(needle) > -1
    );
  });
}
onMounted(async () => {
  if (props.id != "new") {
    recipe.value = await recipeService.getById(props.id);
    imgSrc.value = await imageService.getImage(props.id);
  }
  filterOptions.value = await recipeService.getTags();
});
const filterOptions = ref([]);
const file = ref(null);
const portions = ref(1);
const portionedIngredients = computed(() => {
  if (recipe.value.ingredients) {
    return recipe.value.ingredients.map((ingredient) => {
      return {
        name: ingredient.name,
        unit: ingredient.unit,
        amount: ingredient.amount / portions.value,
      };
    });
  }
  return null;
});
const tags = computed(() => {
  if (recipe.value.tags) {
    return recipe.value.tags.map((tag) => {
      return tag.value;
    });
  }
  return null;
});
function addStep() {
  if (!Array.isArray(recipe.value.steps)) {
    recipe.value.steps = [];
  }
  recipe.value.steps.push({ text: "" });
}
function addIngredient() {
  if (!Array.isArray(recipe.value.ingredients)) {
    recipe.value.ingredients = [];
  }
  recipe.value.ingredients.push({ name: "", amount: 0, unit: "" });
}
function addTag(val) {
  if (!recipe.value.tags) {
    recipe.value.tags = [];
  }
  if (!tags.value.includes(val)) {
    filterOptions.value.push(val);
  }
  recipe.value.tags.push({ value: val });
}
function removeTag(tag) {
  recipe.value.tags.splice(tag, tag + 1);
}
async function save() {
  let _recipe = recipe.value;
  _recipe.ingredients = portionedIngredients.value;
  recipe.value = await recipeService.createOrUpdate(_recipe);
}
async function deleteRecipe() {
  await recipeService.deleteRecipe(props.id);
  router.push("/");
}

async function uploadImage() {
  if (file.value) {
    await imageService.upload(props.id, file.value);
  }
}
</script>

<style></style>
