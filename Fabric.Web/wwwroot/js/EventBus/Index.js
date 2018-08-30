const EventBus = new Vue();

const onBus = (eventName, callback) => {
    EventBus.$off(eventName, callback);
    EventBus.$on(eventName, callback);
};

const offBus = (eventName) => {
    EventBus.$off(eventName);
};

const emitBus = (eventName, payload) => {
    EventBus.$emit(eventName, payload);
};

export { onBus, offBus, emitBus };