



mainApp.controller("passengersController", function ($scope, $http) {
    var url_event_info = api_url + '/api/events/GetEvent/'

    $scope.event_fk_original = ORIGINAL_LIST;
    //$scope.event_fk_new = 9;
    $scope.sum_original = 0;
    //$scope.sum_new = 0;


    var url_transportlists = api_url + '/api/transportlists/GetPassengers/';
    $scope.init = function () {

        $http.get(url_transportlists + $scope.event_fk_original).success(function (response) {
            $scope.passengers_original = response;
            $scope.sum_original = getSum($scope.passengers_original);
        });
        //$http.get(url_transportlists + $scope.event_fk_new).success(function (response) {
        //    $scope.passengers_new = response;
        //    $scope.sum_new = getSum($scope.passengers_new);
        //});
        
        $http.get(url_event_info + $scope.event_fk_original).success(function (response) {
            
            $scope.event_original = response;

        });


    }

    function getSum(passengers) {
        var sum = 0;
        for (var i = 0; i < passengers.length; i++) {
            var passenger = passengers[i];
            sum += passenger.PAX;
        }
        return sum;
    }
    $scope.duplicate = function (original_event_fk) {
        
        var url_duplicate_event = api_url + '/api/events/DuplicateEvent/' + original_event_fk;
        var url_guides = api_url + '/api/guides/';

        $http.post(url_duplicate_event).success(function (res) {
            //$scope.event_new = response;
            $http.get(url_event_info + res.ID).success(function (response) {

                $scope.event_new = response;

            });

            $scope.passengers_new = [];
          //  $('#btnDuplicate').hide('fast');
             $('#btnDuplicate').prop('disabled', true)
            $('#div_sum').show('fast');

            
            $scope.sum_new = 0;
           
            $http.get(url_guides).success(function (response) {

                $scope.guides = response;
                $('#newList_div').show("slide", { direction: "left" }, 1000);

            });
   
        });
    }


    $scope.UpdateEvent = function () {
        var url_save_event = api_url + '/api/events/UpdateEvent/' + $scope.event_new.ID;
        $http.put(url_save_event, $scope.event_new).success(function (response) {
            var speed = 200;
            $(".shootRight").fadeIn(speed).fadeOut(speed).fadeIn(speed).fadeOut(speed).fadeIn(speed);
           // $('.shootRight').show('slow');
        });
    }






    $scope.movePassenger = function (passenger, index, direction) {
        var to_event_fk = direction == 'right' ? $scope.event_new.ID : $scope.event_fk_original;
        var url_update_event = api_url + '/api/transportlists/UpdateEvent_Fk/' + passenger.soldActivityID + '/' + to_event_fk;

        $http.put(url_update_event).success(function (response) {
            if (direction === 'right') {
                moveRight(passenger, index);
            }
            else {
                moveLeft(passenger, index);
            }
            $scope.sum_original = getSum($scope.passengers_original);
            $scope.sum_new = getSum($scope.passengers_new);
            // $scope.init();
        });
    }
    function moveRight(passenger, index) {
        $scope.passengers_original.splice(index, 1);
        $scope.passengers_new.unshift(passenger);
    }
    function moveLeft(passenger, index) {
        $scope.passengers_new.splice(index, 1);
        $scope.passengers_original.unshift(passenger);
    }
});

//mainApp.controller("passengersController2", function ($scope, $http) {

//    var url_transportlists = api_url + '/api/transportlists/GetPassengersInHotels/8';

//    $http.get(url_transportlists).success(function (response) {
//        $scope.passengers = response;
//    });

//});