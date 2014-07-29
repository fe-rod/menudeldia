appModule.factory('tarjetaRepository', function($resource) {
    return {
        getTarjetas: function () {
            return $resource('/api/Tarjeta/GetTarjetas').query();
        }
    }
});