
var _url_activities = url_activities + '/' + CATEGORY;
var _url_events = url_events + '/GetEventsList/' + CATEGORY + '/114';

function Event(data) {

    this.ID = ko.observable(data.ID);
    var _d = new Date(data.date);
    this.date = ko.observable(_d.yyyymmdd());
    var _t;
    if (data.time) {
        _t = data.time.HHmm();
    }
    this.time = ko.observable(_t);
    this.activity_fk = ko.observable(data.activity_fk);
    this.activity_name = ko.observable(data.activity_name);
    this.guide_fk = ko.observable(data.guide_fk);
    this.guide_name = ko.observable(data.guide_name);
    this.comments = ko.observable(data.comments);
    var d_u = new Date(data.date_update);
    this.date_update = ko.observable(d_u.yyyymmdd());
    this.direction = ko.observable(data.direction);
    this.people = ko.observable(data.people);

    this.editable = ko.observable(false);
    this.editBtnClass = ko.observable('fa fa-pencil fa-fw');
    this.editBtnText = ko.observable('');

    this.print_url = ko.computed(function () {
        var _url_print_base = "../print/ListTrans_" + data.direction + "_print.aspx?";
        return _url_print_base +
       "event_fk=" + data.ID +
       "&date=" + data.date +
       "&time=" + data.time +
       "&activity_fk=" + data.activity_fk +
       "&guide_fk=" + data.guide_fk +
       "&comments=" + data.comments;
    });
    this.split_url = ko.computed(function () {
        return "EventSplit.aspx?ORIGINAL_LIST=" + data.ID;
    });
}

function Activity(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
}

function Guide(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
}



function AppEventModel(data) {
    var self = this;

    self.activities = ko.observableArray([]);
    self.events = ko.observableArray([]);

    self.guides = ko.observableArray([]);


    //self.new_ID = ko.observable();
    self.new_date = ko.observable();
    self.new_time = ko.observable();
    self.new_activity_fk = ko.observable();
    self.new_guide_fk = ko.observable();
    self.new_comments = ko.observable();

    $.getJSON(_url_activities, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Activity(item);
        });
        self.activities(mappedData);
    });



    $.getJSON(_url_events, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Event(item);
        });
        $('#trLoading').hide();
        self.events(mappedData);
    });

    $.getJSON(url_guides, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Guide(item);
        });
        self.guides(mappedData);
    });


    //// Operations
    self.add_server = function () {

        if (isValid()) {
            var _event = {
                //ID: self.new_ID(),
                date: self.new_date(),
                time: self.new_time(),
                activity_fk: self.new_activity_fk(),
                guide_fk: self.new_guide_fk(),
                comments: self.new_comments(),
                category: CATEGORY

            };
            $.post(url_events, _event, function (event_from_server) {

                _event.ID = event_from_server.ID;
                _event.date_update = Date.now();

                var select_str_activity = "#ddlActivities option[value='" + self.new_activity_fk() + "']"
                var _activity_name = $(select_str_activity).text()
                _event.activity_name = _activity_name;

                var select_str_guide = "#ddlGuides option[value='" + self.new_guide_fk() + "']"
                var _guide_name = $(select_str_guide).text()
                _event.guide_name = _guide_name;


                self.events.unshift(new Event(_event));
                //self.new_ID('')
                self.new_date('')
                self.new_time('')
                self.new_activity_fk('')
                self.new_guide_fk('')
                self.new_comments('')

            });
        }
    };

    self.edit_mode = function () {
        //If edite mode, we want to save it
        if (this.editable() == true) {

            self.UpdateEvent(this);
            this.editBtnText('');
            this.editBtnClass('fa fa-pencil fa-fw');
        }
        else {
            this.editable(true);
            this.editBtnText('');
            this.editBtnClass('fa fa-save fa-fw');
        }
    };

    self.UpdateEvent = function (item) {

        var _url = url_events + '/UpdateEvent/' + item.ID();

        if (item.comments() == null) {
            item.comments('');
        }

        $.ajax({
            method: "PUT",
            url: _url,
            data: item,
            dataType: "json",
        }).done(function () {

            var guide_fk = item.guide_fk();
            var match = ko.utils.arrayFirst(self.guides(), function (guide) {
                return guide.ID() === guide_fk;
            });
            item.guide_name(match.name());
            item.editable(false);

            //self.feedback_sale('');
        }).fail(function (error) {
            // self.feedback_sale(error.responseText)
            alert(error);
        });
    }




    self.CancelEvent = function () {

        if (confirm('Are you sure you want to delete this row?')) {
            var item = this;
            var _url = url_events + '/CancelEvent/' + this.ID();
            $.ajax({
                method: "PUT",
                url: _url,
                //data: item,
                dataType: "json",
            }).done(function () {

                self.events.remove(item);
                //self.feedback_sale('');
            }).fail(function (error) {
                alert(error.responseText);
                // self.feedback_sale(error.responseText);
            })
        }
    }

}







