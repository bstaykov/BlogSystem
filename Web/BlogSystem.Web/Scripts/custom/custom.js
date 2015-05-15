function checkImageSize(inputId, maxSize) {
    var input = document.getElementById(inputId);
    if (input.files && input.files.length == 1) {
        if (input.files[0].size > maxSize) {
            alert("Picture must be less than 2MB.");
            return false;
        }
    }
    return true;
}

var validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];

function checkFileExtension(inputId) {
    var input = document.getElementById(inputId);
    if (input.type == "file") {
        var fileName = input.value;
        if (fileName.length > 0) {
            var isValid = false;
            for (var j = 0; j < validFileExtensions.length; j++) {
                var currentExtension = validFileExtensions[j];
                if (fileName.substr(fileName.length - currentExtension.length, currentExtension.length).toLowerCase() == currentExtension.toLowerCase()) {
                    isValid = true;
                    break;
                }
            }

            if (!isValid) {
                alert("Sorry, " + fileName + " is invalid, allowed extensions are: " + validFileExtensions.join(", "));
                return false;
            }
        }
    }

    return true;
}