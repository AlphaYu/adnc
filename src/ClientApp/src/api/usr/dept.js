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
  if (data.id > 0) { methodName = 'put' }
  return request({
    url: '/usr/depts',
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
