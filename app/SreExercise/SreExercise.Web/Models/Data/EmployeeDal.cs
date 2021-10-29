using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace SreExercise.Web.Models.Data
{
    public class EmployeeDal
    {
        private readonly string _connectionString;

        public EmployeeDal(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if ((_connectionString = configuration.GetConnectionString("MySql")) == null)
            {
                throw new ArgumentException(
                    "Cannot find the connection string 'MySql' in app settings.",
                    nameof(configuration));
            }
        }

        public async Task<Employee> GetAsync(string id)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
SELECT `id`, `email`, `mobile`, `title`, `department`
FROM `employees`
WHERE `id` = @id;";
            return await connection.QuerySingleOrDefaultAsync<Employee>(sql,
                new
                {
                    id
                }
            );
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
SELECT `id`, `email`, `mobile`, `title`, `department`
FROM `employees`;";
            return (await connection.QueryAsync<Employee>(sql)).ToArray();
        }

        public async Task<bool> TryCreateAsync(Employee employee)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
SELECT COUNT(1)
FROM `employees`
WHERE id = @id;";

            // check whether the ID existed or not
            var count = await connection.ExecuteScalarAsync<int>(sql,
                new
                {
                    id = employee.Id
                }
            );
            if (count > 0)
            {
                return false;
            }

            sql = @"
INSERT INTO `employees` (`id`, `email`, `mobile`, `title`, `department`)
VALUES (@id, @email, @mobile, @title, @department);";

            // return the count of affected rows
            count = await connection.ExecuteAsync(sql,
                new
                {
                    id = employee.Id,
                    email = employee.Email,
                    mobile = employee.Mobile,
                    title = employee.Title,
                    department = employee.Department
                }
            );

            return count > 0;
        }

        public async Task UpdateAsync(Employee employee)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
UPDATE `employees`
SET
`id` = @id,
`email` = @email,
`mobile` = @mobile,
`title` = @title,
`department` = @department
WHERE `id` = @id;";
            await connection.ExecuteAsync(sql,
                new
                {
                    id = employee.Id,
                    email = employee.Email,
                    mobile = employee.Mobile,
                    title = employee.Title,
                    department = employee.Department
                }
            );
        }

        public async Task DeleteAsync(Employee employee)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
DELETE FROM `employees`
WHERE `id` = @id;";
            await connection.ExecuteAsync(sql,
                new
                {
                    id = employee.Id
                }
            );
        }
    }
}