<template>
  <div class="app-container">
    <div class="block">
      <iframe id="mainIframe" ref="mainIframe" :src="ifmsrc" width="100%" height="768px" frameborder="0" scrolling="auto" name="mainIframe" />
    </div>
  </div>
</template>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>

<script>
export default {
  name: 'Swagger',
  data: function() {
    return {
      ifmsrc: process.env.VUE_APP_BASE_API + '/sys/index.html'
    }
  },
  mounted() {
  /*
    const iframe = document.getElementById('mainIframe')
    var iwindow = iframe.contentWindow
    var idoc = iwindow.document

    console.log('window', iwindow)// 获取iframe的window对象
    console.log('document', idoc) // 获取iframe的document
    console.log('html', idoc.documentElement)// 获取iframe的html
    console.log('head', idoc.head) // 获取head
    console.log('body', idoc.body) // 获取body
    */
    // idoc.getElementById("exit").style.visibility = "visible";
    // document.getElementsByClassName("btn authorize locked")[0].click()
    // document.getElementsByTagName("input")[0].value="444";
    // 无法执行onchange事件
    // document.getElementsByClassName("btn modal-btn auth authorize button")[0].click();
  }
}
</script>
