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
    url: '/sys/depts',
    method: 'get'
  })
}

export function save(data) {
  return request({
    url: '/sys/depts',
    method: 'post',
    data
  })
}

export function del(id) {
  return request({
    url: `/sys/depts/${id}`,
    method: 'delete'
  })
}
