

var _url_activities = url_activities + '/transport/' + DIRECTION;
var _url_flights = url_transportlists + '/getFlights/' + DIRECTION;
var _url_event_post = url_events;

//var _url_activities = url_activities + '/transport/out';
//var _url_event_post = url_events;//+ '//';
//var _url_print = '../print/ListDeparture_Print.aspx?';
//var _url_flights = url_flights + '/GetFlightTransportDep/' + self.date_start() + '/' + newValue;
//api/transportlists/GetFlights/{date}/{activity_fk}/{direction}

function Flight(data) {
    this.num = ko.observable(data.num);
    var _date = new Date(data.date).yyyymmdd();
    this.date = ko.observable(_date);
    this.time = ko.observable(data.time.HHmm());
    this.sum = ko.observable(data.sum);
    this.selected = ko.observable(false);
}

function Activity(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
}

function Guide(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
}

function Event(data) {
    this.ID = ko.observable(data.ID);
    var d = new Date(data.date);
    this.date = ko.observable(d.yyyymmdd());
    var _t;
    if (data.time) {
        _t=data.time.HHmm();
    }
    this.time = ko.observable(_t);
    this.activity_fk = ko.observable(data.activity_fk);
    this.activity_name = ko.observable(data.activity_name);
    this.guide_fk = ko.observable(data.guide_fk);
    this.guide_name = ko.observable(data.guide_name);
    this.comments = ko.observable(data.comments);
    this.people = ko.observable(data.people);
    this.selected = ko.observable(false);
}

function Plan(data, isEditMode) {

    this.event_fk = ko.observable(data.event_fk);
    this.hotel_name = ko.observable(data.hotel_name);
    this.hotel_fk = ko.observable(data.hotel_fk);
    var _t;
    if (data.time) {
        _t = data.time.HHmm();
    }
    this.time = ko.observable(_t);
    this.PAX = ko.observable(data.PAX);
    this.editable = ko.observable(isEditMode);
}



