var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "filterdata-listesi/LoadTable",
                type: "POST"
            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[0, "desc"]],
            stateSave: true,


            columns: [

                { data: "surveyUserGuid", name: "SurveyUserGUID" },
                { data: "acikyas", name: "ACIKYAS" },
                { data: "grupyas", name: "GRUPYAS" },
                { data: "cinsiyet", name: "CINSIYET" },
                { data: "il", name: "IL" },
                { data: "ilce", name: "ILCE" },
                { data: "bolge", name: "BOLGE" },
                { data: "hhr", name: "HHR" },
                { data: "egitimGk", name: "EgitimGK" },
                { data: "egitimHr", name: "EgitimHR" },
                { data: "emekli", name: "Emekli" },
                { data: "ameslekGk", name: "AMeslekGK" },
                { data: "ameslekHr", name: "AMeslekHR" },
                { data: "ymeslekGk", name: "YMeslekGK" },
                { data: "ymeslekHr", name: "YMeslekHR" },
                { data: "ymeslekDetayGk", name: "YMeslekDetayGK" },
                { data: "ymeslekDetayHr", name: "YMeslekDetayHR" },
                { data: "yses", name: "YSES" },
                { data: "g02", name: "G02" },
                { data: "g03", name: "G03" },
                { data: "timestamp", name: "Timestamp" },
            ],

            columnDefs: [
                {
                    targets: -1,
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


    var handleSearchDatatable = function () {
        const filterSearch = document.querySelector('[data-kt-user-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            dt.search(e.target.value).draw();
        });
    }
    var handleFilterDatatable = () => {
        filterPayment = document.querySelectorAll('[data-kt-docs-table-filter="payment_type"] [name="payment_type"]');
        const filterButton = document.querySelector('[data-kt-docs-table-filter="filter"]');

        filterButton.addEventListener('click', function () {
            let paymentValue = '';

            filterPayment.forEach(r => {
                if (r.checked) {
                    paymentValue = r.value;
                }

                if (paymentValue === 'all') {
                    paymentValue = '';
                }
            });
            dt.search(paymentValue).draw();
        });
    }
    var handleResetForm = () => {

        const resetButton = document.querySelector('[data-kt-subscription-table-filter="reset"]');


        resetButton.addEventListener('click', function () {

            filterPayment[0].checked = true;
            dt.search('').draw();
        });
    }

    return {
        init: function () {
            initDatatable();
            handleSearchDatatable();
            handleResetForm();
        }
    }
}();

KTUtil.onDOMContentLoaded(function () {
    KTDatatablesServerSide.init();
});