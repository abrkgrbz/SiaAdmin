document.addEventListener('DOMContentLoaded', function () {
    // İlk yükleme - verileri al
    loadPassiveUsersData();
    loadProfileLifetimeData();

    // DateRangePicker'ları başlat
    $('.date-filter').daterangepicker({
        startDate: moment().subtract(6, 'month'),
        endDate: moment(),
        locale: {
            format: 'DD/MM/YYYY',
            separator: ' - ',
            applyLabel: 'Uygula',
            cancelLabel: 'Vazgeç',
            fromLabel: 'Dan',
            toLabel: 'a',
            customRangeLabel: 'Seç',
            daysOfWeek: ['Pt', 'Sl', 'Çr', 'Pr', 'Cm', 'Ct', 'Pz'],
            monthNames: [
                'Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran',
                'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'
            ],
            firstDay: 1
        },
        ranges: {
            'Bugün': [moment(), moment()],
            'Dün': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Son 7 Gün': [moment().subtract(6, 'days'), moment()],
            'Son 30 Gün': [moment().subtract(29, 'days'), moment()],
            'Bu Ay': [moment().startOf('month'), moment().endOf('month')],
            'Geçen Ay': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'Son 6 Ay': [moment().subtract(6, 'month'), moment()],
            'Bu Yıl': [moment().startOf('year'), moment()]
        }
    });

    // Yenile butonları için olay dinleyicileri
    $('.refresh-btn').on('click', function () {
        const target = $(this).data('target');
        const dateFilter = $(this).prev('.date-filter').val();

        if (!dateFilter) return;

        // Tarih aralığını ayır
        const dates = dateFilter.split(' - ');
        const startDate = moment(dates[0], 'DD/MM/YYYY').format('YYYY-MM-DD');
        const endDate = moment(dates[1], 'DD/MM/YYYY').format('YYYY-MM-DD');

        console.log(`${target} bileşeni ${startDate} - ${endDate} tarih aralığı ile yenileniyor`);

        if (target === 'passive-user-chart' || target === 'total-passive-users' || target === 'max-month') {
            loadPassiveUsersData(startDate, endDate);
        } else if (target === 'profile-lifetime-chart' || target === 'avg-lifetime' || target === 'short-usage-rate') {
            loadProfileLifetimeData(startDate, endDate);
        } else if (target === 'metrics-table') {
            // Tabloyu güncellemek için her iki endpoint'e de ayrı ayrı istek at
            loadPassiveUsersData(startDate, endDate, true);
            loadProfileLifetimeData(startDate, endDate, true);
        }
    });
});

// Pasif kullanıcı verileri için istek
function loadPassiveUsersData(startDate = null, endDate = null, isTableUpdate = false) {
    // Parametreleri hazırla
    const params = {};
    if (startDate && endDate) {
        params.startDate = startDate;
        params.endDate = endDate;
    } else { }

    // AJAX isteği
    $.ajax({
        url: '/passive-users',
        method: 'GET',
        data: params,
        success: function (response) {
            if (response && response.data) { 
                if (isTableUpdate) { 
                    window.passiveUsersData = response.data;
                    updateTableIfBothDataReceived();
                    checkAndUpdateTable();
                } else { 
                    createBarChart(response.data);
                    updatePassiveUsersMetrics(response.data);
                }
            }
        },
        error: function (err) {
            console.error("Pasif kullanıcı verisi alınamadı:", err);
        }
    });
}
 
function loadProfileLifetimeData(startDate = null, endDate = null, isTableUpdate = false) {
    // Parametreleri hazırla
    const params = {};
    if (startDate && endDate) {
        params.startDate = startDate;
        params.endDate = endDate;
    }

    // AJAX isteği
    $.ajax({
        url: '/profile-lifetime',
        method: 'GET',
        data: params,
        success: function (response) {
            if (response && response.data) {
                console.log(response.data)
                if (isTableUpdate) {
                    // Global değişkene kaydet, diğer veriyi bekle
                    window.profileLifetimeData = response.data;
                    updateTableIfBothDataReceived();
                    checkAndUpdateTable();
                } else {
                    // Grafiği ve metrikleri güncelle
                    createLineChart(response.data);
                    updateProfileLifetimeMetrics(response.data);
                }
            }
        },
        error: function (err) {
            console.error("Profil yaşam süresi verisi alınamadı:", err);
        }
    });
}
 
function updateTableIfBothDataReceived() {
    if (window.passiveUsersData && window.profileLifetimeData) {
        updateDetailedTable(window.passiveUsersData, window.profileLifetimeData);
         
    }
}

