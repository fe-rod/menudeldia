using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuDelDia.Presentacion.Models.ApiModels.site
{
    public class RegisterApiModel
    {
        public RegisterApiModel()
        {
            Cards = new List<Guid>();
            Tags = new List<Guid>();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string LogoPath { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IList<Guid> Cards { get; set; }

        [EmailAddress]
        public string EmailUserName { get; set; }

        public string Password { get; set; }
        public IList<Guid> Tags { get; set; }
    }
}
