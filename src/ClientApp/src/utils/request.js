import axios from 'axios'
// 超大数字处理 https://juejin.im/post/5cd3a87bf265da0393788246
// import JSONbig from 'json-bigint'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import { getToken } from '@/utils/auth'
import router from '@/router'

// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  withCredentials: true, // send cookies when cross-domain requests
  timeout: 30000 // request timeout
  // `transformResponse` allows changes to the response data to be made before
  // it is passed to then/catch
  /*
  transformResponse: [function(data) {
    // Do whatever you want to transform the data
    return JSONbig.parse(data)
  }]
  */
})

// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent
    var token = getToken()
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}` // 让每个请求携带自定义token 请根据实际情况自行修改
    }
    return config
  },
  error => {
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  response => {
    if (response.headers.token) {
      // 如果后台通过header返回token，说明token已经更新，则更新客户端本地token
      store.dispatch('user/updateToken', { token: response.headers.token })
    }
    return response.data
  },
  error => {
    if (typeof error.response === 'undefined') {
      Message({
        message: error,
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    }
    if (error.response.status === 401) {
      store.dispatch('user/logout').then(() => {
        router.replace({
          path: '/login',
          query: { redirect: router.currentRoute.path }
        })
      })
      return
    }
    if (error.response.status >= 500) {
      Message({
        message: error.response.data || error.message || error,
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    }
    Message({
      dangerouslyUseHTMLString: true,
      message: error.response.data.detail || error.response.data.error || error.message || error,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
