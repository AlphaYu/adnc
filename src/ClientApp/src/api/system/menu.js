import request from '@/utils/request'

export function getList() {
  return request({
    url: '/sys/menus',
    method: 'get'
  })
}

export function listForRouter(params) {
  return request({
    url: '/sys/menus/routers',
    method: 'get',
    params
  })
}

export function save(data) {
  return request({
    url: '/sys/menus',
    method: 'post',
    data
  })
}

export function delMenu(id) {
  return request({
    url: `/sys/menus/${id}`,
    method: 'delete'
  })
}
export function menuTreeListByRoleId(roleId) {
  return request({
    url: `/sys/menus/${roleId}/menuTree`,
    method: 'get'
  })
}
