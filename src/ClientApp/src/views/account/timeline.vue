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
        <p><span class="title"><i class="el-icon-open" />&nbsp;&nbsp;{{ user.status == '1'?'启用' : '禁用' }}</span></p>
        <p><span class="title"><i class="el-icon-location-outline" />&nbsp;&nbsp;</span></p>
      </el-col>
      <el-col :span="18" style="padding-left:10px;">
        <el-tabs v-model="activeName" @tab-click="handleClick">
          <el-tab-pane label="个人资料" name="profile" />
          <el-tab-pane label="最近活动" name="timeline" />
          <el-tab-pane label="修改密码" name="updatePwd" />
        </el-tabs>
        <el-timeline :reverse="reverse">
          <el-timeline-item
            v-for="(activity, index) in activities"
            :key="index"
            :timestamp="activity.createTime"
          >
            {{ activity.logName }}
          </el-timeline-item>
        </el-timeline>
      </el-col>
    </el-row>

  </div>
</template>

<script src="./timeline.js"></script>

<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/common.scss";
</style>

