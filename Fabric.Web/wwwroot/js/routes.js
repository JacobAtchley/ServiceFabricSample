import RouterView from '/js/Components/RouterView.js';
import Home from '/js/Components/Home.js';
import Chat from '/js/Components/Chat/Index.js';
import People from '/js/Components/People/Index.js';
import PeopleEdit from '/js/Components/People/Edit.js';

export default [{
    path: '/',
    name: 'default',
    component: RouterView,
    props: false,
    redirect: {name: 'home'},
    children: [{
        path: '',
        name: 'home',
        component: Home
    },{
        path: '/chat',
        name: 'chat',
        component: Chat
    },{
        path: '/people',
        name: 'people-default',
        component: RouterView,
        redirect: {name: 'people'},
        children : [{
            path: '',
            name: 'people',
            component: People
        },{
            path: ':id',
            name: 'people-edit',
            component: PeopleEdit,
            props: route => ({
                id: route.params.id ? route.params.id.toString() : null,
                person: route.params.person || null
            })
        }]
    }]
}];