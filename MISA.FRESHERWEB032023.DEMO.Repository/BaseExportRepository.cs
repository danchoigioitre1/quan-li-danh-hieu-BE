using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHER032023.COMMON;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL
{
    public class BaseExportRepository<TEntity> : IBaseExportRepository<TEntity>
    {
        protected readonly string _connetionString;
        public BaseExportRepository(IConfiguration configuration)
        {
            // Lấy connectionString kết nối với DB
            _connetionString = configuration["ConnetionString"] ?? "";
        }
        public async Task<List<TEntity>> GetMappingTable(string subSystemCode)
        {
            try
            {
                var table = typeof(TEntity).Name;
                if (table == "EmulationMapping")
                {
                    table = "Emulation";
                }
                // Khởi tạo kết nối tới DB
                var connection = await GetOpenConnectionAsync();
                var sql = $"SELECT * FROM  import{table}mapping i WHERE I.SubSystemCode = '{subSystemCode}'";

                // Thức hiện câu lệnh sql và trả về danh sách tương ứng
                var data = await connection.QueryAsync<TEntity>(sql);
                // Đóng kết nối tới DB
                await connection.CloseAsync();
                // Trả về kết quả
                return (List<TEntity>)data;
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi 
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
        }

        public async Task<DbConnection> GetOpenConnectionAsync()
        {
            // Khởi tạo đối tượng kết nối
            var connection = new MySqlConnection(_connetionString);
            // Mở kết nối tới DB
            await connection.OpenAsync();
            // Trả về đối tượng kết nối
            return connection;
        }
    }
}
