using System;
using System.Data.SqlClient;
using System.Data;

namespace FileReaderAPI.Helpers
{
    public class DatabaseHelper
    {
        public SqlCommand CommandGeneratorStoredProcedure(string commandText, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;

            return command;
        }

        public IDataReader ExecuteCommandReader(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                //WriteLog.RunningLog("Exception thrown - " + ex.Message);
                throw new System.ArgumentException(ex.Message, ex.InnerException);
            }
            //finally
            //{
            //    command.Connection.Close();
            //}
        }

        public int ExecuteCommandNonQuery(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, ex.InnerException);
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public int ExecuteScalarInt(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return (Convert.ToInt32(command.ExecuteScalar()));

            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, ex.InnerException);
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public string ExecuteScalarString(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return command.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                //WriteLog.RunningLog("Exception thrown - " + ex.Message);

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public Boolean ExecuteScalarBoolean(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return Convert.ToBoolean(command.ExecuteScalar());

            }
            catch (Exception ex)
            {
                //WriteLog.RunningLog("Exception thrown - " + ex.Message);

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public DateTime ExecuteScalarDateTime(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return Convert.ToDateTime(command.ExecuteScalar());

            }
            catch (Exception ex)
            {
                //WriteLog.RunningLog("Exception thrown - " + ex.Message);

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public double ExecuteScalarDouble(SqlCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                return Convert.ToDouble(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                //WriteLog.RunningLog($"ExecuteScalarDouble Exception; Message: {ex.Message}, Stack Trace: {ex.StackTrace}");

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public string GetDbServer(string dbType)
        {

            string environment = System.Configuration.ConfigurationManager.AppSettings["Environment"];

            if (environment == "Test")
            {
                string whDbServerEnvironment = System.Configuration.ConfigurationManager.AppSettings["TestWhDb"];
                string navDbServerEnvironment = System.Configuration.ConfigurationManager.AppSettings["TestDb"];

                if (dbType == "nav")
                {
                    return navDbServerEnvironment;
                }
                else
                {
                    return whDbServerEnvironment;

                }

            }
            else
            {
                string whDbServerEnvironment = System.Configuration.ConfigurationManager.AppSettings["LiveWhDb"];
                string navDbServerEnvironment = System.Configuration.ConfigurationManager.AppSettings["LiveDb"];

                if (dbType == "nav")
                {
                    return navDbServerEnvironment;
                }
                else
                {
                    return whDbServerEnvironment;

                }

            }

        }

        public string GetConnString(string keyName, string dbServer)
        {
            string ConnSuffix = System.Configuration.ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
            string ConnPrefix = this.GetDbServer(dbServer);

            return ConnSuffix + ConnPrefix;

        }
    }
}