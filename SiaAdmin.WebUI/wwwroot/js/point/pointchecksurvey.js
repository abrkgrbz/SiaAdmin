function showModal() {
    
    $('#kt_modal_2').modal('show');
   
};
const form = document.getElementById('kt_docs_formvalidation_text');
var validator = FormValidation.formValidation(
    form,
    {
        fields: {
            'SurveyId': {
                validators: {
                    notEmpty: {
                        message: 'Lütfen bir proje seçiniz!'
                    }
                }
            },
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

// Submit button handler
const submitButton = document.getElementById('kt_docs_formvalidation_text_submit');
submitButton.addEventListener('click', function (e) {
    e.preventDefault();
    if (validator) {
        validator.validate().then(function (status) {
            console.log('validated!');
            if (status == 'Valid') {
                submitButton.setAttribute('data-kt-indicator', 'on');
                submitButton.disabled = true;
                setTimeout(function () {

                    $.ajax({
                        url: 'puan-kontrol/CheckPointBySurveyId',
                        type: 'POST',
                        cache: false,
                        processData: false,
                        data: new FormData(form),
                        contentType: false,
                        success: function (data) { 
                            submitButton.removeAttribute('data-kt-indicator');
                            submitButton.disabled = false;
                            var responseData = data.getPointListBySurveyIdViewModels;
                            if (responseData != null) {
                                $('#kt_datatable_example_1').DataTable().destroy();
                                 
                                console.log(responseData)
                                showModal();
                                $("#kt_datatable_example_1").DataTable({
                                    data: responseData,
                                    columns: [
                                        { data: 'surveyUserGUID' },
                                        { data: 'surveyPoints' },
                                        { data: 'approved' },
                                        { data: 'active' },
                                        { data: 'text' },
                                        { data: 'timeStamp' },
                                        { data: 'updateTime' },
                                    ],
                                    columnDefs: [

                                        {
                                            targets: 2,
                                            render: function (data, type, row) {
                                                if (data == 1) {
                                                    return `<div class="badge badge-light-success" >Onaylandı</div>`
                                                } else {
                                                    return `<div class="badge badge-light-danger" >Onaylanmadı</div>`
                                                }
                                            }
                                        },

                                        {
                                            targets: 3,
                                            render: function (data, type, row) {
                                                if (data== 1) {
                                                    return `<div class="badge badge-light-success" >Aktif</div>`
                                                } else {
                                                    return `<div class="badge badge-light-danger" >Pasif</div>`
                                                }
                                            }
                                        },
                                        {
                                            targets: 5,
                                            render: function (data, type, row) {
                                                moment.locale("tr");
                                                return moment(data).format('lll'); 
                                            },
                                        },
                                        {
                                            targets:6,
                                            render: function (data, type, row) {
                                                moment.locale("tr");
                                                return moment(data).format('lll'); 
                                            },
                                        },
                                    ]
                                });
                            }
                            else {
                                Swal.fire({
                                    text: data.message,
                                    icon: "error",
                                    buttonsStyling: false,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn btn-danger"
                                    }
                                }).then((result) => {

                                    if (result.isConfirmed) {
                                        setTimeout(function () {
                                            location.reload(true)
                                        },
                                            2500);
                                    }
                                });

                            }
                        },
                        error: function (data) {
                            alert(data.message)
                            toastr.error(data.message, "Başarısız");
                            setTimeout(function () {
                                location.reload(true)
                            },
                                2000);
                        }
                    });

                }, 2000);
            }
        });
    }
});
