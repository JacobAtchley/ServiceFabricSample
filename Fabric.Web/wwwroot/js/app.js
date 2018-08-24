import routes from '/js/routes.js';
import store from '/js/store/index.js';

Vue.use(VueMaterial.default);
Vue.use(VueRouter);

const vue = new Vue({
    el: '#app',
    store,
    router: new VueRouter({
        routes
    })
});