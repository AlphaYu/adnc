
export function setSideMenus(Menus) {
  /* 存储的是Json 字符串*/
  localStorage.setItem('sideMenus', JSON.stringfy(Menus))
}

export function getSideMenus() {
  const strMenus = localStorage.getItem('sideMenus')
  return JSON.parse(strMenus)
}


export function traverseRoutes(menus){
  let routes = menus.map( menu =>{
    if(menu.component){
      let name = menu.component;
      menu.component = ()=> import(`@/${name}`)
    }

    if(menu.children && menu.children.length){
      menu.children = traverseRoutes(menu.children);
    }
    return menu;
  })
  return routes;
}
