import { remove, getList, save } from '@/api/cms/banner'
import { getToken } from '@/utils/auth'
import { Loading } from 'element-ui'
import { getApiUrl } from '@/utils/utils'

export default {
  data() {
    return {
      uploadUrl: '',
      uploadFileId: '',
      uploadHeaders: {
        'Authorization': ''
      },
      loadingInstance: {},
      formVisible: false,
      formTitle: '添加banner',
      deptList: [],
      isAdd: true,
      options: [
        { label: '首页', value: 'index' },
        { label: '新闻', value: 'news' },
        { label: '产品', value: 'product' },
        { label: '解决方案', value: 'solution' },
        { label: '案例', value: 'case' }
      ],
      form: {
        id: '',
        title: '',
        url: '',
        img: '',
        idFile: '',
        type: 'index'
      },
      listQuery: {
        title: undefined
      },
      list: null,
      listLoading: true,
      selRow: {}
    }
  },
  computed: {
    rules() {
      return {
        title: [
          { required: true, message: '标题不能为空', trigger: 'blur' }
        ],
        url: [
          { required: true, message: 'URL不能为空', trigger: 'blur' }
        ]
      }
    }
  },
  created() {
    this.init()
  },
  methods: {
    init() {
      this.uploadUrl = getApiUrl() + '/file'
      this.uploadHeaders['Authorization'] = getToken()
      this.fetchData()
    },
    fetchData() {
      this.listLoading = true
      getList(this.listQuery).then(response => {
        this.list = response.data
        for (var index in this.list) {
          let item = this.list[index]
          item.img = getApiUrl() + '/file/getImgStream?idFile=' + item.idFile
          console.log(item)
        }

        this.listLoading = false
      })
    },
    search() {
      this.fetchData()
    },
    reset() {
      this.listQuery.title = ''
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
        title: '',
        url: '',
        idFile: this.uploadFileId,
        type: ''
      }
    },
    add() {
      this.resetForm()
      this.formTitle = '添加banner'
      this.formVisible = true
      this.isAdd = true
    },
    save() {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          save({
            id: this.form.id,
            title: this.form.title,
            url: this.form.url,
            idFile: this.uploadFileId,
            type: this.form.type
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
    },
    handleBeforeUpload() {
      if (this.uploadFileId !== '') {
        this.$message({
          message: this.$t('common.mustSelectOne'),
          type: 'warning'
        })
        return false
      }
      this.loadingInstance = Loading.service({
        lock: true,
        text: this.$t('common.uploading'),
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.7)'
      })
    },
    handleUploadSuccess(response, raw) {
      this.loadingInstance.close()
      if (response.code === 20000) {
        console.log(response.data)
        this.uploadFileId = response.data.id
        this.form.fileName = response.data.originalFileName
      } else {
        this.$message({
          message: this.$t('common.uploadError'),
          type: 'error'
        })
      }
    }
  }
}
