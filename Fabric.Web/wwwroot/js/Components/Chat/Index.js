import factory from '/js/Utils/ComponentFactory.js';
import { onBus, offBus } from '/js/EventBus/Index.js';

export default factory({
    name: 'chat',
    path: '/js/Components/Chat/Index.html',
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
        mounted () {
            onBus('Hello', message => {
                this.calls.push(message);
            });
        },
        destroyed() {
            offBus('Hello');
        }
    }
});