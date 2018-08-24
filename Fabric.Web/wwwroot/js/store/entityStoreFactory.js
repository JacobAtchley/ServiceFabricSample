const storeFactory = function(options) {
    return {
        namespaced: true,

        state : {
            entities: null
        },

        getters : {
        },

        actions : {
        getAll({ commit, state }){
            return new Promise((resolve, reject) => {
                    if(state.entities && state.entities.length > 0){
                        resolve(state.entities);
                    }

                    axios.get(options.rootUrl)
                        .then(x => {
                            var entities = (x.data ||[]).map(d => options.mapper(d));
                            commit('setEntities', entities);
                            resolve(entities);
                        } )
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