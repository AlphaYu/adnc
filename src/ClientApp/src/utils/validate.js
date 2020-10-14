/**
 * Created by PanJiaChen on 16/11/18.
 */

/**
 * @param {string} path
 * @returns {Boolean}
 */
export function isExternal(path) {
  return /^(https?:|mailto:|tel:)/.test(path)
}

/**
 * 校验用户名,必须是5-32位
 * @param {string} str
 * @returns {Boolean}
 */
export function isvalidUsername(str) {
  var reg = /^[0-9A-Za-z]{5,32}$/
  return reg.test(str)
}
