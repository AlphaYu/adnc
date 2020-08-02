import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/contacts/list',
    method: 'get',
    params
  })
}
