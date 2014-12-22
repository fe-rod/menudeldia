using System;

namespace MenuDelDia.Presentacion.Models.ApiModels
{
    public class CardApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CardType { get; set; }
    }
}