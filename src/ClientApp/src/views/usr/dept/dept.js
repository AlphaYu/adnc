import treeTable from '@/components/TreeTable'
import { list, save, del } from '@/api/usr/dept'
// 权限判断指令
import permission from '@/directive/permission/index.js'

export default {
  directives: { permission },
  name: 'customTreeTableDemo',
  components: { treeTable },
  data() {
    return {
      expandAll: true,
      data: [],
      deptTreeData: [],
      formVisible: false,
      formTitle: '',
      isAdd: false,
      showTree: false,
      defaultProps: {
        id: 'id',
        label: 'name',
        children: 'children'
      },
      form: {
        id: '',
        simpleName: '',
        fullName: '',
        pid: '',
        ordinal: '',
        tips: ''
      },
      rules: {
        simpleName: [
          { required: true, message: '请输入部门简称', trigger: 'blur' },
          { min: 2, max: 16, message: '长度在 2 到 16 个字符', trigger: 'blur' }
        ],
        fullName: [
          { required: true, message: '请输入部门全称', trigger: 'blur' },
          { min: 2, max: 32, message: '长度在 2 到 32 个字符', trigger: 'blur' }
        ],
        ordinal: [
          { required: true, message: '请输入排序', trigger: 'blur' }
        ]
      }

    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      this.listLoading = true
      list().then(data => {
        this.data = data
        this.deptTreeData = this.convertToTreeData(data)
      })
    },
    convertToTreeData(listData) {
      var params = []
      for (var index in listData) {
        var obj = {}
        obj['id'] = listData[index].id
        obj['label'] = listData[index].simpleName
        if (listData[index].children != null && listData[index].children.length > 0) { obj['children'] = this.convertToTreeData(listData[index].children) }
        params.push(obj)
      }
      return params
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
    add() {
      this.form = { ordinal: 1 }
      this.formTitle = '添加部门'
      this.formVisible = true
      this.isAdd = true
      if (this.$refs['form'] !== undefined) {
        this.$refs['form'].resetFields()
      }
    },
    save() {
      var self = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          const deptData = {
            id: self.form.id,
            simpleName: self.form.simpleName,
            fullName: self.form.fullName,
            ordinal: parseInt(self.form.ordinal),
            pid: self.form.pid,
            tips: self.form.tips
          }
          save(deptData).then(response => {
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
    edit(row) {
      if (this.$refs['form'] !== undefined) {
        this.$refs['form'].resetFields()
      }
      this.form = Object.assign({}, row)
      if (this.form.pid === 0) {
        this.form.pid = undefined
      }
      this.formTitle = '编辑部门'
      this.formVisible = true
      this.isAdd = false
    },
    remove(row) {
      this.$confirm('确定删除该记录?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        del(row.id).then(response => {
          this.$message({
            message: '删除成功',
            type: 'success'
          })
          this.fetchData()
        })
      })
    }
  }
}
