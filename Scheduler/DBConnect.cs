using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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
            string query = "SELECT s.next_reminder_date, s.frequency, s.time, s.from_address, s.to_address, s.company_id, te.subject, te.body FROM schedule s inner join task_email te on s.task_id = te.task_id where s.isActive = 1";
            string query1 = "select company_id, email_id, password, port, host from email_config where isActive=1";

            try {
           
                //Open connection
                if (this.OpenConnection() == true)
                {
                    Email email = new Email();
                    List<EmailConfigEntity> emailConfigList = new List<EmailConfigEntity>();

                    //Create Command
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd1.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        EmailConfigEntity emailEntity = new EmailConfigEntity();

                        emailEntity.EmailId = dataReader["email_id"].ToString();
                        emailEntity.Password = dataReader["password"].ToString();
                        emailEntity.Port = dataReader["port"].ToString();
                        emailEntity.Host = dataReader["host"].ToString();
                        emailEntity.CompanyId = Convert.ToInt32(dataReader["company_id"].ToString());

                        emailConfigList.Add(emailEntity);
                    }

                    dataReader.Close();

                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        DateTime scheduleTime = Convert.ToDateTime(dataReader["next_reminder_date"].ToString());

                        if (scheduleTime <= DateTime.Now)
                        {
                            int company_id = Convert.ToInt32(dataReader["company_id"].ToString());
                            EmailConfigEntity emailEntity = emailConfigList.Find(item => item.CompanyId == company_id);

                            email.Send(Convert.ToInt32(dataReader["company_id"].ToString()), dataReader["from_address"].ToString(), dataReader["to_address"].ToString(), dataReader["subject"].ToString()
                                , dataReader["body"].ToString());
                        }
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
