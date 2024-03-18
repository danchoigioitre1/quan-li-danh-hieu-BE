using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHER032023.COMMON;
using MISA.FRESHER032023.COMMON.Models;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL.Repository
{
    public abstract class BaseRepository<TEntity, TEntityCreate> : IBaseRepository<TEntity, TEntityCreate>
    {
        protected readonly string _connetionString;
        public BaseRepository(IConfiguration configuration)
        {
            // Lấy connectionString kết nối với DB
            _connetionString = configuration["ConnetionString"] ?? "";
        }

        public virtual Task CreateAsync(TEntityCreate entityCreate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            // Khởi tạo kết nối tới DB 
            var connection = await GetOpenConnectionAsync();
            // Khởi tạo transaction
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                var table = typeof(TEntity).Name;
                if (table == "EmulationBase")
                {
                    table = "Emulation";
                }

                // Khởi tạo câu lệnh sql 
                var sqlDelete = $"proc_delete{table}ById";
                // Khởi tạo tham số đầu vào
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add($"${table}Id", id);
                // Thực thi câu lệnh sql 
                var result = await connection.ExecuteAsync(sqlDelete, dynamicParam, commandType: System.Data.CommandType.StoredProcedure, transaction:transaction);
                // Xác nhận thay đổi trên DB
                await transaction.CommitAsync();
                // Trả về kết quả lấy được
                return result;

            }
            catch (Exception ex)
            {
                // Hủy bỏ transaction và throw 1 ex
                await transaction.RollbackAsync();
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
            finally
            {
                // Đóng kết nối
                await connection.CloseAsync();
                await transaction.DisposeAsync();
            }


        }

        public virtual Task<List<TEntity>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            try
            {
                var table = typeof(TEntity).Name;
                if (table == "EmulationBase")
                {
                    table = "Emulation";
                }
                // Mở kết nối tới DB
                var connection = await GetOpenConnectionAsync();
                // Khởi tạo câu lệnh sql
                var sql = $"proc_get{table}ById";
                // Khởi tạo tham số đầu vào
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add($"${table}Id", id);
                // Thực thi câu lệnh sql và gán giá trị trả về 
                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(sql, dynamicParam, commandType: System.Data.CommandType.StoredProcedure);
                // Đóng kết nối tới DB
                await connection.CloseAsync();
                // Trả về giá trị
                return entity;
            }
            catch (Exception ex)
            {
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

        public virtual Task UpdateAsync(Guid id, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
