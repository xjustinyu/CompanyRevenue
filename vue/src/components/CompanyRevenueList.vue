<script setup>
    import { ref, onMounted } from 'vue'
    import axios from 'axios'

    const companyRevenueList = ref([])
    const allDataLoaded = ref(false)
    let currentPage = 1

    const loadMore = async () => {
        const response = await axios.get(`https://localhost:7017/CompanyRevenue?pageNumber=${currentPage}&pageSize=50`)
        if (response.data.length > 0) {
            companyRevenueList.value.push(...response.data)
            currentPage++
        } else {
            allDataLoaded.value = true
        }
    }

    onMounted(loadMore)
</script>

<template>
    <div class="company-revenue-list">
        <table class="table">
            <thead>
                <tr>
                    <th>出表日期</th>
                    <th>資料年月</th>
                    <th>公司代號</th>
                    <th>公司名稱</th>
                    <th>產業別</th>
                    <th>營業收入-當月營收</th>
                    <th>營業收入-上月營收</th>
                    <th>營業收入-去年當月營收</th>
                    <th>營業收入-上月比較增減(%)</th>
                    <th>營業收入-去年同月增減(%)</th>
                    <th>累計營業收入-當月累計營收</th>
                    <th>累計營業收入-去年累計營收</th>
                    <th>累計營業收入-前期比較增減(%)</th>
                    <th>備註</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in companyRevenueList" :key="item.id">
                    <td>{{ item.reportDate }}</td>
                    <td>{{ item.dataMonth }}</td>
                    <td>{{ item.companyId }}</td>
                    <td>{{ item.companyName }}</td>
                    <td>{{ item.companyType }}</td>
                    <td>{{ item.revenueThisMonth }}</td>
                    <td>{{ item.revenueLastMonth }}</td>
                    <td>{{ item.revenueThisMonthLastYear }}</td>
                    <td>{{ (item.revenueCompareLastMonthPercentage).toFixed(2) }}</td>
                    <td>{{ (item.revenueCompareMonthLastYearPercentage).toFixed(2) }}</td>
                    <td>{{ item.revenueTotalThisMonth }}</td>
                    <td>{{ item.revenueTotalLastYear }}</td>
                    <td>{{ (item.revenueCompareLastPeriodPercentage).toFixed(2) }}</td>
                </tr>
            </tbody>
        </table>
        <button @click="loadMore" v-if="!allDataLoaded">載入更多</button>
    </div>
</template>

<style scoped>
    .company-revenue-list {
        margin: 2px;
    }

    .table {
        width: auto;
        border-collapse: collapse;
        table-layout: auto;        
    }

        .table th,
        .table td {
            padding: 8px;
            border: 1px solid #ccc;
            text-align: center;
            white-space: nowrap; 
            word-break: break-word; 
        }
</style>
