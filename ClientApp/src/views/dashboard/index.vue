<template>
  <div class="dashboard-container">
    <div class="dashboard-text">欢迎光临:{{name}} </div>

    <el-row :gutter="20">
      <el-col :span="6">
        <el-card class="box-card">
          <el-col :span="12">
            <svg-icon  icon-class="documentation" ></svg-icon>
          </el-col>
          <el-col :span="12" >
            <div class="card-panel-text">{{ $t('dashboard.document') }}</div>
            <div class="card-panel-num">
              <a href="http://enilu.gitee.io/web-flash" target="_blank">Document</a>
            </div>
          </el-col>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="box-card">
          <el-col :span="12">
            <svg-icon  icon-class="user" ></svg-icon>
          </el-col>
          <el-col :span="12" >
            <div class="card-panel-text">{{ $t('dashboard.newUser') }}</div>
            <div class="card-panel-num">102,400</div>
          </el-col>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="box-card">
          <el-col :span="12">
            <svg-icon  icon-class="message" ></svg-icon>
          </el-col>
          <el-col :span="12" >
            <div class="card-panel-text">{{ $t('dashboard.message') }}</div>
            <div class="card-panel-num">5</div>
          </el-col>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="box-card">
          <el-col :span="12">
            <svg-icon  icon-class="money" ></svg-icon>
          </el-col>
          <el-col :span="12" >
            <div class="card-panel-text">{{ $t('dashboard.income') }}</div>
            <div class="card-panel-num">10000</div>
          </el-col>
        </el-card>
      </el-col>
    </el-row>

    <el-row>
      <el-col :span="24">
        <div class="chart section">
        <v-chart :options="lineData"/>
        </div>
      </el-col>
    </el-row>

    <el-row :gutter="20" >
      <el-col :span="12">
        <el-collapse   class="section">
          <el-collapse-item title="2019双11All in cloud低至一折" name="1">
            <div><a href="https://www.aliyun.com/1111/2019/home?userCode=alts44ap" target="_blank">点击查看详情</a></div>
          </el-collapse-item>
          <el-collapse-item title="主机爆款限时优惠" name="2">
            <div><a href="https://www.aliyun.com/acts/hotsale?userCode=alts44ap" target="_blank">点击查看详情</a> </div>
          </el-collapse-item>
          <el-collapse-item title="企业级云服务器五折优惠" name="3">
            <div><a href="https://promotion.aliyun.com/ntms/act/enterprise-discount.html?userCode=alts44ap" target="_blank">点击查看详情</a> </div>
          </el-collapse-item>
          <el-collapse-item title="全民云计算云主机低至4折" name="4">
            <div><a href="https://promotion.aliyun.com/ntms/act/qwbk.html?userCode=alts44ap" target="_blank">点击查看详情</a> </div>
          </el-collapse-item>
          <el-collapse-item title="商标注册服务低至8折" name="5">
            <div><a href="https://tm.aliyun.com/?userCode=alts44ap" target="_blank">点击查看详情</a> </div>
          </el-collapse-item>
          <el-collapse-item title="云短信产品低至8折" name="6">
            <div><a href="https://www.aliyun.com/acts/alicomcloud/new-discount?userCode=alts44ap" target="_blank">点击查看详情</a> </div>
          </el-collapse-item>



        </el-collapse>
      </el-col>

      <el-col :span="12">
        <v-chart :options="barData" class="chart section"/>
      </el-col>
    </el-row>

    <el-row :gutter="20" >
      <el-col :span="12">
        <v-chart :options="pieData" class="chart section"/>
      </el-col>
      <el-col :span="12">
        <el-table :data="tableData" class="section">
          <el-table-column
            prop="date"
            :label="$t('dashboard.date')"
            width="180">
          </el-table-column>
          <el-table-column
            prop="name"
            :label="$t('dashboard.name')"
            width="180">
          </el-table-column>
          <el-table-column
            prop="address"
            :label="$t('dashboard.addr')">
          </el-table-column>
        </el-table>

      </el-col>

    </el-row>
  </div>
</template>


<script src="./dashboard.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  .el-row{
    margin-bottom: 20px;
    &:last-child {
      margin-bottom: 0;
    }
  }

.dashboard {

  &-container {
    padding: 15px;
    background-color: #f0f2f5;
  }
  &-text {
    font-size: 14px;
    line-height: 22px;
    padding-bottom:15px;
  }
}
.echarts{
  width: 100%;
  height: 100%;
}
.box-card{
  height:108px;
}
  .chart{
    height: 350px;
  }
  .section{
    padding:20px;
    background-color: white;
    border: 1px solid #ebeef5;
    box-shadow: 0 2px 12px 0 rgba(0,0,0,.1);
  }

  .box-card > div >div > .svg-icon {
    width: 4em;
    height: 4em;
    color:#34bfa3;
  }
  .card-panel-text{
    padding-top:10px;
    font-size:16px;
    color:gray;
  }
  .card-panel-num{
    padding-top:10px;
    font-size:20px;
    font-weight: bold;
  }
</style>
