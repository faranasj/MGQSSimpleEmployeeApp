using System;
using System.Collections.Generic;
using SimpleEmployeeApp.Entities;
using System.IO;
using MGQSSimpleEmployeeAppFile.Constants;
using SimpleEmployeeApp.Enums;
using SimpleEmployeeApp.Shared;
using MySql.Data.MySqlClient;

namespace SimpleEmployeeApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        
        // text file and binary file are types of file. file name, file extension and file directory are 3 components of files.
        // stream is the movement of data from one source to a destination over a communication path
        // public string file = @"Files\employee.txt"; // is a relative path. a directory path is the path to the directory
        public string connectionString = @"server=127.0.0.1;user=root;database=employeeDB;port=3306;password=Iqballulah1.";
        public MySqlConnection conn;
        public EmployeeRepository()
        {
            conn = new MySqlConnection();
        }
        public List<Employee> GetAll()
        {
            conn = new(connectionString);
            List<Employee> employees = new List<Employee>();

            try 
            {
                conn.Open();

                // SQL Query to execute
                // selecting only first 10 rows for demo
                string sql = "SELECT * FROM employees;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                // read the data
                while (rdr.Read())
                {
                    Employee employee = new();
                    employee.Id = (int)rdr["EmployeeId"];
                    employee.Code = (string)rdr["EmployeeCode"];
                    employee.FirstName = (string)rdr["FirstName"];
                    employee.LastName = (string)rdr["LastName"];
                    employee.MiddleName = (string)rdr["MiddleName"];
                    employee.Phone = (string)rdr["Phone"];
                    employee.Email = (string)rdr["Email"];
                    employee.DateJoined = (DateTime)rdr["DateJoined"];
                    employees.Add(employee);
                }
                rdr.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
            conn.Close();
            return employees;
        }
        public Employee GetByCode(string code)
        {
            // return employees.Find(i => i.Code == code);
            conn = new(connectionString);

            Employee employee = new();

            try 
            {
                conn.Open();
                var sql = "SELECT * FROM employees where EmployeeCode = '" + code + "'";

                MySqlCommand command = new MySqlCommand(sql, conn);

                MySqlDataReader rdr = command.ExecuteReader();

                if (rdr.Read())
                {
                    employee.Id = (int)rdr["EmployeeId"];
                    employee.Code = (string)rdr["EmployeeCode"];
                    employee.FirstName = (string)rdr["FirstName"];
                    employee.LastName = (string)rdr["LastName"];
                    employee.MiddleName = (string)rdr["MiddleName"];
                    employee.Phone = (string)rdr["Phone"];
                    employee.Email = (string)rdr["Email"];
                    employee.DateJoined = (DateTime)rdr["DateJoined"];
                    employee.Password = (string)rdr["EmployeePassword"];
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return employee;
        }
        public Employee GetById(int id)
        {
            // return employees.Find(i => i.Id == id);
            conn = new(connectionString);

            Employee employee = null;

            try 
            {
                conn.Open();
                var sql = "SELECT EmployeedId, EmployeeCode, FirstName, LastName, Phone FROM employees where EmployeeId = '" + id + "'";

                MySqlCommand command = new MySqlCommand(sql, conn);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    employee.Id = reader.GetInt32(1);
                    employee.Code = reader.GetString(2);
                    employee.FirstName = reader.GetString(3);
                    employee.LastName = reader.GetString(4);
                    employee.Phone = reader.GetString(5);

                    employee = new Employee();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return employee;
        }
        public bool CreateRecord(Employee employee)
        {
            conn = new(connectionString);

            try 
            {
                Console.WriteLine("Connecting to MySql...");
                conn.Open();

                string query = "INSERT INTO employees(EmployeeCode, FirstName, LastName, MiddleName, Gender, EmployeeRole, Phone, Email, EmployeePassword, DateJoined) values('" + employee.Code + "', '" + employee.FirstName + "', '" + employee.LastName + "', '" + employee.MiddleName + "', '" + employee.Gender + "', '" + employee.Role + "', '" + employee.Phone + "', '" + employee.Email + "', '" + employee.Password + "', '" + employee.DateJoined.ToString("yyyy-MM-dd") + "');";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
            conn.Close();
            return false;
        }
        public bool Update(Employee employee)
        {
            conn = new(connectionString);

            try
            {
                conn.Open();

                var sql = "UPDATE employees SET FirstName ='" + employee.FirstName + "', LastName ='" + employee.LastName + "', MiddleName ='" + employee.MiddleName + "', EmployeeRole ='" + employee.Role + "', DateJoined ='" + employee.DateJoined + "', WHERE EmployeeId ='" + employee.Id + "'";

                MySqlCommand command = new MySqlCommand(sql, conn);
                int count = command.ExecuteNonQuery();

                if (count > 0)
                {
                    conn.Close();
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return false;
        }
        public bool Delete(int id)
        {
            conn = new(connectionString);
            
            try
            {
                Console.WriteLine("Connecting to MySql...");
                conn.Open();

                string query = $"DELETE FROM employees WHERE EmployeeId = {id}";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            conn.Close();
            return false;
        }
        public int EmployeeCount()
        {
            try
            {
                conn = new(connectionString);

                string sql = "SELECT count(*) FROM employees";

                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    {
                        int totalEmployee = reader.GetInt32(0);
                        return totalEmployee;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return 0;
        }
        // private void ReadFromFile()
        // {
        //     try
        //     {
        //         if (File.Exists(Constants.fullPath))
        //         {
        //             var allLines = File.ReadAllLines(Constants.fullPath);
        //             foreach (var line in allLines)
        //             {
        //                 var employee = Employee.ToEmployee(line);
        //                 employees.Add(employee);
        //             }
        //         }
        //         else
        //         {
        //             var dir = Constants.dir;
        //             Directory.CreateDirectory(dir);
        //             var filename = Constants.fileName;
        //             var fullPath = Path.Combine(dir, filename);
        //             using (File.Create(fullPath)) // using is used to define a scope, it does the close and flush without having to be specific about it.
        //             {
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        // }
        // public void WriteToFile(Employee employee)
        // {
        //     try 
        //     {
        //         using (StreamWriter writer = new StreamWriter(Constants.fullPath, true))
        //     {
        //         writer.WriteLine(employee.ToString());
        //     }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }          
        // }
        // public void RefreshFile()
        // {
        //     try
        //     {
        //         using (StreamWriter writer = new StreamWriter(Constants.fullPath))
        //         {
        //             foreach (var e in employees)
        //             {
        //                 writer.WriteLine(e.ToString());
        //             }
        //         }
        //     }
        //     catch(Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        // }
    }
}