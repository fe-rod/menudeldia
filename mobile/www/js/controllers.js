angular.module('starter.controllers', ['ionic'])

    .controller('MenusCtrl', function($scope, $rootScope, $stateParams, Menus, $ionicPopover, $timeout) {
        $scope.menus = Menus.all();
        $scope.menu = Menus.get($stateParams.menuId);
        $rootScope.hideTabs = false;
        $rootScope.hideFilter = false;

        $ionicPopover.fromTemplateUrl('templates/filterPopover.html', function(popover) {
            $rootScope.popover = popover;
        });

        $scope.loadMore = function() {

            $timeout(function(){
                $scope.menus.push(
                    {id:'10', name:'Zapallitos rellenos de carne', price:'160', description: 'Pata de pollo con pure de papa y calabaza', likes:'5', comments:'2', store: { id: 1, icon: 'placeholder' , name:'Toca y pica', phone: '12345', distance: 0.3}}
                );
                $scope.menus.push(
                    {id:'11', name:'Tortilla de papas con tomate y orégano', price:'160', description: 'Pata de pollo con pure de papa y calabaza', likes:'5', comments:'2', store: { id: 1, icon: 'placeholder' , name:'Toca y pica', phone: '12345', distance: 0.3}}
                );
                $scope.menus.push(
                    {id:'12', name:'Tallarines con pollo y salsa de soja', price:'160', description: 'Pata de pollo con pure de papa y calabaza', likes:'5', comments:'2', store: { id: 1, icon: 'placeholder' , name:'Toca y pica', phone: '12345', distance: 0.3}}
                );
                $scope.$broadcast('scroll.infiniteScrollComplete')
            }, 3000);
//            $http.get('/more-items').success(function(items) {
//                useItems(items);
//
//            });
        };

    })
    .controller('MenuDetailCtrl', function($scope, $rootScope, $stateParams, Menus) {
        $scope.menu = Menus.get($stateParams.menuId);
        $rootScope.hideTabs = true;
        $rootScope.hideFilter = true;
    })

    .controller('StoresCtrl', function($scope,$rootScope, Stores) {
        $scope.stores = Stores.all();
        $rootScope.hideTabs = false;
        $rootScope.hideFilter = true;
    })

    .controller('StoreDetailCtrl', function($scope, $rootScope, $stateParams, Stores) {
        $scope.store = Stores.get($stateParams.storeId);
        $rootScope.hideTabs = true;
        $rootScope.hideFilter = true;
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
