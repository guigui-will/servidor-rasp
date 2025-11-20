// Program.cs
using System.Data;
using System.Data.SqlClient;

class Program
{
    static async Task Main()
    {
        var connString = "Server=localhost;Database=AppDb;Trusted_Connection=True;TrustServerCertificate=True;";

        using var conn = new SqlConnection(connString);
        await conn.OpenAsync();

        // CREATE TABLE (se não existir)
        var createSql = @"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Clientes' AND xtype='U')
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(120) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    CriadoEm DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);";
        using (var cmd = new SqlCommand(createSql, conn))
            await cmd.ExecuteNonQueryAsync();

        // INSERT
        var insertSql = "INSERT INTO Clientes (Nome, Email) VALUES (@Nome, @Email);";
        using (var cmd = new SqlCommand(insertSql, conn))
        {
            cmd.Parameters.AddWithValue("@Nome", "Everton");
            cmd.Parameters.AddWithValue("@Email", "willzinho@example.com");
            await cmd.ExecuteNonQueryAsync();
        }

        // SELECT
        var selectSql = "SELECT Id, Nome, Email, CriadoEm FROM Clientes ORDER BY Nome;";
        using (var cmd = new SqlCommand(selectSql, conn))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32(0);
                var nome = reader.GetString(1);
                var email = reader.GetString(2);
                var criadoEm = reader.GetDateTime(3);
                Console.WriteLine($"{id} | {nome} | {email} | {criadoEm:O}");
            }
        }

        // UPDATE
        var updateSql = "UPDATE Clientes SET Nome = @Nome WHERE Email = @Email;";
        using (var cmd = new SqlCommand(updateSql, conn))
        {
            cmd.Parameters.AddWithValue("@Nome", "willzinho Silva");
            cmd.Parameters.AddWithValue("@Email", "everton@example.com");
            await cmd.ExecuteNonQueryAsync();
        }

        // DELETE
        var deleteSql = "DELETE FROM Clientes WHERE Email = @Email;";
        using (var cmd = new SqlCommand(deleteSql, conn))
        {
            cmd.Parameters.AddWithValue("@Email", "willzinho@example.com");
            await cmd.ExecuteNonQueryAsync();
        }
    }# servidor-rasp
