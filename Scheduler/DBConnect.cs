using MySql.Data.MySqlClient;
using System;

namespace Scheduler
{
    class DBConnect
    {
        private MySqlConnection connection;
        
        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string host = "localhost";
            int port = 3306;
            string database = "work_manamgement";
            string username = "root";
            string password = "";

            connection = GetDBConnection(host, port, database, username, password);
        }

        public static MySqlConnection
                 GetDBConnection(string host, int port, string database, string username, string password)
        {
            // Connection String.
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Helper.WriteToFile("Cannot connect to server Error on: {0} " + ex.Message + ex.StackTrace);
                        break;

                    case 1045:
                        Helper.WriteToFile("Invalid username/password Error on: {0} " + ex.Message + ex.StackTrace);
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Helper.WriteToFile("Cannot close connection Error on: {0} " + ex.Message + ex.StackTrace);

                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
        }

        //Update statement
        public void Update()
        {
        }

        //Delete statement
        public void Delete()
        {
        }

        //Select statement
        public void Select()
        {
            string query = "SELECT * FROM schedule";

            try {
           
                //Open connection
                if (this.OpenConnection() == true)
                {
                    Email email = new Email();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        string s = dataReader["id"].ToString();
                        email.Send();
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                }
                else
                {
                    Helper.WriteToFile("DB Connection not open Error");
                }
            } catch (Exception ex) {
                Helper.WriteToFile("Error in reading schedule data: {0} " + ex.Message + ex.StackTrace);
            }
        }

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
}
