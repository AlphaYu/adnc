import { mapGetters } from 'vuex'

export default {

  name: 'swagger',
  computed: {
    ...mapGetters([
      'name'

    ])
  }
}
