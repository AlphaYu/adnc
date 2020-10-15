import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/maint/opslogs',
    method: 'get',
    params
  })
}

export function getListByUser(params) {
  return request({
    url: '/maint/users/opslogs',
    method: 'get',
    params
  })
}
