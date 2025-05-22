var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPanel;

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_table_users").DataTable({
            ajax: {
                url: "filterdata-listesi/LoadTable",
                type: "POST",
                data: function (d) {
                    // Tüm filtreleri ekle
                    d.filters = getActiveFilters();
                    return d;
                }
            },
            searchDelay: 500,
            processing: true,
            serverSide: true,
            order: [[20, "desc"]], // Timestamp sütununa göre sıralama
            stateSave: true,
            dom: '<"d-flex justify-content-between align-items-center"fl>t<"d-flex justify-content-between"ip>',
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.4/i18n/tr.json',
                search: "",
                searchPlaceholder: "Ara...",
                lengthMenu: "_MENU_ kayıt göster"
            },
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
                    },
                    searchable: false
                }
            ],
            initComplete: function () {
                // DataTable arama kutusu yerine kendi arama kutumuz
                $('.dataTables_filter').hide();

                // Kendi arama kutumuz için olay dinleyici
                $('#quick_search').on('keyup', function () {
                    dt.search(this.value).draw();
                });
            }
        });

        table = dt.$;

        dt.on('draw', function () {
            KTMenu.createInstances();
        });
    }

    // Kendi arama kutumuz için olay dinleyici
    var handleSearchDatatable = function () {
        const filterSearch = document.querySelector('#quick_search');
        filterSearch.addEventListener('keyup', function (e) {
            dt.search(e.target.value).draw();
        });
    }

    // Filtre Panelini Göster/Gizle
    var initFilterPanel = function () {
        $('#btn_toggle_filters').on('click', function () {
            $('#filter_panel').slideToggle();
        });
    }

    // Filtrele butonu tıklama olayı
    var handleFilterButton = function () {
        $('#btn_filter').on('click', function () {
            dt.ajax.reload();
            updateActiveFilters();
        });
    }

    // Temizle butonu tıklama olayı
    var handleResetForm = function () {
        $('#btn_reset').on('click', function () {
            // Tüm filtreleri sıfırla
            $('#gender_filter, #age_group_filter, #retirement_filter, #province_filter, #district_filter, #region_filter, #education_filter, #occupation_filter, #ses_filter').val('');
            $('#quick_search').val('');

            // Aktif filtreleri temizle
            $('#active_filters').empty().hide();

            // Tabloyu güncelle
            dt.search('').draw();
            dt.ajax.reload();
        });
    }

    // Excel'e aktar butonu tıklama olayı
    var handleExcelExport = function () {
        $('#btn_excel').on('click', function () {
            exportToExcel();
        });
    }

   
    // İlleri yükleme
    var loadProvinces = function () {
        try {
            var provinceSelect = $('#province_filter');
            provinceSelect.html('<option value="">Tümü</option>');

            // Statik illeri yükle
            $.each(LocationData.provinces, function (i, province) {
                provinceSelect.append($('<option>', {
                    value: province.id,
                    text: province.name
                }));
            });
        } catch (error) {
            console.error("İller yüklenirken hata oluştu:", error);
        }
    }

    // Bölgeleri yükleme
    var loadRegions = function () {
        try {
            var regionSelect = $('#region_filter');
            regionSelect.html('<option value="">Tümü</option>');

            // Statik bölgeleri yükle
            $.each(LocationData.regions, function (i, region) {
                regionSelect.append($('<option>', {
                    value: region.id,
                    text: region.name
                }));
            });
        } catch (error) {
            console.error("Bölgeler yüklenirken hata oluştu:", error);
        }
    }

    // Eğitim seçeneklerini yükleme
    var loadEducation = function () {
        try {
            var educationSelect = $('#education_filter');
            educationSelect.html('<option value="">Tümü</option>');

            // Statik eğitim seçeneklerini yükle
            $.each(LocationData.educationOptions, function (i, education) {
                educationSelect.append($('<option>', {
                    value: education.id,
                    text: education.name
                }));
            });
        } catch (error) {
            console.error("Eğitim seçenekleri yüklenirken hata oluştu:", error);
        }
    }

    // Meslek gruplarını yükleme
    var loadOccupations = function () {
        try {
            var occupationSelect = $('#occupation_filter');
            occupationSelect.html('<option value="">Tümü</option>');

            // Statik meslek gruplarını yükle
            $.each(LocationData.occupations, function (i, occupation) {
                occupationSelect.append($('<option>', {
                    value: occupation.id,
                    text: occupation.name
                }));
            });
        } catch (error) {
            console.error("Meslek grupları yüklenirken hata oluştu:", error);
        }
    }

   
    // Aktif filtreleri güncelleme
    var updateActiveFilters = function () {
        const filterLabels = {
            'gender_filter': 'Cinsiyet',
            'age_group_filter': 'Yaş Grubu',
            'retirement_filter': 'Emekli',
            'province_filter': 'İl',
            'district_filter': 'İlçe',
            'region_filter': 'Bölge',
            'education_filter': 'Eğitim',
            'occupation_filter': 'Meslek',
            'ses_filter': 'SES'
        };

        const container = $('#active_filters');
        container.empty();

        let hasFilters = false;

        // Her bir filtreyi kontrol et
        for (const [filterId, label] of Object.entries(filterLabels)) {
            const filterElem = $(`#${filterId}`);
            const value = filterElem.val();

            if (value && value !== '') {
                hasFilters = true;

                // Gösterilecek değeri belirle (select elementleri için seçili metnini al)
                let displayValue = value;
                if (filterElem.is('select')) {
                    displayValue = filterElem.find('option:selected').text();
                }

                // Filtre etiketi oluştur
                const badge = $(`
                    <span class="badge badge-light-primary py-1 px-2 me-2 mb-1">
                        ${label}: ${displayValue}
                        <a href="#" class="ms-2 text-primary remove-filter" data-filter="${filterId}">
                            <i class="fas fa-times-circle"></i>
                        </a>
                    </span>
                `);

                container.append(badge);
            }
        }

        if (hasFilters) {
            container.show();
        } else {
            container.hide();
        }

        // Filtre silme işlevi
        $('.remove-filter').on('click', function (e) {
            e.preventDefault();
            const filterId = $(this).data('filter');

            // İlgili filtreyi sıfırla
            $(`#${filterId}`).val('');

            // Tabloyu güncelle
            dt.ajax.reload();
            updateActiveFilters();
        });
    }

    // Aktif filtreleri alma
    var getActiveFilters = function () {
        return {
            gender: $('#gender_filter').val(),
            ageGroup: $('#age_group_filter').val(),
            retirement: $('#retirement_filter').val(),
            province: $('#province_filter').val(),
            district: $('#district_filter').val(),
            region: $('#region_filter').val(),
            education: $('#education_filter').val(),
            occupation: $('#occupation_filter').val(),
            ses: $('#ses_filter').val()
        };
    }

    // Excel'e aktarma işlemi
    var exportToExcel = function () {
        // İşlem başladı bildirimi
        Swal.fire({
            title: 'İşleniyor...',
            text: 'Excel dosyası hazırlanıyor',
            icon: 'info',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        // Filtreleri al
        const filters = getActiveFilters();

        // API'ye Excel indirme isteği gönder
        fetch('filterdata-listesi/ExportToExcel', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ filters: filters }),
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Excel oluşturma hatası');
                }
                return response.blob();
            })
            .then(blob => {
                // İndirme işlemi
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = url;
                a.download = 'Anket_Verileri_' + new Date().toISOString().split('T')[0] + '.xlsx';
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);

                // Başarı mesajı
                Swal.fire({
                    title: 'Başarılı!',
                    text: 'Excel dosyası başarıyla indirildi',
                    icon: 'success',
                    confirmButtonText: 'Tamam'
                });
            })
            .catch(error => {
                console.error('Excel aktarma hatası:', error);

                // Hata mesajı
                Swal.fire({
                    title: 'Hata!',
                    text: 'Excel dosyası oluşturulurken bir hata oluştu',
                    icon: 'error',
                    confirmButtonText: 'Tamam'
                });
            });
    }

    return {
        init: function () {
            initDatatable();
            handleSearchDatatable();
            initFilterPanel();
            handleFilterButton();
            handleResetForm();
            handleExcelExport(); 

            // Statik referans verilerini yükle
            loadProvinces();
            loadRegions();
            loadEducation();
            loadOccupations();
        }
    }
}();

KTUtil.onDOMContentLoaded(function () {
    KTDatatablesServerSide.init();
});