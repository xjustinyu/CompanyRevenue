<script setup>
    import { ref, onMounted, watch, computed } from 'vue'
    import axios from 'axios'

    const companyRevenueList = ref([])
    const totalCount = ref(0)
    const pageSize = ref(50)
    const currentPage = ref(1)
    const totalPages = computed(() => Math.ceil(totalCount.value / pageSize.value))
    let oldPageSize = pageSize.value

    const getPageData = async () => {
        const response = await axios.get(`https://localhost:7017/CompanyRevenue?pageNumber=${currentPage.value}&pageSize=${pageSize.value}`)
        if (response.data.companyRevenueList.length > 0) {
            companyRevenueList.value = [...response.data.companyRevenueList]
            totalCount.value = response.data.totalCount
        }
    }

    const nextPage = () => {
        if (currentPage.value * pageSize.value < totalCount.value) {
            currentPage.value++
        }
    }

    const prevPage = () => {
        if (currentPage.value > 1) {
            currentPage.value--
        }
    }

    watch([currentPage, pageSize], () => {
        //當前頁數 > 最大頁數
        if (currentPage.value > totalPages.value) {
            currentPage.value = totalPages.value
        }

        //單頁筆數 > 總筆數
        if (pageSize.value >= totalCount.value) {
            pageSize.value = totalCount.value
        }

        //單頁筆數有變動時修改當前頁數為1
        if (pageSize.value !== oldPageSize) {
            currentPage.value = 1
            oldPageSize = pageSize.value
        }

        getPageData()
    })

    onMounted(getPageData)
</script>

<template>
    <div class="company-revenue-list">
        <div>
            總數: {{ totalCount }}
            單頁筆數: <input type="number" min="1" :max="totalCount" v-model.number="pageSize" />
            總頁數: {{totalPages}}
        </div>
        <div>
            <button @click="prevPage" :disabled="currentPage === 1">←</button>
            當前頁數: <input type="number" min="1" :max="Math.ceil(totalCount / pageSize)" v-model.number="currentPage" />
            <button @click="nextPage" :disabled="currentPage * pageSize >= totalCount">→</button>
        </div>
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
        <!--<button @click="loadMore" v-if="!allDataLoaded">載入更多</button>-->
    </div>
</template>

<style scoped>
    .company-revenue-list {
        margin: 2px;
    }

    .table {
        width: 100%;
        border-collapse: collapse;
        table-layout: auto;
    }

        .table th {
            border: 1px solid #ccc;
            font-size: 1vw;
            background-color: white;
            position: sticky;
            top: 0;
        }
        .table td {
            padding: 8px;
            border: 1px solid #ccc;
            text-align: center;
            white-space: nowrap;
            word-break: break-word;
            font-size: 1.2vw;
        }
</style>
