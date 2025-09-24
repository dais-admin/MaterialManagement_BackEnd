using Aspose.Cells;
using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Interfaces.Report;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Helpers;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAIS.CoreBusiness.Services.Reports
{
    public class MaterialAuditReportService: IMaterialAuditReportService
    {
        private readonly IGenericRepository<Material> _genericRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialAuditReportService> _logger;
        private readonly IMaterialService _materialService;
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;
        public MaterialAuditReportService(IGenericRepository<Material> genericRepository, IMapper mapper,
            IMaterialService materialService,ILogger<MaterialAuditReportService> logger, MaterialServiceInfrastructure materialServiceInfrastructure) 
        { 
            _genericRepository = genericRepository;
            _mapper = mapper;
            _logger = logger;
            _materialService = materialService;
            _materialServiceInfrastructure = materialServiceInfrastructure;
        }
        public async Task<List<MaterialAuditReportDto>> GetMaterialAuditReport()
        {
            _logger.LogInformation("MaterialService:GetMaterialAuditReport:Method Start");
            List<MaterialAuditReportDto> materialAuditReportListDto =await  GetAuditReport();       
            _logger.LogInformation("MaterialService:GetMaterialAuditReport:Method End");
            return materialAuditReportListDto;
        }

        public async Task<string> GenerateMaterialAuditReport(string folderPath,string parameters)
        {
            _logger.LogInformation("MaterialService:GetMaterialAuditReport:Method Start");
            List<MaterialAuditReportDto> materialAuditReportListDto = await GetAuditReport();
            string generatedFilePath= GenerateExcel(materialAuditReportListDto, folderPath,parameters);
            _logger.LogInformation("MaterialService:GetMaterialAuditReport:Method End");
            return generatedFilePath;
        }
        public async Task<List<MaterialAuditReportDto>> GetMaterialsAuditWithFilterAsync(MaterialFilterDto materialFilterDto)
        {
            _materialServiceInfrastructure.Logger.LogInformation("MaterialAuditReportService:GetMaterialsAuditWithFilterAsync:Method Start");
            List<MaterialAuditReportDto> materialAuditReportListDto = new List<MaterialAuditReportDto>();
            try
            {
               
                var materialList = await _materialServiceInfrastructure.GenericRepository.Query()
                    .Where(x => x.WorkPackageId == materialFilterDto.WorkPackageId
                    && x.System == materialFilterDto.System
                    && x.IsRehabilitation == (materialFilterDto.KindOfMaterial == "Material" ? false : true))
                    .Include(x => x.MaterialType)
                    .Include(x => x.Category)
                    .Include(x => x.Region)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Location)
                    .Include(x => x.Supplier)                    
                    .Include(x => x.WorkPackage)
                    .ToListAsync().ConfigureAwait(false);
            
                foreach (var material in materialList)
                {
                    var materialAuditReportDto = new MaterialAuditReportDto()
                    {
                        MaterialName = material.MaterialName,
                        MaterialCode = material.MaterialCode,
                        System = material.System,
                        IsRehabilitation=material.IsRehabilitation,
                        CreatedBy = material.CreatedBy,
                        CreatedOn = material.CreatedDate.ToString(),
                        UpdatedBy = material.UpdatedBy,
                        UpdatedOn = material.UpdatedDate.ToString(),
                        IsDeleted = material.IsDeleted,
                        Location= _mapper.Map<LocationOperationDto>(material.Location),
                        WorkPackage=_mapper.Map<WorkPackageDto>( material.WorkPackage)

                    };
                    materialAuditReportListDto.Add(materialAuditReportDto);
                }
            }
            catch (Exception ex)
            {
                _materialServiceInfrastructure.Logger.LogError(ex.Message, ex);
                throw ex;
            }
            _materialServiceInfrastructure.Logger.LogInformation("MaterialAuditReportService:GetMaterialsAuditWithFilterAsync:Method End");
            return materialAuditReportListDto;
        }

        private async Task<List<MaterialAuditReportDto>> GetAuditReport()
        {
            List<MaterialAuditReportDto> materialAuditReportListDto = new List<MaterialAuditReportDto>();
            try
            {
                var materialList = await _genericRepository.GetAll();
                foreach (var material in materialList)
                {
                    var materialAuditReportDto = new MaterialAuditReportDto()
                    {
                        MaterialName = material.MaterialName,
                        MaterialCode = material.MaterialCode,
                        CreatedBy = material.CreatedBy,
                        CreatedOn = material.CreatedDate.ToString(),
                        UpdatedBy = material.UpdatedBy,
                        UpdatedOn = material.UpdatedDate.ToString(),
                        IsDeleted=material.IsDeleted
                    };
                    materialAuditReportListDto.Add(materialAuditReportDto);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:" + ex.Message);
                throw ex;
            }
            return materialAuditReportListDto;
        }
        private string GenerateExcel(List<MaterialAuditReportDto> materialAuditReportListDto,string folderPath,string parameters)
        {
            int dataRowIndex = 5;
            string generatedFilePath = string.Empty;
            try
            {
                License license = new License();
                // Set the license of Aspose.Cells to avoid the evaluation limitations
                //license.SetLicense(dataDir + "Aspose.Cells.lic");

                // initiate an instance of Workbook
                Workbook book = new Workbook();
                // access first (default) worksheet
                var sheet = book.Worksheets[0];
                // access CellsCollection of first worksheet
                sheet.Cells.Merge(0, 0, 1, 7);
                sheet.Cells.Merge(1, 0, 1, 7);
                sheet.Cells["A1"].Value = "BWSSB Stage 5 -  Material Management System";
                sheet.Cells["A2"].Value = "Material Summary Report";
                sheet.Cells["A4"].Value = "Sl.No";
                sheet.Cells["B4"].Value = "Material Name";
                sheet.Cells["C4"].Value = "Material Code";
                sheet.Cells["D4"].Value = "Created By";
                sheet.Cells["E4"].Value = "Created On";
                sheet.Cells["F4"].Value = "Updated By";
                sheet.Cells["G4"].Value = "Updated On";
                // write Datat to cells 
                for(int i = 0; i < materialAuditReportListDto.Count; i++)
                {
                    sheet.Cells.InsertRow(dataRowIndex+i);
                    sheet.Cells[$"A{dataRowIndex + i}"].Value= i+1;
                    sheet.Cells[$"B{dataRowIndex + i}"].Value = materialAuditReportListDto[i].MaterialName;
                    sheet.Cells[$"C{dataRowIndex + i}"].Value = materialAuditReportListDto[i].MaterialCode;
                    sheet.Cells[$"D{dataRowIndex + i}"].Value = materialAuditReportListDto[i].CreatedBy;
                    sheet.Cells[$"E{dataRowIndex + i}"].Value = materialAuditReportListDto[i].CreatedOn;
                    sheet.Cells[$"F{dataRowIndex + i}"].Value = materialAuditReportListDto[i].UpdatedBy;
                    sheet.Cells[$"G{dataRowIndex + i}"].Value = materialAuditReportListDto[i].UpdatedOn;
                }
                // save spreadsheet to disc
                string fileName = "MaterialAuditReport_" + 
                    DateTime.Now.Year.ToString()+ DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString()
                    + DateTime.Now.Minute.ToString()+".xlsx";
                generatedFilePath = folderPath + fileName;
                book.Save(generatedFilePath, SaveFormat.Xlsx);

            }
            catch(Exception ex)
            {
                _logger.LogError("Excel Error:" + ex.Message);
                throw ex;
            }

            return generatedFilePath;

        }
        private void ExcelToHTML()
        {
            var workbook = new Aspose.Cells.Workbook("template.xlsx");
            
            //This is how we can export workbook to the HTML file
            workbook.Save("output.html", Aspose.Cells.SaveFormat.Html);
        }
    }
}
