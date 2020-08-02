<template>
  <div class="app-container">
    <div class="block">
      <el-row :gutter="20">
        <el-col :span="6">
          <el-input v-model="listQuery.account" size="mini" placeholder="请输入帐号" />
        </el-col>
        <el-col :span="6">
          <el-input v-model="listQuery.name" size="mini" placeholder="请输入姓名" />
        </el-col>
        <el-col :span="6">
          <el-button type="success" size="mini" icon="el-icon-search" @click.native="search">{{ $t('button.search') }}</el-button>
          <el-button type="primary" size="mini" icon="el-icon-refresh" @click.native="reset">{{ $t('button.reset') }}</el-button>
        </el-col>
      </el-row>
      <br>
      <el-row>
        <el-col :span="24">
          <el-button v-permission="['/user/add']" type="success" size="mini" icon="el-icon-plus" @click.native="add">
            {{ $t('button.add') }}
          </el-button>
          <el-button v-permission="['/user/freeze']" type="primary" size="mini" icon="el-icon-delete" @click.native="changeUserStatusBatch(1)">
            {{ $t('button.enable') }}
          </el-button>
          <el-button v-permission="['/user/freeze']" type="danger" size="mini" icon="el-icon-delete" @click.native="changeUserStatusBatch(2)">
            {{ $t('button.disable') }}
          </el-button>
        </el-col>
      </el-row>
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
      @current-change="handleCurrentChange"
      @selection-change="handleSelectionChange"
    >
      <el-table-column type="selection" width="50" />
      <el-table-column label="账号">
        <template slot-scope="scope">
          {{ scope.row.account }}
        </template>
      </el-table-column>
      <el-table-column label="姓名">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
      <el-table-column label="性别">
        <template slot-scope="scope">
          {{ scope.row.sexName }}
        </template>
      </el-table-column>
      <el-table-column label="角色">
        <template slot-scope="scope">
          {{ scope.row.roleName }}
        </template>
      </el-table-column>
      <el-table-column label="部门">
        <template slot-scope="scope">
          {{ scope.row.deptName }}
        </template>
      </el-table-column>
      <el-table-column label="邮箱">
        <template slot-scope="scope">
          {{ scope.row.email }}
        </template>
      </el-table-column>
      <el-table-column label="电话">
        <template slot-scope="scope">
          {{ scope.row.phone }}
        </template>
      </el-table-column>
      <el-table-column label="创建时间">
        <template slot-scope="scope">
          {{ scope.row.createTime }}
        </template>
      </el-table-column>
      <el-table-column label="状态">
        <template slot-scope="scope">
          <!--
            <el-tag :type="row.statusName === '启用' ? 'success' : 'danger'">
            {{ row.statusName }}
          </el-tag>
          -->
          <el-switch v-model="scope.row.status==1" @change="changeUserStatus(scope.row)" />
        </template>
      </el-table-column>
      <el-table-column
        fixed="right"
        label="操作"
        width="180"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{row}">
          <el-button v-permission="['/user/edit']" type="primary" size="mini" @click="edit(row)">编辑</el-button>
          <el-button v-permission="['/user/setRole']" type="success" size="mini" @click="openRole(row)">角色分配</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="total, sizes, prev, pager, next, jumper"
      :page-sizes="[5, 10, 20, 50, 100,500]"
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
      <el-form ref="form" :model="form" :rules="rules" label-width="120px" label-position="right">
        <el-row>
          <el-col :span="12">
            <el-form-item label="账户" prop="account">
              <el-input v-model="form.account" minlength="1" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="姓名" prop="name">
              <el-input v-model="form.name" minlength="1" />
            </el-form-item>
          </el-col>

          <el-col :span="12">
            <el-form-item label="性别">
              <el-radio-group v-model="form.sex">
                <el-radio :label="1">男</el-radio>
                <el-radio :label="2">女</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="邮箱" prop="email">
              <el-input v-model="form.email" />
            </el-form-item>
          </el-col>
          <el-col v-show="isAdd" :span="12">
            <el-form-item label="密码" prop="password">
              <el-input v-model="form.password" type="password" />
            </el-form-item>
          </el-col>
          <el-col v-show="isAdd" :span="12">
            <el-form-item label="确认密码" prop="rePassword">
              <el-input v-model="form.rePassword" type="password" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="电话" prop="phone">
              <el-input v-model="form.phone" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="所属部门">
              <el-input
                v-model="form.deptName"
                placeholder="请选择所属部门"
                readonly="readonly"
                @click.native="deptTree.show = !deptTree.show"
              />
              <el-tree
                v-if="deptTree.show"
                empty-text="暂无数据"
                :expand-on-click-node="false"
                :data="deptTree.data"
                :props="deptTree.defaultProps"
                class="input-tree"
                @node-click="handleNodeClick"
              />

            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="是否启用" prop="status">
              <el-switch v-model="form.status" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="出生日期">
              <el-date-picker v-model="form.birthday" type="date" placeholder="选择日期" style="width: 100%;" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item>
          <el-button type="primary" @click="saveUser">{{ $t('button.submit') }}</el-button>
          <el-button @click.native="formVisible = false">{{ $t('button.cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>

    <el-dialog
      title="角色分配"
      :visible.sync="roleDialog.visible"
      width="50%"
    >
      <el-form>
        <el-row>
          <el-col :span="12">
            <el-tree
              ref="roleTree"
              :data="roleDialog.roles"
              show-checkbox
              node-key="id"
              :default-checked-keys="roleDialog.checkedRoleKeys"
              :props="roleDialog.defaultProps"
            />

          </el-col>
        </el-row>
        <el-form-item>
          <el-button type="primary" @click="setRole(roleDialog.id)">{{ $t('button.submit') }}</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
  </div>
</template>

<script src="./user.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>

