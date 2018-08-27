import factory from '/js/Utils/ComponentFactory.js';
import mixin from '/js/store/people/mixin.js';

export default factory({
    name: 'home',
    path: '/js/Components/People/Index.html',
    vue: {
        mixins: [mixin],
        data() {
            return {
                loading: true
            };
        },
        computed:{
            hasPeople() {
                return !!(this.entities && this.entities.length > 0);
            },
            people: {
                get() {
                    return this.entities;
                },
                set() {

                }
            }
        },
        mounted() {
            this.loading = true;
            this.getAll().then(() => this.loading = false);
        }
    }
});
