using Fooddily.Models;
using Fooddily.ViewModels.Tag;

namespace Fooddily.Mappers
{
    public static class TagMapper
    {
        public static TagViewModel ToViewModel(this Tag tag)
        {
            return new TagViewModel
            {
                Id = tag.Id,
                Nombre = tag.Nombre,
                TipoTagId = tag.TipoTagId,
            };
        }

        public static Tag ToModel(this TagViewModel tag)
        {
            return new Tag
            {
                Id = tag.Id,
                Nombre = tag.Nombre,
                TipoTagId = tag.TipoTagId,
            };
        }		
	}
}

