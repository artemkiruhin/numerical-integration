<template>
  <div>
    <h2 class="mb-4">Сравнение точности методов</h2>

    <div class="mb-4">
      <button v-for="fn in functions" :key="fn.label" @click="selectFunction(fn)" class="btn btn-outline-primary me-2">
        {{ fn.label }}
      </button>
    </div>

    <div v-if="selectedFunction" class="card">
      <div class="card-body">
        <h5 class="card-title">Результаты для {{ selectedFunction.label }}</h5>

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
      functions: [
        { label: 'x^2', function: 'x^2', a: 0, b: 1, n: 100 },
        { label: 'sin(x)', function: 'sin(x)', a: 0, b: Math.PI, n: 100 },
        { label: 'e^x', function: 'e^x', a: 0, b: 1, n: 100 }
      ],
      selectedFunction: null,
      result: null,
      error: null
    }
  },
  methods: {
    async selectFunction(fn) {
      this.selectedFunction = fn
      this.error = null
      try {
        const response = await axios.post('/api/integration/calculate', {
          Function: fn.function,
          A: fn.a,
          B: fn.b,
          N: fn.n
        })
        this.result = response.data
      } catch (err) {
        this.error = err.response?.data?.error || 'Ошибка вычислений'
      }
    }
  }
}
</script>