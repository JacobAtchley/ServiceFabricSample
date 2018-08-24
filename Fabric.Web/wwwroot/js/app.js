import routes from '/js/routes.js';
import store from '/js/index.js';

Vue.use(VueMaterial.default);
Vue.use(VueRouter);

const vue = new Vue({
    el: '#app',
    router: new VueRouter({
        routes,
        store
    })
});