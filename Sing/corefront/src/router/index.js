import { createRouter, createWebHashHistory } from "vue-router";
const routes = [
  {
    path: "/upload",
    name: "upload",
    component: () => import("../pages/upload.vue"),
  },
  {
    path: "/kmain",
    name: "kmain",
    component: () => import("../pages/kmain.vue"),
  },
  {
    path: "/end",
    name: "end",
    component: () => import("../pages/end.vue"),
  },
  
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
