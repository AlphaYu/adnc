import request from '@/utils/request'

/*
export function tree() {
  return request({
    url: '/depts/tree',
    method: 'get'
  })
}
*/

export function list() {
  return request({
    url: '/usr/depts',
    method: 'get'
  })
}

export function save(data) {
  let methodName = 'post'
  let url = '/usr/depts'
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
    url: `/usr/depts/${id}`,
    method: 'delete'
  })
}
