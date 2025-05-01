
"use strict";
var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_churndata").DataTable({
            ajax: {
                url: "churn-data/LoadTable",
                type: "POST"
            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[6, "desc"]],
            stateSave: true,
            language: {
                url: '/assets/customjs/turkish.json',
            },

            columns: [
                { data: "internalGUID", name: "InternalGUID" },
                { data: "registrationDate", name: "RegistrationDate" },
                { data: "lastLogin", name: "LastLogin" },
                { data: "msisdn", name: "Msisdn" },
                { data: "email", name: "Email" },
                { data: "cazipdegil", name: "CaziDeğil" },
                { data: "ilgisiz", name: "ilgisiz" },
                { data: "kimole", name: "kimOle" },
                { data: "korkuyorum", name: "korkuyorum" },
                { data: "vakitsizim", name: "vakitSizim" },
                { data: "dusukcene", name: "dusukCene" },
                { data: "diger", name: "Diger" }
            ],
            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        moment.locale("tr");
                        return moment(data).format('lll');
                        searchable: false
                    },
                },
                {
                    targets: 2,
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


    var handleSearchDatatable = function () {
        const filterSearch = document.querySelector('[data-eod-table-filter="search"]');
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

    var exportExcel = () => {
        const exportButton = document.getElementById("exportButton");
        exportButton.addEventListener('click', function (e) {
            $.ajax({
                url: '/churn-data/ExportExcel',
                type: 'POST',
                xhrFields: {
                    responseType: 'blob'
                },
                success: function (blob) {
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = "ChurnData.xlsx";
                    link.click();
                }
            });
        });
     
    }
    return {
        init: function () {
            initDatatable();
            handleSearchDatatable();
            exportExcel();
        }
    }
}();

KTUtil.onDOMContentLoaded(function () {
    KTDatatablesServerSide.init();
});
