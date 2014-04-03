﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;

namespace Persons
{
    class DBPersonAccessor : IPersonAccessor
    {
        private SqlCeConnection connection;
        private const string CONNECTION_CONFIGURATION = "Data Source = AppDB.sdf; Password = ''";

        public DBPersonAccessor()
        {
 
        }

        ~DBPersonAccessor()
        {
            connection.Close();
        }

        public List<Person> GetAll()
        {

            string query = "SELECT * FROM PersonTable";
            return GetPersonListByQuery(query);
        }

        public List<Person> GetByName(string name)
        {
            string query = "SELECT * FROM PersonTable WHERE NameField LIKE '%" + name + "%'";
            return GetPersonListByQuery(query);
            
        }

        public void DeleteByName(string name) //не работает, вероятно запрос написан с ошибкой
        {
            using (connection = new SqlCeConnection(CONNECTION_CONFIGURATION))
            {
                connection.Open();
                SqlCeCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM PersonTable WHERE NameField LIKE '%" + name + "%'";
                cmd.ExecuteNonQuery();

            }

        }

        public void Insert(Person person) //не работает, вероятно запрос написан с ошибкой
        {
            using (connection = new SqlCeConnection(CONNECTION_CONFIGURATION))
            {
                connection.Open();
                SqlCeCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO PersonTable VALUES ("+person.Id+","+person.FullName+","+person.BirthDate.ToShortDateString()+")";
                cmd.ExecuteNonQuery();

            }
        }


        public List<Person> GetPersonListByQuery(string query)
        {

            using (connection = new SqlCeConnection(CONNECTION_CONFIGURATION))
            {

      
          
                connection.Open();
                SqlCeCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;
                SqlCeDataReader reader = cmd.ExecuteReader();


                List<Person> result = new List<Person>();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string fullName = reader.GetString(1);
                    string birthDate = reader.GetString(2);
                    Person readPerson = new Person(id, fullName, DateTime.Parse(birthDate));
                    result.Add(readPerson);
                }



                cmd.ExecuteNonQuery();
                reader.Close();
                connection.Close();

                return result;
            }
            
        }



    }


 
}
	
 
