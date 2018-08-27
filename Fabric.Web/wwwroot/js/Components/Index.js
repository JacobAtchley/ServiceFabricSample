export default {
    template: `<div class="page-container">
    <md-app>
        <md-app-toolbar class="md-primary">
            <span class="md-title">Service Fabric Test</span>
        </md-app-toolbar>
        <md-app-drawer md-permanent="full">
            <md-toolbar class="md-transparent" md-elevation="0">
                Navigation
            </md-toolbar>
            <md-list>
                <md-list-item>
                    <router-link :to="{name: 'home'}">
                        Home
                    </router-link>
                </md-list-item>
                <md-list-item>
                    <router-link :to="{name: 'chat'}">
                        Chat
                    </router-link>
                </md-list-item>
                <md-list-item>
                    <router-link :to="{name: 'people'}">
                        People
                    </router-link>
                </md-list-item>
            </md-list>
        </md-app-drawer>
        <md-app-content>
            <router-view></router-view>
        </md-app-content>
    </md-app>
</div>`
};