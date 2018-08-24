const storeFactory = function(options) {
    return {
        namespaced: true,

        state : {
            entities: []
        },

        getters : {
        },

        actions : {
        getAll({ commit, state }){
            return new Promise((resolve, reject) => {
                
                if(state.entities){
                    resolve(state.entities);
                }

                axios.get(options.rootUrl)
                .then(x => (x.data ||[]).map(d => options.mapper(d)))
                .then(x => commit('setEntities',x))
                .catch(e => {
                    reject(e);
                });

            });
            }
        },

        mutations : {
            setEntities (state, entities){
                state.entities = entities;
            }
        }
    };
};

export default storeFactory;