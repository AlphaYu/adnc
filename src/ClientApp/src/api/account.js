import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/usr/session',
    method: 'post',
    data
  })
}

export function getInfo() {
  return request({
    url: '/usr/session',
    method: 'get'
  })
}

export function logout(token) {
  return request({
    url: '/usr/session',
    method: 'delete'
  })
}

export function updatePwd(params) {
  return request({
    url: '/usr/session/password',
    method: 'put',
    data: params
  })
}

