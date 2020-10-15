import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/maint/nloglogs',
    method: 'get',
    params
  })
}