function checkAndUpdateTable() {
    console.log("Tablo güncellemesi kontrol ediliyor:");
    console.log("Pasif kullanıcı verisi:", window.passiveUsersData ? "Mevcut" : "Eksik");
    console.log("Profil yaşam süresi verisi:", window.profileLifetimeData ? "Mevcut" : "Eksik");

    if (window.passiveUsersData && window.profileLifetimeData) {
        console.log("Her iki veri de mevcut, tablo güncelleniyor...");
        updateDetailedTable(window.passiveUsersData, window.profileLifetimeData);
    } else {
        console.log("Henüz her iki veri de alınmadı, bekleniyor...");
    }
}
 
function updatePassiveUsersMetrics(data) {
    if (!data || data.length === 0) return;

    // Toplam pasif kullanıcı sayısı
    const total = data.reduce((sum, item) => sum + item.Adet, 0);
    $('#total-passive-users').text(total.toLocaleString());

    // En yüksek aya sahip veriyi bul
    const maxMonth = data.reduce((max, item) =>
        item.Adet > max.Adet ? item : max, data[0]);
    $('#max-month').text(maxMonth.LastSeen);
}

 
function updateProfileLifetimeMetrics(data) {
    if (!data || data.length === 0) return;

     
    const total = data.reduce((sum, item) => sum + item.ProfilYasamSaatDegeri, 0);
    const average = Math.round(total / data.length);
    $('#avg-lifetime').text(average);

    // Kısa süreli kullanım oranı (50 saatten az olan profiller)
    const shortUsage = data.filter(item => item.ProfilYasamSaatDegeri < 50).length;
    const shortUsageRate = Math.round((shortUsage / data.length) * 100);
    $('#short-usage-rate').text(`${shortUsageRate}%`);
}

// Detaylı tablo güncelleme
function updateDetailedTable(passiveUsers, lifetimeData) {
 
    // Tabloyu temizle
    const tableBody = $('#metrics-table tbody');
    tableBody.empty();

    // Ayları birleştir ve benzersiz yap
    const months = [...new Set([
        ...passiveUsers.map(item => item.lastSeen),
        ...lifetimeData.map(item => item.lastSeen)
    ])].sort();

    // Her ay için satır ekle
    months.forEach(month => {
        // İlgili veriyi bul
        const passiveData = passiveUsers.find(item => item.lastSeen === month) || { Adet: 0 };
        const lifetimeItem = lifetimeData.find(item => item.lastSeen === month) || { ProfilYasamSaatDegeri: 0 };

        // Durum ve değişim belirle
        let status = 'Orta';
        let statusClass = 'badge-light-warning';

        if (lifetimeItem.profilYasamSaatDegeri > 120) {
            status = 'Çok İyi';
            statusClass = 'badge-light-primary';
        } else if (lifetimeItem.profilYasamSaatDegeri > 100) {
            status = 'İyi';
            statusClass = 'badge-light-success';
        }

        // Değişim yüzdesini hesapla
        // Önceki ayı bulalım (varsa)
        const monthIndex = months.indexOf(month);
        let changePercent = 0;
        let changeClass = 'text-success';
        let changeIcon = '↑';

        if (monthIndex > 0) {
            const prevMonth = months[monthIndex - 1];
            const prevLifetimeData = lifetimeData.find(item => item.lastSeen === prevMonth);

            if (prevLifetimeData) {
                const currentValue = lifetimeItem.profilYasamSaatDegeri;
                const prevValue = prevLifetimeData.profilYasamSaatDegeri;

                if (prevValue > 0) {
                    changePercent = Math.round(((currentValue - prevValue) / prevValue) * 100);

                    if (changePercent < 0) {
                        changeClass = 'text-danger';
                        changeIcon = '↓';
                        changePercent = Math.abs(changePercent);
                    }
                }
            }
        }

        // Satır oluştur
        const row = `
            <tr>
                <td>${month}</td>
                <td>${passiveData.adet.toLocaleString()}</td>
                <td>${Math.round(lifetimeItem.profilYasamSaatDegeri)}</td>
                <td><span class="kt-badge ${statusClass}">${status}</span></td>
                <td><span class="${changeClass}">${changeIcon} ${changePercent}%</span></td>
            </tr>
        `;
        console.log(row)
        tableBody.append(row);
    });
}

