using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHERWEB032023.DL.Repository.EmulationRepo;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using MISA.FRESHER032023.COMMON;
using MISA.FRESHER032023.COMMON.Models;

namespace MISA.FRESHERWEB032023.DL.Repository.Emulation
{
    public class EmulationRepository : BaseRepository<EmulationBase, EmulationCreate>, IEmulationRepository
    {

        public EmulationRepository(IConfiguration configuration) : base(configuration)
        {
        }


        public override async Task UpdateAsync(Guid emulationId, EmulationBase emulation)
        {
            // Khởi tạo kết nối tới DB
            var connection = await GetOpenConnectionAsync();
            // Khởi tạo transaction
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Khởi tạo câu lệnh sql
                var query = $"UPDATE emulation e SET e.EmulationCode = @EmulationCode, e.EmulationTarget = @EmulationTarget,e.EmulationStatus = @EmulationStatus,e.EmulationType = @EmulationType,e.EmulationLevel = @EmulationLevel,e.EmulationName=@EmulationName  WHERE e.EmulationId = @emulationId";
                // KHởi tạo các tham số đầu vào
                var parameters = new DynamicParameters();
                // Lấy danh sách các keys trong emulation
                var properties = emulation.GetType().GetProperties();
                // Lặp qua các keys để thêm các tham số đầu vào
                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var paramName = "@" + property.Name;
                    var paramValue = property.GetValue(emulation);
                    parameters.Add(paramName, paramValue);
                }
                parameters.Add("@emulationId", emulationId);
                // Thực hiện câu lệnh sql
                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                // Xác nhận thay đổi trên DB
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi
                await transaction.RollbackAsync();
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
            finally
            {
                // Đóng kết nối 
                await transaction.DisposeAsync();
                await connection.CloseAsync();
            }

        }


