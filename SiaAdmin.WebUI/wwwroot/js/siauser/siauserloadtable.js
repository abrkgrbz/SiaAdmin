﻿
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
                url: "sia-kullanici-tablosu/LoadTable",
                type: "POST"
            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[0, "desc"]],
            stateSave: true,


            columns: [ 
                { data: "surveyUserGuid", name: "SurveyUserGuid" },
                 
            ],
            columnDefs: [

                {
                    targets: 1,
                    render: function (data, type, row) { 
                        return `<button class="btn btn-sm btn-icon btn-bg-light btn-active-color-primary w-30px h-30px" 
                                   onclick="submitForm('${row.surveyUserGuid}')">
                                <!--begin::Svg Icon | path: icons/duotune/arrows/arr064.svg-->
                                <span class="svg-icon svg-icon-2">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <rect opacity="0.5" x="18" y="13" width="13" height="2" rx="1" transform="rotate(-180 18 13)" fill="currentColor"></rect>
                                        <path d="M15.4343 12.5657L11.25 16.75C10.8358 17.1642 10.8358 17.8358 11.25 18.25C11.6642 18.6642 12.3358 18.6642 12.75 18.25L18.2929 12.7071C18.6834 12.3166 18.6834 11.6834 18.2929 11.2929L12.75 5.75C12.3358 5.33579 11.6642 5.33579 11.25 5.75C10.8358 6.16421 10.8358 6.83579 11.25 7.25L15.4343 11.4343C15.7467 11.7467 15.7467 12.2533 15.4343 12.5657Z" fill="currentColor"></path>
                                    </svg>
                                </span>
                                <!--end::Svg Icon-->
                            </button>`
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
        const filterSearch = document.querySelector('[data-eod-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            dt.search(e.target.value).draw();
        });
    }
   
    var refreshDatatable = function () {
        const refreshButton = document.getElementById("table_refresh");
        refreshButton.addEventListener('click', function (e) {
            dt.search("").draw();
        });
    }

    return {
        init: function () {
            initDatatable();
            handleSearchDatatable();
            refreshDatatable();
        }
    }
}();
function submitForm(guid) {
    var guidInput = document.getElementById("guidInput");
    if (guidInput) {
        guidInput.value = guid;
        document.getElementById("profileForm").submit();
    } else {
        console.error("Element 'guidInput' not found!");
    }
}

KTUtil.onDOMContentLoaded(function () {
    KTDatatablesServerSide.init();
});
