import { createRouter,createWebHashHistory } from "vue-router";
import { state } from '../state/state.js';

const routes=[
    {
        path:"/",
        name:"main",
        component: () => import("../App.vue"),
        meta: { requiresAuth: true },
    },
    {
      path:"/upload",
      name:"upload",
      component: () => import("../pages/upload.vue"),
      meta: { requiresAuth: true },
  },
  {
    path:"/kmain",
    name:"kmain",
    component: () => import("../pages/kmain.vue"),
    meta: { requiresAuth: true },
  },
  {
    path:"/end",
    name:"end",
    component: () => import("../pages/end.vue"),
    meta: { requiresAuth: true },
  },
    
    
];

const router= createRouter(
    {
        history:createWebHashHistory(),
        routes,
    },
);

router.beforeEach((to, from, next) => {
    if (to.meta.requiresAuth && !state.isAuthenticated) {
      next('/login');
    } else {
      next();
    }
  });
export default router;