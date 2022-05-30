<template>
  <div
    class="login-container"
  >
    <el-form
      ref="loginForm"
      :model="loginForm"
      :rules="loginRules"
      class="login-form"
      auto-complete="on"
      label-position="left"
    >
      <div class="login-box">
        <div class="title-container">
          <h3 class="title">{{ $t("login.title") }}</h3>
          <lang-select class="set-language" />
        </div>

        <el-form-item prop="username">
          <span class="svg-container">
            <svg-icon icon-class="user" class-name="custom-class" />
          </span>
          <el-input
            v-model="loginForm.username"
            :placeholder="$t('login.username')"
            name="username"
            type="text"
            auto-complete="on"
          />
        </el-form-item>

        <el-form-item prop="password">
          <span class="svg-container">
            <svg-icon icon-class="password" class-name="custom-class" />
          </span>
          <el-input
            v-model="loginForm.password"
            :type="pwdType"
            :placeholder="$t('login.password')"
            name="password"
            auto-complete="on"
            @keyup.enter.native="handleLogin"
          />
          <span class="show-pwd" @click="showPwd">
            <svg-icon icon-class="eye" />
          </span>
        </el-form-item>

        <el-button
          :loading="loading"
          type="primary"
          style="width: 100%; margin-bottom: 30px"
          @click.native.prevent="handleLogin"
        >{{ $t("login.logIn") }}</el-button>

        <div style="position:relative">
          <div class="tips">
            <span>{{ $t('login.username') }} : alpha2008</span>
            <span>{{ $t('login.password') }} : alpha2008</span>
          </div>
        </div>
      </div>

    </el-form>
  </div>
</template>

<script src="./login.js"></script>
<style rel="stylesheet/scss" lang="scss" src="./login.scss"></style>
