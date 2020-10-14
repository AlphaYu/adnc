<template>
  <div class="app-container">
    <div class="user">
      <strong>{{ user.name }}</strong><br>
      <small>{{ user.deptFullName }} &nbsp;&nbsp; {{ user.roles }}</small>
    </div>
    <el-row class="user-content" style="">
      <el-col :span="6" class="profile">
        <img class="user-avatar" src="@/assets/img/avatar.gif"><br>
        <p><span class="title"><i class="el-icon-phone" />&nbsp;&nbsp;{{ user.phone }}</span></p>
        <p><span class="title"><i class="el-icon-message" />&nbsp;&nbsp;{{ user.email }}</span></p>
        <p><span class="title"><i class="el-icon-open" />&nbsp;&nbsp;{{ user.status == 1?'启用' : '禁用' }}</span></p>
        <p><span class="title"><i class="el-icon-location-outline" />&nbsp;&nbsp;</span></p>
      </el-col>
      <el-col :span="18" style="padding-left:10px;">
        <el-tabs v-model="activeName" @tab-click="handleClick">
          <el-tab-pane label="个人资料" name="profile" />
          <el-tab-pane label="最近活动" name="timeline" />
          <el-tab-pane label="修改密码" name="updatePwd" />
        </el-tabs>
        <el-form ref="form" :model="form" label-width="150px" :rules="formRules">

          <el-row>
            <el-col :span="24">
              <el-form-item label="原密码" prop="oldPassword">
                <el-input v-model="form.oldPassword" type="password" />
              </el-form-item>
            </el-col>
            <el-col :span="24">
              <el-form-item label="新密码" prop="password">
                <el-input v-model="form.password" type="password" minlength="5" />
              </el-form-item>
            </el-col>

            <el-col :span="24">
              <el-form-item label="重复密码" prop="rePassword">
                <el-input v-model="form.rePassword" type="password" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item>
            <el-button type="primary" @click="updatePwd">{{ $t('button.submit') }}</el-button>
          </el-form-item>
        </el-form>
      </el-col>
    </el-row>

  </div>
</template>

<script src="./updatePwd.js"></script>

<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>

