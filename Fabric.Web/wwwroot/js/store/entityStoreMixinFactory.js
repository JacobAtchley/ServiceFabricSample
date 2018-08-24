const mapState = Vuex.mapState;
const mapActions = Vuex.mapActions;

const mixinFactory = function(options) {
    return {
        computed: mapState({
            entities : state => state[options.module].entities
        }),
        methods: mapActions(options.module, [
            'getAll'
        ]),
        created () {
            if(options.fetchOnCreated){
                this.getAll();
            }
        }
    };
};

export default mixinFactory;