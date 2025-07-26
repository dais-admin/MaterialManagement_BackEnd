
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using Newtonsoft.Json;
using Spire.Xls;
using System.Globalization;
using System.IO;
using System.Text.Json.Nodes;

namespace DAIS.API.ExcelReader
{
    public class ExcelDataImporter : IExcelDataImporter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static string UserName = string.Empty;
        public ExcelDataImporter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            UserName = _httpContextAccessor.HttpContext?.User.Identity!.Name!;
        }
        public async Task<List<ExcelReaderMetadata>> ImportExcelDataAsync(Stream fileStream, CancellationToken token)
        {
            var retult = await ReadExcelFileToJson(fileStream, token).ConfigureAwait(false);
            return retult;
        }

        private static async Task<List<ExcelReaderMetadata>> ReadExcelFileToJson(Stream fileStream, CancellationToken token)
        {
            string createdBy = UserName is null ? "Admin" : UserName;
            var jsonList = new List<ExcelReaderMetadata>();

            using (var workbook = new Workbook())
            {
                workbook.LoadFromStream(fileStream);
                var worksheet = workbook.Worksheets[0];

                int startRow = 2;
                int endRow = worksheet.LastRow;

                int endCol = worksheet.LastColumn;


                for (int row = startRow; row <= endRow; row++)
                {

                    var record = new ExcelReaderMetadata();


                    string col17 = worksheet.Range[row, 17].Value;
                    DateTime? purchaseDate = !string.IsNullOrEmpty(col17) ?
                                              Convert.ToDateTime(col17)
                                             : null;



                    var col18 = worksheet.Range[row, 18].Value.Trim();
                    DateTime? commissioningDate = !string.IsNullOrEmpty(col18) ?
                                                  Convert.ToDateTime(col18)
                                                  : null;

                    string col19 = worksheet.Range[row, 19].Value.Trim();
                    DateTime? yearOfInstallation = !string.IsNullOrEmpty(col19)
                                                   ? Convert.ToDateTime(col19)
                                                   : null;

                    var col20 = worksheet.Range[row, 20].Value.Trim();
                    DateTime? designLifeDate = !string.IsNullOrEmpty(col20)
                                               ? Convert.ToDateTime(col20)
                                               : null;

                    var col21 = worksheet.Range[row, 21].Value.Trim();
                    DateTime? endPeriodLifeDate = !string.IsNullOrEmpty(col21)
                                                ? Convert.ToDateTime(col19)
                                                : null;
                    var jsonObject = new
                    {
                        Asset = new
                        {
                            Project = new
                            {
                                ProjectName = worksheet.Range[row, 1].Value.Trim(),
                            },
                            WorkPackage = new
                            {
                                WorkPackageName = worksheet.Range[row, 2].Value.Trim()
                            },
                            LocationOfOperation = new
                            {
                                Name = worksheet.Range[row, 3].Value.Trim()
                            },
                            Region = new
                            {
                                Name = worksheet.Range[row, 4].Value.Trim()

                            },
                            Devision = new
                            {
                                Name = worksheet.Range[row, 5].Value.Trim()
                            },
                            MaterialType = new
                            {
                                Name = worksheet.Range[row, 6].Value.Trim()
                            },
                            Category = new
                            {
                                Name = worksheet.Range[row, 7].Value.Trim()
                            },
                            Supplier = new
                            {
                                Name = worksheet.Range[row, 8].Value.Trim()
                            },
                            ManuFacturer = new
                            {
                                Name = worksheet.Range[row, 9].Value.Trim()
                            },
                            System = worksheet.Range[row, 10].Value.Trim(),
                            MaterialName = worksheet.Range[row, 11].Value.Trim(),
                            MaterialCode = worksheet.Range[row, 12].Value.Trim(),
                            MaterialQty = worksheet.Range[row, 13].Value.Trim(),
                            LocationRFId = worksheet.Range[row, 14].Value.Trim(),
                            ModelNumber = worksheet.Range[row, 15].Value.Trim(),
                            TagNumber = worksheet.Range[row, 16].Value.Trim(),
                            PurchaseDate = purchaseDate,
                            CommissioningDate = commissioningDate,
                            YearOfInstallation = yearOfInstallation,
                            DesignLifeDate = designLifeDate,
                            EndPeriodLifeDate = endPeriodLifeDate,
                            IsRehabilitation = false,
                            MaterialStatus = worksheet.Range[row, 22].Value.Trim()

                        }
                    };
                    record.CreatedBy = createdBy;
                    record.CreatedDate = DateTime.UtcNow;
                    record.IsDeleted = false;
                    record.IsRead = false;
                    record.Message = JsonConvert.SerializeObject(jsonObject);
                    jsonList.Add(record);


                }

            }





            //foreach (var row in worksheet.Rows)
            //{
            //    var record = new ExcelReaderMetadata();

            //    if (firstRow)
            //    {
            //        firstRow = false;
            //        continue;
            //    }
            //    string row21 = row. Cell(21).GetString().Trim();
            //    DateTime? purchaseDate = !string.IsNullOrEmpty(row21) ?
            //                             DateTime.Parse(row21)
            //                             : null;

            //    string row22 = row.Cell(22).GetString().Trim();
            //    DateTime? yearOfInstallation = !string.IsNullOrEmpty(row22)
            //                                   ? DateTime.Parse(row22)
            //                                   : null;

            //    var row23 = row.Cell(23).GetString().Trim();
            //    DateTime? commissioningDate = !string.IsNullOrEmpty(row23) ?
            //                                  DateTime.Parse(row23)
            //                                  : null;
            //    var row24 = row.Cell(24).GetString().Trim();

            //    DateTime? designLifeDate = !string.IsNullOrEmpty(row24)
            //                               ? DateTime.Parse(row24)
            //                               : null;

            //    var row25 = row.Cell(25).GetString().Trim();
            //    DateTime? endPeriodLifeDate = !string.IsNullOrEmpty(row25)
            //                                ? DateTime.Parse(row25) : null;

            //    var jsonObject = new
            //    {

            //        Asset = new
            //        {
            //            Project = new
            //            {
            //                Name = row.Cell(1).GetString().Trim(),
            //                Code = row.Cell(2).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            MaterialType = new
            //            {
            //                Name = row.Cell(3).GetString().Trim(),
            //                Code = row.Cell(4).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            Category = new
            //            {
            //                Name = row.Cell(5).GetString().Trim(),
            //                Code = row.Cell(6).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            Region = new
            //            {
            //                Name = row.Cell(7).GetString().Trim(),
            //                Code = row.Cell(8).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            LocationOfOperation = new
            //            {
            //                Name = row.Cell(9).GetString().Trim(),
            //                Code = row.Cell(10).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            Devision = new
            //            {
            //                Name = row.Cell(11).GetString().Trim(),
            //                Code = row.Cell(12).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            ManuFacturer = new
            //            {
            //                Name = row.Cell(13).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            Supplier = new
            //            {
            //                Name = row.Cell(14).GetString().Trim(),
            //                CreatedBy = createdBy
            //            },
            //            System = row.Cell(15).GetString().Trim(),
            //            MaterialCode = row.Cell(16).GetString().Trim(),//TODO: this is Auto
            //            MaterialName = row.Cell(17).GetString().Trim(),
            //            TagNumber = row.Cell(18).GetString().Trim(),
            //            LocationRFId = row.Cell(19).GetString().Trim(),
            //            ModelNumber = row.Cell(20).GetString().Trim(),
            //            PurchaseDate = purchaseDate,
            //            YearOfInstallation = yearOfInstallation,
            //            CommissioningDate = commissioningDate,
            //            DesignLifeDate = designLifeDate,
            //            EndPeriodLifeDate = endPeriodLifeDate,
            //            IsRehabilitation = false,
            //            MaterialStatus = row.Cell(26).GetString().Trim(),//TODO This is enum
            //            CreatedBy = createdBy

            //        }
            //    };

            //    record.CreatedBy = createdBy;
            //    record.CreatedDate = DateTime.UtcNow;
            //    record.IsDeleted = false;
            //    record.IsRead = false;
            //    record.Message = JsonConvert.SerializeObject(jsonObject);
            //    jsonList.Add(record);
            //}


            return await Task.FromResult(jsonList);
        }
    }
}
