// Bildirim Gönderme İşlemleri
"use strict";

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

    // Başlangıç işlemleri
    var initComponents = function () {
        // Modal elementi
        modal = new bootstrap.Modal(document.querySelector('#send_mobil_notification'));

        // Form elementi
        form = document.querySelector('#kt_modal_new_target_form');

        // Butonlar
        submitButton = document.querySelector('#kt_modal_new_target_submit');
        cancelButton = form.querySelector('[data-bs-dismiss="modal"]');

        // Tarih seçici (datepicker)
        datepicker = $("#kt_datepicker").flatpickr({
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            minDate: "today",
            time_24hr: true
        });

        // Diğer elementler
        recipientCountElement = document.querySelector('#recipient_count');
        lastSentElement = document.querySelector('#last_sent');
        statusElement = document.querySelector('#notification_status');

        // Programlı gönderim toggle
        $('#scheduledNotification').on('change', function () {
            if ($(this).is(':checked')) {
                $('#scheduledTimeSection').removeClass('d-none');
            } else {
                $('#scheduledTimeSection').addClass('d-none');
            }
        });
         
        // Select2 başlatma
        $('[data-control="select2"]').select2({
            dropdownParent: $("#send_mobil_notification")
        });
    }

    // Form gönderimi
    var handleForm = function () {
        // Form doğrulama kuralları
        let validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'title': {
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
                    'message': {
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
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleInvalidClass: '',
                        eleValidClass: ''
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
                        url: 'send-notification',
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
                    // Form doğrulaması başarısız oldu
                    toastr.error("Lütfen tüm gerekli alanları doldurun.", "Hata");
                }
            });
        });
    }

    // Projeye göre bildirim verilerini getir
    var loadProjectNotificationData = function (projectId) {
        $.ajax({
            type: 'GET',
            url: 'project-notification-info/' + projectId,
            dataType: 'json',
            success: function (data) {
                // Son gönderim bilgisi
                if (data.lastSent) {
                    lastSentElement.textContent = data.lastSent;

                    // Soğuma süresi kontrolü
                    if (data.canSend === false) {
                        statusElement.className = 'badge badge-warning';
                        statusElement.textContent = 'Soğuma Süresi';
                        submitButton.setAttribute('disabled', true);

                        // Uyarı mesajı göster
                        toastr.warning(data.message, "Bildirim Kısıtlaması");
                    } else {
                        statusElement.className = 'badge badge-success';
                        statusElement.textContent = 'Gönderime Hazır';
                        submitButton.removeAttribute('disabled');
                    }
                } else {
                    lastSentElement.textContent = 'Daha önce gönderim yapılmadı';
                    statusElement.className = 'badge badge-success';
                    statusElement.textContent = 'Gönderime Hazır';
                    submitButton.removeAttribute('disabled');
                }

                // Alıcı sayısı
                if (data.recipientCount) {
                    recipientCountElement.textContent = data.recipientCount;
                } else {
                    recipientCountElement.textContent = 'Bilinmiyor';
                }
            },
            error: function () {
                lastSentElement.textContent = 'Bilgi alınamadı';
                recipientCountElement.textContent = 'Bilinmiyor';
                statusElement.className = 'badge badge-danger';
                statusElement.textContent = 'Bilgi Alınamadı';
            }
        });
    }

    // Modalı aç ve projeye göre hazırla
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
        dropzone.removeAllFiles(true);

        // Modalı aç
        modal.show();
    }

    // Toastr ayarları
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

    // Public methods
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

// Sayfa yüklendiğinde başlat
document.addEventListener('DOMContentLoaded', function () {
    NotificationSender.init();
});

// Bildirim gönderme butonları için global listener
document.addEventListener('click', function (e) {
    // "send-notification" sınıfına sahip butonları yakala
    if (e.target && e.target.closest('.btn-send-notification')) {
        const btn = e.target.closest('.btn-send-notification');
        const projectId = btn.getAttribute('data-project-id');
        const projectName = btn.getAttribute('data-project-name');

        // Notification modalını aç
        NotificationSender.openNotificationModal(projectId, projectName);
    }
});