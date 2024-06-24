
const form = document.getElementById('point_add_form');
var validator = FormValidation.formValidation(
    form,
    {
        fields: {
            'blacklist_excel_file': {
                validators: {
                    notEmpty: {
                        message: 'Lütfen Bir Dosya Seçiniz!'
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

const submitButton = document.getElementById('submitPointExcelFile');
submitButton.addEventListener('click', function (e) {
    e.preventDefault();
    if (validator) {
        validator.validate().then(function (status) {
            if (status == 'Valid') {
                submitButton.setAttribute('data-kt-indicator', 'on');
                submitButton.disabled = true;
                setTimeout(function () {
                    $.ajax({
                        url: '@Url.Action("SaveBlockListExcel", "BlockUser")',
                        type: 'POST',
                        dataType: 'json',
                        cache: false,
                        data: new FormData(document.getElementById("point_add_form")),
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            if (data.response == 200) {
                                Swal.fire({
                                    text: data.message,
                                    icon: "success",
                                    buttonsStyling: !1,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn btn-primary"
                                    }
                                }).then((function (t) {
                                    location.reload(true)
                                    t.isConfirmed && n.hide()
                                }))
                            } else {
                                Swal.fire({
                                    text: data.message + "" + data.list,
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn btn-primary"
                                    }
                                }).then((function (t) {
                                    location.reload(true)
                                    t.isConfirmed && n.hide()
                                }))
                            }

                        },
                        error: function (data) {
                            alert(data.error)
                            console.log(data)
                            Swal.fire({
                                text: data.message,
                                icon: "danger",
                                buttonsStyling: !1,
                                confirmButtonText: "Tamam",
                                customClass: {
                                    confirmButton: "btn btn-primary"
                                }
                            }).then((function (t) {
                                location.reload(true)
                                t.isConfirmed && n.hide()
                            }))
                        }
                    });
                    submitButton.removeAttribute('data-kt-indicator');
                    submitButton.disabled = false;
                }, 2000);
            }
        });
    }
});