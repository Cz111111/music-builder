
import { createRouter,createWebHashHistory } from "vue-router";
import { useUsername } from "@/stores/username";
//const getUserName = useUsername()

const routes=[
  {
    path:"/",
    name:"intro",
    component: () => import("../pages/intro.vue"),
     //meta: { requiresAuth: false },
},
  // {
  //     path:"/login",
  //     name:"login",
  //     component: () => import("../views/login/login.vue"),
  //      //meta: { requiresAuth: false },
  // },
  {
    path:"/register",
    name:"register",
    component: () => import("../views/register/register.vue"),
     //meta: { requiresAuth: false },
  },
  // {
  //     path:"/index",
  //     name:"index",
  //     component: () => import("../views/index/index.vue"),
  //     // meta: { requiresAuth: true },
  // },
  {
    path:"/index/createsong",
    name:"createsong",
    component: () => import("../views/index/createsong/createsong.vue"),
     //meta: { requiresAuth: true },
},
{
  path:"/index/mysong",
  name:"mysong",
  component: () => import("../views/index/mysong/mysong.vue"),
   //meta: { requiresAuth: true },
},
/* {
  path:"/index/download",
  name:"download",
  component: () => import("../views/index/download/download.vue"),
   //meta: { requiresAuth: true },
}, */
{
  path:"/render",
  name:"render",
  component: () => import("../views/render/render.vue"),
   //meta: { requiresAuth: true },
},
{
  path: "/upload",
  name: "upload",
  component: () => import("../pages/upload.vue"),
  //meta: { requiresAuth: true },
},
{
  path: "/kmain",
  name: "kmain",
  component: () => import("../pages/kmain.vue"),
  //meta: { requiresAuth: true },
},
{
  path: "/edit",
  name: "edit",
  component: () => import("../views/musicEdit/edit.vue"),
  //meta: { requiresAuth: true },
},
];

const router= createRouter(
  {
      history:createWebHashHistory(),
      routes,
  },
);


export default router;
