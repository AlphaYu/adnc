import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/loginlogs',
    method: 'get',
    params
  })
}