import { openDB } from "idb";

export const idbService = {
  openDatatabase,
};
const dbName = "RecipeDatabase";

async function openDatatabase() {
  const db = await openDB(dbName, 2, {
    upgrade(updb) {
      updb.createObjectStore("Recipes", { keyPath: "id" });
      updb.createObjectStore("Images", { keyPath: "id" });
    },
  });
  return db;
}
