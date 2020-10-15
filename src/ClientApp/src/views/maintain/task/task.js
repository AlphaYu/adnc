import { remove, getList, save, disable, enable } from '@/api/maint/task'
import permission from '@/directive/permission/index.js'

export default {
  directives: { permission },
  data() {
    return {
      formVisible: false,
      formTitle: '添加任务',
      deptList: [],
      isAdd: true,
      form: {
        id: '',
        name: '',
        jobClass: '',
        cron: '',
        note: '',
        disabled: true,
        data: ''
      },
      rules: {
        name: [
          { required: true, message: '请输入任务名', trigger: 'blur' },
          { min: 2, max: 20, message: '长度在 3 到 20 个字符', trigger: 'blur' }
        ],
        jobClass: [
          { required: true, message: '请输入执行类', trigger: 'blur' }
        ],
        cron: [
          { required: true, message: '请输入定时规则', trigger: 'blur' }
        ]

      },
      listQuery: {
        name: undefined
      },
      total: 0,
      list: null,
      listLoading: true,
      selRow: {}
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'gray',
        deleted: 'danger'
      }
      return statusMap[status]
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
        this.list = response
        this.listLoading = false
      })
    },
    search() {
      this.listQuery.page = 1
      this.fetchData()
    },
    reset() {
      this.listQuery.name = ''
      this.listQuery.page = 1
      this.fetchData()
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    handleClose() {

    },
    handleCurrentChange(currentRow, oldCurrentRow) {
      this.selRow = currentRow
    },
    resetForm() {
      this.form = {}
    },
    add() {
      this.resetForm()
      this.formTitle = '添加任务'
      this.formVisible = true
      this.isAdd = true
    },
    save() {
      var self = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          save({
            id: self.form.id,
            name: self.form.name,
            jobClass: self.form.jobClass,
            cron: self.form.cron,
            data: self.form.data,
            note: self.form.note
          }).then(response => {
            console.log(response)
            this.$message({
              message: '提交成功',
              type: 'success'
            })
            this.fetchData()
            this.formVisible = false
          })
        } else {
          return false
        }
      })
    },
    checkSel() {
      if (this.selRow && this.selRow.id) {
        return true
      }
      this.$message({
        message: '请选中操作项',
        type: 'warning'
      })
      return false
    },
    enable(id) {
      this.$confirm('确定启用该定时任务?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        enable(id).then(response => {
          this.$message({
            message: '操作成功',
            type: 'success'
          })
          this.fetchData()
        })
      }).catch(() => {
      })
    },
    disable(id) {
      this.$confirm('确定禁用该定时任务?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        disable(id).then(response => {
          this.$message({
            message: '操作成功',
            type: 'success'
          })
          this.fetchData()
        })
      }).catch(() => {
      })
    },
    viewLog(taskId) {
      this.$router.push({ path: '/task/taskLog', query: { taskId: taskId }})
    },
    editItem(record){
      this.selRow = record
      this.edit()
    },
    edit() {
      if (this.checkSel()) {
        this.isAdd = false
        this.form = this.selRow
        this.formTitle = '修改任务'
        this.formVisible = true
      }
    },
    removeItem(record){
      this.selRow = record
      this.remove()
    },
    remove() {
      if (this.checkSel()) {
        var id = this.selRow.id
        this.$confirm('确定删除该记录?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          remove(id).then(response => {
            this.$message({
              message: '操作成功',
              type: 'success'
            })
            this.fetchData()
          })
        }).catch(() => {
        })
      }
    }

  }
}
