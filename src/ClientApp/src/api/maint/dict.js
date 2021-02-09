import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/maint/dicts',
    method: 'get',
    params
  })
}

export function save(params) {
  let methodName = 'post'
  let url = '/maint/dicts'
  if (params.id > 0) {
    methodName = 'put'
    url = url + '/' + params.id
  }
  return request({
    url: url,
    method: methodName,
    data: params
  })
}

export function remove(id) {
  return request({
    url: `/maint/dicts/${id}`,
    method: 'delete'
  })
}
