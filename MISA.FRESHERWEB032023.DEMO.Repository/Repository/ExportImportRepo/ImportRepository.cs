using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHER032023.COMMON;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using System.Transactions;
using MISA.FRESHER032023.COMMON.Models.Base;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;

namespace MISA.FRESHERWEB032023.DL.Repository.ExportImportRepo
{
    public class ImportRepository : BaseImportRepository<EmulationMapping>, IImportRepository
    {
        public ImportRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<string>> GetListEmulationCode()
        {
            try
            {


                var connection = await GetOpenConnectionAsync();
                // Khởi tạo câu lệnh sql
                var sql = $"SELECT EmulationCode from emulation";

                // Thực thi câu lệnh sql và gán giá trị trả về 
                var list = await connection.QueryAsync<List<string>>(sql);
                // Đóng kết nối tới DB
                await connection.CloseAsync();
                // Trả về giá trị
                return (List<string>)list;

            }
            catch (Exception ex)
            {

                throw new InternalException(Resource.ResourceManager.GetString("errorSql") ?? "");
            }
        }

        public async Task MultipleAddEmlation(List<EmulationExcel> listData)
        {
            // Khởi tạo kết nối tới DB
            var connection = await GetOpenConnectionAsync();
            // Khởi tạo transaction
            var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Khởi tạo câu lệnh sql
                var comma = "";
                var query = "INSERT INTO emulation (EmulationId, EmulationName, EmulationCode, EmulationStatus, EmulationTarget, EmulationLevel, EmulationType) VALUES ";
                // Khởi tạo các tham số đầu vào
                for (int i = 0; i < listData.Count; i++)
                {
                    var data = listData[i];
                    var insertStatement = $"(UUID(), '{data.EmulationName}', '{data.EmulationCode}', {data.EmulationStatus}, {data.EmulationTarget}, {data.EmulationLevel}, {data.EmulationType})";


                    if (i != listData.Count - 1)
                    {
                        comma = ",";

                    }
                    else
                    {
                        comma = ";";
                    }
                    query = query + insertStatement + comma;
                }

                await connection.ExecuteAsync(query, transaction: transaction);
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
    }
}
