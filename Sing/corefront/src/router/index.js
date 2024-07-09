import { createRouter,createWebHashHistory } from "vue-router";
//import { state } from '../state/state.js';

const routes=[
   
    {
      path:"/",
      name:"upload",
      component: () => import("../pages/upload.vue"),
  },
  {
    path:"/kmain",
    name:"kmain",
    component: () => import("../pages/kmain.vue"),
  },
  {
    path:"/end",
    name:"end",
    component: () => import("../pages/end.vue"),
  },
    
    
];

const router= createRouter(
    {
        history:createWebHashHistory(),
        routes,
    },
);


export default router;