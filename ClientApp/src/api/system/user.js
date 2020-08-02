import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/users',
    method: 'get',
    params
  })
}

export function saveUser(params) {
  return request({
    url: '/sys/users',
    method: 'post',
    data: params
  })
}

export function remove(userId) {
  return request({
    url: `/sys/users/${userId}`,
    method: 'delete'
  })
}

export function setRole(userId, roleIds) {
  return request({
    url: `/sys/users/${userId}/roles`,
    method: 'put',
    data:roleIds
  })
}

export function changeStatus(userId, status) {
  return request({
    url: `/sys/users/${userId}/status`,
    method: 'put',
    data: {"value":status}
  })
}

export function changeStatusBatch(params) {
  return request({
    url: '/sys/users/batch/status',
    method: 'put',
    data: params
  })
}
