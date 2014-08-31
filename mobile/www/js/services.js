angular.module('starter.services', [])

/**
 * A simple example service that returns some data.
 */
.factory('Friends', function() {
  // Might use a resource here that returns a JSON array

  // Some fake testing data
  var friends = [
    { id: 0, name: 'Scruff McGruff' },
    { id: 1, name: 'G.I. Joe' },
    { id: 2, name: 'Miss Frizzle' },
    { id: 3, name: 'Ash Ketchum' }
  ];

  return {
    all: function() {
      return friends;
    },
    get: function(friendId) {
      // Simple index lookup
      return friends[friendId];
    }
  }
})
.factory('Menus', function() {

  var menuList = [
        {id:'0', name:'Pollo asado con pure', image:'pollo', description: 'Pata de pollo con pure de papa y calabaza', likes:'5', comments:'2', local:'Toca y          pica', store: { id: 1, phone: '12345'}},
        {id:'1', name:'Carne con papas y boniatos', image:'carne', description: 'Carne al horno con papa y boniatos. Imperdibles', likes:'1', comments:'1',            local:'Toca y pica', store: { id: 2, phone: '12345'}},
        {id:'2', name:'Strogonoff de pollo', image:'strogo', description: 'Strogonoff de pollo con arroz. Plato muy generoso, para compartir', likes:'5',              comments:'4', local:'Fans', store: { id: 3, phone: '54321'}},
        {id:'3', name:'Wrap canadiense', image:'wrap', description: 'Wrap canadiense con papas fritas o ensalada', likes:'10', comments:'3', local:'Fans', store: { id: 4, phone: '54321'}},
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
