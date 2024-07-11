
import { createRouter,createWebHashHistory } from "vue-router";


const routes=[
  {
      path:"/",
      name:"login",
      component: () => import("../views/login/login.vue"),
      // meta: { requiresAuth: false },
  },
  {
    path:"/register",
    name:"register",
    component: () => import("../views/register/register.vue"),
    // meta: { requiresAuth: true },
  },
  {
      path:"/index",
      name:"index",
      component: () => import("../views/index/index.vue"),
      // meta: { requiresAuth: true },
  },
  {
    path:"/index/createsong",
    name:"createsong",
    component: () => import("../views/index/createsong/createsong.vue"),
    // meta: { requiresAuth: true },
},
{
  path:"/render",
  name:"render",
  component: () => import("../views/render/render.vue"),
  // meta: { requiresAuth: true },
},
];

const router= createRouter(
  {
      history:createWebHashHistory(),
      routes,
  },
);

// router.beforeEach((to, from, next) => {
//   if (to.meta.requiresAuth && !state.isAuthenticated) {
//     next('/');
//   } else {
//     next();
//   }
// });
export default router;
