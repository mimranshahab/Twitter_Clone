using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;


namespace Twitter_Clone.API.Data
{
    //public class DatabaseContext
    //{
    //    private readonly string _connectionString;

    //    public DatabaseContext(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //    }

    //    public SqlConnection CreateConnection()
    //    {
    //        return new SqlConnection(_connectionString);
    //    }
    //}
    public class DatabaseContext : IDisposable
    {
        //private readonly string _connectionString;
        private IDbConnection _connection;

        //public DatabaseContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}
        private readonly string _connectionString;
        private readonly ILogger<DatabaseContext> _logger;

        //public DatabaseContext(string connectionString, ILogger<DatabaseContext> logger)
        //{
        //    _connectionString = connectionString;
        //    _logger = logger;
        //    TestConnection();
        //}

        //private void TestConnection()
        //{
        //    try
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        connection.Open();
        //        _logger.LogInformation("Database connection successful.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Database connection failed.");
        //        throw;
        //    }
        //}

        //public SqlConnection CreateConnection()
        //{
        //    return new SqlConnection(_connectionString);
        //}
        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public IDbConnection Connection => _connection;

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
