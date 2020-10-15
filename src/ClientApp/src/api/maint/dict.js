import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/maint/dicts',
    method: 'get',
    params
  })
}

export function save(params) {
  return request({
    url: '/maint/dicts',
    method: 'post',
    data: params
  })
}

export function update(params) {
  return request({
    url: '/maint/dicts',
    method: 'put',
    data: params
  })
}

export function remove(id) {
  return request({
    url: `/maint/dicts/${id}`,
    method: 'delete'
  })
}
