var start = moment().subtract(29, 'days');
var end = moment();

$("#kt_daterangepicker_1").daterangepicker({
    startDate: start,
    endDate: end,
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
        'Bu Yıl': [moment().startOf('year'), moment().endOf('month')],
        'Geçen Yıl': [moment().startOf('year').format('01/01/2023'), moment().startOf('year') - 1],

    }
});