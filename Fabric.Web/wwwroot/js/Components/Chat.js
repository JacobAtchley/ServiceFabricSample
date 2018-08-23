import factory from '/js/Utils/ComponentFactory.js';

export default factory({
    name: 'chat',
    path: '/js/Components/chat.html',
    vue: {
        data() {
            return {
                calls: [],
                message: ''
            };
        },
        methods: {
            postMessage() {
                if (!this.message) {
                    return;
                }

                axios.post('/api/chat', { message: this.message })
                    .then(() => this.message = '');
            }
        },
        mounted: function () {
            var connection = new signalR.HubConnectionBuilder().withUrl('/hub').build();

            connection.on("Hello", message => {
                this.calls.push(message);
            });

            connection
                .start()
                .catch(err => {
                    console.error(err.toString());
                });
        }
    }
})