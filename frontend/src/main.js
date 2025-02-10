// main.js
import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import IntegrationMethods from './components/IntegrationMethods.vue'
import CompareMethods from './components/CompareMethods.vue'
import ErrorEstimation from './components/ErrorEstimation.vue'
import SimpleFunctionsComparison from './components/SimpleFunctionsComparison.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', component: IntegrationMethods },
        { path: '/compare', component: CompareMethods },
        { path: '/error-estimation', component: ErrorEstimation },
        { path: '/simple-functions', component: SimpleFunctionsComparison }
    ]
})

createApp(App).use(router).mount('#app')