<template>
  <el-select v-model="dictValue"
             :size="size"
             :placeholder="placeholder">
    <el-option
      v-for="item in dictList"
      :key="item.num"
      :label="item.name"
      :value="item.num">
    </el-option>
  </el-select>
</template>

<script>
  import {getDicts} from '@/api/maint/dict'

  export default {
    model: {
      prop: 'defaultValue', //接收props内值
      event: 'changeData' //自定义事件名
    },
    props: {
      size: {
        type: String,
        default: 'mini'
      },
      defaultValue: {
        type: String,
        default: ''
      },
      dictName: {
        type: String,
        default: ''
      },
      placeholder: {
        type: String,
        default: ''
      }
    },
    data() {
      return {
        dictValue: '',
        dictList: []
      }
    },
    watch: {
      // 监听父组件传入的数据，更新到本地
      defaultValue(newVal, oldVal) {
        console.log('par->son newVal:'+newVal+",oldVal:"+oldVal)
        this.dictValue = newVal
      },
      // 监听本地数据的变化，通知父组件更新
      dictValue(newVal, oldVal) {
        console.log('son->par newVal:'+newVal+', oldVal:'+oldVal)
        this.$emit('change', newVal)
      }
    },
    created() {
      this.getDictList()
    },
    methods: {
      getDictList() {
        //从后台获取字典列表
        this.dictValue = this.defaultValue
        getDicts(this.dictName).then(response => {
          this.dictList = response.data
        })
      }
    }
  }
</script>

<style lang="scss" scoped>

</style>
