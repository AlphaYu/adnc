import { updatePwd } from '@/api/user'

export default {
  data() {
    return {
      form: {
        oldPassword: '',
        password: '',
        rePassword: ''
      },
      activeName: 'updatePwd',
      user: {}
    }
  },
  computed: {
    rules() {
      return {
        password: [
          { required: true, message: '密码不能为空', trigger: 'blur' },
          { min: 5, max: 100, message: '密码长度不能小于5', trigger: 'blur' }
        ]
      }
    }
  },
  mounted() {
    this.init()
  },
  methods: {
    init() {
      this.user = this.$store.state.user.profile
    },
    handleClick(tab, event) {
      this.$router.push({ path: '/account/' + tab.name })
    },
    updatePwd() {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          updatePwd({
            oldPassword: this.form.oldPassword,
            password: this.form.password,
            rePassword: this.form.rePassword
          }).then(response => {
            this.$message({
              message: '密码修改成功',
              type: 'success'
            })
            // 退出登录，该操作是个异步操作，所以后面跳转到登录页面延迟1s再执行（如果有更好的方法再调整）
            this.$store.dispatch('user/logout')
            const self = this
            setTimeout(function() {
              self.$router.push(`/login`)
            }, 1000)
          }).catch(() => {
          })
        } else {
          return false
        }
      })
    }
  }
}
