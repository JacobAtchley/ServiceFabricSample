import { onBus } from '/js/EventBus/Index.js';

const hasEntities = state => {
    return !!(state.entities && state.entities.length > 0);
};

const getEntityStoreIndex = (state, id) => {
    return state.entities.findIndex(x => x.id === id);
};

const updateEntityInStore = function(commit, state, entity){
    if(!hasEntities(state)){
        return;
    }

    var storeEntityIndex = getEntityStoreIndex(state, entity.id);

    if(storeEntityIndex >= 0) {
        //use splice so vue can update the vue. setting the index will not make the DOM reactive.
        state.entities.splice(storeEntityIndex, 1, entity);
    }
    else{
        state.entities.push(entity);
    }

    commit('setEntities', state.entities);
};

const removeEntityInStore = function(commit, state, id){
    if(!hasEntities(state)){
        return;
    }

    var storeEntityIndex = getEntityStoreIndex(state, id);

    state.entities.splice(storeEntityIndex);

    commit('setEntities', state.entities);
};

const storeFactory = function(options) {
    
    return {
        namespaced: true,

        state : {
            entities: null
        },

        getters : {
        },

        actions : {
            initStore({commit, state}){
                if (options.busName) {
                    onBus(options.busName, (update) => {
                        switch(update.reason || ''){
                            case 'Added':
                            case 'Updated':
                                updateEntityInStore(commit, state, update.entity);
                                break;
                            case 'Deleted':
                                removeEntityInStore(commit, state, update.entity.id);
                                break;
                        }
                    });
                }
            },
            getAll({ commit, state }){
                return new Promise((resolve, reject) => {
                    if(hasEntities(state)){
                        resolve(state.entities);
                        return;
                    }

                    axios.get(options.rootUrl)
                        .then(x => {
                            var entities = (x.data ||[]).map(d => options.mapper(d));
                            commit('setEntities', entities);
                            resolve(entities);
                        })
                        .catch(e => {
                            console.error(e);
                            reject(e);
                        });
                });
            },
            
            getById({state}, id){
                return new Promise((resolve, reject) => {
                    let entity = null;

                    if(hasEntities(state)) {
                        entity = state.entities.find(x => x.id === id);
                    }

                    if(!!entity) {
                        return resolve(entity);
                    }

                    axios.get(`${options.rootUrl}/${id}`)
                        .then(x => {
                            if(!!x.data){
                                resolve(options.mapper(x.data));
                                return;
                            }

                            resolve(null);
                        })
                        .catch(e => {
                            console.error(e);
                            reject(e);
                        });
                });
            },

            addEntity({ commit, state}, entity){
                return new Promise((resolve, reject) => {
                    axios.post(options.rootUrl, entity)
                        .then(x => {
                            updateEntityInStore(commit, state, x.data);
                            resolve(x.data);
                        })
                        .catch(e => {
                            console.error(e);
                            reject(e);
                        });
                });
            },

            updateEntity({commit, state}, entity){
                return new Promise((resolve, reject) => {
                    axios.put(`${options.rootUrl}/${entity.id}`, entity)
                        .then(x => {
                            updateEntityInStore(commit, state, x.data);
                            resolve(x.data);
                        })
                        .catch(e => {
                            console.error(e);
                            reject(e);
                        });
                });
            },

            deleteEntity({commit, state}, id){
                return new Promise((resolve, reject) => {
                    axios.delete(`${options.rootUrl}/${id}`)
                        .then(() => {
                            removeEntityInStore(commit, state, id);
                            resolve();
                        })
                        .catch(e => {
                            console.error(e);
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