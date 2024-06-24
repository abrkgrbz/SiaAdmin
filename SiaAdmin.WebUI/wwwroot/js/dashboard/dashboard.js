 
$.ajax({
    url: 'dashboard/panelist-sayisi',
    method: 'GET', success: function (data) {

        console.log(data)
        am5.ready(function () {

            var root = am5.Root.new("chartdiv");
            root.setThemes([
                am5themes_Animated.new(root)
            ]);
             
            var chart = root.container.children.push(am5percent.PieChart.new(root, {
                startAngle: 180,
                endAngle: 360,
                layout: root.verticalLayout,
                innerRadius: am5.percent(50),
            }));

            var series = chart.series.push(am5percent.PieSeries.new(root, {
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
                cornerRadius: 2
            });

            series.ticks.template.setAll({
                forceHidden: true
            });


            series.data.setAll([
                { value: data.data[0].adet, category: "Aktif" },
                { value: data.data[1].adet, category: "Pasif" },
            ]);
            series.labels.template.setAll({
                text: "{category}"
            });
            var legend = chart.children.push(am5.Legend.new(root, {
                centerX: am5.percent(50),
                x: am5.percent(50),
                marginTop: 15,
                marginBottom: 15,
            }));
            legend.markerRectangles.template.adapters.add("fillGradient", function () {
                return undefined;
            })
            
            legend.data.setAll(series.dataItems);
            series.appear(1000, 100);

        }); // end am5.ready()
    }, error: function (err) {
        console.log(err)
        alert("Grafik görüntülenemiyor. Bir şeyler yanlış gitti.");
    }
});

$.ajax({
    url: 'dashboard/tanitim-anketi-dolduran-sayisi',
    method: 'GET', success: function (data) {
        console.log(data)
        if (data.data != null) {
            am5.ready(function () {

                var root = am5.Root.new("chartdiv2");
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);

                var chart = root.container.children.push(am5percent.PieChart.new(root, {
                    startAngle: 180,
                    endAngle: 360,
                    layout: root.verticalLayout,
                    innerRadius: am5.percent(50),
                }));

                var series = chart.series.push(am5percent.PieSeries.new(root, {
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


                series.data.setAll([
                    { value: data.data[1].adet, category: "Gecerli" },

                    { value: data.data[2].adet, category: "İptal" },
                    { value: data.data[0].adet, category: "Geçersiz" },

                ]);
                series.labels.template.setAll({
                    text: "{category}"
                });
                var legend = chart.children.push(am5.Legend.new(root, {
                    centerX: am5.percent(50),
                    x: am5.percent(50),
                    marginTop: 15,
                    marginBottom: 15,
                }));
                legend.markerRectangles.template.adapters.add("fillGradient", function () {
                    return undefined;
                })

                legend.data.setAll(series.dataItems);
                series.appear(1000, 100);

            }); // end am5.ready()
        }
       
    }, error: function (err) {
        alert("Unable to display chart. Something went wrong.");
    }
});

$.ajax({
    url: 'dashboard/toplam-anket-bilgisi',
    method: 'GET', success: function (dataList) {
        console.log(dataList)
        am5.ready(function () {

            var root = am5.Root.new("chartdiv3");

            root.setThemes([
                am5themes_Animated.new(root)
            ]);
            var chart = root.container.children.push(am5xy.XYChart.new(root, {
                panX: true,
                panY: true,
                wheelX: "panX",
                wheelY: "zoomX",
                pinchZoomX: true,
                paddingLeft: 0,
                paddingRight: 1
            }));

            var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
            cursor.lineY.set("visible", false);

            var xRenderer = am5xy.AxisRendererX.new(root, {
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
            })

            var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                maxDeviation: 0.3,
                categoryField: "kampanya",
                renderer: xRenderer,
                tooltip: am5.Tooltip.new(root, {})
            }));

            var yRenderer = am5xy.AxisRendererY.new(root, {
                strokeOpacity: 0.1
            })

            var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                maxDeviation: 0.3,
                renderer: yRenderer
            }));

            var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                name: "Series 1",
                xAxis: xAxis,
                yAxis: yAxis,
                valueYField: "adet",
                sequencedInterpolation: true,
                categoryXField: "kampanya",
                tooltip: am5.Tooltip.new(root, {
                    labelText: "{valueY}"
                })
            }));

            series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
            series.columns.template.adapters.add("fill", function (fill, target) {
                return chart.get("colors").getIndex(series.columns.indexOf(target));
            });

            series.columns.template.adapters.add("stroke", function (stroke, target) {
                return chart.get("colors").getIndex(series.columns.indexOf(target));
            });



            xAxis.data.setAll(dataList.data);
            series.data.setAll(dataList.data);

            series.appear(1000);
            chart.appear(1000, 100);

        }); // end am5.ready()

    }, error: function (err) {
        alert("Unable to display chart. Something went wrong.");
    }
});
