using Fooddily.Models;
using Fooddily.ViewModels.Tarjeta;

namespace Fooddily.Mappers
{
    public static class TarjetaMapper
    {
        public static TarjetaViewModel ToViewModel(this Tarjeta tarjeta)
        {
            return new TarjetaViewModel
            {
                Id = tarjeta.Id,
                Nombre = tarjeta.Nombre,
            };
        }

        public static Tarjeta ToModel(this TarjetaViewModel tarjeta)
        {
            return new Tarjeta
            {
                Id = tarjeta.Id,
                Nombre = tarjeta.Nombre,
            };
        }		
	}
}

