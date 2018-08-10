Vue.use(VueMaterial.default);

 new Vue({
    el: '#app',
    data: function() {
        return {
            calls: [],
            message: ''
        };
    },
    methods: {
        postMessage: function() {
            var vm = this;

            if (!vm.message) {
                return;
            }

            axios.post('/api/test/chat', { message: vm.message }).then(() => vm.message = '');
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