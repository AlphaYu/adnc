import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sys/cfgs',
    method: 'get',
    params
  })
}

export function exportXls(params) {
  return request({
    url: '/sys/cfgs/execl',
    method: 'get',
    params
  })
}

export function save(params) {
  return request({
    url: '/sys/cfgs',
    method: 'post',
    data: params
  })
}

export function remove(id) {
  console.log(id)
  return request({
    url: `/sys/cfgs/${id}`,
    method: 'delete'
  })
}
