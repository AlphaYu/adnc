<template>
  <div class="app-container">
    <div class="block">
      <el-row :gutter="20">
        <el-col :span="6">
          <el-input v-model="listQuery.name" size="mini" placeholder="请输入字典名称"></el-input>
        </el-col>
        <el-col :span="6">
          <el-button type="success" size="mini" icon="el-icon-search" @click.native="search">{{ $t('button.search') }}</el-button>
          <el-button type="primary" size="mini" icon="el-icon-refresh" @click.native="reset">{{ $t('button.reset') }}</el-button>
        </el-col>
      </el-row>
      <br>
      <el-row>
        <el-col :span="24">
          <el-button type="success" size="mini" icon="el-icon-plus" @click.native="add">{{ $t('button.add') }}</el-button>
        </el-col>
      </el-row>
    </div>
    <el-table :data="list" v-loading="listLoading" element-loading-text="Loading" border fit highlight-current-row
              @current-change="handleCurrentChange"
    >
      <el-table-column label="ID">
        <template slot-scope="scope">
          {{scope.row.id}}
        </template>
      </el-table-column>
      <el-table-column label="名称">
        <template slot-scope="scope">
          {{scope.row.name}}
        </template>
      </el-table-column>
      <el-table-column label="详情">
        <template slot-scope="scope">
          {{scope.row.detail}}
        </template>
      </el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" icon="el-icon-edit" @click.native="editItem(scope.row)">{{ $t('button.edit') }}</el-button>
          <el-button type="danger" size="mini" icon="el-icon-delete" @click.native="removeItem(scope.row)">{{ $t('button.delete') }}</el-button>
        </template>

      </el-table-column>

    </el-table>
    <el-dialog
      :title="formTitle"
      :visible.sync="formVisible"
      width="60%">
      <el-form ref="form" :model="form" :rules="rules" label-width="120px">

        <el-form-item label="名称" prop="name">
          <el-input v-model="form.name"  minlength=1></el-input>
        </el-form-item>
        <el-form-item
          v-for="(rec, index) in form.details"
          :label="'字典' + (index+1)"
          :key="rec.key"
          :prop="'details.' + index + '.value'"
          :rules="{
            required: true, message: '不能为空', trigger: 'blur'
          }"
        >
          <el-col :span="9">
          <el-input v-model="rec.key" placeholder="值"></el-input>
          </el-col>
          <el-col class="line" :span="1">&nbsp; </el-col>
          <el-col :span="9">
          <el-input v-model="rec.value" placeholder="名称"></el-input>
          </el-col>
          <el-col :span="4">&nbsp;
          <el-button @click.prevent="removeDetail(rec)" type="danger" icon="el-icon-delete" >{{ $t('button.delete')
            }}</el-button>
          </el-col>
        </el-form-item>

        <el-form-item>
          <el-button type="primary" @click="save">{{ $t('button.submit') }}</el-button>
          <el-button @click="addDetail">新增字典</el-button>
          <el-button @click.native="formVisible = false">{{ $t('button.cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>

  </div>
</template>

<script src="./dict.js"></script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>
