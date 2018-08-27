const mapState = Vuex.mapState;
const mapActions = Vuex.mapActions;

const mixinFactory = function(options) {
    return {
        computed: mapState({
            entities : state => state[options.module].entities
        }),
        methods: Object.assign({}, mapActions(options.module, [
            'getAll',
            'getById',
            'addEntity',
            'updateEntity'
        ]), {
            mergeEntity(e){
                return !!e.id ? this.updateEntity(e) : this.addEntity(e);
            }
        }),
        created () {
            if(options.fetchOnCreated){
                this.getAll();
            }
        }
    };
};

export default mixinFactory;