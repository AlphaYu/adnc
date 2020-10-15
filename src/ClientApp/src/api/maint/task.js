import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/maint/tasks',
    method: 'get',
    params
  })
}

export function save(params) {
  return request({
    url: '/maint/tasks',
    method: 'post',
    data: params
  })
}

export function remove(id) {
  return request({
    url: `/maint/tasks/${id}`,
    method: 'delete'
  })
}

export function logList(params) {
  return request({
    url: '/maint/tasks/logs',
    method: 'get',
    params
  })
}

export function enable(id) {
  return request({
    url: `/maint/tasks/${id}/status`,
    method: 'put',
    data: 0
  })
}

export function disable(id) {
  return request({
    url: `/maint/tasks/${id}/status`,
    method: 'put',
    data: 1
  })
}
