//Controlers urls


var url_flights = api_url + '/api/flights';
var url_agencies = api_url + '/api/agencies';
var url_hotels = api_url + '/api/hotels';
var url_clients = api_url + '/api/clients';
var url_reservations = api_url + '/api/reservations';
var url_guides = api_url + '/api/guides';
var url_invoice = api_url + '/api/Invoice'; 
var url_transportlists = api_url + '/api/transportlists';//url_flightLists
//var url_departures = api_url + '/api/departures';
var url_search = api_url + '/api/search';
var url_sales = api_url + '/api/sales';

var url_products = api_url + '/api/products';
var url_activities = api_url + '/api/activities';
var url_events = api_url + '/api/events';



// Common functions:

Date.prototype.yyyymmdd = function () {
    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = this.getDate().toString();
    return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]); // padding
};

String.prototype.HHmm = function () {
    var array = this.split(':');
    var HH = array[0];
    var mm = array[1];
    return (HH[1] ? HH : "0" + HH[0]) + ':' + (mm[1] ? mm : "0" + mm[0]); // padding
};

function isValidContainer(container_Id) {
    var isValid = true;

    var fg = $("#" + container_Id + " .form-group ");
    $(fg).removeClass("has-error");
    var inputs = $('input, select', fg);
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

//var container = $('#' + container_Id);
//var xxx = $(".form-group > input", container);
//var yyy = $( container, ".form-group > input");
////var inputs = $(".form-group > input");
//var fg_ = $("#modal_sale .form-group ");
//$(".form-group").removeClass("has-error");
//var inputs = $(".form-group input[type='text'],.form-group input[type='tel'],.form-group input[type='radio'],.form-group input[type='time'], .form-group input[type='number'], .form-group select");






function validateDateFormat(dateInput)
{
    var date_val = $(dateInput).val();
    //var regEx = /^\d{4}-\d{1,2}-\d{1,2}$/;
    var regEx = /^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$/;

    //var regEx = /^\d{4}-\d{2}-\d{2}$/;
    return date_val.match(regEx) != null;


}

function isValid() {
    var isValid = true;
    $(".form-group").removeClass("has-error");
    $(".form-control-feedback").removeClass("glyphicon-warning-sign");

    var inputs = $(".form-group input[type='text'],.form-group input[type='tel'],.form-group input[type='radio'],.form-group input[type='time'], .form-group input[type='number'], .form-group select");

    for (var i = 0; i < inputs.length; i++) {
        if (!inputs[i].validity.valid) {
            isValid = false;
            $(inputs[i]).closest(".form-group").addClass("has-error");
        }
    }

    var dateInputs = $('.date');
    for (var i = 0; i < dateInputs.length; i++) {
        if (!validateDateFormat(dateInputs[i]) && dateInputs[i].validity.valid) {
            isValid = false;
            $(dateInputs[i]).closest(".form-group").addClass("has-error");
            $(dateInputs[i]).next(".form-control-feedback").addClass("glyphicon-warning-sign");  
        }
    }


    //if (validateDateFormat(dateInput)) {
    //    isValid = false;
    //    $(inputs[i]).closest(".form-group").addClass("has-error");
    //}

    if (isValid) {
        return true;
    }
    else {
        return false
    }

}