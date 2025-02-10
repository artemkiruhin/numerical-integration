<template>
  <div>
    <h2 class="mb-4">Численное интегрирование</h2>

    <form @submit.prevent="calculate" class="mb-4">
      <div class="row g-3">
        <div class="col-md-3">
          <label class="form-label">Функция f(x)</label>
          <input v-model="form.function" type="text" class="form-control" required>
        </div>
        <div class="col-md-3">
          <label class="form-label">Нижний предел (a)</label>
          <input v-model="form.a" type="number" class="form-control" step="any" required>
        </div>
        <div class="col-md-3">
          <label class="form-label">Верхний предел (b)</label>
          <input v-model="form.b" type="number" class="form-control" step="any" required>
        </div>
        <div class="col-md-3">
          <label class="form-label">Количество разбиений (n)</label>
          <input v-model="form.n" type="number" class="form-control" min="1" required>
        </div>
      </div>
      <button type="submit" class="btn btn-primary mt-3">Рассчитать</button>
    </form>

    <div v-if="result" class="card">
      <div class="card-body">
        <h5 class="card-title">Результаты интегрирования</h5>

        <div class="row">
          <div class="col-md-6">
            <table class="table table-bordered">
              <thead>
              <tr>
                <th>Метод</th>
                <th>Результат</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <td>Трапеций</td>
                <td>{{ result.trapezoidalResult.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Симпсона</td>
                <td>{{ result.simpsonResult.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Гаусса</td>
                <td>{{ result.gaussianResult.toFixed(6) }}</td>
              </tr>
              <tr>
                <td>Точное значение</td>
                <td>{{ result.exactValue.toFixed(6) }}</td>
              </tr>
              </tbody>
            </table>
          </div>

          <div class="col-md-6">
            <div class="card">
              <div class="card-header">Погрешности</div>
              <ul class="list-group list-group-flush">
                <li class="list-group-item">
                  Трапеций: {{ result.errors.trapezoidal.toFixed(6) }}
                </li>
                <li class="list-group-item">
                  Симпсона: {{ result.errors.simpson.toFixed(6) }}
                </li>
                <li class="list-group-item">
                  Гаусса: {{ result.errors.gaussian.toFixed(6) }}
                </li>
              </ul>
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
      error: null
    }
  },
  methods: {
    async calculate() {
      try {
        const response = await axios.post('/api/integration/calculate', {
          Function: this.form.function,
          A: parseFloat(this.form.a),
          B: parseFloat(this.form.b),
          N: parseInt(this.form.n)
        })
        this.result = response.data
        this.error = null
      } catch (err) {
        this.error = err.response?.data?.error || 'Ошибка вычислений'
        this.result = null
      }
    }
  }
}
</script>