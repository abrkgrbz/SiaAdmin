var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;
     
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "bekleyen-proje-listesi/LoadTable",
                type: "POST"
            },
            language: {
                url: '/assets/customjs/turkish.json',

            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[0, "desc"]],
            stateSave: true, 
            columns: [

                { data: "surveyId", name: "SurveyId" },
                { data: "tarih", name: "Tarih" },
                { data: "adet", name: "Adet" }, 
              
            ],

            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        moment.locale("tr");
                        return moment(data).format('lll');
                        searchable: false
                    },
                }
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