
"use strict";
var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "eod-table/LoadTable", 
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
                { data: "surveyUserGuid", name: "SurveyUserGuid" },
                { data: "toplamKatilim", name: "ToplamKatilim" },
                { data: "olumluKatilim", name: "OlumluKatilim" },
                { data: "davetEdilenAnketSayisi", name: "DavetEdilenAnketSayisi" },
                { data: "davetEdilenArkadasSayisi", name: "DavetEdilenArkadasSayisi" },
                { data: "toplamPuan", name: "ToplamPuan" },
                { data: "toplamPara", name: "ToplamPara" },
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
