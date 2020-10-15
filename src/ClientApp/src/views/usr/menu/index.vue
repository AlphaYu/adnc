<template>
  <div class="app-container">
    <div class="block">
      <el-button type="success" size="mini" icon="el-icon-plus" @click.native="add">{{ $t('button.add') }}</el-button>
    </div>
    <el-table
      :data="data"
      style="width: 100%;margin-bottom: 20px;"
      row-key="id"
      border
      :default-expand-all="false"
      :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
    >
      <el-table-column label="名称">
        <template slot-scope="scope">
          <el-button type="text" @click="edit(scope.row)">{{ scope.row.name }}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="编码">
        <template slot-scope="scope">
          <span>{{ scope.row.code }}</span>
        </template>
      </el-table-column>
      <!--
      <el-table-column label="图标">
        <template slot-scope="scope">
          <svg-icon :icon-class="scope.row.icon" />
        </template>
      </el-table-column>
      -->
      <el-table-column label="组件">
        <template slot-scope="scope">
          <span>{{ scope.row.component }}</span>
        </template>
      </el-table-column>
      <el-table-column label="类型">
        <template slot-scope="scope">
          <span>{{ scope.row.isMenu?'菜单':'按钮' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="URL">
        <template slot-scope="scope">
          <span>{{ scope.row.url }}</span>
        </template>
      </el-table-column>
      <el-table-column label="是否启用">
        <template slot-scope="scope">
          <span>{{ scope.row.statusName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="是否隐藏">
        <template slot-scope="scope">
          <span>{{ scope.row.hidden }}</span>
        </template>
      </el-table-column>
      <el-table-column label="顺序">
        <template slot-scope="scope">
          <span>{{ scope.row.num }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template slot-scope="scope">
          <el-button v-permission="['/menu/edit']" type="primary" size="mini" icon="el-icon-edit" @click="edit(scope.row)">{{ $t('button.edit') }}</el-button>
          <el-button v-permission="['/menu/remove']" type="danger" size="mini" icon="el-icon-delete" @click="remove(scope.row)">{{ $t('button.delete') }}</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog
      :title="formTitle"
      :visible.sync="formVisible"
      width="70%"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="120px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="名称" prop="name">
              <el-input v-model="form.name" minlength="1" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="请求地址" prop="url">
              <el-input v-model="form.url" minlength="1" />
            </el-form-item>
          </el-col>

          <el-col :span="12">
            <el-form-item label="是否是菜单">
              <el-radio-group v-model="form.isMenu">
                <el-radio :label="true">是</el-radio>
                <el-radio :label="false">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="是否启用">
              <el-radio-group v-model="form.status">
                <el-radio :label="true">是</el-radio>
                <el-radio :label="false">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="菜单编号" prop="code">
              <el-input v-model="form.code" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="组件" prop="num">
              <el-input v-model="form.component" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="是否隐藏">
              <el-radio-group v-model="form.hidden">
                <el-radio :label="true">是</el-radio>
                <el-radio :label="false">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="图标">
              <el-input v-model="form.icon" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="排序" prop="num">
              <el-input v-model="form.num" type="number" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="父菜单">
              <el-input
                v-model="form.pname"
                placeholder="请选择父菜单"
                readonly="readonly"
                @click.native="showTree = !showTree"
              />
              <el-tree
                v-if="showTree"
                empty-text="暂无数据"
                :expand-on-click-node="false"
                :data="data"
                :props="defaultProps"
                class="input-tree"
                @node-click="handleNodeClick"
              />

            </el-form-item>
          </el-col>

        </el-row>
        <el-form-item>
          <el-button type="primary" @click="save">{{ $t('button.submit') }}</el-button>
          <el-button @click.native="formVisible = false">{{ $t('button.cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
  </div>
</template>

<script src="./menu.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
