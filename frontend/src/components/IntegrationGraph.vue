<!-- components/IntegrationGraph.vue -->
<template>
  <div class="chart-container">
    <Line
        v-if="chartData"
        :data="chartData"
        :options="chartOptions"
    />
  </div>
</template>

<script>
import { Line } from 'vue-chartjs';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
} from 'chart.js';
import * as math from 'mathjs';

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

export default {
  name: 'IntegrationGraph',
  components: { Line },
  props: {
    func: {
      type: String,
      required: true
    },
    a: {
      type: Number,
      required: true
    },
    b: {
      type: Number,
      required: true
    },
    n: {
      type: Number,
      required: true
    }
  },
  data() {
    return {
      chartData: null,
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        animation: {
          duration: 0
        },
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Function Graph'
          }
        },
        scales: {
          x: {
            type: 'linear',
            display: true,
            title: {
              display: true,
              text: 'x'
            }
          },
          y: {
            display: true,
            title: {
              display: true,
              text: 'f(x)'
            }
          }
        }
      }
    }
  },
  watch: {
    func: 'updateChart',
    a: 'updateChart',
    b: 'updateChart',
    n: 'updateChart'
  },
  mounted() {
    this.updateChart();
  },
  methods: {
    evaluateFunction(x) {
      try {
        return math.evaluate(this.func, { x });
      } catch (error) {
        console.error('Error evaluating function:', error);
        return 0;
      }
    },
    updateChart() {
      const points = [];
      const numPoints = Math.max(200, this.n * 2); // Use at least 200 points for smooth plotting
      const step = (this.b - this.a) / numPoints;

      for (let i = 0; i <= numPoints; i++) {
        const x = this.a + i * step;
        const y = this.evaluateFunction(x);
        points.push({ x, y });
      }

      this.chartData = {
        datasets: [
          {
            label: 'f(x)',
            data: points,
            borderColor: '#8884d8',
            backgroundColor: 'rgba(136, 132, 216, 0.5)',
            tension: 0.4,
            pointRadius: 0
          }
        ]
      };
    }
  }
}
</script>

<style scoped>
.chart-container {
  position: relative;
  height: 400px;
  width: 100%;
}
</style>