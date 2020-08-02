import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/notices',
    method: 'get',
    params
  })
}
