using System;
using Microsoft.Owin;

namespace MenuDelDia.Presentacion.Models.ApiModels
{
    public class SuggestionApiModel
    {
        public string Uuid { get; set; }
        public string Message{ get; set; }
    }
}