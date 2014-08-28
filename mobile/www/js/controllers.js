angular.module('starter.controllers', [])

.controller('DashCtrl', function($scope) {

    var menuList = [
        {name:'Pollo asado con pure', image:'pollo', description: 'Pata de pollo con pure de papa y calabaza', likes:'5', comments:'2', local:'Toca y pica'},
        {name:'Carne con papas y boniatos', image:'carne', description: 'Carne al horno con papa y boniatos. Imperdibles', likes:'1', comments:'1', local:'Toca y pica'},
        {name:'Strogonoff de pollo', image:'strogo', description: 'Strogonoff de pollo con arroz. Plato muy generoso, para compartir', likes:'5',              comments:'4', local:'Fans'},
        {name:'Wrap canadiense', image:'wrap', description: 'Wrap canadiense con papas fritas o ensalada', likes:'10', comments:'3', local:'Fans'},
    ];

        $scope.menus = menuList;
        })

        .controller('FriendsCtrl', function($scope, Friends) {
        $scope.friends = Friends.all();
        })

        .controller('FriendDetailCtrl', function($scope, $stateParams, Friends) {
        $scope.friend = Friends.get($stateParams.friendId);
        })

        .controller('AccountCtrl', function($scope) {
        });
