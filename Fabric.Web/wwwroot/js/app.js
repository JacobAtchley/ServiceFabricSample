import routes from '/js/routes.js';
import store from '/js/store/index.js';
import myApp from '/js/Components/Index.js';
import { start } from '/js/EventBus/SignalR.js';

Vue.use(VueMaterial.default);
Vue.use(VueRouter);
Vue.use(VeeValidate);

myApp.router = new VueRouter({
    routes
});

myApp.store = store;

const app = new Vue(myApp);

app.$mount('#app');

start(['Hello', 'EntityModified;Person']);