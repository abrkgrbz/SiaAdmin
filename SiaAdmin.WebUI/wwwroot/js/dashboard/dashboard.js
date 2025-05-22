// dashboard.js - Revize edilmiş versiyon
class DashboardManager {
    constructor() {
        this.charts = {};
        this.initialize();
    }

    initialize() {
        // Sayfa yüklendiğinde tüm verileri yükle
        this.loadAllData();
    }

    loadAllData() {
        // Tüm metrikleri yükle
        this.loadMetric('panelist');
        this.loadMetric('survey');
        this.loadMetric('login');
        this.loadMetric('profile');

        // Tüm grafikleri yükle
        this.loadChart('chart1', 'chartdiv');
        this.loadChart('chart2', 'chartdiv2');
        this.loadChart('chart5', 'chartdiv5');
        this.loadChart('chart6', 'chartdiv6');
        this.loadChart('chart3', 'chartdiv3');

        // Anket özetini yükle
        this.loadSurveySummary();
    }

    loadMetric(type, startDate = null, endDate = null) {
        const endpoints = {
            'panelist': '/dashboard/panelist-sayisi',
            'survey': '/dashboard/tanitim-anketi-dolduran-sayisi',
            'login': '/dashboard/son-gorulen-adet',
            'profile': '/dashboard/son-gorulen-saat'
        };

        const url = endpoints[type];
        const params = startDate && endDate ? { startDate, endDate } : {};

        $.ajax({
            url: url,
            method: 'GET',
            data: params,
            success: (data) => {
                this.updateMetricUI(type, data);
            },
            error: (err) => {
                console.error(`${type} metrik verisi alınamadı:`, err);
                alert(`${type} verisi alınamadı. Bir hata oluştu.`);
            }
        });
    }

    loadChart(chartId, divId, startDate = null, endDate = null) {
        let url = '';
        switch (chartId) {
            case 'chart1':
                url = '/dashboard/panelist-sayisi';
                break;
            case 'chart2':
                url = '/dashboard/tanitim-anketi-dolduran-sayisi';
                break;
            case 'chart5':
                url = '/dashboard/son-gorulen-adet';
                break;
            case 'chart6':
                url = '/dashboard/son-gorulen-saat';
                break;
            case 'chart3':
                url = '/dashboard/toplam-anket-bilgisi';
                break;
        }

        const params = startDate && endDate ? { startDate, endDate } : {};

        $.ajax({
            url: url,
            method: 'GET',
            data: params,
            beforeSend: () => {
                this.disposeChart(divId);
            },
            success: (data) => {
                this.createChart(chartId, divId, data);
            },
            error: (err) => {
                console.error(`${chartId} grafik verisi alınamadı:`, err);
                alert(`Grafik verisi alınamadı. Bir hata oluştu.`);
            }
        });
    }

    loadSurveySummary(startDate = null, endDate = null) {
        const params = startDate && endDate ? { startDate, endDate } : {};

        $.ajax({
            url: '/dashboard/toplam-anket-bilgisi',
            method: 'GET',
            data: params,
            success: (data) => {
                this.updateSurveySummaryUI(data);
            },
            error: (err) => {
                console.error('Anket özeti alınamadı:', err);
                alert('Anket özeti alınamadı. Bir hata oluştu.');
            }
        });
    }

    createChart(chartId, divId, data) {
        switch (chartId) {
            case 'chart1':
                this.createPieChart(divId, data, 'panelist');
                break;
            case 'chart2':
                this.createPieChart(divId, data, 'survey');
                break;
            case 'chart5':
            case 'chart6':
            case 'chart3':
                this.createColumnChart(divId, data);
                break;
        }
    }

    createPieChart(divId, data, type) {
        am5.ready(() => {
            const root = am5.Root.new(divId);
            this.charts[divId] = root;

            root.setThemes([am5themes_Animated.new(root)]);

            const chart = root.container.children.push(am5percent.PieChart.new(root, {
                startAngle: 180,
                endAngle: 360,
                layout: root.verticalLayout,
                innerRadius: am5.percent(50),
            }));

            const series = chart.series.push(am5percent.PieSeries.new(root, {
                startAngle: 180,
                endAngle: 360,
                valueField: "value",
                categoryField: "category",
                legendValueText: "{value}",
                alignLabels: false
            }));

            series.states.create("hidden", {
                startAngle: 180,
                endAngle: 180
            });

            series.slices.template.setAll({
                cornerRadius: 3
            });

            series.ticks.template.setAll({
                forceHidden: true
            });

            let chartData = [];
            if (type === 'panelist') {
                chartData = [
                    { value: data.data[0].adet, category: "Aktif" },
                    { value: data.data[1].adet, category: "Pasif" }
                ];
            } else if (type === 'survey') {
                chartData = [
                    { value: data.data[1].adet, category: "Geçerli" },
                    { value: data.data[2].adet, category: "İptal" },
                    { value: data.data[0].adet, category: "Geçersiz" }
                ];
            }

            series.data.setAll(chartData);
            series.labels.template.setAll({
                text: "{category}"
            });

            const legend = chart.children.push(am5.Legend.new(root, {
                centerX: am5.percent(50),
                x: am5.percent(50),
                marginTop: 15,
                marginBottom: 15,
            }));

            legend.markerRectangles.template.adapters.add("fillGradient", () => undefined);
            legend.data.setAll(series.dataItems);
            series.appear(1000, 100);
        });
    }

