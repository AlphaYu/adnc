import request from '@/utils/request'
const apiUrl = process.env.BASE_API
export function getApiUrl() {
  return apiUrl
}
export function getList(params) {
  return request({
    url: '/banner/list',
    method: 'get',
    params
  })
}


export function save(params) {
  return request({
    url: '/banner',
    method: 'post',
    params
  })
}

export function remove(id) {
  return request({
    url: '/banner',
    method: 'delete',
    params: {
      id: id
    }
  })
}
