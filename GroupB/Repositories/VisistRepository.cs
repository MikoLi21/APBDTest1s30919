
using GroupB.Models;
using Microsoft.Data.SqlClient;

namespace GroupB.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly IConfiguration _configuration;

    public VisitRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Visit?> GetVisitByIdAsync(int id)
    {
        const string query = @"
            SELECT visit_id, client_id, mechanic_id, date
            FROM Visit
            WHERE visit_id = @Id";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Visit
            {
                VisitId = reader.GetInt32(0),
                ClientId = reader.GetInt32(1),
                MechanicId = reader.GetInt32(2),
                Date = reader.GetDateTime(3)
            };
        }

        return null;
    }
    public async Task<bool> VisitExistsAsync(int visitId)
    {
        const string query = "SELECT 1 FROM Visit WHERE visit_id = @Id";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", visitId);

        var result = await command.ExecuteScalarAsync();
        return result != null;
    }

    public async Task AddVisitAsync(Visit visit)
    {
        const string query = @"
        INSERT INTO Visit (visit_id, client_id, mechanic_id, date)
        VALUES (@VisitId, @ClientId, @MechanicId, @Date)";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@VisitId", visit.VisitId);
        command.Parameters.AddWithValue("@ClientId", visit.ClientId);
        command.Parameters.AddWithValue("@MechanicId", visit.MechanicId);
        command.Parameters.AddWithValue("@Date", visit.Date);

        await command.ExecuteNonQueryAsync();
    }

}

