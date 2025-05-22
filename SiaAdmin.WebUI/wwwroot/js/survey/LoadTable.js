
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
                url: '/adm/assets/customjs/turkish.json',
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
                        if (data === 1) {

                            return `<div class="badge badge-light-success" >Aktif</div>`
                        } else {

                            return `<div class="badge badge-light-danger" >Pasif</div>`
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
                                    <a href="#"  class="menu-link px-3" data-kt-docs-table-filter="edit_row">
                                        Güncelle
                                    </a>
                                </div>

                                     ${(() => {
                                if (row.surveyActive == 1 && row.id > 5000) {

                                    return `  <div class="menu-item px-3">
                                    <a href="#"  class="menu-link px-3 btn-send-notification"  data-survey-id="${row.id}" data-survey-name="${row.surveyText || 'Proje'}" data-bs-toggle="modal" data-bs-target="#send_mobil_notification"  ">
                                        Mobil Bildirim
                                    </a>
                                </div>`;
                                }
                                else {
                                    return ``;
                                }
                            })()}
                                
                                 
                                <div class="menu-item px-3"> 
                                    ${(() => {
                                if (row.surveyActive == 1) {
                                    return `  <a href="#" onclick="CloseProject(${row.id})" class="menu-link px-3" data-kt-docs-table-filter="delete_row">
                                           Kapat
                                       </a>`;
                                }
                                else {
                                    return ``;
                                }
                            })()}
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

function CloseProject(id) {
    Swal.fire({
        html: `Proje Kodu <strong>${id} olan</strong> projeyi kapatmak istediğinize eminmisiniz? `,
        icon: "info",
        buttonsStyling: false,
        showCancelButton: true,
        confirmButtonText: "Evet",
        cancelButtonText: 'İptal',
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: 'btn btn-danger'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            var formData = new FormData();
            formData.append("SurveyId", id);
            $.ajax({
                type: 'POST',
                url: 'proje-kapat',
                dataType: 'json',
                cache: false,
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    console.log(data)
                    if (data.data == true) {
                        Swal.fire(data.message, "", "success").then((result) => { setTimeout(() => { location.reload(true) }, 1000) });

                    }
                },
                error: function (data) {
                    if (data.status == 403) {
                        Swal.fire("Bu işlemi yapmak için yetkiniz bulunmamaktadır!", "", "error");
                    }
                }
            });
        } else if (!result.isConfirmed) {
            Swal.fire("Proje kapatma işleminden vazgeçildi!", "", "warning");
        }
    });;
}
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



