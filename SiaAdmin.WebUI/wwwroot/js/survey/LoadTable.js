
var KTDatatablesServerSide = function () {
    var table;
    var dt;
    var filterPayment;
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "proje-listesi/LoadTable",
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

                { data: "id", name: "Id", },
                { data: "surveyText", name: "SurveyText", width: "20%" },
                { data: "surveyDescription", name: "SurveyDescription", width: "40%" },
                { data: "surveyLink", name: "SurveyLink", width: "40%" },
                { data: "surveyPoints", name: "SurveyPoints" },
                { data: "surveyActive", name: "SurveyActive" },
                { data: null },
            ],
            columnDefs: [

                {
                    targets: 4,
                    render: function (data, type, row) {
                        if (data > 0) {
                            return `<div class="badge badge-light-success" >${data}</div>`
                        } else {
                            return `<div class="badge badge-light-danger" >${data}</div>`
                        }
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        if (data === 0) {
                            return `<div class="badge badge-light-danger" >Pasif</div>`
                        } else {
                            return `<div class="badge badge-light-success" >Aktif</div>`
                        }
                    }
                },

                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {

                        return `
                            <a href="#" class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                Seçenekler
                                <span class="svg-icon svg-icon-5 m-0">
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                            <polygon points="0 0 24 0 24 24 0 24"></polygon>
                                            <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                                        </g>
                                    </svg>
                                </span>
                            </a> 
                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true"> 
                                <div class="menu-item px-3">
                                    <a href="#" class="menu-link px-3" data-kt-docs-table-filter="edit_row">
                                        Güncelle
                                    </a>
                                </div> 
                                 
                                <div class="menu-item px-3">
                                    <a href="#" class="menu-link px-3" data-kt-docs-table-filter="delete_row">
                                        Kapat
                                    </a>
                                </div>
                                <div class="menu-item px-3">
                                    <a href="#" onclick="showCountAssigned(${row.id})" class="menu-link px-3" data-kt-docs-table-filter="delete_row">
                                        Atama Sayısı
                                    </a>
                                </div>
                            </div> 
                        `;
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
        const filterSearch = document.querySelector('[data-survey-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            dt.search(e.target.value).draw();
        });
    }
    var handleFilterDatatable = () => {
        // Select filter options
        filterPayment = document.querySelectorAll('[data-kt-docs-table-filter="payment_type"] [name="payment_type"]');
        const filterButton = document.querySelector('[data-kt-docs-table-filter="filter"]');

        // Filter datatable on submit
        filterButton.addEventListener('click', function () {
            // Get filter values
            let paymentValue = '';

            // Get payment value
            filterPayment.forEach(r => {
                if (r.checked) {
                    paymentValue = r.value;
                }

                // Reset payment value if "All" is selected
                if (paymentValue === 'all') {
                    paymentValue = '';
                }
            });

            // Filter datatable --- official docs reference: https://datatables.net/reference/api/search()
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


function showCountAssigned(id) {
    toastr.options = {
        "closeButton": true,
        "debug": true,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toastr-bottom-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    var formData = new FormData();
    formData.append("Id", id);
    $.ajax({
        type: 'POST',
        url: 'count-survey-assigned',
        dataType: 'json',
        cache: false,
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {  
            toastr.info(`Bu Projeye Toplamda : ${data.count} adet atama yapılmıştır`, "Atanmış Anket Sayısı");
    },
        error: function (data) {
            alert(data)
        }
    });
}