import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/dicts',
    method: 'get',
    params
  })
}

export function save(params) {
  return request({
    url: '/sys/dicts',
    method: 'post',
    data: params
  })
}

export function update(params) {
  return request({
    url: '/sys/dicts',
    method: 'put',
    data: params
  })
}

export function remove(id) {
  return request({
    url: `/sys/dicts/${id}`,
    method: 'delete'
  })
}
