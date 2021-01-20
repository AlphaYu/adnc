<template>
  <div class="app-container">
    <div class="block">
      <el-row :gutter="24">
        <el-col :span="4">
          <el-input v-model="listQuery.account" size="mini" placeholder="账号" />
        </el-col>
        <el-col :span="4">
          <el-input v-model="listQuery.device" size="mini" placeholder="设备" />
        </el-col>
        <el-col :span="4">
          <el-date-picker
            v-model="listQuery.beginTime"
            type="datetime"
            size="mini"
            placeholder="起始日期"
            style="width: 100%;"
            default-time="00:00:01"
            :picker-options="pickerOptions"
          />
        </el-col>
        <el-col :span="4">
          <el-date-picker
            v-model="listQuery.endTime"
            type="datetime"
            size="mini"
            placeholder="结束日期"
            style="width: 100%;"
            :picker-options="pickerOptions"
          />
        </el-col>
        <el-col :span="8">
          <el-button type="success" size="mini" icon="el-icon-search" @click.native="search">{{ $t('button.search') }}</el-button>
          <el-button type="primary" size="mini" icon="el-icon-refresh" @click.native="reset">{{ $t('button.reset') }}</el-button>
        </el-col>
      </el-row>
      <br>
    </div>

    <el-table
      v-loading="listLoading"
      :stripe="true"
      :data="list"
      row-key="id"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row
    >
      <el-table-column label="Id" width="220px">
        <template slot-scope="scope">
          {{ scope.row.id }}
        </template>
      </el-table-column>
      <el-table-column label="账号" width="120px">
        <template slot-scope="scope">
          {{ scope.row.account }}
        </template>
      </el-table-column>
      <el-table-column label="姓名" width="120px">
        <template slot-scope="scope">
          {{ scope.row.userName }}
        </template>
      </el-table-column>
      <el-table-column label="日志类型" width="120px">
        <span>登录日志</span>
      </el-table-column>
      <el-table-column label="设备" width="100px">
        <template slot-scope="scope">
          {{ scope.row.device }}
        </template>
      </el-table-column>
      <el-table-column label="登录Ip" width="150px">
        <template slot-scope="scope">
          {{ scope.row.remoteIpAddress }}
        </template>
      </el-table-column>
      <el-table-column label="成功" width="60px">
        <template slot-scope="scope">
          {{ scope.row.succeed }}
        </template>
      </el-table-column>
      <el-table-column label="状态码" width="70px">
        <template slot-scope="scope">
          {{ scope.row.statusCode }}
        </template>
      </el-table-column>      
      <el-table-column label="登录信息">
        <template slot-scope="scope">
          {{ scope.row.message }}
        </template>
      </el-table-column>
      <el-table-column label="时间" width="180px">
        <template slot-scope="scope">
          {{ scope.row.createTime }}
        </template>
      </el-table-column>
    </el-table>

    <el-pagination
      background
      layout="total, sizes, prev, pager, next, jumper"
      :page-sizes="[10,20, 50, 100,500]"
      :page-size="listQuery.pageSize"
      :total="total"
      :current-page.sync="listQuery.pageIndex"
      @size-change="changeSize"
      @current-change="fetchPage"
      @prev-click="fetchPrev"
      @next-click="fetchNext"
    />

  </div>
</template>

<script src="./loginlog.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
