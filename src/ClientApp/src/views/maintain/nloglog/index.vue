<template>
  <div class="app-container">
    <div class="block">
      <el-row :gutter="24">
        <el-col :span="4">
          <el-input v-model="listQuery.level" size="mini" placeholder="级别" />
        </el-col>
        <el-col :span="4">
          <el-input v-model="listQuery.method" size="mini" placeholder="方法名" />
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

    <el-table v-loading="listLoading" :data="list" element-loading-text="Loading" border fit highlight-current-row>
      <el-table-column type="expand">
        <template slot-scope="scope">
          <el-input v-model="scope.row.exception" type="textarea" rows="30" />
        </template>
      </el-table-column>

      <el-table-column
        label="级别"
        prop="level"
        width="60px"
      />
      <el-table-column
        label="控制器"
        prop="properties.controller"
        width="200px"
      />
      <el-table-column
        label="方法"
        prop="properties.method"
        width="200px"
      />
      <el-table-column
        label="请求地址"
        prop="properties.queryUrl"
      />
      <el-table-column
        label="类型"
        prop="properties.requestMethod"
        width="80px"
      />
      <el-table-column
        label="请求内容"
        prop="properties.queryContent"
      />
      <el-table-column
        label="时间"
        prop="date"
        width="180px"
      />
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

<script src="./nloglog.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
