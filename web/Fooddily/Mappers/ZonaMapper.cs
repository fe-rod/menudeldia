using Fooddily.Models;
using Fooddily.ViewModels.Tarjeta;
using Fooddily.ViewModels.Zona;

namespace Fooddily.Mappers
{
    public static class ZonaMapper
    {
        public static ZonaViewModel ToViewModel(this Zona zona)
        {
            return new ZonaViewModel
            {
                Id = zona.Id,
                Nombre = zona.Nombre,
            };
        }

        public static Zona ToModel(this ZonaViewModel zona)
        {
            return new Zona
            {
                Id = zona.Id,
                Nombre = zona.Nombre,
            };
        }		
	}
}

