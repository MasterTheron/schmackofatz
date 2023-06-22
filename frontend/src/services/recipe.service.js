import { imageService, idbService } from "./";
import { fetchWrapper } from "src/helpers";
export const recipeService = {
  getAll,
  getById,
  createOrUpdate,
  deleteRecipe,
  sync,
  getTags,
};

const openDatatabse = idbService.openDatatabase;
const storeName = "Recipes";
const route = import.meta.env.VITE_API;

async function sync() {
  const lastSync =
    localStorage.getItem("lastSync") ?? new Date(0).toISOString();
  const res = await fetch(`${route}Recipe/sync/${lastSync}`);
  const recipes = await res.json();
  if (recipes.length > 0) {
    console.log("sync");
    const db = await openDatatabse();
    const tx = db.transaction(storeName, "readwrite");
    const store = tx.objectStore(storeName);
    await Promise.all(
      recipes.map(async (recipe) => {
        await store.put(recipe);
      })
    );
    tx.commit();
    db.close();
    await Promise.all(
      recipes.map(async (recipe) => {
        await imageService.downloadImage(recipe.id);
      })
    );
    localStorage.setItem("lastSync", new Date().toISOString());
    return true;
  }
  localStorage.setItem("lastSync", new Date().toISOString());
  return false;
}
async function getAll() {
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readonly");
  const store = tx.objectStore(storeName);
  const recipes = store.getAll();
  db.close();
  return recipes;
}

async function getById(id) {
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readonly");
  const store = tx.objectStore(storeName);
  const recipe = store.get(id);
  db.close();
  return recipe;
}

async function createOrUpdate(recipe) {
  const res = await fetchWrapper.post(`${route}Recipe`, recipe);
  const recipeFromDb = await res.json();
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readwrite");
  const store = tx.objectStore(storeName);
  store.put(recipeFromDb);
  tx.commit();
  db.close();
  return recipeFromDb;
}

async function deleteRecipe(id) {
  const res = await fetch(`${route}Recipe/${id}`, {
    method: "DELETE",
    headers: { Accept: "application/json", "Content-Type": "application/json" },
  });
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readwrite");
  const store = tx.objectStore(storeName);
  store.delete(id);
  db.close();
  return await res.json();
}

async function getTags() {
  const recipes = await getAll();
  const tags = recipes
    .map((recipe) => {
      return recipe.tags?.map((tag) => tag.value);
    })
    .flat();
  return [...new Set(tags)];
}
