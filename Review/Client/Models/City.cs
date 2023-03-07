namespace Client.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateAbbreviation { get; set; }
        public int Population { get; set; }
        public decimal Area { get; set; }
        public string StateName { get; set; }
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
