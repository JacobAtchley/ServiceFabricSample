Vue.use(VueMaterial.default);

 new Vue({
    el: '#app',
    data: function() {
        return {
            calls: []
        };
    },
    methods: {
        checkIt: function() {
            var vm = this;

            axios.get('/api/test/orleans')
                .then(function(x) {
                    vm.calls.push(x.data);
                });
        }
    }
});