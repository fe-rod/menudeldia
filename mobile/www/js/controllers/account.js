angular.module('todayMenu')
    .controller('AccountCtrl', function($scope, $ionicModal, $cordovaToast) {
        $ionicModal.fromTemplateUrl('templates/account/comments.html', function($ionicModal) {
            $scope.commentsModal = $ionicModal;
            $scope.comments = {value: ''};
        }, {
            // Use our scope for the scope of the modal to keep it simple
            scope: $scope,
            // The animation we want to use for the modal entrance
            animation: 'slide-in-up'
        });

        $ionicModal.fromTemplateUrl('templates/account/suggest.html', function($ionicModal) {
            $scope.suggestModal = $ionicModal;
            $scope.suggestions = {value: ''};
        }, {
            // Use our scope for the scope of the modal to keep it simple
            scope: $scope,
            // The animation we want to use for the modal entrance
            animation: 'slide-in-up'
        });

        $scope.sendComments = function(){
            $scope.commentsModal.hide();
            $scope.comments.value = '';
            $cordovaToast.showLongBottom('Gracias por enviarnos tus comentarios').then(function(success) {
                // success
            }, function (error) {
                // error
            });

        }

        $scope.sendSuggestions = function(){
            $scope.suggestModal.hide();
            $scope.suggestions.value = '';
            $cordovaToast.showLongBottom('Gracias por enviarnos tus comentarios').then(function(success) {
                // success
            }, function (error) {
                // error
            });
        }

    });
