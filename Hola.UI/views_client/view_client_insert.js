
function Flight(data) {
    this.num = ko.observable(data.num);
    var _date = new Date(data.date).yyyymmdd();
    this.date = ko.observable(_date);
    var t = data.time;
    this.time = ko.observable(t.HHmm());
    //this.time = ko.observable(data.time);
    this.direction = ko.observable(data.direction);

    this.details = ko.dependentObservable(function () {
        return (this.date() + ' - ' + this.num() + ' - ' + this.time())
    }, this);
}

function Agency(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}

function Hotel(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}


function Product(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.rate = ko.observable(data.rate);
    this.category = ko.observable(data.category);
}





var ReservationViewModel = function () {


    var self = this;
    //Arrays:
    self.agencies = ko.observableArray([]);
    self.hotels = ko.observableArray([]);
    self.flights = ko.observableArray([]);
    self.products = ko.observableArray([]);


    //fields - input-text
    self.date_arr = ko.observable()
    self.date_dep = ko.observable()

    self.PNR = ko.observable();
    self.names = ko.observable();
    self.PAX = ko.observable();
    self.phone = ko.observable();
    self.oneway = ko.observable(false);
    self.comments = ko.observable();

    //fields - selected values
    self.num_arr = ko.observable();
    self.num_dep = ko.observable();
    self.agency_fk = ko.observable();
    self.hotel_fk = ko.observable();

    self.trans_product_fk = ko.observable('1');

    ///AGENCY METHODS:///
    //get agencies from server:
    $.getJSON(url_agencies, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Agency(item);
        });
        self.agencies(mappedData);
    });

    ///HOTEL METHODS:///
    //get agencies from server:
    $.getJSON(url_hotels, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Hotel(item);
        });
        self.hotels(mappedData);
    });

    ///FLIGHT METHODS:///
    //get flights from server:
    $.getJSON(url_flights, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Flight(item);
        });
        self.flights(mappedData);
    });

    //flights arrival + date filter:
    self.flights_filter_arr = ko.computed(function () {
        return ko.utils.arrayFilter(self.flights(), function (flight) {
            return flight.date() == self.date_arr() && flight.direction() == 'IN';
        });
    });

    //flights departure + date filter:
    self.flights_filter_dep = ko.computed(function () {
        return ko.utils.arrayFilter(self.flights(), function (flight) {
            return flight.date() == self.date_dep() && flight.direction() == 'OUT';
        });
    });

    //get products from server:
    $.getJSON(url_products, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Product(item);
        });
        self.products(mappedData);
    });

    self.products_trans = ko.computed(function () {
        return ko.utils.arrayFilter(self.products(), function (product) {
            return product.category() == 'transport';
        });
    });



    self.arrival_validation = function () {

        if (!self.date_arr()) {
            popMessage('info', 'Please choose <b>Arrival Date</b> first');
        }
        else if (self.flights_filter_arr().length == 0) {
            popMessage('warning', 'There are no arrival flights on the selected date. Please choose another date or <a target="_blank" href="../pages/flights.aspx">click here to add a new flight</a>');
        }

    };


    self.departure_validation = function () {

        if (!self.date_dep()) {
            popMessage('info', 'Please choose <b>Departure Date</b> first');
        }
        else if (self.flights_filter_dep().length == 0) {
            popMessage('warning', 'There are no departure flights on the selected date. Please choose another date or <a target="_blank" href="../pages/flights.aspx">click here to add a new flight</a>');
        }

    };

    self.date_arr.subscribe(function (new_) {


    });


    function popMessage(severity, html) {
        var div; var clone;
        switch (severity) {
            case 'success':
                div = $("#system_feedback").children("div#message_success");
                break;
            case 'info':
                div = $("#system_feedback").children("div#message_info");
                break;
            case 'warning':
                div = $("#system_feedback").children("div#message_warning");
                break;
            case 'danger':
                div = $("#system_feedback").children("div#message_danger");
                break;
            default:
                break;
        }

        var old_messages = $("div#system_feedback").children('div');
        $.each(old_messages, function (index, value) {
            if ($(this).attr('id') == '') {
                $(this).css('opacity', '0.3');
            }

        });

        var clone = $(div).clone().attr('id', '').show('fast');
        if (html != undefined) {
            $(clone).append(html);
        }
        $("#system_feedback").prepend(clone);

    }




    self.validate = function () {



        var count = 0;
        $('[required=required]').each(function (index) {

            if ($(this).val() == '0' || $(this).val() == '' || $(this).val() == undefined) {
                $(this).parent().addClass('has-error');
                count++;
            }
            else {
                $(this).parent().removeClass('has-error');

            }
        });
        if (count > 0) {
            popMessage('danger', 'Please fill out all required fields.')
            return false;
        }
        else {

            return true
            ;
        }

    };



    //// Operations
    self.insert_client = function () {

        if (self.validate()) {
            var new_obj = {

                PNR: self.PNR(),
                names: self.names(),
                PAX: self.PAX(),
                phone: self.phone(),
                num_arr: self.num_arr(),
                date_arr: self.date_arr(),
                num_dep: self.num_dep(),
                date_dep: self.date_dep(),
                hotel_fk: self.hotel_fk(),
                agency_fk: self.agency_fk(),
                //oneway: self.oneway(),
                product_fk: self.trans_product_fk(),
                comments: self.comments(),



            };


            $.post(url_reservations, new_obj)
            .done(function (obj_from_server) {
                popMessage('success');
                document.getElementById("form_client").reset();
                $('#txtComments').text('');
            })
            .fail(function (error) {
                popMessage('danger', error.responseText);
            });


        }

    };



};
//// Activates knockout.js
//ko.applyBindings(new ReservationViewModel());