// Sınıf tanımlaması
var NotificationSender = function () {
    // Değişkenler
    var modal;
    var form;
    var submitButton;
    var cancelButton;
    var datepicker;
    var dropzone;
    var selectedProjectId = null;
    var recipientCountElement;
    var lastSentElement;
    var statusElement;
     
    var initComponents = function () { 
        modal = new bootstrap.Modal(document.querySelector('#send_mobil_notification'));
         
        form = document.querySelector('#form_mobile_noti');

        // Butonlar
        submitButton = document.querySelector('#btn_submit_mobile_notification');
        cancelButton = form.querySelector('[data-bs-dismiss="modal"]');
         
        datepicker = $("#kt_datepicker").flatpickr({
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            minDate: "today",
            time_24hr: true
        });
         
        recipientCountElement = document.querySelector('#recipient_count');
        lastSentElement = document.querySelector('#last_sent');
        statusElement = document.querySelector('#notification_status');
         
        $('#scheduledNotification').on('change', function () {
            if ($(this).is(':checked')) {
                $('#scheduledTimeSection').removeClass('d-none');
            } else {
                $('#scheduledTimeSection').addClass('d-none');
            }
        }); 
        $('[data-control="select2"]').select2({
            dropdownParent: $("#send_mobil_notification")
        });
    }
     
    var handleForm = function () {

        // Form doğrulama kuralları
        let validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'NotificationTitle': {
                        validators: {
                            notEmpty: {
                                message: 'Bildirim başlığı gereklidir'
                            },
                            stringLength: {
                                max: 80,
                                message: 'Başlık 80 karakterden uzun olamaz'
                            }
                        }
                    },
                    'NotificationContent': {
                        validators: {
                            notEmpty: {
                                message: 'Bildirim içeriği gereklidir'
                            },
                            stringLength: {
                                max: 1000,
                                message: 'İçerik 1000 karakterden uzun olamaz'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger,
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: ".fv-row",
                        eleInvalidClass: "",
                        eleValidClass: ""
                    })
                }
            }
        );

        // Form gönderimi
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();

            // Form doğrulama
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    // Butonu devre dışı bırak ve yükleniyor göster
                    submitButton.setAttribute('disabled', true);
                    submitButton.querySelector('.indicator-label').style.display = 'none';
                    submitButton.querySelector('.indicator-progress').style.display = 'inline-block';

                    // Form verilerini al
                    var formData = new FormData(form);
                    formData.append("Id", selectedProjectId);
                    $.ajax({
                        type: 'POST',
                        url: 'send-mobile-notification',
                        dataType: 'json',
                        cache: false,
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            toastr.success(data.message || 'Mobil gönderimler başarıyla gerçekleştirildi.', "Mobil Uygulama Bildirim");

                            modal.hide();
                            form.reset();
                            dropzone.removeAllFiles(true);

                            submitButton.removeAttribute('disabled');
                            submitButton.querySelector('.indicator-label').style.display = 'inline-block';
                            submitButton.querySelector('.indicator-progress').style.display = 'none';

                            // Opsiyonel: Tabloyu yeniden yükle
                            if (typeof dataTable !== 'undefined') {
                                dataTable.ajax.reload();
                            }
                        },
                        error: function (response) {
                            // Hata mesajı
                            let errorMessage = "Mobil gönderimler yapılamadı lütfen tekrar deneyiniz";

                            if (response.responseJSON && response.responseJSON.message) {
                                errorMessage = response.responseJSON.message;
                            }

                            toastr.error(errorMessage, "Hata");

                            // Butonu sıfırla
                            submitButton.removeAttribute('disabled');
                            submitButton.querySelector('.indicator-label').style.display = 'inline-block';
                            submitButton.querySelector('.indicator-progress').style.display = 'none';
                        }
                    });
                } else { 
                    toastr.error("Lütfen tüm gerekli alanları doldurun.", "Hata");
                }
            });
        });
    }
     
    var loadProjectNotificationData = function (projectId) {
        // Ana bildirim bilgilerini yükle
        $.ajax({
            type: 'GET',
            url: 'project-notification-info/' + projectId,
            dataType: 'json',
            success: function (data) { 
                if (data.lastSent) {
                    lastSentElement.textContent = data.lastSent;
                    // Soğuma süresi kontrolü
                    if (data.canSend === false) {
                        statusElement.className = 'badge badge-warning';
                        statusElement.textContent = 'Beklemede';
                        submitButton.setAttribute('disabled', true); 
                        submitButton.querySelector('.indicator-label').textContent = "Gönderilemez";
                        submitButton.classList.add('btn-danger');
                        toastr.warning(data.message, "Bildirim Kısıtlaması");
                    } else {
                        statusElement.className = 'badge badge-success';
                        statusElement.textContent = 'Gönderime Hazır';

                        submitButton.removeAttribute('disabled');
                        submitButton.querySelector('.indicator-label').textContent = "Gönder";
                        submitButton.classList.remove('btn-danger'); // btn-danger sınıfını kaldır
                        submitButton.classList.add('btn-primary'); 
                    }
                } else {
                    lastSentElement.textContent = 'Daha önce gönderim yapılmadı';
                    statusElement.className = 'badge badge-success';
                    statusElement.textContent = 'Gönderime Hazır';
                    submitButton.removeAttribute('disabled');
                    submitButton.querySelector('.indicator-label').textContent = "Gönder";
                    submitButton.classList.remove('btn-danger'); // btn-danger sınıfını kaldır
                    submitButton.classList.add('btn-primary'); 
                }

                // Başlangıçta Yükleniyor göster
                recipientCountElement.textContent = 'Yükleniyor...';
            },
            error: function () {
                lastSentElement.textContent = 'Bilgi alınamadı';
                recipientCountElement.textContent = 'Bilinmiyor';
                statusElement.className = 'badge badge-danger';
                statusElement.textContent = 'Bilgi Alınamadı';
            }
        });

        // Kullanıcı sayısını ayrı bir çağrı ile asenkron olarak getir
        $.ajax({
            type: 'GET',
            url: 'project-notification-info-count-user/' + projectId,
            dataType: 'json',
            success: function (data) { 
                if (data.recipientCountElement !== undefined) {
                    recipientCountElement.textContent = data.recipientCountElement;
                } else {
                    recipientCountElement.textContent = 'Bilinmiyor';
                }
            },
            error: function () {
                recipientCountElement.textContent = 'Bilinmiyor';
            }
        });
    }
     
    var openModal = function (projectId, projectName) {
        console.log(projectId)
        // Proje ID'sini sakla
        selectedProjectId = projectId;
        document.querySelector('#hiddenRowId').value = projectId;

        // Modal başlığını projeye göre ayarla
        if (projectName) {
            const modalTitle = document.querySelector('#send_mobil_notification .modal-title');
            modalTitle.textContent = projectName + ' - Bildirim Gönder';
        }

        // Bildirim verilerini yükle
        loadProjectNotificationData(projectId);

        // Formu sıfırla
        form.reset();

        // Modalı aç
        modal.show();
    }
     
    var initToastr = function () {
        toastr.options = {
            "closeButton": true,
            "debug": false,
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
    }
     
    return {
        init: function () {
            initComponents();
            handleForm();
            initToastr();
        },

        // Projeye göre bildirim modalını aç
        openNotificationModal: function (projectId, projectName) {
            openModal(projectId, projectName);
        }
    };
}();
 
