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
      ifmsrc: "http://193.112.75.77:8666/healthchecks-ui"
    }
  }
}
</script>
