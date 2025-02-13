<template>
  <div class="container mt-4 mb-4">
    <h1 class="text-center mb-4">Калькулятор численного интегрирования</h1>

    <div class="row justify-content-center">
      <div class="col-md-10 col-lg-8">
        <!-- Форма ввода -->
        <div class="card shadow-sm">
          <div class="card-header bg-primary text-white">
            <h5 class="card-title mb-0">Параметры вычислений</h5>
          </div>
          <div class="card-body">
            <form @submit.prevent="calculateIntegration">
              <div class="mb-3">
                <label for="function" class="form-label">Функция f(x):</label>
                <input
                    type="text"
                    class="form-control"
                    id="function"
                    v-model="request.function"
                    placeholder="Пример: x^2 + 2*x + 1"
                    required
                >
                <div class="form-text">Используйте синтаксис JavaScript для математических выражений</div>
              </div>

              <div class="row g-3">
                <div class="col-md-6">
                  <div class="mb-3">
                    <label for="a" class="form-label">Нижний предел (a):</label>
                    <input
                        type="number"
                        class="form-control"
                        id="a"
                        v-model="request.a"
                        step="any"
                        required
                    >
                  </div>
                </div>
                <div class="col-md-6">
                  <div class="mb-3">
                    <label for="b" class="form-label">Верхний предел (b):</label>
                    <input
                        type="number"
                        class="form-control"
                        id="b"
                        v-model="request.b"
                        step="any"
                        required
                    >
                  </div>
                </div>
              </div>

              <div class="mb-4">
                <label for="n" class="form-label">Количество интервалов (n):</label>
                <input
                    type="number"
                    class="form-control"
                    id="n"
                    v-model="request.n"
                    min="1"
                    required
                >
                <div class="form-text">Рекомендуется не менее 100 для сложных функций</div>
              </div>

              <div class="d-grid">
                <button
                    type="submit"
                    class="btn btn-primary btn-lg"
                    :disabled="loading"
                >
                  <i class="bi bi-calculator me-2"></i>
                  {{ loading ? 'Вычисляем...' : 'Рассчитать' }}
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- График -->
        <div v-if="request.function" class="card shadow-sm mt-4">
          <div class="card-header bg-info text-white">
            <h5 class="card-title mb-0">Визуализация функции</h5>
          </div>
          <div class="card-body">
            <IntegrationGraph
                :func="request.function"
                :a="Number(request.a)"
                :b="Number(request.b)"
                :n="Number(request.n)"
            />
          </div>
        </div>

        <!-- Результаты -->
        <div v-if="result" class="card shadow-sm mt-4">
          <div class="card-header bg-success text-white">
            <h5 class="card-title mb-0">Результаты интегрирования</h5>
          </div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-hover align-middle">
                <thead class="table-light">
                <tr>
                  <th>Метод</th>
                  <th>Результат</th>
                  <th>Погрешность</th>
                </tr>
                </thead>
                <tbody>
                <tr>
                  <td><i class="bi bi-pentagon me-2"></i>Метод трапеций</td>
                  <td class="font-monospace">{{ formatNumber(result.trapezoidalResult) }}</td>
                  <td class="font-monospace text-danger">{{ formatNumber(result.errors?.trapezoidalError) }}</td>
                </tr>
                <tr>
                  <td><i class="bi bi-triangle me-2"></i>Метод Симпсона</td>
                  <td class="font-monospace">{{ formatNumber(result.simpsonResult) }}</td>
                  <td class="font-monospace text-danger">{{ formatNumber(result.errors?.simpsonError) }}</td>
                </tr>
                <tr>
                  <td><i class="bi bi-circle me-2"></i>Метод Гаусса</td>
                  <td class="font-monospace">{{ formatNumber(result.gaussianResult) }}</td>
                  <td class="font-monospace text-danger">{{ formatNumber(result.errors?.gaussianError) }}</td>
                </tr>
                <tr class="table-primary">
                  <td colspan="2"><strong>Точное значение:</strong></td>
                  <td class="font-monospace fw-bold">{{ formatNumber(result.exactValue) }}</td>
                </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <!-- Ошибки -->
        <div
            v-if="error"
            class="alert alert-danger d-flex align-items-center mt-4 shadow"
            role="alert"
        >
          <i class="bi bi-exclamation-triangle-fill me-3 fs-4"></i>
          <div>{{ error }}</div>
          <button
              type="button"
              class="btn-close ms-auto"
              @click="error = null"
          ></button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import IntegrationGraph from "@/components/IntegrationGraph.vue";

export default {
  name: 'App',
  components: {IntegrationGraph},
  data() {
    return {
      request: {
        function: '',
        a: 0,
        b: 1,
        n: 100
      },
      result: null,
      error: null,
      loading: false
    }
  },
  methods: {
    async calculateIntegration() {
      this.loading = true;
      this.error = null;
      this.result = null;

      try {
        const response = await axios.post('http://localhost:5018/api/Integration/calculate', this.request);
        this.result = response.data;
      } catch (err) {
        this.error = err.response?.data?.error || 'An error occurred while calculating the integration';
      } finally {
        this.loading = false;
      }
    },
    formatNumber(num) {
      if (num === undefined || num === null) return '-';
      return Number(num).toExponential(6);
    }
  }
}
</script>

<style>
.card {
  border-radius: 1rem;
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-2px);
}

.table-hover tr:hover {
  background-color: rgba(0, 123, 255, 0.05);
}

.font-monospace {
  font-family: 'Fira Code', monospace;
}

.alert {
  border-radius: 0.75rem;
}

.form-control:focus {
  box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
}
</style>