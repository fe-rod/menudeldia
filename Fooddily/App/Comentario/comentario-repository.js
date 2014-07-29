appModule.factory('comentarioRepository', function($resource) {
    return {
        saveComentario: function (comentario) {
            return $resource('/api/Comentario/SaveComentario').save(comentario);
        }
    }
});