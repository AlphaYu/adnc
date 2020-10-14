import { getList } from '@/api/cms/fileInfo'
import { getApiUrl } from '@/utils/utils'

export default {
  data() {
    return {
      listQuery: {
        page: 1,
        limit: 20,
        originalFileName: undefined
      },
      total: 0,
      list: null,
      listLoading: true,
      selRow: {},
      downloadUrl: ''
    }
  },
  created() {
    this.init()
  },
  methods: {
    init() {
      this.downloadUrl = getApiUrl() + '/file/download?idFile='
      this.fetchData()
    },
    fetchData() {
      this.listLoading = true
      getList(this.listQuery).then(response => {
        this.list = response.data.records
        this.listLoading = false
        this.total = response.data.total
      })
    },
    search() {
      this.fetchData()
    },
    reset() {
      this.listQuery.originalFileName = ''
      this.fetchData()
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    handleClose() {

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
    handleCurrentChange(currentRow, oldCurrentRow) {
      this.selRow = currentRow
    },
    download(id,fileName) {
      window.location.href = this.downloadUrl + id+'&fileName='+fileName
    }

  }
}
