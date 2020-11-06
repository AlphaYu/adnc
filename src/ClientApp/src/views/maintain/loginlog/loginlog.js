import { clear, getList } from '@/api/maint/loginlog'
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
      listQuery: {
        pageIndex: 1,
        pageSize: 10,
        beginTime: undefined,
        endTime: undefined,
        device: undefined,
        account: undefined
      },
      total: 0,
      list: [],
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
      this.listQuery.device = ''
      this.listQuery.account = ''
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
    }
  }
}
