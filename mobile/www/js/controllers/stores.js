angular.module('todayMenu')

    .controller('StoresCtrl', function($scope,$rootScope, Stores) {
        $scope.stores = Stores.all();
        $rootScope.hideTabs = false;
        $rootScope.hideFilter = true;
    })

    .controller('StoreDetailCtrl', function($scope, $rootScope, $stateParams, Stores) {
        $scope.store = Stores.get($stateParams.storeId);
        $rootScope.hideTabs = true;
        $rootScope.hideFilter = true;
    });
