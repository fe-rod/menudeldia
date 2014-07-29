appModule.factory('tagRepository', function($resource) {
    return {
        getTags: function () {
            return $resource('/api/Tag/GetTags').query();
        }
    }
});