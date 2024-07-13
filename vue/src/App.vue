<script setup>
    import CompanyRevenueList from './components/CompanyRevenueList.vue'
</script>

<template>
    <header>
        <h1> 公司營業收入列表 </h1>

    </header>

    <main>
        <CompanyRevenueList />
    </main>
</template>

<style scoped>

    header {
        line-height: 1.5;
        width: 100%;
        text-align: left;
        display: flex;
    }

</style>
