import { updatePwd } from '@/api/account'

export default {
  data() {
    return {
      form: {
        oldPassword: '',
        password: '',
        rePassword: ''
      },
      activeName: 'updatePwd',
      user: {},
      formRules: {
        oldPassword: [
          { required: true, message: '原密码不能为空', trigger: 'blur' },
          { min: 5, max: 16, message: '原密码长度不能小于5,大于16', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '密码不能为空', trigger: 'blur' },
          { min: 5, max: 16, message: '密码长度不能小于5,大于16', trigger: 'blur' }
        ],
        rePassword: [
          { required: true, message: '重复密码不能为空', trigger: 'blur' },
          { validator: (rule, value, callback) => {
            if (value !== this.form.password) { callback(new Error('两次输入密码不一致!')) } else { callback() }
          }
          }
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
