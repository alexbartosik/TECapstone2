using Server.Models;
using System.Collections.Generic;

namespace Server.DAO
{
    public interface ICityDao
    {
        IList<City> ListCities();
    }
}
