using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ado
{
    class Program
    {
        static MySqlConnection connection = new MySqlConnection("SERVER=localhost;UID=root;");

        static void Main(string[] args)
        {
            try
            {
                connection.Open();
                System.Console.WriteLine("Connected");
            }
            catch (SqlException e)
            {
                System.Console.WriteLine(e.Message);
            }

            try
            {
                createDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine("Datenbank erzeugen fehlgeschlagen: " + e.Message);
            }

            try
            {
                MySqlCommand c = connection.CreateCommand();
                c.CommandText = "USE ado";
                c.ExecuteNonQuery();
                Console.WriteLine("Benutze Schema ado");
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler: kann Schema ado nicht benutzen!");
            }

            try
            {
                createTable();
            }
            catch (Exception e)
            {
                Console.WriteLine("Tabelle erzeugen fehlgeschlagen: " + e.Message);
            }

            try
            {
                putData();
            }
            catch (Exception e)
            {
                Console.WriteLine("Daten schreiben fehlgeschlagen: " + e.Message);
            }

            try
            {
                doSomeQueries();
            }
            catch (Exception e)
            {
                Console.WriteLine("Daten anfragen fehlgeschlagen: " + e.Message);
            }

            connection.Close();
            System.Console.WriteLine("Connection closed");
            Console.ReadKey();
        }

        private static void createDatabase()
        {
            MySqlCommand c = connection.CreateCommand();
            c.CommandText = "CREATE DATABASE ado";
            c.ExecuteNonQuery();
            Console.WriteLine("DB erzeugen erfolgreich!");
        }

        private static void createTable()
        {
            MySqlCommand c = connection.CreateCommand();
            c.CommandText = "CREATE TABLE IF NOT EXISTS personen (name varchar(255), firstname varchar(255), age int) ";
            c.ExecuteNonQuery();
            Console.WriteLine("Tabelle Personen erzeugt!");
        }

        private static void putData()
        {

            writeUser("Wurst", "Hans", 28);
            writeUser("Mann", "Deckel", 59);
        }

        private static void writeUser(string name, string fname, int age)
        {
            MySqlCommand c = connection.CreateCommand();
            c.CommandText = string.Format("INSERT INTO personen values('{0}', '{1}', {2})", name, fname, age);
            c.ExecuteNonQuery();
            Console.WriteLine("{0} {1} eingefügt!", fname, name);
        }

        private static void doSomeQueries()
        {
            MySqlCommand c = connection.CreateCommand();
            c.CommandText = "SELECT * FROM personen WHERE name = @name";
            c.Parameters.Add(new MySqlParameter("name", "wurst"));
            MySqlDataReader r = c.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine("{0} {1} {2}", r.GetValue(0).ToString(), r.GetValue(1).ToString(), r.GetValue(2).ToString());
            }


        }
    }
}
