<template>
  <Suspense>
    <template #default>
      <router-view />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
</template>

<script setup lang="ts">

</script>
