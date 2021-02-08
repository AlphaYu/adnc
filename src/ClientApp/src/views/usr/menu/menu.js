import treeTable from '@/components/TreeTable'
import { getList, save, delMenu } from '@/api/usr/menu'
import permission from '@/directive/permission/index.js'
import IconSelect from '@/components/IconSelect'

export default {
  name: 'menus',
  components: { treeTable, IconSelect },
  directives: { permission },
  data() {
    return {
      showTree: false,
      defaultProps: {
        id: 'code',
        label: 'name',
        children: 'children'
      },
      listLoading: true,
      expandAll: false,
      formTitle: '',
      formVisible: false,
      isAdd: false,
      form: {
        id: 0,
        name: '',
        code: '',
        url: '',
        pCode: '',
        isMenu: true,
        ordinal: 1,
        component: '',
        icon: '',
        status: true,
        hidden: false
      },
      rules: {
        name: [
          { required: true, message: '请输入菜单名称', trigger: 'blur' },
          { min: 2, max: 16, message: '长度在 2 到 16 个字符', trigger: 'blur' }
        ],
        code: [
          { required: true, message: '请输入编码', trigger: 'blur' },
          { min: 2, max: 16, message: '长度在 2 到 16 个字符', trigger: 'blur' }
        ],
        url: [
          { required: true, message: '请输入资源地址', trigger: 'blur' },
          { min: 2, max: 64, message: '长度在 2 到 16 个字符', trigger: 'blur' }
        ],
        component: [
          { required: true, message: '请输入组件代码', trigger: 'blur' },
          { min: 2, max: 64, message: '长度在 2 到 64 个字符', trigger: 'blur' }
        ],
        icon: [
          { required: true, message: '请选择图标', trigger: 'blur' },
          { min: 2, max: 16, message: '长度在 2 到 16 个字符', trigger: 'blur' }
        ],
        ordinal: [
          { required: true, message: '请输入排序', trigger: 'blur' }
        ]
      },
      data: [],
      treeData: [],
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
      getList().then(data => {
        this.data = data
        this.treeData = this.convertToTreeData(data)
        this.listLoading = false
      })
    },
    convertToTreeData(listData) {
      var params = []
      for (var index in listData) {
        var obj = {}
        obj['id'] = listData[index].code
        obj['label'] = listData[index].name
        if (listData[index].children != null && listData[index].children.length > 0) { obj['children'] = this.convertToTreeData(listData[index].children) }
        params.push(obj)
      }
      return params
    },
    selected(name) {
      this.form.icon = name
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
      this.form = { isMenu: true, status: true, hidden: false, icon: '', ordinal: 1 }
      this.formTitle = '添加菜单'
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
          const menuData = self.form
          delete menuData.parent
          delete menuData.children
          menuData.ordinal = parseInt(menuData.ordinal)
          save(menuData).then(response => {
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
      this.form.isMenu = row.isMenu
      this.form.status = row.status === 1
      this.form.hidden = row.hidden
      if (this.form.pCode === '0') {
        this.form.pCode = undefined
      }
      this.formTitle = '编辑菜单'
      this.formVisible = true
      this.isAdd = false
    },
    remove(row) {
      this.$confirm('确定删除该记录?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        delMenu(row.id).then(response => {
          this.$message({
            message: '删除成功',
            type: 'success'
          })
          this.fetchData()
        }).catch(err => {
          this.$notify.error({
            title: '错误',
            message: err
          })
        })
      })
    },
    componentTips() {
      this.$notify({
        title: '提示',
        dangerouslyUseHTMLString: true,
        message: '一级目录请输入layout,<br/>二级目录请输入实际组件路径<br/>如:views/maintain/dict/index<br/>功能按钮不需要输入'
      })
    }
  }
}
