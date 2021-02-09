import { remove, getList, save } from '@/api/maint/dict'
// 权限判断指令
import permission from '@/directive/permission/index.js'

export default {
  directives: { permission },
  data() {
    return {
      formVisible: false,
      formTitle: '添加字典',
      deptList: [],
      roleList: [],
      isAdd: true,
      permissons: [],
      permissonVisible: false,
      form: {
        name: '',
        id: '',
        value: '',
        children: [],
        details: []
      },
      rules: {
        name: [
          { required: true, message: '请输入字典名称', trigger: 'blur' },
          { min: 2, max: 16, message: '长度在 2 到 16 个字符', trigger: 'blur' }
        ]

      },
      listQuery: {
        name: undefined
      },
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
      }).catch(() => {
      })
    },
    search() {
      this.fetchData()
    },
    reset() {
      this.listQuery.name = ''
      this.fetchData()
    },
    handleFilter() {
      this.getList()
    },
    handleClose() {

    },

    handleCurrentChange(currentRow, oldCurrentRow) {
      this.selRow = currentRow
    },
    resetForm() {
      this.form = {
        name: '',
        id: '',
        value: '',
        details: [],
        children: []

      }
    },
    add() {
      this.resetForm()
      this.formTitle = '添加字典'
      this.formVisible = true
      this.isAdd = true
    },
    save() {
      var self = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          var name = self.form.name
          var dictValues = []
          for (var key in self.form.details) {
            var item = self.form.details[key]
            dictValues.push({ 'name': item['name'], 'value': item['value'], 'ordinal': parseInt(item['ordinal']) })
          }
          var id = 0
          if (this.form.id !== '') {
            id = self.form.id
          }
          save({ id: id, name: name, children: dictValues }).then(response => {
            this.$message({
              message: '提交成功',
              type: 'success'
            })
            self.fetchData()
            self.formVisible = false
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
    editItem(record) {
      this.selRow = record
      this.edit()
    },
    edit() {
      if (this.checkSel()) {
        this.isAdd = false
        this.formTitle = '修改字典'
        var children = this.selRow.children
        var details = []
        children.forEach(function(val, index) {
          details.push({ 'name': val['name'], 'value': val['value'], 'ordinal': val['ordinal'] })
        })
        this.form = { name: this.selRow.name, id: this.selRow.id, details: details, children: children }
        this.formVisible = true
      }
    },
    removeItem(record) {
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
            // this.selRow.remove()
          })
        }).catch(() => {
        })
      }
    },
    addDetail() {
      var details = this.form.details

      details.push({
        name: '',
        value: '',
        ordinal: 1
      })
      this.form.details = details
    },
    removeDetail(detail) {
      var details = this.form.details
      details.forEach(function(val, index) {
        if (detail.value === val.value) {
          console.log(detail.key)
          details.splice(index, 1)
        }
      })
      this.form.details = details
    }

  }
}
