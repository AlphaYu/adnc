import { queryByUser } from '@/api/system/opslog'

export default {
  data() {
    return {
      activeName: 'timeline',
      user: {},
      reverse:false,
      activities: []
    }
  },
  mounted() {
    this.init()
  },
  methods: {
    init(){
      this.user = this.$store.state.user.profile
      this.queryByUser()
    },
    handleClick(tab, event){
      this.$router.push({ path: '/account/'+tab.name})
    },
    queryByUser() {
      queryByUser().then(response => {
            console.log(response)
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
