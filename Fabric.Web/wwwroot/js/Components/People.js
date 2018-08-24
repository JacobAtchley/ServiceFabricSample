import factory from '/js/Utils/ComponentFactory.js';
import mixin from '/js/store/people/mixin.js';

export default factory({
    name: 'home',
    path: '/js/Components/People.html',
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
            }
        },
        mounted() {
            this.loading = true;
            this.getAll().then(() => this.loading = false);
        }
    }
});
