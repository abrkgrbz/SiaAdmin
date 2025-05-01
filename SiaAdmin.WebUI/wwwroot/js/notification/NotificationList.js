"use strict";

// Bildirim Listesi Sınıfı
var NotificationList = function () {
    // Değişkenler
    var dataTable;
    var dateRangePicker;

    // Tabloyu başlat
    var initTable = function () {
        // Tablo elementi
        var table = document.querySelector('#kt_notifications_table');

        if (!table) {
            console.error('Tablo elementi bulunamadı');
            return;
        }

        // DataTables konfigürasyonu
        dataTable = $(table).DataTable({
            responsive: true,
            processing: true,
            serverSide: true,
            order: [[2, 'desc']], // Gönderim zamanına göre sırala (yeniden eskiye)
            ajax: {
                url: '/get-all-notifications',
                type: 'POST',
                data: function (d) {
                    // Filtreleme parametrelerini ekle
                    d.status = $('#filter_status').val();
                    d.projectId = $('#filter_project').val();
                    d.dateRange = $('#kt_daterangepicker').val();
                    d.userId = $('#filter_user').val();
                    return d;
                }
            },
            columns: [
                { data: 'project' }, // Proje
                { data: 'title' }, // Başlık
                { data: 'sendTime' }, // Gönderim Zamanı
                {
                    data: 'status', render: function (data) {
                        // Durum renkleri ve etiketleri
                        var statusClass = {
                            0: 'success', // Başarılı
                            1: 'danger',  // Başarısız
                            2: 'warning', // İşlemde
                            3: 'info' // İptal Edildi
                        };

                        var statusText = {
                            0: 'Başarılı',
                            1: 'Başarısız',
                            2: 'İşlemde',
                            3: 'İptal Edildi'
                        };

                        return `<div class="badge badge-${statusClass[data]} fw-bold">${statusText[data]}</div>`;
                    }
                },
                { data: 'recipientCount' }, // Alıcı Sayısı
                { data: 'sender' }, // Gönderen
                {
                    data: 'id', orderable: false, render: function (data, type, row) {
                        // İşlem butonları
                        return `
                    <div class="d-flex justify-content-end flex-shrink-0">
                        <button type="button" class="btn btn-icon btn-light-primary btn-sm me-1 view-notification" data-id="${data}" title="Detay Görüntüle">
                            <i class="ki-duotone ki-eye fs-2">
                                <span class="path1"></span>
                                <span class="path2"></span>
                                <span class="path3"></span>
                            </i>
                        </button>
                        ${row.status === 1 ?
                                `<button type="button" class="btn btn-icon btn-light-info btn-sm retry-notification" data-id="${data}" title="Yeniden Gönder">
                            <i class="ki-duotone ki-abstract-26 fs-2">
                                <span class="path1"></span>
                                <span class="path2"></span>
                            </i>
                        </button>` : ''}
                    </div>`;
                    }
                }
            ],
            // Dil ayarları
            language: {
                url: '/assets/plugins/custom/datatables/tr.json'
            }
        });

        // Tablo yeniden çizildiğinde olayları yeniden bağla
        dataTable.on('draw', function () {
            initNotificationActions();
        });
    };


    var showNotificationDetail = function (id) {
        $.ajax({
            url: '/get-notification-details',
            type: 'GET',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;

                    // Modal elementlerini doldur
                    $('.notification-title').text(data.title);
                    $('.notification-project').text(data.project);
                    $('.notification-date').text(data.sendTime);
                    $('.notification-recipients').text(data.recipientCount);
                    $('.notification-sender').text(data.sender);
                    $('.notification-provider').text(data.provider || 'Bilinmiyor');
                    $('.notification-content').text(data.content);
                    $('.notification-payload').text(data.payload);

                    // Durum bilgisi
                    var statusClass = {
                        0: 'success', // Başarılı
                        1: 'danger',  // Başarısız
                        2: 'warning', // İşlemde
                        3: 'info' // İptal Edildi
                    };

                    var statusText = {
                        0: 'Başarılı',
                        1: 'Başarısız',
                        2: 'İşlemde',
                        3: 'İptal Edildi'
                    };

                    $('.notification-status').html(`<div class="badge badge-light-${statusClass[data.status]} fw-bold">${statusText[data.status]}</div>`);

                    if (data.status === 1 && data.errorDetails) {
                        $('.notification-error-code').text(data.errorDetails.code || '-');
                        $('.notification-error-message').text(data.errorDetails.message || '-');
                        $('.notification-error-container').removeClass('d-none');

                        // Yeniden gönder butonunu göster
                        $('.notification-retry-btn').removeClass('d-none').attr('data-id', data.id);
                    } else {
                        $('.notification-error-container').addClass('d-none');
                        $('.notification-retry-btn').addClass('d-none');
                    }

                    // Modalı göster
                    $('#notification_detail_modal').modal('show');
                } else {
                    toastr.error('Bildirim detayı alınamadı', 'Hata');
                }
            },
            error: function () {
                toastr.error('Sunucu hatası', 'Hata');
            }
        });
    };


    var initNotificationActions = function () {
        // Detay görüntüleme
        $('.view-notification').on('click', function () {

            var id = $(this).data('id');
            showNotificationDetail(id);
        });

        // Bildirimi yeniden gönderme
        $('.retry-notification').on('click', function () {
            var id = $(this).data('id');

            Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu bildirimi yeniden göndermek istediğinize emin misiniz?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Evet, yeniden gönder',
                cancelButtonText: 'İptal',
                buttonsStyling: false,
                customClass: {
                    confirmButton: 'btn btn-primary',
                    cancelButton: 'btn btn-light'
                }
            }).then(function (result) {
                if (result.isConfirmed) {
                    // Yeniden gönderim isteği
                    $.ajax({
                        url: '/Notification/RetryNotification',
                        type: 'POST',
                        data: { id: id },
                        dataType: 'json',
                        success: function (response) {
                            if (response.success) {
                                toastr.success('Bildirim yeniden gönderildi', 'Başarılı');
                                dataTable.ajax.reload();
                                $('#notification_detail_modal').modal('hide');
                            } else {
                                toastr.error(response.message || 'Bildirim gönderilemedi', 'Hata');
                            }
                        },
                        error: function () {
                            toastr.error('Sunucu hatası', 'Hata');
                        }
                    });
                }
            });
        });
    };

    var initEvents = function () {
        dateRangePicker = $('#kt_daterangepicker').daterangepicker({
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Temizle',
                applyLabel: 'Uygula',
                format: 'DD.MM.YYYY'
            }
        });

        // Tarih aralığı seçildiğinde
        dateRangePicker.on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
        });

        // Tarih aralığı temizlendiğinde
        dateRangePicker.on('cancel.daterangepicker', function () {
            $(this).val('');
        });

        // Proje listesini yükle
        loadProjects();

        // Kullanıcı listesini yükle
        loadUsers();

        // Filtre butonları
        $('#apply_filter').on('click', function () {
            dataTable.ajax.reload();
        });

        // Tümünü Göster butonu
        $('#show_all').on('click', function () {
            $('#filter_status').val('').trigger('change');
            $('#filter_project').val('').trigger('change');
            $('#filter_user').val('').trigger('change');
            $('#kt_daterangepicker').val('');
            dataTable.ajax.reload();
            toastr.success('Tüm bildirimler listeleniyor', 'Bilgi');
        });

        $('#reset_filter').on('click', function () {
            $('#filter_status').val('').trigger('change');
            $('#filter_project').val('').trigger('change');
            $('#filter_user').val('').trigger('change');
            $('#kt_daterangepicker').val('');
            dataTable.ajax.reload();
        });

        // Durum filtresi değiştiğinde
        $('#filter_status').on('change', function () {
            dataTable.ajax.reload();
        });

        // Proje filtresi değiştiğinde
        $('#filter_project').on('change', function () {
            dataTable.ajax.reload();
        });

        // Dışa aktarma butonu
        $('#export_notifications').on('click', function () {
            // Filtreleri al
            var filters = {
                status: $('#filter_status').val(),
                projectId: $('#filter_project').val(),
                dateRange: $('#kt_daterangepicker').val(),
                userId: $('#filter_user').val()
            };

            // Dışa aktarma URL'sini oluştur
            var url = '/Notification/ExportNotifications?' + $.param(filters);

            // Yeni pencerede aç
            window.location.href = url;
        });


        $('.notification-retry-btn').on('click', function () {
            var id = $(this).data('id');

            $.ajax({
                url: '/Notification/RetryNotification',
                type: 'POST',
                data: { id: id },
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        toastr.success('Bildirim yeniden gönderildi', 'Başarılı');
                        dataTable.ajax.reload();
                        $('#notification_detail_modal').modal('hide');
                    } else {
                        toastr.error(response.message || 'Bildirim gönderilemedi', 'Hata');
                    }
                },
                error: function () {
                    toastr.error('Sunucu hatası', 'Hata');
                }
            });
        });
    };

    var loadProjects = function () {
        $.ajax({
            url: '/get-surveys',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var projects = response.data;

                    // Filtre için proje listesi
                    var filterSelect = $('#filter_project');
                    filterSelect.empty();
                    filterSelect.append('<option value="">Tüm Projeler</option>');

                    // Yeni bildirim için proje listesi
                    var projectSelect = $('#project_select');
                    projectSelect.empty();
                    projectSelect.append('<option value=""></option>');

                    // Projeleri ekle
                    projects.forEach(function (project) {
                        filterSelect.append(`<option value="${project.id}">${project.name}</option>`);
                        projectSelect.append(`<option value="${project.id}">${project.name}</option>`);
                    });

                    // Select2'yi güncelle
                    filterSelect.trigger('change');
                    projectSelect.trigger('change');
                }
            }
        });
    };

    var loadUsers = function () {
        $.ajax({
            url: '/Notification/GetUsers',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var users = response.data;

                    // Kullanıcı listesi
                    var userSelect = $('#filter_user');
                    userSelect.empty();
                    userSelect.append('<option value="">Tüm Kullanıcılar</option>');

                    // Kullanıcıları ekle
                    users.forEach(function (user) {
                        userSelect.append(`<option value="${user.id}">${user.name}</option>`);
                    });

                    // Select2'yi güncelle
                    userSelect.trigger('change');
                }
            }
        });
    };

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
    };

    return {
        init: function () {
            initTable();
            initEvents();
            initToastr();
        },


        refreshTable: function () {
            if (dataTable) {
                dataTable.ajax.reload();
            }
        }
    };
}();


document.addEventListener('DOMContentLoaded', function () {
    NotificationList.init();
});