var KTUsersAddUser = function () {
    const t = document.getElementById("kt_modal_add_user"),
        e = t.querySelector("#kt_modal_add_user_form"),
        n = new bootstrap.Modal(t);
    return {
        init: function () {
            (() => {
                var o = FormValidation.formValidation(e, {
                    fields: {
                        SurveyId: {
                            validators: {
                                notEmpty: {
                                    message: "Proje Seçilmesi Zorunludur"
                                }
                            }
                        },
                        SurveyPoints: {
                            validators: {
                                notEmpty: {
                                    message: "Proje Puani  Zorunludur"
                                }
                            }
                        },
                        ExcelFile: {
                            validators: {
                                notEmpty: {
                                    message: "Internal GUID  Alani Zorunludur"
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
                });
                const i = t.querySelector('[data-kt-users-modal-action="submit"]');
                i.addEventListener("click", (t => {
                    t.preventDefault(), o && o.validate().then((function (t) {
                        console.log("validated!"), "Valid" == t ? (i.setAttribute("data-kt-indicator", "on"), i.disabled = !0,
                            setTimeout((function () {
                                var a = document.forms["kt_modal_add_user_form"]["SurveyStartDate"].value;
                                var b = document.forms["kt_modal_add_user_form"]["SurveyValidity"].value;
                                ajaxPost(a, b);
                                i
                            }))) : Swal.fire({
                                text: "Lütfen zorunlu alanları doldurunuz!",
                                icon: "error",
                                buttonsStyling: !1,
                                confirmButtonText: "Tamam!",
                                customClass: {
                                    confirmButton: "btn btn-primary"
                                }
                            })
                    }))
                })), t.querySelector('[data-kt-users-modal-action="cancel"]').addEventListener("click", (t => {
                    t.preventDefault(), Swal.fire({
                        text: "Formu İptal Etmek İstiyormusunuz?",
                        icon: "warning",
                        showCancelButton: !0,
                        buttonsStyling: !1,
                        confirmButtonText: "Evet!",
                        cancelButtonText: "Hayır",
                        customClass: {
                            confirmButton: "btn btn-primary",
                            cancelButton: "btn btn-active-light"
                        }
                    }).then((function (t) {
                        t.value ? (e.reset(), n.hide()) : "cancel" === t.dismiss && Swal.fire({
                            text: "Form İptal Edildi.",
                            icon: "error",
                            buttonsStyling: !1,
                            confirmButtonText: "Tamam!",
                            customClass: {
                                confirmButton: "btn btn-primary"
                            }
                        })
                    }))
                })), t.querySelector('[data-kt-users-modal-action="close"]').addEventListener("click", (t => {
                    t.preventDefault(), Swal.fire({
                        text: "Formu İptal Etmek İstiyormusunuz?",
                        icon: "warning",
                        showCancelButton: !0,
                        buttonsStyling: !1,
                        confirmButtonText: "Evet!",
                        cancelButtonText: "Hayır",
                        customClass: {
                            confirmButton: "btn btn-primary",
                            cancelButton: "btn btn-active-light"
                        }
                    }).then((function (t) {
                        t.value ? (e.reset(), n.hide()) : "cancel" === t.dismiss && Swal.fire({
                            text: "Form İptal Edilmedi!.",
                            icon: "error",
                            buttonsStyling: !1,
                            confirmButtonText: "Tamam!",
                            customClass: {
                                confirmButton: "btn btn-primary"
                            }
                        })
                    }))
                }))
            })()
        }
    }
}();
KTUtil.onDOMContentLoaded((function () {
    KTUsersAddUser.init()
}));

function ajaxPost(a, b) {
    if (a == null || a == "") {
        Swal.fire({
            text: "Proje Başlangıç Tarihi alanı boş bırakılarak devam edilcektir",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "Tamam!",
            customClass: {
                confirmButton: "btn btn-primary"
            }
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'proje-atama-listesi/proje-ata',
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: new FormData(document.getElementById("kt_modal_add_user_form")),
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        if (data.succeeded== true) {
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
                                text: data.responseJSON.Errors,
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
                        Swal.fire({
                            text: data.responseJSON.Errors,
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
                });
            }
        });
    }
    if (b == null || b == "") {
        Swal.fire({
            text: "Proje Geçerlilik Tarihi alanı boş bırakılarak devam edilcektir",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "Tamam!",
            customClass: {
                confirmButton: "btn btn-primary"
            }
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'proje-atama-listesi/proje-ata',
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: new FormData(document.getElementById("kt_modal_add_user_form")),
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        if (data.succeeded== true) {
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
                                text: data.responseJSON.Errors,
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
                        
                        Swal.fire({
                            text: data.responseJSON.Errors,
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
                });
            }
        });
    }
    if ((a == null || a == "") && (b == null || b == "")) {
        Swal.fire({
            text: "Proje Başlangıç ve Geçerlilik Tarihi alanı boş bırakılarak devam edilcektir",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "Tamam!",
            customClass: {
                confirmButton: "btn btn-primary"
            }
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'proje-atama-listesi/proje-ata',
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: new FormData(document.getElementById("kt_modal_add_user_form")),
                    processData: false,
                    contentType: false,
                    success: function (data) { 
                        if (data.succeeded == true) { 
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
                                text: data.responseJSON.Errors,
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
                        console.log(data)
                        Swal.fire({
                            text: data.responseJSON.Errors,
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
                });
            }
        });
    }
    if (!!a && !!b) {
        $.ajax({
            url: 'proje-atama-listesi/proje-ata',
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: new FormData(document.getElementById("kt_modal_add_user_form")),
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.succeeded== true) {
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
                        text: data.responseJSON.Errors,
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
               
                Swal.fire({
                    text: data.responseJSON.Errors,
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
    }
}