  appModule.factory('comercioRepository', function($resource) {
    return {
        getComercios: function () {
            return $resource('/api/Comercio/GetComercios').query();
        },
        getComercio: function (id) {
            return $resource('/api/Comercio/GetComercio').get(id);
        }
    }
});