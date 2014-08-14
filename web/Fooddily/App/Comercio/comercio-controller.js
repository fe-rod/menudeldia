appModule.controller("ComercioController", function ($scope, $http, comercioRepository, menuRepository) {
    /*  INICIALIZO  */
    //Tabla de comercios
    $scope.comercio = { menus: [] };
    $scope.addMode = false;
    $scope.sortBy = 'puntuacion';
    $scope.reverse = true;
    
    //Tabla de menús
    $scope.menu = {
        nombre: '',
        descripcion: '',
        recurrente: false,
        tags: [],
        dias: []
    };
    $scope.mSortBy = 'nombre';
    $scope.mReverse = true;

    /*  LÓGICA  */
    //Obtengo los comercios
    $scope.comercios = comercioRepository.getComercios();
    
    //Obtengo el detalle de un comercio seleccionado
    $scope.getObject = function (object) {
        $scope.comercio = comercioRepository.getComercio({ id: object.id });
    };
    
    //Guardo un menú
    $scope.saveMenu = function (menu) {
        $scope.errors = [];
        menu.comercioid = $scope.comercio.id;
        menuRepository.saveMenu(menu).$promise.then(
            function () {
                $scope.addMode = false;

                $scope.menu = {
                    nombre: '',
                    descripcion: '',
                    recurrente: false,
                    tags: [],
                    dias: []
                };
                $scope.getObject($scope.comercio);
            },
            function (response) { $scope.errors = response.data; });
    };

    //Elimino un menú
    $scope.eliminarMenu = function (menu) {
        var index = $scope.comercio.menus.indexOf(menu);

        menuRepository.eliminarMenu(menu);

        $scope.comercio.menus.splice(index, 1);
    };
    
    /*  CHECKBOX DÍAS  */
    $scope.dias = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];

    // toggle selection for a given day by name
    $scope.toggleSelection = function toggleSelection(day) {
        var idx = $scope.menu.dias.indexOf(day);

        // is currently selected
        if (idx > -1) {
            $scope.menu.dias.splice(idx, 1);
        }
        // is newly selected
        else {
            $scope.menu.dias.push(day);
        }
    };

    /*  RATING  */
    $scope.max = 5;
    $scope.isReadonly = true;

    $scope.ratingStates = [
      { stateOn: 'glyphicon-ok-sign', stateOff: 'glyphicon-ok-circle' },
      { stateOn: 'glyphicon-star', stateOff: 'glyphicon-star-empty' },
      { stateOn: 'glyphicon-heart', stateOff: 'glyphicon-ban-circle' },
      { stateOn: 'glyphicon-heart' },
      { stateOff: 'glyphicon-off' }
    ];
    
    /*  TAGS  */
    $scope.getTagClass = function (tag) {
        switch (tag.tipoTagId) {
            case 1: return 'badge badge-info';
            case 2: return 'label label-important';
            case 3: return 'badge badge-success';
            case 4: return 'label label-inverse';
            case 5: return 'badge badge-warning';
        }
    };
    
    $scope.selected = undefined;

    $scope.selectedCallback = function () {
        $scope.menu.tags.push($scope.selected);
        $scope.selected = undefined;
    };

    // Any function returning a promise object can be used to load values asynchronously
    $scope.getTag = function(val) {
        return $http({ method: 'GET', isArray: true, url: '/api/Tag/SearchTags/', params: { search: val }}).then(function (res) {
            var tags = [];
            angular.forEach(res.data, function(item){
                tags.push({nombre: item.nombre, tipoTagId: item.tipoTagId, id: item.id});
            });
            return tags;
        });
    };
    
    /*  DATEPICKER  */
    $scope.dt = new Date();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.dateOptions = {
        'year-format': "'yy'",
        'starting-day': 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'shortDate'];
    $scope.format = $scope.formats[0];
    
    /*  PAGINATING  */
    //Para los comercios
    $scope.currentPage = 0;
    $scope.pageSize = 10;
    $scope.numberOfPages = function () {
        return Math.ceil($scope.comercios.length / $scope.pageSize);
    };

    //Para los menús de un comercio
    $scope.mCurrentPage = 0;
    $scope.mNumberOfPages = function () {
        return Math.ceil($scope.comercio.menus.length / $scope.pageSize);
    };
});