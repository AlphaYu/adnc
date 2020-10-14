import treeTable from '@/components/TreeTable'
import { list, save, del } from '@/api/system/dept'
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
      formVisible: false,
      formTitle: '',
      isAdd: false,

      showTree: false,
      defaultProps: {
        id: 'id',
        label: 'simpleName',
        children: 'children'
      },
      form: {
        id: '',
        simpleName: '',
        fullName: '',
        pid: '',
        num: '',
        tips: ''
      },
      rules: {
        simpleName: [
          { required: true, message: '请输入菜单名称', trigger: 'blur' },
          { min: 3, max: 20, message: '长度在 3 到 20 个字符', trigger: 'blur' }
        ],
        fullName: [
          { required: true, message: '请输入编码', trigger: 'blur' },
          { min: 2, max: 20, message: '长度在 2 到 20 个字符', trigger: 'blur' }
        ],
        num: [
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
        this.listLoading = false
      })
    },
    handleNodeClick(data, node) {
      console.log(data)
      this.form.pid = data.id
      this.form.pname = data.simpleName
      this.showTree = false
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
      this.form = {}
      this.formTitle = '添加菜单'
      this.formVisible = true
      this.isAdd = true
    },
    save() {
      var self = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          console.log('form', self.form)
          const menuData = { 
            id: self.form.id, 
            simpleName: self.form.simpleName, 
            fullName: self.form.fullName, 
            num: parseInt(self.form.num), 
            pid: self.form.pid, 
            tips: self.form.tips 
          }
          menuData.parent = null
          save(menuData).then(response => {
            console.log(response)
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
      this.form = row

      if (row.parent) {
        this.form.pid = row.parent.id
        this.form.pname = row.parent.simpleName
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
