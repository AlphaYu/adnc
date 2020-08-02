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
  name: 'Druid',
  data: function() {
    return {
      ifmsrc: 'http://193.112.75.77:18886'
    }
  },
  created: function() {
    this.init()
  },
  methods: {
    init() {
      this.$notify({
        title: '提示',
        message: '用户名/密码:admin/admin',
        duration: 3000
      })
    }
  }
}
</script>
