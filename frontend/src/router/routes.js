const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      {
        path: "/recipe/:id",
        component: () => import("pages/RecipePage.vue"),
        props: true,
      },
      {
        path: "/recipe/:id/edit",
        component: () => import("pages/RecipeEditPage.vue"),
        props: true,
      },
      {
        path: "",
        component: () => import("pages/RecipeOverviewPage.vue"),
      },
      {
        path: "/login",
        component: () => import("pages/LoginPage.vue"),
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
