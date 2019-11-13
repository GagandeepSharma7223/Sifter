using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class UnitUnitIngredient
    {
        public int? Id { get; set; }
        public int? UnitId { get; set; }
        public int? IngredientId { get; set; }
    }
}