document.addEventListener('DOMContentLoaded', function () {
    NotificationSender.init();
});
 
document.addEventListener('click', function (e) { 
    if (e.target && e.target.closest('.btn-send-notification')) {
        const btn = e.target.closest('.btn-send-notification');
        const projectId = btn.getAttribute('data-survey-id');
        const projectName = btn.getAttribute('data-survey-name');
         
        NotificationSender.openNotificationModal(projectId, projectName);
    }
});

// Excel İndirme Fonksiyonunu Tanımlama
function exportTableToExcel() {
    // Excel indirme isteğini göndermeden önce kullanıcıya bilgi ver
    Swal.fire({
        title: 'Excel İndiriliyor',
        text: 'Excel dosyası hazırlanıyor, lütfen bekleyin...',
        icon: 'info',
        allowOutsideClick: false,
        showConfirmButton: false,
        willOpen: () => {
            Swal.showLoading();
        }
    });

    // AJAX isteği ile Excel dosyasını indir
    $.ajax({
        type: 'GET',
        url: 'export-survey-excel',
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data, status, xhr) {
            Swal.close();

            // Dosya adını belirle (sunucudan gelen header'a göre veya varsayılan)
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }
            if (!filename) {
                filename = 'projeler_listesi_' + new Date().toISOString().slice(0, 10) + '.xlsx';
            }

            // Dosyayı indir
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = filename;
            link.click();
            window.URL.revokeObjectURL(link.href);
        },
        error: function (xhr, status, error) {
            Swal.fire({
                title: 'Hata',
                text: 'Excel dosyası indirme sırasında bir hata oluştu. Lütfen tekrar deneyin.',
                icon: 'error',
                confirmButtonText: 'Tamam'
            });
            console.error('Excel indirme hatası:', error);
        }
    });
}

// Sayfa yüklendiğinde Excel İndirme butonuna tıklama olayını ekle
document.addEventListener('DOMContentLoaded', function () {
    var exportButton = document.getElementById('btnExcelExport');
    if (exportButton) {
        exportButton.addEventListener('click', exportTableToExcel);
    }
});