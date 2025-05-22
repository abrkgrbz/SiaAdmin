 
document.addEventListener('DOMContentLoaded', function () {
    // Tarih aralığı seçicileri başlat
    const dateInputs = [
        'date_panelist',
        'date_survey',
        'date_login',
        'date_profile',
        'date_chart1',
        'date_chart2',
        'date_chart5',
        'date_chart6',
        'date_chart3'
    ];

    dateInputs.forEach(inputId => {
        const element = document.getElementById(inputId);
        if (element) {
            initializeDateRangePicker(element, inputId);
        }
    });
});

function initializeDateRangePicker(element, inputId) {
    var start = moment().subtract(29, 'days');
    var end = moment();

    $(element).daterangepicker({
        startDate: start,
        endDate: end,
        autoUpdateInput: false, // Input otomatik olarak güncellenmesin
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Uygula",
            "cancelLabel": "Vazgeç",
            "fromLabel": "Dan",
            "toLabel": "a",
            "customRangeLabel": "Seç",
            "daysOfWeek": [
                "Pt",
                "Sl",
                "Çr",
                "Pr",
                "Cm",
                "Ct",
                "Pz"
            ],
            "monthNames": [
                "Ocak",
                "Şubat",
                "Mart",
                "Nisan",
                "Mayıs",
                "Haziran",
                "Temmuz",
                "Ağustos",
                "Eylül",
                "Ekim",
                "Kasım",
                "Aralık"
            ],
            "firstDay": 1
        },
        "ranges": {
            'Bugün': [moment(), moment()],
            'Dün': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Son 7 Gün': [moment().subtract(6, 'days'), moment()],
            'Son 30 Gün': [moment().subtract(29, 'days'), moment()],
            'Bu Ay': [moment().startOf('month'), moment().endOf('month')],
            'Geçen Ay': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'Bu Yıl': [moment().startOf('year'), moment()],
            'Geçen Yıl': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        }
    });

    // Apply event handler
    $(element).on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));

        // Otomatik yenile
        const type = inputId.replace('date_', '');
        if (inputId.includes('chart')) {
            refreshChart(type);
        } else {
            refreshMetric(type);
        }
    });

    // Cancel event handler  
    $(element).on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });
}
 