import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/article/list',
    method: 'get',
    params
  })
}


export function save(params) {
  return request({
    url: '/article',
    method: 'post',
    data : params
  })
}

export function remove(id) {
  return request({
    url: '/article',
    method: 'delete',
    params: {
      id: id
    }
  })
}

export function get(id) {
  return request({
    url: '/article',
    method: 'get',
    params: {
      id: id
    }
  })
}
