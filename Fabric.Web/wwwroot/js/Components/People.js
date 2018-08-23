import factory from '/js/Utils/ComponentFactory.js';
import Person from '/js/Models/Person.js';

export default factory({
    name: 'home',
    path: '/js/Components/People.html',
    vue: {
        data() {
            return {
                people: [],
                loading: true
            };
        },
        computed:{
            hasPeople() {
                return !!(this.people && this.people.length > 0);
            }
        },
        mounted() {
            this.loading = true;

            axios.get('/api/grains/people')
                .then(x => this.people = (x.data ||[]).map(d => new Person(d)))
                .catch(e => {
                    console.log(e);
                    window.alert(e);
                }).then(x => this.loading = false);
        }
    }
});
