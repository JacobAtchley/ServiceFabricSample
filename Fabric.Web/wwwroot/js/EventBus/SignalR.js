import { emitBus } from '/js/EventBus/Index.js';

const connection = new signalR
    .HubConnectionBuilder()
    .withUrl('/hub')
    .configureLogging(signalR.LogLevel.Trace)
    .build();

const start = (events) => {
    connection
    .start()
    .catch(err => {
        console.error(err);
    });
    
    if(events && events.length > 0){
        events.forEach(e => {
            connection.on(e, message => {
                console.log('Received Signal R Message', e, message);
                emitBus(e, message);
            });       
        });
    }
};

export { start };