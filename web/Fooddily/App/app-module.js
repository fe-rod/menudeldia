var appModule = angular.module("appModule", ['ngRoute', 'ngResource', 'ui.bootstrap', 'bootstrap-tagsinput']).
    config(function ($routeProvider, $locationProvider) {
        $routeProvider.when('/Fooddily/Menu', { templateUrl: '/Templates/menu.html', controller: 'MenuController' });
        $routeProvider.when('/Fooddily/Comercio', { templateUrl: '/Templates/comercio.html', controller: 'ComercioController' });
        $locationProvider.html5Mode(true);
    });