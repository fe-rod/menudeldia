angular.module('todayMenu')

.factory('Stores', function() {
  // Might use a resource here that returns a JSON array

  // Some fake testing data
  var stores = [
    { id: 0, name: 'Tocá y picá', email: 'mail@mail.com', icon:'placeholder', phone: '2' },
    { id: 1, name: 'Fans', email: 'mail@mail.com', icon:'fans', phone: '2' },
    { id: 2, name: 'Café Berro', email: 'mail@mail.com', icon:'cafeberro', phone: '2' },
    { id: 3, name: 'Martínez Gourmet', email: 'mail@mail.com', icon:'placeholder', phone: '2' },
    { id: 4, name: 'Cassis', email: 'mail@mail.com', icon:'placeholder', phone: '2' },
    { id: 5, name: 'Chivitos Lo de Pepe', email: 'mail@mail.com', icon:'chivitoslodepepe', phone: '2' },
    { id: 6, name: 'Rotisería Disco', email: 'mail@mail.com', icon:'disco', phone: '2' },
  ];

  return {
    all: function() {
      return stores;
    },
    get: function(storeId) {
      // Simple index lookup
      return stores[storeId];
    }
  }
});
