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
    },
    mounted: function () {
        var vm = this;
        var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

        connection.on("Hello", function (message) {
            vm.calls.push(message + ' (From Signal R)');
        });

        connection.start().catch(function (err) {
            return console.error(err.toString());
        });
    }
});