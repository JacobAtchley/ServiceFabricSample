import Home from '/js/Components/Home.js';
import Chat from '/js/Components/Chat.js';
import People from '/js/Components/People.js';

export default [{
        path: '/',
        name: 'home',
        component: Home
    },{
        path: '/chat',
        name: 'chat',
        component: Chat
    },{
        path: '/people',
        name: 'people',
        component: People
    }
];