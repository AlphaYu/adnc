import { logList } from '@/api/maint/task'

export default {
  data() {
    return {
      taskId: '',
      listQuery: {
        page: 1,
        limit: 20,
        taskId: undefined
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
      this.listQuery.taskId = this.$route.query.taskId
      this.fetchData()
    },
    fetchData() {
      this.listLoading = true
      logList(this.listQuery).then(response => {
        this.list = response.data
        this.listLoading = false
        this.total = response.data.total
      })
    },
    fetchNext() {
      this.listQuery.page = this.listQuery.page + 1
      this.fetchData()
    },
    fetchPrev() {
      this.listQuery.page = this.listQuery.page - 1
      this.fetchData()
    },
    fetchPage(page) {
      this.listQuery.page = page
      this.fetchData()
    },
    changeSize(limit) {
      this.listQuery.limit = limit
      this.fetchData()
    },
    back() {
      this.$router.go(-1)
    }

  }
}
