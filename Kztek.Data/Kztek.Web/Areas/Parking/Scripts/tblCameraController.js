$(function () {
    tblCameraController.LoadImage();

    $("body").on("change", "#CameraType", function () {
        tblCameraController.LoadImage();
    })

    $("body").on("click", "#btnSave", function () {
        $("#ResizeImage").val($("#size").val());

    })
    $("body").on("click", "#btnDel", function () {
        $("#ResizeImage").val("");
        $("#result").empty();
    })
})

var tblCameraController = {
    LoadImage: function () {
        var pcid = $("#PCID").val();
        var camid = $("#CameraID").val();

        $.ajax({
            url: _prefixParkingDomain + '/tblCamera/Partial_Image',
            type: 'POST',
            data: {
                camid: camid,
                pcid: pcid
            },
            success: function (response) {
                $('#boxCameraImage').html('');
                $('#boxCameraImage').html(response);

                tblCameraController.CropFunction();
            }
        });
    },
    CropFunction: function () {
        var image = document.querySelector('#image');
        var data = document.querySelector('#data');
        var cropBoxData = document.querySelector('#cropBoxData');
        var button = document.getElementById('btnCrop');
        var result = document.getElementById('result');
        var minAspectRatio = 0.5;
        var maxAspectRatio = 1.5;
        var cropper = new Cropper(image, {
            //ready: function (event) {
            //    // Zoom the image to its natural size
            //    cropper.zoomTo(1);
            //},
            ready: function () {
                var cropper = this.cropper;
                var containerData = cropper.getContainerData();
                var cropBoxData = cropper.getCropBoxData();
                var aspectRatio = cropBoxData.width / cropBoxData.height;
                var newCropBoxWidth;

                if (aspectRatio < minAspectRatio || aspectRatio > maxAspectRatio) {
                    newCropBoxWidth = cropBoxData.height * ((minAspectRatio + maxAspectRatio) / 2);

                    cropper.setCropBoxData({
                        left: (containerData.width - newCropBoxWidth) / 2,
                        width: newCropBoxWidth
                    });
                }
            },

            crop: function (event) {
                data.textContent = JSON.stringify(cropper.getData());

                cropBoxData.textContent = JSON.stringify(cropper.getCropBoxData());

                var cropBox = cropper.getData();

                var a = cropBox.x + "," + cropBox.y + "," + cropBox.width + "," + cropBox.height;

                $("#size").val(a);
               
            },

            zoom: function (event) {
                // Keep the image in its natural size
                if (event.detail.oldRatio === 1) {
                    event.preventDefault();
                }
            },
        });

        button.onclick = function () {
            result.innerHTML = '';
           
            result.appendChild(cropper.getCroppedCanvas({
                width: 160,
                height: 90,
                minWidth: 256,
                minHeight: 256,
                maxWidth: 4096,
                maxHeight: 4096,
                fillColor: '#fff',
                imageSmoothingEnabled: false,
                imageSmoothingQuality: 'high',
            }));
        };
    }
}