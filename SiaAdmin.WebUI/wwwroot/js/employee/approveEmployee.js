$('div.modal').on('shown.bs.modal', function (t) {

    var id = $(this).attr('id');
     
});


var form = document.querySelector("#form_approve_emploee");
var submitButton = form.querySelector("#approve_employee_submit");
var cancelButton = form.querySelector("#approve_employee_cancel");
cancelButton.addEventListener("click", function (t) {
    t.preventDefault();
    form.reset();

});
submitButton.addEventListener('click', function (e) {

    e.preventDefault();
    submitButton.setAttribute('data-kt-indicator', 'on');
    submitButton.disabled = true;
    setTimeout(function () {

        $.ajax({
            url: '',
            type: 'POST',
            cache: false,
            processData: false,
            data: new FormData(form),
            contentType: false,
            success: function (data) {
                submitButton.removeAttribute('data-kt-indicator');
                submitButton.disabled = false;

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
});