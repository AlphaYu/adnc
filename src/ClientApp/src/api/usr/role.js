import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/usr/roles/page',
    method: 'get',
    params
  })
}

export function save(data) {
  let methodName = 'post'
  let url = '/usr/roles'
  if (data.id > 0) {
    methodName = 'put'
    url = url + '/' + data.id
  }
  return request({
    url: url,
    method: methodName,
    data
  })
}

export function remove(roleId) {
  return request({
    url: `/usr/roles/${roleId}`,
    method: 'delete'
  })
}

export function roleTreeListByUserId(userId) {
  return request({
    url: `/usr/roles/${userId}/rolestree`,
    method: 'get'
  })
}

export function changePermissons(roleId, permissons) {
  return request({
    url: `/usr/roles/${roleId}/permissons`,
    method: 'put',
    data: permissons
  })
}
