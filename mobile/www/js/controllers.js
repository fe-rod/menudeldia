angular.module('starter.controllers', ['ionic'])

    .controller('MenusCtrl', function($scope, Menus) {
        $scope.menus = Menus.all();

        $scope.like = function(menuId){
            $scope.menus[menuId].likes++;
        }

    })

    .controller('StoresCtrl', function($scope, Stores) {
        $scope.stores = Stores.all();
    })

    .controller('StoreDetailCtrl', function($scope, $stateParams, Stores) {
        $scope.store = Stores.get($stateParams.storeId);
    })

    .controller('AccountCtrl', function($scope) {
    })

    .controller('MapCtrl', function($scope, $ionicLoading, $compile) {

        //$scope.store = $stateParams.storeId);

        function initialize() {
            var myLatlng = new google.maps.LatLng(-34.917191,-56.152229);

            var mapOptions = {
                center: myLatlng,
                zoom: 16,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map"),
                mapOptions);

            //Marker + infowindow + angularjs compiled ng-click
            var contentString = "<div><a ng-click='clickTest()'>Click me!</a></div>";
            var compiled = $compile(contentString)($scope);

            var infowindow = new google.maps.InfoWindow({
                content: compiled[0]
            });

            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: 'Uluru (Ayers Rock)'
            });

            google.maps.event.addListener(marker, 'click', function() {
                infowindow.open(map,marker);
            });
//
//        $scope.map = map;
        }
        ionic.Platform.ready(initialize);

//    $scope.centerOnMe = function() {
//        if(!$scope.map) {
//            return;
//        }
//
//        $scope.loading = $ionicLoading.show({
//            content: 'Getting current location...',
//            showBackdrop: false
//        });
//
//        navigator.geolocation.getCurrentPosition(function(pos) {
//            $scope.map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
//            $scope.loading.hide();
//        }, function(error) {
//            alert('Unable to get location: ' + error.message);
//        });
//    };

//    $scope.clickTest = function() {
//        alert('Example of infowindow with ng-click')
//    };

    });
