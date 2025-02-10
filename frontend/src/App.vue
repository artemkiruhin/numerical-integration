<template>
  <div class="container mt-4">
    <nav class="nav nav-tabs mb-4">
      <router-link to="/" class="nav-link">Методы интегрирования</router-link>
      <router-link to="/compare" class="nav-link">Сравнение методов</router-link>
      <router-link to="/error-estimation" class="nav-link">Оценка погрешности</router-link>
      <router-link to="/simple-functions" class="nav-link">Простые функции</router-link>
    </nav>

    <router-view></router-view>
  </div>
</template>