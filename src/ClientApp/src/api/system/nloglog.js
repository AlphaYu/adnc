import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/nloglogs',
    method: 'get',
    params
  })
}