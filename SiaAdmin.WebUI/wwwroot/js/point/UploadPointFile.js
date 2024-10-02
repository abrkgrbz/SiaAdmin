// set the dropzone container id
const id = "#point_upload";
const dropzone = document.querySelector(id);

// set the preview element template
var previewNode = dropzone.querySelector(".dropzone-item");
previewNode.id = "";
var previewTemplate = previewNode.parentNode.innerHTML;
previewNode.parentNode.removeChild(previewNode);

var myDropzone = new Dropzone(id, { 
    url: "puan-yukle/UploadPoint",
    paramName:"ExcelFile",
    parallelUploads: 1,
    previewTemplate: previewTemplate,
    maxFilesize: 2,  
    acceptedFiles:"application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    autoQueue: false,  
    dictDefaultMessage: 'Yüklemek istediğiniz dosyaları buraya bırakın',
    dictFallbackMessage: "Tarayıcınız sürükle bırak yüklemelerini desteklemiyor",
    dictFileTooBig: "Dosya boyutu çok büyük ({{filesize}} Mb). Yükleyebileceğiniz en büyük dosya boyutu: {{maxFilesize}} Mb.",
    dictInvalidFileType: "Bu tür dosyaları yükleyemezsiniz",
    dictResponseError: "Sunucu hatası. Hata kodu : {{statusCode}}",
    dictCancelUpload: "Yüklemeyi İptal Et",
    dictUploadCanceled: "Yükleme iptal edildi",
    dictCancelUploadConfirmation: "Bu yüklemeyip iptal etmek istediğinizden emin misiniz ?",
    dictRemoveFile: "Dosyayı Sil",
    dictMaxFilesExceeded: "Başka dosya yükleyemezsiniz.",
    previewsContainer: id + " .dropzone-items",  
    clickable: id + " .dropzone-select"  
});
myDropzone.on('error', function (file, response) {
    
    $(file.previewElement).find('.dropzone-error').text(response.Errors);
});
myDropzone.on("addedfile", function (file) {
    // Hookup the start button
   
    file.previewElement.querySelector(id + " .dropzone-start").onclick = function () { myDropzone.enqueueFile(file); };
    const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
    dropzoneItems.forEach(dropzoneItem => {
        dropzoneItem.style.display = '';
    });
    dropzone.querySelector('.dropzone-upload').style.display = "inline-block";
    dropzone.querySelector('.dropzone-remove-all').style.display = "inline-block";
});

// Update the total progress bar
myDropzone.on("totaluploadprogress", function (progress) {
    const progressBars = dropzone.querySelectorAll('.progress-bar');
    progressBars.forEach(progressBar => {
        progressBar.style.width = progress + "%";
    });
});

myDropzone.on("sending", function (file) {
    // Show the total progress bar when upload starts
    const progressBars = dropzone.querySelectorAll('.progress-bar');
    progressBars.forEach(progressBar => {
        progressBar.style.opacity = "1";
    });
    // And disable the start button
    file.previewElement.querySelector(id + " .dropzone-start").setAttribute("disabled", "disabled");
});

// Hide the total progress bar when nothing's uploading anymore
myDropzone.on("complete", function (progress) {
    const progressBars = dropzone.querySelectorAll('.dz-complete');
    
    setTimeout(function () {
        progressBars.forEach(progressBar => {
            progressBar.querySelector('.progress-bar').style.opacity = "0";
            progressBar.querySelector('.progress').style.opacity = "0";
            progressBar.querySelector('.dropzone-start').style.opacity = "0";
        });
    }, 300);
});

 
dropzone.querySelector(".dropzone-upload").addEventListener('click', function () {
    myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED));
});

 
dropzone.querySelector(".dropzone-remove-all").addEventListener('click', function () {
    dropzone.querySelector('.dropzone-upload').style.display = "none";
    dropzone.querySelector('.dropzone-remove-all').style.display = "none";
    myDropzone.removeAllFiles(true);
});

 
myDropzone.on("queuecomplete", function (progress) { 
    const uploadIcons = dropzone.querySelectorAll('.dropzone-upload');
    uploadIcons.forEach(uploadIcon => {
        uploadIcon.style.display = "none";
    });
});

 
myDropzone.on("removedfile", function (file) {
    if (myDropzone.files.length < 1) {
        dropzone.querySelector('.dropzone-upload').style.display = "none";
        dropzone.querySelector('.dropzone-remove-all').style.display = "none";
    }
});