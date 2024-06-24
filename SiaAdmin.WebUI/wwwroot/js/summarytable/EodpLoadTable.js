 
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
                url: "eodp-table/LoadTable",
                type: "POST"
            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[10, "desc"]],
            stateSave: true,


            columns: [

                { data: "surveyId", name: "SurveyId" },
                { data: "surveyText", name: "SurveyText" },
                { data: "surveyDescription", name: "SurveyDescription" },
                { data: "surveyValidity", name: "SurveyValidity", },
                { data: "surveyStartDate", name: "SurveyStartDate" },
                { data: "surveyDbaddress", name: "SurveyDbaddress" },
                { data: "surveyStatus", name: "SurveyStatus" },
                { data: "surveyCompleteCount", name: "SurveyCompleteCount" },
                { data: "surveyAssigned", name: "SurveyAssigned" },
                { data: "surveyAssignedAvail", name: "SurveyAssignedAvail" },
                { data: "timestamp", name: "Timestamp" },

            ],
            columnDefs: [

                {
                    targets: 4,
                    render: function (data, type, row) {
                        if (data == null) {
                            return `<div class="badge badge-light-danger" >-</div>`
                        } else {
                            return `<div class="badge badge-light-success" >${data}</div>`
                        }
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        if (data === null) {
                            return `<div class="badge badge-light-danger" >-</div>`;
                        }
                        moment.locale("tr");
                        return moment(data).format('lll');
                    }
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        moment.locale("tr");
                        return moment(data).format('lll');
                    }
                },
                {
                    targets: -1,
                    render: function (data, type, row) {
                        moment.locale("tr");
                        return moment(data).format('lll');
                    }
                },
                {
                    targets: 6,
                    render: function (data, type, row) {
                        if (data === "VALID") {
                            return `<div class="badge badge-light-success" >Online</div>`
                        }
                        else if (data === "B2022") {
                            return `<div class="badge badge-secondary">BACKUP Alınmış</div>`
                        }
                        else {
                            return ` <div class="badge badge-light-danger">Bilinmiyor</div>`
                        }
                    }
                }

            ]

        });

    table = dt.$;

    dt.on('draw', function () {
        KTMenu.createInstances();
                });
            }


    var handleSearchDatatable = function () {
                const filterSearch = document.querySelector('[data-eodp-table-filter="search"]');
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
 