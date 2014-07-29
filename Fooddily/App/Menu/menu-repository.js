appModule.factory('menuRepository', function($resource) {
    return {
        getMenus: function (dt) {
            return $resource('/api/Menu/SearchMenus').query({ dt: dt });
        },
        getMenu: function (id) {
            return $resource('/api/Menu/GetMenu').get(id);
        },
        saveMenu: function (menu) {
            return $resource('/api/Menu/SaveMenu').save(menu);
        },
        eliminarMenu: function (id) {
            return $resource('/api/Menu/DeleteMenu').delete(id);
        }
    }
});