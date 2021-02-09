<template>
  <div class="app-container">
    <div class="block">
      <el-row :gutter="20">
        <el-col :span="6">
          <el-input v-model="listQuery.roleName" size="mini" placeholder="请输入角色名称" />
        </el-col>
        <el-col :span="6">
          <el-button type="success" size="mini" icon="el-icon-search" @click.native="search">{{ $t('button.search') }}</el-button>
          <el-button type="primary" size="mini" icon="el-icon-refresh" @click.native="reset">{{ $t('button.reset') }}</el-button>
        </el-col>
      </el-row>
      <br>
      <el-row>
        <el-col :span="24">
          <el-button v-permission="['/role/add']" type="success" size="mini" icon="el-icon-plus" @click.native="add">{{ $t('button.add') }}</el-button>
        </el-col>
      </el-row>
    </div>

    <el-table
      v-loading="listLoading"
      :data="list"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row
      @current-change="handleCurrentChange"
    >

      <el-table-column label="名称">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
      <el-table-column label="描述">
        <template slot-scope="scope">
          {{ scope.row.tips }}
        </template>
      </el-table-column>
      <el-table-column
        fixed="right"
        label="操作"
        width="360"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{row}">
          <el-button v-permission="['/role/edit']" type="primary" size="mini" icon="el-icon-edit" @click="edit(row)">{{ $t('button.edit') }}</el-button>
          <el-button v-permission="['/role/setAuthority']" type="primary" size="mini" icon="el-icon-setting" @click.native="openPermissions(row)">权限配置</el-button>
          <el-button v-permission="['/role/delete']" type="danger" size="mini" icon="el-icon-delete" @click.native="remove(row)">{{ $t('button.delete') }}</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="total, sizes, prev, pager, next, jumper"
      :page-sizes="[10, 20, 50, 100,500]"
      :page-size="listQuery.pageSize"
      :total="total"
      @size-change="changeSize"
      @current-change="fetchPage"
      @prev-click="fetchPrev"
      @next-click="fetchNext"
    />

    <el-dialog
      :title="formTitle"
      :visible.sync="formVisible"
      width="70%"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="80px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="名称" prop="name">
              <el-input v-model="form.name" minlength="1" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="描述" prop="tips">
              <el-input v-model="form.tips" minlength="1" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="排序">
              <el-input v-model="form.ordinal" type="number" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item>
          <el-button type="primary" @click="save">{{ $t('button.submit') }}</el-button>
          <el-button @click.native="formVisible = false">{{ $t('button.cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
    <el-dialog
      title="权限配置"
      :visible.sync="permissonVisible"
      width="50%"
    >
      <el-form>
        <el-row>
          <el-col :span="12">
            <el-tree
              ref="permissonTree"
              :data="permissons"
              show-checkbox
              node-key="id"
              :default-checked-keys="checkedPermissionKeys"
              :props="defaultProps"
            />

          </el-col>
        </el-row>
        <el-form-item>
          <el-button type="primary" @click="savePermissions">{{ $t('button.submit') }}</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>

  </div>
</template>

<script src="./role.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
