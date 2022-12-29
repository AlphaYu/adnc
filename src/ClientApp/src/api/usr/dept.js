import request from '@/utils/request'

/*
export function tree() {
  return request({
    url: '/organizations/tree',
    method: 'get'
  })
}
*/

export function list() {
  return request({
    url: '/usr/organizations',
    method: 'get'
  })
}

export function save(data) {
  let methodName = 'post'
  let url = '/usr/organizations'
  if (data.id > 0) {
    methodName = 'put'
    url = url + '/' + data.id
  }
  return request({
    url: url,
    method: methodName,
    data
  })
}

export function del(id) {
  return request({
    url: `/usr/organizations/${id}`,
    method: 'delete'
  })
}
