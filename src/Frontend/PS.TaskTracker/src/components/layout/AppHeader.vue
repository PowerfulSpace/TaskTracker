<template>
  <n-layout-header bordered>
    <div class="header-content">
      <router-link to="/" class="brand">
        <img :src="logo" alt="logo" class="logo" />
        <span class="brand-name">Task Tracker</span>
      </router-link>
    </div>
  </n-layout-header>
</template>

<script setup lang="ts">
import { NLayoutHeader } from 'naive-ui'
import logo from '@/assets/icons/logo.svg'
</script>

<style scoped>
.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  height: 64px;
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
  text-decoration: none;
  color: inherit;
}

.logo {
  height: 32px;
  display: block;
}

.brand-name {
  font-size: 18px;
  font-weight: 600;
}
</style>
