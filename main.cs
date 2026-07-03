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
        using (var cmd = new SqlCommand(createSql