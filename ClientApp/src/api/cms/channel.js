import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/channel/list',
    method: 'get',
    params
  })
}


export function save(params) {
  return request({
    url: '/channel',
    method: 'post',
    params
  })
}

export function remove(id) {
  return request({
    url: '/channel',
    method: 'delete',
    params: {
      id: id
    }
  })
}
