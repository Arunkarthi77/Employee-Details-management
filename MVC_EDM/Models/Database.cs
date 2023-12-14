using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;

namespace MVC_EDM.Models
{
    public class Database
    {
        //  private readonly string _connectionstring;
        //  private IConfiguration _configuration;
        // public Database(IConfiguration configuration)
        // {
        //     _configuration=configuration;
        //     _connectionstring=configuration["ConnectionStrings:DefaultConnection"];
        // }
        //  static SqlConnection sqlconnection = new SqlConnection("Persist Security Info=False;User ID=sa;Password=Aspire@123;Initial Catalog=UserDetails;Server=ASPIRE1275\\SQLEXPRESS;Encrypt=False;");

         static SqlConnection sqlConnection =new SqlConnection("Data Source=LAPTOP-4HPNMO2K; Database=UserDetails; Integrated security=true") ;
//   "Server=LAPTOP-4HPNMO2K;Database=UserDetails;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;"

// "Persist Security Info=False;User ID=sa;Password=Aspire@123;Initial Catalog=UserDetails;Server=ASPIRE1275\\SQLEXPRESS;Encrypt=False;"
  
   //  SqlConnection sqlConnection=new SqlConnection(_connectionstring);
        public static string login(Details emp)
        {

            string? EmployeeId = emp.EmployeeId;
            string? EmployeeName = emp.EmployeeName;
            string? Password = emp.Password;
           // SqlConnection sqlConnection=new SqlConnection(_connectionstring);

            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("Select Count(*) from Credentials WHERE EmployeeId='" + EmployeeId + "' AND EmployeeName='" + EmployeeName + "' AND Password='" + Password + "';", sqlConnection);
                int Count = Convert.ToInt32(command.ExecuteScalar());

                if (Count == 1)
                {

                    return "ok";

                }


            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException);
            }
            finally
            {
                sqlConnection.Close();

            }
            return "fails";



        }



        public static string createEmployee(Details emp)
        {

            Regex passwordValidation = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
           // SqlConnection sqlConnection=new SqlConnection(_connectionstring);
            if (passwordValidation.IsMatch(emp.Password))
                 
            {
                
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("Select Count(*) from Credentials WHERE EmployeeId='" + emp.EmployeeId + "';", sqlConnection);
                int Count = Convert.ToInt32(command.ExecuteScalar());
                sqlConnection.Close();
                if (Count == 1)
                {
                    return "Exists";

                }
                else
                {
                    
                    sqlConnection.Open();
                    // string insertquery = $"INSERT INTO Credentials (EmployeeId,EmployeeName,Password,Role) VALUES('{emp.EmployeeId}','{emp.EmployeeName}','{emp.Password}','{emp.Role}')";
                    // SqlCommand sqlcommand = new SqlCommand(insertquery, sqlConnection);
                    // sqlcommand.ExecuteNonQuery();
                    // sqlConnection.Close();
        
            SqlCommand sqlCommand = new SqlCommand("CredentialsStoredProcedure", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Operation","addEmployee");
            sqlCommand.Parameters.AddWithValue("@EmployeeID",emp.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EmployeeName",emp.EmployeeName);
            sqlCommand.Parameters.AddWithValue("@Password",emp.Password);
            sqlCommand.Parameters.AddWithValue("@Role",emp.Role);
             sqlCommand.ExecuteNonQuery();
              sqlConnection.Close();
                    return "Success";

                }
            }
            else
            {
                return "Format";
            }



        }
        public static int getRole(Details emp)
        {

           //  SqlConnection sqlConnection=new SqlConnection(_connectionstring);
            sqlConnection.Open();
             SqlCommand command = new SqlCommand("Select Count(*) from Credentials WHERE EmployeeId='" + emp.EmployeeId + "' AND EmployeeName='" + emp.EmployeeName + "'  AND Role='Employee';", sqlConnection);
            //  SqlCommand command = new SqlCommand($"Select Role from Credentials WHERE EmployeeId='" + emp.EmployeeId + "' AND EmployeeName='" + emp.EmployeeName + "'  AND Role='Employee';", sqlconnection);
        //    SqlCommand command = new SqlCommand($"Select Role from Credentials WHERE EmployeeId='" + emp.EmployeeId + "' AND EmployeeName='" + emp.EmployeeName + "' ;", sqlconnection);
            //  string role= Convert.ToString(command.ExecuteScalar());
            int Count = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();
            //  Console.WriteLine($"hi {role},{emp.EmployeeId}");
            //  return role;
            if (Count == 1)
            {

                return 1;

            }
            else
            {
                return 2;
            }


        }
        public static string forgotPassword(Details emp)
        {

            try
            {
           //      SqlConnection sqlConnection=new SqlConnection(_connectionstring);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("Select Count(*) from Credentials WHERE Employeeid='" + emp.EmployeeId + "' AND EmployeeName='" + emp.EmployeeName + "';", sqlConnection);
                int Count = Convert.ToInt32(command.ExecuteScalar());
                sqlConnection.Close();
                if (Count == 1)
                {
                    return "Correct";

                }


            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return "incorrect";

        }

        public static string resetPassword(Details emp)
        {


            Regex passwordValidation = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (String.Equals(emp.Password, emp.ConfirmPassword))
            {
                if (passwordValidation.IsMatch(emp.Password))
                {
                //     SqlConnection sqlConnection=new SqlConnection(_connectionstring);
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand($"Update Credentials set Password = '{emp.Password}' where EmployeeId = '{emp.EmployeeId}' AND EmployeeName='{emp.EmployeeName}';", sqlConnection);
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                    return "done";
                }
                else
                {
                    return "format";
                }

            }

            else
            {
                return "match";
            }


        }
        public static string deleteAccount(string EmployeeId)
        {
            Console.WriteLine(EmployeeId);
          //  SqlConnection sqlConnection=new SqlConnection(_connectionstring);
            try
            {

                sqlConnection.Open();
                SqlCommand command = new SqlCommand($"Delete Credentials where EmployeeId = '{EmployeeId}';", sqlConnection);
                command.ExecuteNonQuery();
                return "Success";


            }
            catch (SqlException sqlexception)
            {
                Console.WriteLine(sqlexception);
            }
            finally
            {
                sqlConnection.Close();
            }
            return "fails";
        }


    }


}
