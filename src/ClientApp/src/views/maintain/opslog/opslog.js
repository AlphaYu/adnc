import { clear, getList } from '@/api/maint/opslog'
import permission from '@/directive/permission/index.js'

export default {
  directives: { permission },
  data() {
    return {
      pickerOptions: {
        shortcuts: [{
          text: '今天',
          onClick(picker) {
            picker.$emit('pick', new Date());
          }
        }, {
          text: '昨天',
          onClick(picker) {
            const date = new Date();
            date.setTime(date.getTime() - 3600 * 1000 * 24);
            picker.$emit('pick', date);
          }
        }, {
          text: '一周前',
          onClick(picker) {
            const date = new Date();
            date.setTime(date.getTime() - 3600 * 1000 * 24 * 7);
            picker.$emit('pick', date);
          }
        }] },
      options: [{
        value: '1',
        label: '业务日志'
      }, {
        value: '2',
        label: '异常日志'
      }
      ],
      listQuery: {
        pageIndex: 1,
        pageSize: 10,
        beginTime: undefined,
        endTime: undefined,
        method: undefined,
        account: undefined
      },
      total: 0,
      list: null,
      listLoading: true,
      selRow: {}
    }
  },
  created() {
    this.init()
  },
  methods: {
    init() {
      this.fetchData()
    },
    fetchData() {
      this.listLoading = true
      getList(this.listQuery).then(response => {
        this.list = response.data
        this.listLoading = false
        this.total = response.totalCount
      })
    },
    search() {
      this.listQuery.pageIndex = 1
      this.fetchData()
    },
    reset() {
      this.listQuery.beginTime = ''
      this.listQuery.endTime = ''
      this.listQuery.method = ''
      this.listQuery.accout = ''
      this.listQuery.pangeIndex = 1
      // this.fetchData()
    },
    handleFilter() {
      this.listQuery.pageIndex = 1
      this.getList()
    },
    fetchNext() {
      this.listQuery.pangeIndex = this.listQuery.pangeIndex + 1
      this.fetchData()
    },
    fetchPrev() {
      this.listQuery.pangeIndex = this.listQuery.pangeIndex - 1
      this.fetchData()
    },
    fetchPage(pangeIndex) {
      this.listQuery.pangeIndex = pangeIndex
      this.fetchData()
    },
    changeSize(pageSize) {
      this.listQuery.pageSize = pageSize
      this.fetchData()
    },
    clear() {
      this.$confirm('确定清空数据?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        clear().then(response => {
          this.$message({
            message: '清空成功',
            type: 'sucess'
          })
          this.fetchData()
        })
      }).catch(() => {
      })
    }

  }
}
