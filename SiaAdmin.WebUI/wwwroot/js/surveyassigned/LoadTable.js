var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "proje-atama-listesi/LoadTable",
                type: "POST"
            },
            language: {
                url: '/assets/customjs/turkish.json',

            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[0, "asc"]],
            stateSave: true,


            columns: [
                { data: "id", name: "Id", width: "5%" },
                { data: "surveyId", name: "SurveyId", },
                { data: "surveyUserGuid", name: "SurveyUserGuid" },
                { data: "tarih", name: "Tarih" },
                { data: "timestamp", name: "Timestamp" }
            ],
            columnDefs: [
                {
                    targets: 3,
                    render: function (data, type, row) {
                        moment.locale("tr");
                        return moment(data).format('lll');
                        searchable: false
                    },
                },
                {
                    targets: 4,
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