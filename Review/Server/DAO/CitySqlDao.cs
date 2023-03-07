using Microsoft.VisualBasic;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Server.DAO
{
    public class CitySqlDao : ICityDao
    {
        private readonly string connectionString;

        public CitySqlDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public CitySqlDao() { }

        public IList<City> ListCities()
        {
            List<City> cityList = new List<City>();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "SELECT " +
                                        "c.city_id, " +
                                        "c.city_name, " +
                                        "c.state_abbreviation, " +
                                        "c.population, " +
                                        "c.area, " +
                                        "s.state_name, " +
                                        "s.state_nickname " +
                                    "FROM city c " +
                                        "JOIN state s ON s.state_abbreviation = c.state_abbreviation " +
                                    "ORDER BY s.state_name, c.city_name";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    City city = new City();

                    city.Id = Convert.ToInt32(reader["city_id"]);
                    city.Name = Convert.ToString(reader["city_name"]);
                    city.StateAbbreviation = Convert.ToString(reader["state_abbreviation"]);
                    city.Population = Convert.ToInt32(reader["population"]);
                    city.Area = Convert.ToDecimal(reader["area"]);
                    city.StateName = Convert.ToString(reader["state_name"]);
                    city.StateNickname = Convert.ToString(reader["state_nickname"]);

                    cityList.Add(city);
                }
            }

            return cityList;
        }

        public City AddCity(City city)
        {
            const string sql = "INSERT INTO city (city_name, state_abbreviation, population, area) " +
                                "VALUES (@name, @stateAbbreviation, @population, @area);" +
                                "SELECT @@IDENTITY";

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@name", city.Name);
                command.Parameters.AddWithValue("@stateAbbreviation", city.StateAbbreviation);
                command.Parameters.AddWithValue("@population", city.Population);
                command.Parameters.AddWithValue("@area", city.Area);

                city.Id = Convert.ToInt32(command.ExecuteScalar());

                return city;
            }
        }
    }
}
