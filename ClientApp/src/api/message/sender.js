import request from '@/utils/request'

export function getList(params) {
    return request({
        url: '/message/sender/list',
        method: 'get',
        params
    })
}

export function queryAll(params) {
  return request({
    url: '/message/sender/queryAll',
    method: 'get'
  })
}

export function save(params) {
    return request({
        url: '/message/sender',
        method: 'post',
        params
    })
}

export function remove(id) {
    return request({
        url: '/message/sender',
        method: 'delete',
        params: {
            id: id
        }
    })
}
