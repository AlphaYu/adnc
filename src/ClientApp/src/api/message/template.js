import request from '@/utils/request'

export function getList(params) {
    return request({
        url: '/message/template/list',
        method: 'get',
        params
    })
}


export function save(params) {
    return request({
        url: '/message/template',
        method: 'post',
        params
    })
}

export function remove(id) {
    return request({
        url: '/message/template',
        method: 'delete',
        params: {
            id: id
        }
    })
}
