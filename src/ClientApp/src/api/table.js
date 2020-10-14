import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/table/list',
    method: 'get',
    params
  })
}