        public override async Task CreateAsync(EmulationCreate emulationCreate)
        {
            // Khởi tạo kết nối tới DB
            var connection = await GetOpenConnectionAsync();
            // Khởi tạo transaction
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Khởi tạo câu lệnh sql
                var query = "INSERT INTO emulation (EmulationId, EmulationCode, EmulationTarget,EmulationStatus,EmulationType,EmulationLevel,EmulationName) VALUES (UUID(), @EmulationCode, @EmulationTarget,@EmulationStatus,@EmulationType,@EmulationLevel,@EmulationName)";
                // Khởi tạo các tham số đầu vào
                var parameters = new DynamicParameters();
                // Lấy danh sách các keys trong emulationCreate
                var properties = emulationCreate.GetType().GetProperties();
                // Lặp qua các keys để thêm các tham số đầu vào
                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var paramName = "@" + property.Name;
                    var paramValue = property.GetValue(emulationCreate);
                    parameters.Add(paramName, paramValue);
                }
                // Thực hiện câu lệnh sql
                await connection.ExecuteAsync(query, parameters, transaction: transaction);
                // Xác nhận thay đổi trên DB
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi
                await transaction.RollbackAsync();
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
            finally
            {
                // Đóng kết nối tới DB
                await transaction.DisposeAsync();
                await connection.CloseAsync();
            }

        }

        public async Task<List<EmulationBase>?> GetPageAsync(int pageSize, int pageNumber, string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus)
        {
            try
            {
                // Mở kết nối tới DB, khởi tạo câu lệnh sql, Khởi tạo tham số đầu vào 
                var connection = await GetOpenConnectionAsync();
                var sql = "proc_getAllEmulation";
                var parameters = new DynamicParameters();

                // Thêm các tham số đầu vào
                parameters.Add("$pageSize", pageSize);
                parameters.Add("$pageNumber", pageNumber);
                parameters.Add("$searchName", searchName);
                parameters.Add("$searchCode", searchCode);
                parameters.Add("$EmulationLevel", EmulationLevel);
                parameters.Add("$EmulationTarget", EmulationTarget);
                parameters.Add("$EmulationType", EmulationType);
                parameters.Add("$EmulationStatus", EmulationStatus);

                // Thức hiện câu lệnh sql và trả về danh sách tương ứng
                var emulationList = await connection.QueryAsync<EmulationBase?>(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Đóng kết nối tới DB
                await connection.CloseAsync();
                // Trả về kết quả
                return (List<EmulationBase>?)emulationList;
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }


        }

        public async Task<int> CheckCode(string emulationCode, string id)
        {
            try
            {
                // Khởi tạo kết nối tới DB, tham số đầu vào, sql
                var connection = await GetOpenConnectionAsync();
                var parameters = new DynamicParameters();
                var sql = "";

                // TH thêm mới: kết quả trả về 0 => chưa tồn tại mã code trong DB, 1 => Đã tồn tại mã code trong DB
                // TH sửa: kết quả trả về > 1 => có 1 bản ghi khác bản ghi đã chọn có mã code này trong DB, 0 => mã code chưa tồn tại trong DB
                if (id == null)
                {
                    // sql cho trường hợp check code khi thêm mới
                    sql = "SELECT COUNT(*) FROM emulation e WHERE e.EmulationCode = @emulationCode";
                }
                else
                {
                    // sql cho trường hợp check code khi sửa thông tin
                    sql = "SELECT COUNT(*) FROM emulation e WHERE e.EmulationCode = @emulationCode AND e.EmulationId <> @id";
                    parameters.Add("@id", id);
                }
                // Thêm tham số đầu vào
                parameters.Add("@emulationCode", emulationCode);
                // Thực hiện câu lệnh sql và trả về kết quả
                var emulationDB = await connection.ExecuteScalarAsync<int>(sql, parameters);

                // Đóng kết nối tới DB
                await connection.CloseAsync();
                return emulationDB;
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi 
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            };

        }

        public async Task MultipleDelete(List<Guid> listId)
        {
            // KHởi tạo kết nối tới DB và transaction
            var connection = await GetOpenConnectionAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Khởi tạo câu lệnh sql và tham số đầu vào
                string sql = $"DELETE FROM emulation WHERE EmulationId IN @listId";
                var parameters = new DynamicParameters();
                // Thêm tham số đầu vào
                parameters.Add("@listId", listId);
                // Thực hiện câu lệnh sql
                await connection.ExecuteAsync(sql, parameters, transaction: transaction);
                // Xác nhận thay đổi trong DB
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi và hủy bỏ transaction
                await transaction.RollbackAsync();
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
            finally
            {
                // Đóng kết nối tới DB và transaction
                await transaction.DisposeAsync();
                await connection.CloseAsync();
            }

        }

        public async Task MultipleChangeStatusAsync(Guid[] listId, int status)
        {
            // KHởi tạo kết nối tới DB và transaction
            var connection = await GetOpenConnectionAsync();
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Khởi tạo câu lệnh sql và tham số đầu vào
                string sql = $"UPDATE emulation e SET e.EmulationStatus = @status WHERE EmulationId IN @listId";
                var parameters = new DynamicParameters();
                parameters.Add("@listId", listId);
                parameters.Add("@status", status);
                // Thực hiện câu lệnh sql
                await connection.ExecuteAsync(sql, parameters, transaction: transaction);
                // Xác nhận thay đổi trong DB
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi và hủy bỏ transaction
                await transaction.RollbackAsync();
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
            finally
            {
                // Đóng kết nối tới DB và transaction
                await connection.CloseAsync();
                await transaction.DisposeAsync();
            }
        }

        public async Task<int> GetTotalAsync(string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus)
        {
            try
            {
                // Khởi tạo câu lệnh sql và tham số đầu vào
                var connection = await GetOpenConnectionAsync();
                var sql = "proc_getTotalData";
                var parameters = new DynamicParameters();
                parameters.Add("$searchName", searchName);
                parameters.Add("$searchCode", searchCode);
                parameters.Add("$EmulationLevel", EmulationLevel);
                parameters.Add("$EmulationTarget", EmulationTarget);
                parameters.Add("$EmulationType", EmulationType);
                parameters.Add("$EmulationStatus", EmulationStatus);
                // Thực hiện và trả về kết quả
                var total = await connection.ExecuteScalarAsync<int>(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Đóng kết nối tới DB
                await connection.CloseAsync();
                return total;
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
        }

        public async Task<List<dynamic>> GetImportMapping()
        {
           
            try
            {
                // Khởi tạo kết nối tới DB
                var connection = await GetOpenConnectionAsync();
                var sql = "SELECT * FROM importmapping";

                // Thức hiện câu lệnh sql và trả về danh sách tương ứng
                var data = await connection.QueryAsync(sql);
                // Đóng kết nối tới DB
                await connection.CloseAsync();
                // Trả về kết quả
                return (List<dynamic>)data;
            }
            catch (Exception ex)
            {
                // throw 1 exception khi xảy ra lỗi 
                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
           
        }
    }
}
