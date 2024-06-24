var KTDatatablesServerSide = function () {
    var table;
    var dt;
    var filterPayment;
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "blocklu-kullanici-listesi/LoadTable",
                type: "POST",

            },

            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[0, "asc"]],
            stateSave: true,
            info: false,
            responsive: true,
            language: {
                url: '//cdn.datatables.net/plug-ins/2.0.1/i18n/tr.json',
            },

            columns: [

                { data: "data", name: "Data" },
                { data: "note", name: "Not"  },
                { data: "active", name: "Aktif"  },
                { data: "timestamp", name: "Zaman"  }
            ],
            columnDefs: [ 
                {
                    targets: 2,
                    render: function (data, type, row) {
                        if (data == 1) {
                            return `<div class="badge badge-light-success" >Aktif</div>`
                        } else {
                            return `<div class="badge badge-light-danger" >Pasif</div>`
                        }
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        moment.locale("tr");
                        return moment(data).format('lll');
                        searchable: false
                    },
                },
                 
            ]

        });

        table = dt.$;

        dt.on('draw', function () {
            KTMenu.createInstances();
        });
    }
    
    return {
        init: function () {
            initDatatable(); 
        }
    }
}();

KTUtil.onDOMContentLoaded(function () {
    KTDatatablesServerSide.init();
});
