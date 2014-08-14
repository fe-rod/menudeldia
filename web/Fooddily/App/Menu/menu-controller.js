appModule.controller("MenuController", function ($scope, $http, menuRepository, comercioRepository, comentarioRepository, zonaRepository, tarjetaRepository) {
    /*  INICIALIZO  */
    //Filtros
    $scope.tags = [];
    $scope.dt = new Date();
    $scope.delivery = false;
    $scope.zonas = zonaRepository.getZonas();
    $scope.tarjetas = tarjetaRepository.getTarjetas();

    //Tabla de menús
    $scope.sortBy = 'puntuacion';
    $scope.reverse = true;
    $scope.menu = { tags: [], comentarios: [] };
    $scope.comercio = { comentarios: [] };

    //Tabla de comentarios
    $scope.mSortBy = 'fecha';
    $scope.mReverse = true;
    $scope.comentario = null;
    
    /*  LÓGICA  */
    //Obtengo los menús
    $scope.getMenus = function() {
         $scope.menus = menuRepository.getMenus($scope.dt);
    };
    $scope.getMenus();

    //Obtengo el detalle de un menú seleccionado
    $scope.getObject = function (object) {
        $scope.comercio = { comentarios: [] };
        $scope.menu = menuRepository.getMenu({ id: object.id });
    };

    //Obtengo el detalle de un comercio seleccionado
    $scope.getComercio = function (object) {
        $scope.menu = { tags: [], comentarios: [] };
        $scope.comercio = comercioRepository.getComercio({ id: object.id });
    };

    //Vuelvo al listado de menús
    $scope.Index = function () {
        $scope.menu.nombre = null;
        $scope.comercio.nombre = null;
    };

    //Guardo un comentario
    $scope.AddComment = function () {
        $scope.errors = [];
        //Seteo el id del menú o comercio
        if ($scope.menu.id != null) {
            $scope.comentario.menuId = $scope.menu.id;
        } else {
            $scope.comentario.comercioId = $scope.comercio.id;
        }
        comentarioRepository.saveComentario($scope.comentario).$promise.then(
            function () {
                //Actualizo
                $scope.menus = menuRepository.getMenus($scope.dt);
                if ($scope.menu.id != null) {
                    $scope.getObject($scope.menu);
                } else {
                    $scope.getComercio($scope.comercio);
                }
                $scope.comentario = { descripcion: '', puntuacion: 0 };
            },
            function (response) { $scope.errors = response.data; });
    };
    
    /*  FILTERS  */
    $scope.applyFilters = function (menu) {
        //Tags
        var tags = true;
        angular.forEach($scope.tags, function(tag) {
            var contained = false;
            angular.forEach(menu.tags, function(mtag) {
                if (tag.id == mtag.id) {
                    contained = true;
                }
            });
            tags = tags && contained;
        });
        //Delivery
        var delivery = true;
        if($scope.delivery) {
            delivery = menu.comercio.delivery;
        }
        //Zona
        var zona = true;
        if ($scope.zona != undefined && $scope.zona != '') {
            zona = menu.comercio.zona.nombre == $scope.zona;
        }
        //Tarjeta
        var tarjeta = true;
        if ($scope.tarjeta != undefined && $scope.tarjeta != '') {
            var contenida = false;
            angular.forEach(menu.comercio.tarjetas, function (cTarjeta) {
                if ($scope.tarjeta == cTarjeta.nombre) {
                    contenida = true;
                }
            });
            tarjeta = contenida;
        }
        return tags && delivery && zona && tarjeta;
    };
    
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
        $scope.tags.push($scope.selected);
        $scope.selected = undefined;
    };

    // Any function returning a promise object can be used to load values asynchronously
    $scope.getTag = function (val) {
        return $http({ method: 'GET', isArray: true, url: '/api/Tag/SearchTags/', params: { search: val } }).then(function (res) {
            var tags = [];
            angular.forEach(res.data, function (item) {
                tags.push({ nombre: item.nombre, tipoTagId: item.tipoTagId, id: item.id });
            });
            return tags;
        });
    };

    /*  DATEPICKER  */
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
    //Para los menús
    $scope.currentPage = 0;
    $scope.pageSize = 10;
    $scope.numberOfPages = function() {
        return Math.ceil($scope.menus.length / $scope.pageSize);
    };
    
    //Para los comentarios de un menú o comercio
    $scope.cCurrentPage = 0;
    $scope.cNumberOfPages = function () {
        if ($scope.menu.id != null) {
            return Math.ceil($scope.menu.comentarios.length / $scope.pageSize);
        } else {
            return Math.ceil($scope.comercio.comentarios.length / $scope.pageSize);
        }
    };
    $scope.cNextDisabled = function () {
        if ($scope.menu.id != null) {
            return $scope.cCurrentPage >= $scope.menu.comentarios.length / $scope.pageSize - 1;
        } else {
            return $scope.cCurrentPage >= $scope.comercio.comentarios.length / $scope.pageSize - 1;
        }
    };
});

//We already have a limitTo filter built-in to angular,
//let's make a startFrom filter
appModule.filter('startFrom', function() {
    return function(input, start) {
        start = +start; //parse to int
        return input.slice(start);
    };
});