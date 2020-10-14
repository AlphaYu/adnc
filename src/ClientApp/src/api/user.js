import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/sys/session',
    method: 'post',
    data
  })
}

export function getInfo() {
  return request({
    url: '/sys/session',
    method: 'get'
  })
}

export function logout(token) {
  return request({
    url: '/sys/session',
    method: 'delete'
  })
}

export function updatePwd(params) {
  return request({
    url: '/sys/session/password',
    method: 'put',
    data: params
  })
}

