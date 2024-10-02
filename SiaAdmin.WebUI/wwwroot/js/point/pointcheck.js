
const form = document.getElementById('kt_docs_formvalidation_text');
var validator = FormValidation.formValidation(
    form,
    {
        fields: {
            'SurveyUserGUID': {
                validators: {
                    notEmpty: {
                        message: 'Kullanıcı Internal GUID Bilgisi Zorunludur!'
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
                        url: 'puan-kontrol/CheckPoint',
                        type: 'POST', 
                        cache: false, 
                        processData: false,
                        data: new FormData(form),
                        contentType: false,
                        success: function (data) {
                            submitButton.removeAttribute('data-kt-indicator');
                            submitButton.disabled = false;
                           
                            if (data.responseCode === 202) {
                               
                                Swal.fire({
                                    text: "Toplam puanı: "+data.totalPoint,
                                    icon: "success",
                                    buttonsStyling: false,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn btn-success"
                                    }
                                }).then((result) => {

                                    if (result.isConfirmed) {
                                        setTimeout(function () {
                                            location.reload(true)
                                        },
                                             500);
                                    }
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
                                            1500);
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
