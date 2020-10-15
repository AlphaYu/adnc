import { getListByUser } from '@/api/maint/opslog'

export default {
  data() {
    return {
      activeName: 'timeline',
      user: {},
      reverse: false,
      activities: [],
      query: {
        pageIndex: 1,
        pageSize: 10
      }
    }
  },
  mounted() {
    this.init()
  },
  methods: {
    init() {
      this.user = this.$store.state.user.profile
      this.queryByUser()
    },
    handleClick(tab, event) {
      this.$router.push({ path: '/account/' + tab.name })
    },
    queryByUser() {
      getListByUser(this.query).then(response => {
        this.activities = response.data
      }).catch((err) => {
        this.$message({
          message: err,
          type: 'error'
        })
      })
    }

  }
}
