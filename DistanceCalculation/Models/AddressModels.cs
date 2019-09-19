using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DistanceCalculation.Models
{
    public class AddressModel
    {
        [Required]
        [Display(Name = "DistanceFrom")]
        public string DistanceFrom { get; set; }

        public List<Locations> listAddress { get; set; }
    }

    public class Locations
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
}
