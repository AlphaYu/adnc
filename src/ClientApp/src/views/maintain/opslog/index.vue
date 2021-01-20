<template>
  <div class="app-container">
    <div class="block">
      <el-row :gutter="24">
        <el-col :span="4">
          <el-input v-model="listQuery.account" size="mini" placeholder="账号" />
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
        <!--
        <el-col :span="4">
          <el-select v-model="listQuery.logType" size="mini" placeholder="日志类型">
            <el-option
              v-for="item in options"
              :key="item.value"
              :label="item.label"
              :value="item.value">
            </el-option>
          </el-select>
        </el-col>
        -->
        <el-col :span="8">
          <el-button type="success" size="mini" icon="el-icon-search" @click.native="search">{{ $t('button.search') }}</el-button>
          <el-button type="primary" size="mini" icon="el-icon-refresh" @click.native="reset">{{ $t('button.reset') }}</el-button>
          <el-button v-permission="['/log/delLog']" type="danger" size="mini" icon="el-icon-delete" @click.native="clear">{{ $t('button.clear') }}</el-button>
        </el-col>
      </el-row>
      <br>
    </div>

    <el-table v-loading="listLoading" :data="list" element-loading-text="Loading" border fit highlight-current-row>
      <el-table-column type="expand">
        <template slot-scope="scope">
          <el-form label-position="left" inline class="flash-table-expand">
            <el-form-item label="日志类型">
              <span>{{ scope.row.logType }}</span>
            </el-form-item>
            <el-form-item label="日志名称">
              <span>{{ scope.row.logName }}</span>
            </el-form-item>
            <el-form-item label="账号">
              <span>{{ scope.row.account }}</span>
            </el-form-item>
            <el-form-item label="姓名">
              <span>{{ scope.row.userName }}</span>
            </el-form-item>
            <el-form-item label="类名">
              <span>{{ scope.row.className }}</span>
            </el-form-item>
            <el-form-item label="方法名">
              <span>{{ scope.row.method }}</span>
            </el-form-item>
            <el-form-item label="Ip">
              <span>{{ scope.row.remoteIpAddress }}</span>
            </el-form-item>
            <el-form-item label="时间">
              <span>{{ scope.row.createTime }}</span>
            </el-form-item>
            <el-form-item label="内容">
              <span>{{ scope.row.message }}</span>
            </el-form-item>
          </el-form>
        </template>
      </el-table-column>
      <el-table-column
        label="Id"
        prop="id"
      />     
      <el-table-column
        label="账号"
        prop="account"
        width="120px"
      />
      <el-table-column
        label="姓名"
        prop="userName"
        width="120px"
      />
      <el-table-column
        label="日志类型"
        prop="logType"
        width="100px"
      />
      <el-table-column
        label="日志名称"
        prop="logName"
        width="140px"
      />     
      <el-table-column
        label="状态"
        prop="succeed"
        width="60px"
      />      
      <el-table-column
        width="500px"
        label="类名"
        prop="className"
      />
      <el-table-column
        label="方法名"
        prop="method"
        width="120px"
      />
      <el-table-column
        label="时间"
        prop="createTime"
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

<script src="./opslog.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
