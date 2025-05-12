using GroupB.Models;
using Microsoft.Data.SqlClient;

namespace GroupB.Repositories;



public class ClientRepository : IClientRepository
{
    private readonly IConfiguration _configuration;

    public ClientRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Client?> GetClientByIdAsync(int clientId)
    {
        const string query = @"
            SELECT client_id, first_name, last_name, date_of_birth
            FROM Client
            WHERE client_id = @Id";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", clientId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Client
            {
                ClientId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                DateOfBirth = reader.GetDateTime(3)
            };
        }

        return null;
    }
}
