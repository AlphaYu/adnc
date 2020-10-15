import { getList } from '@/api/maint/notice'
import { mapGetters } from 'vuex'
import ECharts from 'vue-echarts/components/ECharts'
import 'echarts/lib/chart/bar'
import 'echarts/lib/chart/line'
import 'echarts/lib/chart/pie'
import 'echarts/lib/chart/map'
import 'echarts/lib/chart/radar'
import 'echarts/lib/chart/scatter'
import 'echarts/lib/chart/effectScatter'
import 'echarts/lib/component/tooltip'
import 'echarts/lib/component/polar'
import 'echarts/lib/component/geo'
import 'echarts/lib/component/legend'
import 'echarts/lib/component/title'
import 'echarts/lib/component/visualMap'
import 'echarts/lib/component/dataset'
import 'echarts/map/js/world'
import 'zrender/lib/svg/svg'

export default {

  name: 'dashboard',
  components: {
    chart: ECharts
  },
  data() {
    const data = []
    for (let i = 0; i <= 360; i++) {
      const t = i / 180 * Math.PI
      const r = Math.sin(2 * t) * Math.cos(2 * t)
      data.push([r, i])
    }
    return {
      notice: [],
      lineData: {
        title: {
          text: ''
        },
        tooltip: {
          trigger: 'axis'
        },
        legend: {
          data: [this.$t('dashboard.email'), this.$t('dashboard.ad'), this.$t('dashboard.vedio'), this.$t('dashboard.direct'), this.$t('dashboard.searchEngine')]
        },
        grid: {
          left: '3%',
          right: '4%',
          bottom: '3%',
          containLabel: true
        },
        toolbox: {
          feature: {
            saveAsImage: {}
          }
        },
        xAxis: {
          type: 'category',
          boundaryGap: false,
          data: [this.$t('common.week.mon'), this.$t('common.week.tue'), this.$t('common.week.wed'), this.$t('common.week.thu'), this.$t('common.week.fri'), this.$t('common.week.sat'), this.$t('common.week.sun')]
        },
        yAxis: {
          type: 'value'
        },
        series: [
          {
            name: this.$t('dashboard.email'),
            type: 'line',
            stack: '总量',
            data: [120, 132, 101, 134, 90, 230, 210]
          },
          {
            name: this.$t('dashboard.ad'),
            type: 'line',
            stack: '总量',
            data: [220, 182, 191, 234, 290, 330, 310]
          },
          {
            name: this.$t('dashboard.vedio'),
            type: 'line',
            stack: '总量',
            data: [150, 232, 201, 154, 190, 330, 410]
          },
          {
            name: this.$t('dashboard.direct'),
            type: 'line',
            stack: '总量',
            data: [320, 332, 301, 334, 390, 330, 320]
          },
          {
            name: this.$t('dashboard.searchEngine'),
            type: 'line',
            stack: '总量',
            data: [820, 932, 901, 934, 1290, 1330, 1320]
          }
        ]
      },
      barData: {
        xAxis: {
          type: 'category',
          data: [this.$t('common.week.mon'), this.$t('common.week.tue'), this.$t('common.week.wed'), this.$t('common.week.thu'), this.$t('common.week.fri'), this.$t('common.week.sat'), this.$t('common.week.sun')]
        },
        yAxis: {
          type: 'value'
        },
        series: [{
          data: [120, 200, 150, 80, 70, 110, 130],
          type: 'bar'
        }]
      },
      pieData: {
        title: {
          text: this.$t('dashboard.userFrom'),
          subtext: '纯属虚构',
          x: 'center'
        },
        tooltip: {
          trigger: 'item',
          formatter: '{a} <br/>{b} : {c} ({d}%)'
        },
        legend: {
          orient: 'vertical',
          left: 'left',
          data: [this.$t('dashboard.email'), this.$t('dashboard.ad'), this.$t('dashboard.vedio'), this.$t('dashboard.direct'), this.$t('dashboard.searchEngine')]
        },
        series: [
          {
            name: 'from',
            type: 'pie',
            radius: '55%',
            center: ['50%', '60%'],
            data: [
              { value: 335, name: this.$t('dashboard.direct') },
              { value: 310, name: this.$t('dashboard.email') },
              { value: 234, name: this.$t('dashboard.ad') },
              { value: 135, name: this.$t('dashboard.vedio') },
              { value: 1548, name: this.$t('dashboard.searchEngine') }
            ],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            }
          }
        ]
      },
      tableData: [{
        date: '2016-05-02',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1518 弄'
      }, {
        date: '2016-05-04',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1517 弄'
      }, {
        date: '2016-05-01',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1519 弄'
      }, {
        date: '2016-05-03',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1516 弄'
      }]
    }
  },
  computed: {
    ...mapGetters([
      'name'

    ])
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      this.listLoading = true
      const self = this
      getList(self.listQuery).then(data => {
        for (var i = 0; i < data.length; i++) {
          var notice = data[i]
          self.$notify({
            title: notice.title,
            message: notice.content,
            duration: 3000
          })
        }
        self.listLoading = false
      })
    }
  }
}
