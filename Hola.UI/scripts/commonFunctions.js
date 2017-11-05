Date.prototype.yyyymmdd = function () {
    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = this.getDate().toString();
    return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]); // padding
};


function isValid() {
    var isValid = true;
    $(".form-group").removeClass("has-error");
    var inputs = $(".form-group input[type='text'],.form-group input[type='tel'],.form-group input[type='radio'],.form-group input[type='time'], .form-group select");

    for (var i = 0; i < inputs.length; i++) {
        if (!inputs[i].validity.valid) {
            isValid = false;
            $(inputs[i]).closest(".form-group").addClass("has-error");
        }
    }


    if (isValid) {
        return true;
    }
    else {
        return false
    }

}