import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/fileMgr/list',
    method: 'get',
    params
  })
}
