﻿using System;
using System.Collections.Generic;

namespace MenuDelDia.Presentacion.Models.ApiModels.site
{
    public class RestaurantMenuApiModel
    {
        public Guid RestaurantId { get; set; }

        public IList<MenuApiModel> Menus { get; set; }

    }

    public class MenuApiModel
    {
        public MenuApiModel()
        {
            Menus = new List<DailyMenuApiModel>();
        }
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsDayOpen { get; set; }
        public IList<DailyMenuApiModel> Menus { get; set; }
    }


    public class DailyMenuApiModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