// Sütun grafik oluşturma fonksiyonu
function createBarChart(data) {
    // Eğer grafik zaten varsa temizle
    am5.array.each(am5.registry.rootElements, function (root) {
        if (root && root.dom.id === "passive-user-chart") {
            root.dispose();
        }
    });

    // Root element oluştur
    var root = am5.Root.new("passive-user-chart");

    // Tema ayarla
    root.setThemes([am5themes_Animated.new(root)]);

    // Grafik oluştur
    var chart = root.container.children.push(am5xy.XYChart.new(root, {
        panX: false,
        panY: false,
        wheelX: "panX",
        wheelY: "zoomX",
        layout: root.verticalLayout
    }));

    // Eksenleri oluştur
    var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
        categoryField: "lastSeen", // API'den gelen alan adı
        renderer: am5xy.AxisRendererX.new(root, {
            cellStartLocation: 0.1,
            cellEndLocation: 0.9
        }),
        tooltip: am5.Tooltip.new(root, {})
    }));

    xAxis.data.setAll(data);

    var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
        min: 0,
        renderer: am5xy.AxisRendererY.new(root, {})
    }));

    // Seriyi oluştur
    var series = chart.series.push(am5xy.ColumnSeries.new(root, {
        name: "Pasif Kullanıcılar",
        xAxis: xAxis,
        yAxis: yAxis,
        valueYField: "adet", // API'den gelen alan adı
        categoryXField: "lastSeen", // API'den gelen alan adı
        tooltip: am5.Tooltip.new(root, {
            labelText: "{valueY}"
        })
    }));

    series.columns.template.setAll({
        cornerRadiusTL: 5,
        cornerRadiusTR: 5,
        strokeOpacity: 0,
        fillOpacity: 0.8
    });

    // Renk değişimi
    series.columns.template.adapters.add("fill", function (fill, target) {
        return chart.get("colors").getIndex(series.columns.indexOf(target));
    });

    series.columns.template.adapters.add("stroke", function (stroke, target) {
        return chart.get("colors").getIndex(series.columns.indexOf(target));
    });

    // Hover efekti
    series.columns.template.events.on("pointerover", function (event) {
        var target = event.target;
        am5.array.each(series.columns.iterator(), function (column) {
            if (column !== target) {
                column.set("fillOpacity", 0.5);
            }
        });
    });

    series.columns.template.events.on("pointerout", function () {
        am5.array.each(series.columns.iterator(), function (column) {
            column.set("fillOpacity", 0.8);
        });
    });

    // Verileri ayarla
    series.data.setAll(data);

    // İmleci ekle
    chart.set("cursor", am5xy.XYCursor.new(root, {}));

    // Animasyon başlat
    series.appear(1000);
    chart.appear(1000, 100);
}

// Çizgi grafik oluşturma fonksiyonu
function createLineChart(data) {
   
    // Eğer grafik zaten varsa temizle
    am5.array.each(am5.registry.rootElements, function (root) {
        if (root && root.dom.id === "profile-lifetime-chart") {
            root.dispose();
        }
    });

    // Root element oluştur
    var root = am5.Root.new("profile-lifetime-chart");

    // Tema ayarla
    root.setThemes([am5themes_Animated.new(root)]);

    // Grafik oluştur
    var chart = root.container.children.push(am5xy.XYChart.new(root, {
        panX: true,
        panY: true,
        wheelX: "panX",
        wheelY: "zoomX",
        pinchZoomX: true,
        layout: root.verticalLayout
    }));

    // Eksenleri oluştur
    var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
        categoryField: "lastSeen", // API'den gelen alan adı
        renderer: am5xy.AxisRendererX.new(root, {
            minGridDistance: 30
        }),
        tooltip: am5.Tooltip.new(root, {})
    }));

    xAxis.data.setAll(data);

    var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
        min: 0,
        renderer: am5xy.AxisRendererY.new(root, {})
    }));

    // Seriyi oluştur
    var series = chart.series.push(am5xy.LineSeries.new(root, {
        name: "Profil Yaşam Saati",
        xAxis: xAxis,
        yAxis: yAxis,
        valueYField: "profilYasamSaatDegeri", // API'den gelen alan adı
        categoryXField: "lastSeen", // API'den gelen alan adı
        tooltip: am5.Tooltip.new(root, {
            labelText: "{valueY} saat"
        }),
        stroke: am5.color("#7775FF"),
        strokeWidth: 3
    }));

    // Noktaları oluştur
    series.bullets.push(function () {
        return am5.Bullet.new(root, {
            sprite: am5.Circle.new(root, {
                radius: 6,
                fill: am5.color("#7775FF"),
                stroke: am5.color("#fff"),
                strokeWidth: 2
            })
        });
    });

    // Verileri ayarla
    series.data.setAll(data);

    // İmleci ekle
    chart.set("cursor", am5xy.XYCursor.new(root, {
        behavior: "zoomX"
    }));

    // Animasyon başlat
    series.appear(1000);
    chart.appear(1000, 100);
}