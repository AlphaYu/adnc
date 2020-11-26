// import Vue from "../node_modules/vue/dist/vue.js";
import Vue from 'vue'

// 导入 css文件
import 'normalize.css/normalize.css' // A modern alternative to CSS resets

// 引入第三方插件
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'

import ECharts from 'vue-echarts/components/ECharts.vue'
import '@/styles/index.scss' // global css
// 引入工具类/组件
// import App from './App.vue';
import App from './App' // App.Vue
import store from './store'
// 会默认识别router里面的index.js文件（不能是其他名字）
import router from './router'
// Internationalization
import i18n from './lang'
// @ 以根目录的方式定义相对路径
import '@/icons' // icon
import '@/permission' // permission control

import Treeselect from '@riophae/vue-treeselect'
import '@riophae/vue-treeselect/dist/vue-treeselect.css'
Vue.component('treeselect', Treeselect)

/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online! ! !
 */

// set ElementUI lang to EN
// 使用组件，需要 Vue.use() 的组件，也就是有 install 的组件
Vue.use(ElementUI, { i18n: (key, value) => i18n.t(key, value) })

// register global component,
// 全局注册的行为必须在根 Vue 实例 (通过 new Vue) 创建之前发生
Vue.component('v-chart', ECharts)

// Vue.config 是一个对象，包含 Vue 的全局配置。可以在启动应用之前修改下列 property：
// 阻止你显示显示生产模式的消息阻止你显示显示生产模式的消息
Vue.config.productionTip = false

new Vue({
  el: '#app',
  router, // (缩写) 相当于 routes: routes
  store,
  i18n,
  // render: h => h(App)
  // createElement 函数是用来生成 HTML DOM 元素的
  // https://www.jianshu.com/p/b68184695e1f
  render: function(createElement) {
    return createElement(App)
  }
})
