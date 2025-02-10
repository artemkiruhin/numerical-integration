<template>
  <div>
    <h2 class="mb-4">Сравнение методов интегрирования</h2>

    <form @submit.prevent="compareMethods" class="mb-4">
      <div class="row g-3">
        <div class="col-md-4">
          <label class="form-label">Функция f(x)</label>
          <input v-model="form.function" type="text" class="form-control" required>
        </div>
        <div class="col-md-2">
          <label class="form-label">Нижний предел (a)</label>
          <input v-model="form.a" type="number" class="form-control" step="any" required>
        </div>
        <div class="col-md-2">
          <label class="form-label">Верхний предел (b)</label>
          <input v-model="form.b" type="number" class="form-control" step="any" required>
        </div>
        <div class="col-md-2">
          <label class="form-label">Количество разбиений (n)</label>
          <input v-model="form.n" type="number" class="form-control" min="1" required>
        </div>
        <div class="col-md-2">
          <label class="form-label">Метод сравнения</label>
          <select v-model="form.method" class="form-select" required>
            <option value="trapezoidal">Трапеций</option>
            <option value="simpson">Симпсона</option>
            <option value="gaussian">Гаусса</option>
          </select>
        </div>
      </div>
      <button type="submit" class="btn btn-primary mt-3">Сравнить</button>
    </form>

    <div v-if="isLoading" class="text-center">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Загрузка...</span>
      </div>
    </div>

    <div v-if="result" class="card">
      <div class="card-body">
        <h5 class="card-title">Результаты сравнения</h5>

        <div class="row">
          <div class="col-md-6">
            <table class="table table-bordered">
              <thead>
              <tr>
                <th>Метод</th>
                <th>Результат</th>
                <th>Погрешность</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <td>Трапеций</td>
                <td>{{ result.trapezoidalResult.toFixed(6) }}</td>
                <td>{{ result.errors.trapezoidal.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Симпсона</td>
                <td>{{ result.simpsonResult.toFixed(6) }}</td>
                <td>{{ result.errors.simpson.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Гаусса</td>
                <td>{{ result.gaussianResult.toFixed(6) }}</td>
                <td>{{ result.errors.gaussian.toFixed(6) }}</td>
              </tr>
              </tbody>
            </table>
          </div>

          <div class="col-md-6">
            <div class="card">
              <div class="card-header">График погрешностей</div>
              <div class="card-body">
                <canvas ref="errorChart"></canvas>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="error" class="alert alert-danger mt-3">
      {{ error }}
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { Chart, registerables } from 'chart.js'

Chart.register(...registerables)

export default {
  data() {
    return {
      form: {
        function: 'x^2',
        a: 0,
        b: 1,
        n: 100,
        method: 'trapezoidal'
      },
      result: null,
      error: null,
      isLoading: false,
      chart: null
    }
  },
  methods: {
    async compareMethods() {
      this.isLoading = true
      this.error = null
      try {
        const response = await axios.post('/api/integration/calculate', {
          Function: this.form.function,
          A: parseFloat(this.form.a),
          B: parseFloat(this.form.b),
          N: parseInt(this.form.n)
        })
        this.result = response.data
        this.renderChart()
      } catch (err) {
        this.error = err.response?.data?.error || 'Ошибка вычислений'
      } finally {
        this.isLoading = false
      }
    },
    renderChart() {
      if (this.chart) this.chart.destroy()
      const ctx = this.$refs.errorChart.getContext('2d')
      this.chart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: ['Трапеций', 'Симпсона', 'Гаусса'],
          datasets: [{
            label: 'Погрешность',
            data: [
              this.result.errors.trapezoidal,
              this.result.errors.simpson,
              this.result.errors.gaussian
            ],
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
          }]
        },
        options: {
          responsive: true,
          scales: {
            y: {
              beginAtZero: true
            }
          }
        }
      })
    }
  }
}
</script>