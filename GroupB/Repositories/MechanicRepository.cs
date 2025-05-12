using GroupB.Models;
using Microsoft.Data.SqlClient;

namespace GroupB.Repositories;




public class MechanicRepository : IMechanicRepository
{
    private readonly IConfiguration _configuration;

    public MechanicRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<Mechanic?> GetMechanicByIdAsync(int mechanicId)
    {
        const string query = @"
            SELECT mechanic_id, licence_number
            FROM Mechanic
            WHERE mechanic_id = @Id";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", mechanicId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Mechanic
            {
                MechanicId = reader.GetInt32(0),
                LicenceNumber = reader.GetString(1)
            };
        }

        return null;
    }
    public async Task<Mechanic?> GetMechanicByLicenceAsync(string licenceNumber)
    {
        const string query = "SELECT mechanic_id, licence_number FROM Mechanic WHERE licence_number = @Licence";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Licence", licenceNumber);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Mechanic
            {
                MechanicId = reader.GetInt32(0),
                LicenceNumber = reader.GetString(1)
            };
        }

        return null;
    }

}
