<template>
  <div>
    <h2 class="mb-4">Оценка погрешности</h2>

    <form @submit.prevent="estimateErrors" class="mb-4">
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
      </div>
      <button type="submit" class="btn btn-primary mt-3">Оценить</button>
    </form>

    <div v-if="isLoading" class="text-center">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Загрузка...</span>
      </div>
    </div>

    <div v-if="result" class="card">
      <div class="card-body">
        <h5 class="card-title">Результаты оценки погрешности</h5>

        <div class="row">
          <div class="col-md-6">
            <table class="table table-bordered">
              <thead>
              <tr>
                <th>Метод</th>
                <th>Погрешность</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <td>Трапеций</td>
                <td>{{ result.errors.trapezoidal.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Симпсона</td>
                <td>{{ result.errors.simpson.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Гаусса</td>
                <td>{{ result.errors.gaussian.toFixed(6) }}</td>
              </tr>
              </tbody>
            </table>
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

export default {
  data() {
    return {
      form: {
        function: 'x^2',
        a: 0,
        b: 1,
        n: 100
      },
      result: null,
      error: null,
      isLoading: false
    }
  },
  methods: {
    async estimateErrors() {
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
      } catch (err) {
        this.error = err.response?.data?.error || 'Ошибка вычислений'
      } finally {
        this.isLoading = false
      }
    }
  }
}
</script>