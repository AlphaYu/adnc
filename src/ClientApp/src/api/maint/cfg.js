import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/maint/cfgs',
    method: 'get',
    params
  })
}

export function exportXls(params) {
  return request({
    url: '/maint/cfgs/execl',
    method: 'get',
    params
  })
}

export function save(params) {
  let methodName = 'post'
  if (params.id > 0) { methodName = 'put' }
  return request({
    url: '/maint/cfgs',
    method: methodName,
    data: params
  })
}

export function remove(id) {
  return request({
    url: `/maint/cfgs/${id}`,
    method: 'delete'
  })
}
