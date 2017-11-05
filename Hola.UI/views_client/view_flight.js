
//var _url = api_url + '/api/flights';



//Constructor for an object with two properties
function Flight(data) {
    this.num = ko.observable(data.num);
    var d = new Date(data.date);
    this.date = ko.observable(d.yyyymmdd());
    var t = data.time;
    this.time = ko.observable(t.HHmm());

    this.destination = ko.observable( data.destination);
    this.direction = ko.observable(data.direction);
    //this.time_approved = data.time_approved;
    this.date_update = ko.observable(data.date_update);

    this.editable = ko.observable(false);
    this.editBtnClass = ko.observable('btn btn-circle btn-info');
    this.editBtnText = ko.observable('Edit');
};


function FlightViewModel(data) {

    var self = this;
    ///
    self.flights = ko.observableArray([]);

    var self_list = self.flights;

    self.new_num = ko.observable();
    self.new_date = ko.observable();
    self.new_time = ko.observable();
    self.new_destination = ko.observable();
    self.new_direction = ko.observable();
    self.new_time_approved = ko.observable();
    self.new_date_update = ko.observable();
    self.selected_flight = ko.observable();
    radioSelectedOptionValue: ko.observable();


    $.getJSON(url_flights, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Flight(item);
        });
        self.flights(mappedData);
        $('#trLoading').hide();
    });


    self.new_destination.subscribe(function (new_) {
        switch (new_) {
            case 'BCN':
            case 'Valencia':
                self.new_direction('IN');
                return;
            case 'TLV':
                self.new_direction('OUT');
            default:

        }

    });

    //Operations
    self.add_server = function () {

        if (isValid()) {
            var d = new Date(self.new_date());
            var new_obj = {
                num: self.new_num(),
                date: d.yyyymmdd(),
                time: self.new_time(),
                destination: self.new_destination(),
                direction: self.new_direction(),

            };

            $.post(url_flights, new_obj, function (obj_from_server) {

                new_obj.editBtnText = ko.observable('Edit');
                new_obj.editBtnClass =ko.observable( 'btn btn-circle btn-info');
                new_obj.editable = ko.observable(false);
                new_obj.time = ko.observable(self.new_time());
                new_obj.num = ko.observable(self.new_num());
                new_obj.date = ko.observable(self.new_date());
                self_list.unshift(new_obj);
                self.new_num('');
                self.new_date('');
                self.new_time('');
                self.new_destination('');
                self.new_direction('');

            });
        }
    };



    self.edit_mode = function () {
        //this.editable(!this.editable());
        //If edite mode, we want to save it
        if (this.editable() == true) {

            self.UpdateFlight(this);
            this.editBtnText('Edit');
            this.editBtnClass('btn btn-circle btn-info');
        }
        else {
            this.editable(true);
            this.editBtnText('Save');
            this.editBtnClass('btn btn-circle btn-success');
        }
    };

    self.UpdateFlight = function (item) {

       // var _url = url_flights + '/UpdateFlight/' + item.num() + '/' + item.date() + '/' + item.time();
        var _url = url_flights + '/UpdateFlight';// + item.num() + '/' + item.date() + '/' + item.time();
        //var flight = {
        //    num: item.num(),
        //    date: item.date(),
        //    time:item.time()
        //};
        $.ajax({
            method: "PUT",
            url: _url,
            data: item,
            dataType: "json",
        }).done(function (result) {

            //var guide_fk = item.guide_fk();
            //var match = ko.utils.arrayFirst(self.guides(), function (guide) {
            //    return guide.ID() === guide_fk;
            //});
            //item.guide_name(match.name());
            //item.time(result.time);
         
            item.editable(false);

            //self.feedback_sale('');
        }).fail(function (error) {
            // self.feedback_sale(error.responseText)
            alert(error.responseText);
        });
    }











    self.DeleteFlight = function () {
        if (confirm('Are you sure you want to delete this row?')) {
            var item = this;
            var _url = url_flights + '/DeleteFlight/' + item.num() + '/' + item.date();

            $.ajax({
                method: "DELETE",
                url: _url,
                //data: item,
                dataType: "json",
            }).done(function () {

                self.flights.remove(item);
                //self.feedback_sale('');
            }).fail(function (error) {
                alert(error.responseText);
                // self.feedback_sale(error.responseText);
            })
        }

    }


}

// Activates knockout.js
ko.applyBindings(new FlightViewModel());



