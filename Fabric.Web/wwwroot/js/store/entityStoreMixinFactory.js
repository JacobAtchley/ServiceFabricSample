import {onBus, offBus } from '/js/EventBus/Index.js';

const mapState = Vuex.mapState;
const mapActions = Vuex.mapActions;

const mixinFactory = function(options) {
    return {
        computed: Object.assign({}, mapState({
            entities : state => state[options.module].entities
        }), {
            isFormDirty(){
                return Object.keys(this.fields).map(x => this.fields[x].dirty).some(x => !!x);
            }
        }),
        methods: Object.assign({}, mapActions(options.module, [
            'getAll',
            'getById',
            'addEntity',
            'updateEntity',
            'deleteEntity'
        ]), {
            mergeEntity(e){
                return !!e.id ? this.updateEntity(e) : this.addEntity(e);
            },
            validateAll(){
                return new Promise((resolve, reject) => {
                    this.$validator.validateAll()
                        .then((result) => {
                            if (result) {
                                resolve();
                            }
                            else{
                                reject();
                            }
                    });
                });
            }
        }),
        destroyed() {
            if (options.busName) {
                offBus(options.busName);
            }
        },
        created () {
            if(options.fetchOnCreated){
                this.getAll();
            }

            if (options.busName) {
                onBus(options.busName, (update) => {
                    debugger;
                    console.log(update);
                });
            }
        }
    };
};

export default mixinFactory;