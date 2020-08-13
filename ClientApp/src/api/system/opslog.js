import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/opslogs',
    method: 'get',
    params
  })
}

export function getListByUser(params) {
  return request({
    url: '/sys/users/opslogs',
    method: 'get',
    params
  })
}
