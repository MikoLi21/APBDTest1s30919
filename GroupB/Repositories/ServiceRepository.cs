using GroupB.Models;
using Microsoft.Data.SqlClient;

namespace GroupB.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly IConfiguration _configuration;

    public ServiceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<Service>> GetServicesByVisitIdAsync(int visitId)
    {
        const string query = @"
            SELECT s.service_id, s.name, vs.service_fee
            FROM Service s
            INNER JOIN Visit_Service vs ON s.service_id = vs.service_id
            WHERE vs.visit_id = @VisitId";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@VisitId", visitId);

        using var reader = await command.ExecuteReaderAsync();

        var services = new List<Service>();
        while (await reader.ReadAsync())
        {
            services.Add(new Service
            {
                ServiceId = reader.GetInt32(0),
                Name = reader.GetString(1),
                ServiceFee = reader.GetDecimal(2)
            });
        }

        return services;
    }
    public async Task<Service?> GetServiceByNameAsync(string name)
    {
        const string query = "SELECT service_id, name, base_fee FROM Service WHERE name = @Name";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", name);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Service
            {
                ServiceId = reader.GetInt32(0),
                Name = reader.GetString(1),
                ServiceFee = reader.GetDecimal(2) // можно заменить на base_fee, если не нужен
            };
        }

        return null;
    }

    public async Task AddVisitServiceAsync(int visitId, int serviceId, decimal fee)
    {
        const string query = @"
        INSERT INTO Visit_Service (visit_id, service_id, service_fee)
        VALUES (@VisitId, @ServiceId, @Fee)";

        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@VisitId", visitId);
        command.Parameters.AddWithValue("@ServiceId", serviceId);
        command.Parameters.AddWithValue("@Fee", fee);

        await command.ExecuteNonQueryAsync();
    }

}