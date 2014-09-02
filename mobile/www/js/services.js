angular.module('starter.services', [])

/**
 * A simple example service that returns some data.
 */
.factory('Stores', function() {
  // Might use a resource here that returns a JSON array

  // Some fake testing data
  var stores = [
    { id: 0, name: 'Tocá y picá', email: 'mail@mail.com' },
    { id: 1, name: 'Fans', email: 'mail@mail.com' },
    { id: 2, name: 'Café Berro', email: 'mail@mail.com' },
    { id: 3, name: 'Martínez Gourmet', email: 'mail@mail.com' },
    { id: 4, name: 'Cassis', email: 'mail@mail.com' },
    { id: 5, name: 'Chivitos Lo de Pepe', email: 'mail@mail.com' },
    { id: 6, name: 'Rotisería Disco', email: 'mail@mail.com' },
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
})
.factory('Menus', function() {

  var menuList = [
        {id:'0', name:'Pollo asado con pure', price:'160', image:'pollo', description: 'Pata de pollo con pure de papa y calabaza', likes:'5', comments:'2', store: { id: 1, name:'Toca y pica', phone: '12345', distance: 0.3}},
        {id:'1', name:'Carne con papas y boniatos', price:'150', image:'carne', description: 'Carne al horno con papa y boniatos. Imperdibles', likes:'1', comments:'1', store: { id: 1, name:'Toca y pica', phone: '12345', distance: 0.5}},
        {id:'2', name:'Strogonoff de pollo', price:'210', image:'strogo', description: 'Strogonoff de pollo con arroz. Plato muy generoso, para compartir', likes:'5', comments:'4', store: { id: 2, name:'Fans', phone: '54321', distance: 0.6}},
        {id:'3', name:'Wrap canadiense', price:'180', image:'wrap', description: 'Wrap canadiense con papas fritas o ensalada', likes:'10', comments:'3', store: { id: 2, name:'Fans', phone: '54321', distance: 1.4}},
    ];

  return {
    all: function() {
      return menuList;
    },
    get: function(menuId) {
      // Simple index lookup
      return menuList[menuId];
    }
  }
});
