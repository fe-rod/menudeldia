appModule.factory('zonaRepository', function($resource) {
    return {
        getZonas: function () {
            return $resource('/api/Zona/GetZonas').query();
        }
    }
});