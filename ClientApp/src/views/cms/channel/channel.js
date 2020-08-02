import { remove, getList, save } from '@/api/cms/channel'

export default {
  data() {
    return {
      formVisible: false,
      formTitle: '添加栏目',
      deptList: [],
      isAdd: true,
      form: {
        id: '',
        name: '',
        code: ''
      },
      total: 0,
      list: null,
      listLoading: true,
      selRow: {}
    }
  },

  computed: {
    rules() {
      return {
        cfgName: [
          { required: true, message: '名称不能为空', trigger: 'blur' }
        ],
        cfgValue: [
          { required: true, message: '编码不能为空', trigger: 'blur' }
        ]
      }
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
      getList().then(response => {
        this.list = response.data
        this.listLoading = false
      })
    },
    search() {
      this.fetchData()
    },
    reset() {
      this.fetchData()
    },
    handleFilter() {
      this.getList()
    },
    handleCurrentChange(currentRow, oldCurrentRow) {
      this.selRow = currentRow
    },
    resetForm() {
      this.form = {
        id: '',
        name: '',
        code: ''
      }
    },
    add() {
      this.resetForm()
      this.formTitle = '添加栏目'
      this.formVisible = true
      this.isAdd = true
    },
    save() {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          save({
            id: this.form.id,
            name: this.form.name,
            code: this.form.code
          }).then(response => {
            this.$message({
              message: this.$t('common.optionSuccess'),
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
        message: this.$t('common.mustSelectOne'),
        type: 'warning'
      })
      return false
    },
    edit() {
      if (this.checkSel()) {
        this.isAdd = false
        this.form = this.selRow
        this.formTitle = '编辑栏目'
        this.formVisible = true
      }
    },
    remove() {
      if (this.checkSel()) {
        var id = this.selRow.id
        this.$confirm(this.$t('common.deleteConfirm'), this.$t('common.tooltip'), {
          confirmButtonText: this.$t('button.submit'),
          cancelButtonText: this.$t('button.cancel'),
          type: 'warning'
        }).then(() => {
          remove(id).then(response => {
            this.$message({
              message: this.$t('common.optionSuccess'),
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