function FlightViewModel(data) {

    var self = this;
    ///

    self.result = ko.observable('');

    self.flights = ko.observableArray([]);
    self.activities = ko.observableArray([]);
    self.guides = ko.observableArray([]);
    self.events = ko.observableArray([]);


    //self.date_end = ko.observable();
    self.print_url = ko.observable();

    self.date_start = ko.observable();
    self.activity_fk = ko.observable();
    self.time = ko.observable();
    self.guide_fk_selected = ko.observable();
    self.comments_trans = ko.observable();

    self.event_fk_selected = ko.observable(0);
    self.count_create = ko.observable(0);

    //For Depart///
    //if (DIRECTION='OUT') {
    //    self.time('00:00');
    //}
    self.earliest_flight = ko.observable();
    self.plans = ko.observableArray([]);

    self.GetCreatePlan = function () {

        //if (!isValidContainer('divCreatePlan') && self.event_fk_selected() == 0) {
        //    return false;
        //}
        if (self.event_fk_selected() == 0) {
            return false;
        }
        else {

            var url = url_transportlists + '/GetCreatePlan';
            var _details = {
                activity_fk: self.activity_fk(),
                event_fk: self.event_fk_selected(),
                date: self.date_start(),
                flights: self.selected_flights(),
                direction: 'out'
            };

            $.ajax({
                method: "POST",
                url: url,
                data: _details,
                dataType: "json",
            })
      .done(function (result) {
          var mappedData = $.map(result, function (item) {
              return new Plan(item, true);
          });

          if (mappedData.length > 0) {
              self.plans(mappedData);
              var _event_fk = self.plans()[0].event_fk();
              self.event_fk_selected(_event_fk);
          }
      });
        }
    }

    $.getJSON(_url_activities, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Activity(item);
        });
        self.activities(mappedData);
    });

    self.GetDepartPlan = function () {
        var _url = url_transportlists + '/GetDepartPlan/' + self.event_fk_selected()
        $.getJSON(_url, function (data) {
            var mappedData = $.map(data, function (item) {
                var edit_mode = false;
                if (item.time==undefined) {
                    edit_mode = true;
                }
                return new Plan(item, edit_mode);
            });
            self.plans(mappedData);
        });
    };

    self.updateTime = function () {

        var plan = this;
        var new_time = this.time();
        if (new_time) {

            var line = {
                hotel_fk: plan.hotel_fk(),
                event_fk: self.event_fk_selected(),
                time: plan.time(),
            };
            var _url = url_transportlists + '/UpdateDepartPlan';
            $.ajax({
                method: "PUT",
                url: _url,
                data: line,
                dataType: "json",
            }).done(function (result) {
                plan.editable(false);
                $('#lnk_sort').show();
            });
        }
        else {
            //TODO: requeired
            //alert('error');
        }
    };


    self.UpdateDepartEventTime = function () {
 
        var _url = url_transportlists + '/UpdateDepartEventTime?event_fk=' + self.event_fk_selected();
        $.ajax({
            method: "PUT",
            url: _url,
            //data: {event_fk:self.event_fk_selected()},
            dataType: "json",
        }).done(function (result) {

            self.result('success');
        }).fail(function (error) {
            self.result(error.responseText);
        });
        
    };



    //END Depart//





    self.AssignPassengers = function () {
        var item = this;
        var _url = url_transportlists + "/AssignPassengers";
        //if (isValidContainer('arrivals_wizard')) {




        //TODO: debug this!!!!



        var _details = {
            date: self.date_start(),
            time: self.time(),
            activity_fk: self.activity_fk(),
            guide_fk: item.guide_fk(),
            comments_trans: self.comments_trans(),
            event_fk: item.ID(),
            flights: self.selected_flights(),
            direction: DIRECTION,
        };

        $.ajax({
            method: "PUT",
            url: _url,
            data: _details,
            dataType: "json",
        }).done(function (results) {
            self.event_fk_selected(item.ID());
            var mappedData = $.map(results, function (item) {
                return new Event(item);
            });
            self.events(mappedData);
            item.selected(true);
        }).fail(function (error) {
            alert(error.responseText);
            // self.feedback_sale(error.responseText);
        })
        //}

    };


    self.getEarliestFlight = function () {

        self.earliest_flight( self.selected_flights()[0].time().HHmm())
    };



    self.selected_flights = ko.computed(function () {
        var arr = self.flights();
        return ko.utils.arrayFilter(arr, function (flight) {
            return flight.selected();
        });
    });



    $.getJSON(_url_activities, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Activity(item);
        });
        self.activities(mappedData);
    });

    $.getJSON(url_guides, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Guide(item);
        });
        self.guides(mappedData);
    });


    self.date_start.subscribe(function (newValue) {
        //var _url = _url_flights_in + newValue;
        //$.getJSON(_url, function (allData) {
        //    var mappedData = $.map(allData, function (item) {
        //        return new Flight(item);
        //    });
        //    self.flights(mappedData);

        //});
        //self.update_print_url();

    });

    self.activity_fk.subscribe(function (activity_fk) {

        var _url_f = _url_flights + '/' + activity_fk + '/' + self.date_start();

        $.getJSON(_url_f, function (allData) {
            var mappedData = $.map(allData, function (item) {
                return new Flight(item);
            });
            self.flights(mappedData);

        });

        self.getEvents(activity_fk);
        self.event_fk_selected(0);

        self.update_print_url();

    });


    self.getEvents =  function(_activity_fk)
    {
        var _url_e = url_events + '/GetEventsWithActivities/' + self.date_start() + '/' + _activity_fk;

        $.getJSON(_url_e, function (allData) {
            var mappedData = $.map(allData, function (item) {
                return new Event(item);
            });
            self.events(mappedData);
        });

    }


    self.event_fk_selected.subscribe(function (newValue) {
        if (newValue == 0) {
            $('.conreq').prop('required', true);
        }
        else {
            $('.conreq').prop('required', false);
            //$('.conreq').removeAttr('required', 'required');
        }
    });

    self.required = ko.computed(function () {
        if (self.event_fk_selected == 0) {
            return 'required';
        }
    });




    self.select = function (flight) {
        flight.selected(!flight.selected())
        self.update_print_url();

    };

    //self.date_end.subscribe(function (new_date_end) {
    //    self.update_print_url();
    //});

    self.updateSaleActivties = function () {
        var item = this;
        var _url = url_events + '/transport/' + self.date_start() + '/' + self.activity_fk() + '/' + item.ID();
        $.ajax({
            method: "PUT",
            url: _url,
            dataType: "json",
        }).done(function (results) {

            var mappedData = $.map(results, function (item) {
                return new Event(item);
            });
            self.events(mappedData);

            //debugger;
            //item.people(results);
            //self.feedback_sale('');
        }).fail(function (error) {
            alert(error.responseText);
            // self.feedback_sale(error.responseText);
        })

    }


    self.edit_mode = function () {

        this.editable(!this.editable());
    };


    self.update_print_url = function () {


        if (self.date_start() && self.selected_flights().length > 0) {


            //var url = _url_print +
            //            'date_start=' + self.date_start() +
            //            //'&date_end=' + self.date_end() +
            //            //'&flights=' + self.get_flights_str();

            //self.print_url(url);
        }
    };


    self.total_sum = ko.computed(function () {

        if (self.selected_flights().length > 0) {
            var total = 0;
            for (var p = 0; p < self.selected_flights().length; ++p) {

                total += self.selected_flights()[p].sum();
            }
            return total;
        }
        else return 0;

    });


    self.addEvent = function () {

        if (isValidContainer('list_wizard')) {
 
            var _event = {
                date: self.date_start(),
                time: self.time(),
                activity_fk: self.activity_fk(),
                guide_fk: self.guide_fk_selected(),
                comments: self.comments_trans(),
                category: 'transport'
            };

            $.post(url_events, _event, function (new_event) {
               
                self.getEvents(self.activity_fk())


                //var locaNewEvent = new Event(new_event)
                //locaNewEvent.guide_name(self.guide_fk_selected())
                //locaNewEvent.activity_name(self.activity_fk())
                //self.events.push(locaNewEvent);
                //self.event_fk_selected(new_event.ID);
                //alert(new_event.ID);
            }).done(function () {
                self.count_create(1);
                // alert('done');
                // self.feedback_sale('');
            })
            .fail(function (error) {
                alert(error.responseText);
                // self.feedback_sale(error.responseText);
            });
        }
    };

}












