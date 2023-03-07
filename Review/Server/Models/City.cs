using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Server.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(2, MinimumLength = 2)]
        public string StateAbbreviation { get; set; }

        [Range(0, int.MaxValue)]
        public int Population { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Area { get; set; }

        [StringLength(50)]
        public string StateName { get; set; }

        [StringLength(100)]
        public string StateNickname { get; set; }
        
        public City() { }
        public City(int id, string name, string stateAbbreviation, int population, decimal area, string stateName, string stateNickname)
        {
            Id = id;
            Name = name;
            StateAbbreviation = stateAbbreviation;
            Population = population;
            Area = area;
            StateName = stateName;
            StateNickname = stateNickname;
        }
    }
}
