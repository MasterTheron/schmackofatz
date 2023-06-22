export const imageService = {
  upload,
  getImage,
  getAll,
  downloadImage,
};

import { idbService } from "./";
import { fetchWrapper } from "src/helpers";
const openDatatabse = idbService.openDatatabase;

const route = import.meta.env.VITE_API;
const imageUrl = import.meta.env.VITE_IMAGE_URL;

const storeName = "Images";

async function upload(recipeId, image) {
  const data = new FormData();
  data.append("files", image);
  // const res = await fetch(`${route}Recipe/${recipeId}/image`, {
  //   method: "POST",
  //   body: data,
  // });
  const res = await fetchWrapper.postForm(
    `${route}Recipe/${recipeId}/image`,
    data
  );
}

async function downloadImage(recipeId) {
  console.log(recipeId);
  const res = await fetch(`${imageUrl}${recipeId}`);
  const blob = await res.blob();
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readwrite");
  const store = tx.objectStore(storeName);
  store.put({ id: recipeId, image: blob });
  tx.commit();
  db.close();
  return;
}
async function getImage(recipeId) {
  console.log(recipeId);
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readonly");
  const store = tx.objectStore(storeName);
  const image = await store.get(recipeId);
  db.close();
  return URL.createObjectURL(image.image);
}

async function getAll() {
  const db = await openDatatabse();
  const tx = db.transaction(storeName, "readonly");
  const store = tx.objectStore(storeName);
  const imageList = await store.getAll();
  db.close();
  var imgs = {};
  imageList.forEach((img) => {
    imgs[img.id] = URL.createObjectURL(img.image);
  });
  return imgs;
}
