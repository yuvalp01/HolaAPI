
//var _url = api_url + '/api/guides';

function AppViewModel(data) {
    var self = this;
    ///
    self.guides = ko.observableArray([]);
    var self_list = self.guides;
    self.new_ID = ko.observable();
    self.new_name = ko.observable();
    self.new_phone = ko.observable();

    $.getJSON(url_guides, function (data) {
        ko.mapping.fromJS(data, {}, self_list);
    });
    ///

    //// Operations
    self.add_server = function () {
        if (isValid()) {
            var new_obj = { name: self.new_name(), phone: self.new_phone() };
            $.post(url_guides, new_obj, function (obj_from_server) {
                new_obj.ID = obj_from_server.ID;
                self_list.unshift(new_obj);
                self.new_name('');
                self.new_phone('');
            });
        }
    };

    self.remove_server = function (obj) {

        $.ajax({
            url: url_guides + '/' + obj.ID(),
            type: 'DELETE',
        }).done(function () {
            self_list.remove(obj)
        }).fail(function (error) {
            alert("error");
        });
    }


}

// Activates knockout.js
ko.applyBindings(new AppViewModel());
















