

function Agency(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}
function AgencyViewModel(data) {

    var self = this;

    self.agencies = ko.observableArray([]);

    self.ID = ko.observable();
    self.new_name = ko.observable();
    self.new_address = ko.observable();


    $.getJSON(url_agencies, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Agency(item);
        });
        self.agencies(mappedData);
    });



    //// Operations
    self.add_server = function () {

        if (isValid()) {
            var _agency = { name: self.new_name(), address: self.new_address() };
            $.post(url_agencies, _agency, function (agency_server) {
                _agency.ID = agency_server.ID;
                self.agencies.unshift(new Agency(_agency));
                self.new_name('');
                self.new_address('');
            });
        }

    };

    self.remove_server = function (agency) {

        $.ajax({
            url: url_agencies + '/' + agency.ID(),
            type: 'DELETE',
            //succes: function (result) { }    
        }).done(function () {
            self.agencies.remove(agency)
        }).fail(function (error) {
            alert("error");
        });
    }
}


// Activates knockout.js
ko.applyBindings(new AgencyViewModel());




