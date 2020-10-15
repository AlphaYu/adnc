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
  return request({
    url: '/usr/depts',
    method: 'post',
    data
  })
}

export function del(id) {
  return request({
    url: `/usr/depts/${id}`,
    method: 'delete'
  })
}
