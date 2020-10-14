import Mock from 'mockjs'

const data = Mock.mock({
  'menus':[  
    {
      path: '/product',
      component: 'layout',   
      children: [
        {
          path: 'index',
          name: 'product',
          component: 'views/product/index',
          meta:{title: 'Product',icon: 'form'}
        }
      ]
    },
    { path: '*', redirect: '/404', hidden: true }
  ]
})


export default [
  {
    url:'/menu/list',
    type:'get',
    response: config =>{
      return {
        code:20000,
        data:{
          menus:data.menus
        }
      }
    }
  }


]