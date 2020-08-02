<template>
  <div class="app-container">
    <div class="block">
      <el-row  :gutter="20">
        <el-col :span="6">
          <el-input v-model="listQuery.title" size="mini" placeholder="标题"></el-input>
        </el-col>
        <el-col :span="6">
          <el-input v-model="listQuery.author" size="mini" placeholder="作者"></el-input>
        </el-col>
        <el-col :span="8">
          <el-date-picker
            v-model="rangeDate"
            size="mini"
            type="datetimerange"
            :picker-options="pickerOptions"
            range-separator="至"
            start-placeholder="发布起始日期"
            end-placeholder="发布截至日期"
            value-format="yyyyMMddHHmmss"
            align="right">
          </el-date-picker>
        </el-col>
        <el-col :span="4">
          <el-button type="success" size="mini" icon="el-icon-search" @click.native="search">{{ $t('button.search') }}</el-button>
          <el-button type="primary" size="mini" icon="el-icon-refresh" @click.native="reset">{{ $t('button.reset') }}</el-button>
        </el-col>
      </el-row>
      <br>
      <el-row>
        <el-col :span="24">
          <el-button type="success" size="mini" icon="el-icon-plus" @click.native="add">{{ $t('button.add') }}</el-button>
          <el-button type="primary" size="mini"  icon="el-icon-edit" @click.native="edit">{{ $t('button.edit') }}</el-button>
          <el-button type="danger" size="mini"  icon="el-icon-delete" @click.native="remove">{{ $t('button.delete') }}</el-button>
        </el-col>
      </el-row>
    </div>


    <el-table :data="list" v-loading="listLoading" element-loading-text="Loading" border fit highlight-current-row
              @current-change="handleCurrentChange">

      <el-table-column label="ID">
        <template slot-scope="scope">
          {{scope.row.id}}
        </template>
      </el-table-column>
      <el-table-column label="标题">
        <template slot-scope="scope">
          {{scope.row.title}}
        </template>
      </el-table-column>
      <el-table-column label="发布日期">
        <template slot-scope="scope">
          {{scope.row.createTime}}
        </template>
      </el-table-column>
      <el-table-column label="作者">
        <template slot-scope="scope">
          {{scope.row.author}}
        </template>
      </el-table-column>
      <el-table-column label="文章配图">
        <template slot-scope="scope">
          <img :src="scope.row.img" style="width:200px;">
        </template>
      </el-table-column>

    </el-table>

    <el-pagination
      background
      layout="total, sizes, prev, pager, next, jumper"
      :page-sizes="[10, 20, 50, 100,500]"
      :page-size="listQuery.limit"
      :total="total"
      @size-change="changeSize"
      @current-change="fetchPage"
      @prev-click="fetchPrev"
      @next-click="fetchNext">
    </el-pagination>
  </div>
</template>

<script src="./article.js"></script>


<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