    createColumnChart(divId, dataList) {
        am5.ready(() => {
            const root = am5.Root.new(divId);
            this.charts[divId] = root;

            root.setThemes([am5themes_Animated.new(root)]);

            const chart = root.container.children.push(am5xy.XYChart.new(root, {
                panX: true,
                panY: true,
                wheelX: "panX",
                wheelY: "zoomX",
                pinchZoomX: true,
                paddingLeft: 0,
                paddingRight: 1
            }));

            const cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
            cursor.lineY.set("visible", false);

            const xRenderer = am5xy.AxisRendererX.new(root, {
                minGridDistance: 30,
                minorGridEnabled: true
            });

            xRenderer.labels.template.setAll({
                rotation: -90,
                centerY: am5.p50,
                centerX: am5.p100,
                paddingRight: 15
            });

            xRenderer.grid.template.setAll({
                location: 1
            });

            const xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                maxDeviation: 0.3,
                categoryField: divId === 'chartdiv3' ? "kampanya" : "lastseen",
                renderer: xRenderer,
                tooltip: am5.Tooltip.new(root, {})
            }));

            const yRenderer = am5xy.AxisRendererY.new(root, {
                strokeOpacity: 0.1
            });

            const yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                maxDeviation: 0.3,
                renderer: yRenderer
            }));

            const series = chart.series.push(am5xy.ColumnSeries.new(root, {
                name: "Series 1",
                xAxis: xAxis,
                yAxis: yAxis,
                valueYField: "adet",
                sequencedInterpolation: true,
                categoryXField: divId === 'chartdiv3' ? "kampanya" : "lastseen",
                tooltip: am5.Tooltip.new(root, {
                    labelText: "{valueY}"
                })
            }));

            series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
            series.columns.template.adapters.add("fill", (fill, target) => {
                return chart.get("colors").getIndex(series.columns.indexOf(target));
            });

            series.columns.template.adapters.add("stroke", (stroke, target) => {
                return chart.get("colors").getIndex(series.columns.indexOf(target));
            });

            xAxis.data.setAll(dataList.data);
            series.data.setAll(dataList.data);

            series.appear(1000);
            chart.appear(1000, 100);
        });
    }

    updateMetricUI(type, data) {
        let value = '';
        switch (type) {
            case 'panelist':
                value = data.data[0].adet + data.data[1].adet;
                $('#total_panelist').text(value);
                break;
            case 'survey':
                value = data.data[1].adet;
                $('#total_survey').text(value);
                break;
            case 'login':
                value = data.data[0].lastseen || '--';
                $('#last_login').text(value);
                break;
            case 'profile':
                value = data.data.reduce((sum, item) => sum + item.adet, 0);
                $('#profile_hours').text(value);
                break;
        }
    }

    updateSurveySummaryUI(data) {
        const total = data.data.reduce((sum, item) => sum + item.adet, 0);
        $('#total_surveys').text(total);
    }

    disposeChart(divId) {
        if (this.charts[divId]) {
            this.charts[divId].dispose();
            delete this.charts[divId];
        }
    }
}

// Global fonksiyonlar
const dashboard = new DashboardManager();

function refreshMetric(type) {
    const dateElement = document.getElementById(`date_${type}`);
    const dateRange = $(dateElement).val();
    if (dateRange) {
        const dates = dateRange.split(' - ');
        const startDate = moment(dates[0], 'DD/MM/YYYY').format('YYYY-MM-DD');
        const endDate = moment(dates[1], 'DD/MM/YYYY').format('YYYY-MM-DD');
        dashboard.loadMetric(type, startDate, endDate);
    } else {
        dashboard.loadMetric(type);
    }
}

function refreshChart(chartId) {
    const dateElement = document.getElementById(`date_${chartId}`);
    const dateRange = $(dateElement).val();
    const divId = chartId === 'chart1' ? 'chartdiv' : chartId === 'chart2' ? 'chartdiv2' :
        chartId === 'chart5' ? 'chartdiv5' : chartId === 'chart6' ? 'chartdiv6' :
            'chartdiv3';

    if (dateRange) {
        const dates = dateRange.split(' - ');
        const startDate = moment(dates[0], 'DD/MM/YYYY').format('YYYY-MM-DD');
        const endDate = moment(dates[1], 'DD/MM/YYYY').format('YYYY-MM-DD');
        dashboard.loadChart(chartId, divId, startDate, endDate);
    } else {
        dashboard.loadChart(chartId, divId);
    }
}

function refreshSurveySummary() {
    const dateElement = document.getElementById('date_survey_summary');
    const dateRange = $(dateElement).val();
    if (dateRange) {
        const dates = dateRange.split(' - ');
        const startDate = moment(dates[0], 'DD/MM/YYYY').format('YYYY-MM-DD');
        const endDate = moment(dates[1], 'DD/MM/YYYY').format('YYYY-MM-DD');
        dashboard.loadSurveySummary(startDate, endDate);
    } else {
        dashboard.loadSurveySummary();
    }
}