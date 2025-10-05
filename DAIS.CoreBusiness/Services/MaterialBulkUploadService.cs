using AutoMapper;
using ClosedXML.Excel;
using DAIS.CoreBusiness.Constants;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Spire.Xls;
using System;
using System.Security.Claims;


namespace DAIS.CoreBusiness.Services
{
    public class MaterialBulkUploadService: IMaterialBulkUploadService
    {
        private readonly MaterialServiceInfrastructure _materialServiceInfrastructure;
        private readonly MaterialServiceDependencies _materialServiceDependencies;
        private readonly IBulkUploadDetailService _bulkUploadDetailService;
        public IBulkUploadRepository<Material> _bulkUploadRepository;
        public IMapper _mapper;
        public ILogger<MaterialBulkUploadService> _logger;
        private string userName = string.Empty;
        private Guid projectId = Guid.Empty;
        private string roleName = string.Empty;
        private string newFileName = string.Empty;
        private string folderPath= string.Empty;
        public MaterialBulkUploadService(MaterialServiceDependencies materialServiceDependencies, 
            MaterialServiceInfrastructure materialServiceInfrastructure,
            IBulkUploadRepository<Material> bulkUploadRepository,
            IBulkUploadDetailService bulkUploadDetailService,
            IMapper mapper, ILogger<MaterialBulkUploadService> logger)
        {
            _materialServiceDependencies = materialServiceDependencies;
            _materialServiceInfrastructure = materialServiceInfrastructure;
            _bulkUploadDetailService = bulkUploadDetailService;
            _bulkUploadRepository = bulkUploadRepository;
            _mapper = mapper;
            _logger = logger;
            if (_materialServiceInfrastructure.HttpContextAccessor.HttpContext != null)
            {
                SetUserAndProject(_materialServiceInfrastructure.HttpContextAccessor.HttpContext.User);
            }
        }
        private void SetUserAndProject(ClaimsPrincipal user)
        {
            if (user != null)
            {
                userName = user.Claims.FirstOrDefault(x => x.Type == Claims.NameClaim).Value;
                roleName = user.Claims.FirstOrDefault(x => x.Type == Claims.RoleClaim).Value;
            }
        }
        public async Task<bool> GenrateMaterialUploadTemplate(string path)
        {
            bool isSuccess = false;
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.AddWorksheet("Sheet1");

                    string[] columnNames = {"Project", "WorkPackage",
                 "System(Water Scada/Sewerage Scada)", 
                 "Division","SubDivision","Location of Operation","Region","Material Type","Material Category",
                 "Supplier","Manufacturer","Contractor","MaterialName","MaterialQty",
                 "Measurement","MadelNumber","TagNumber",
                 "Purchase Date","Commissioning Date","Date of Supply",
                 "Installation Date","DesignLife Date","EndPeriodLife Date",
                 "Material Status","Remarks" };

                    // Add headers
                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        var cell = worksheet.Cell(1, i + 1);
                        cell.Value = columnNames[i];
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.Orange;
                        worksheet.Column(i + 1).Width = 20;
                    }

                    await AddColumnDataToSheet(worksheet);

                    // Save the workbook
                    workbook.SaveAs(Path.Combine(path, "MaterialTemplate.xlsx"));
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return isSuccess;
        }

        private async Task<bool> AddColumnDataToSheet(IXLWorksheet worksheet)
        {
            bool isSuccess = false;
            try
            {
                var projectList = await _materialServiceDependencies.ProjectService.GetAllProjects();
                var workPackageList = await _materialServiceDependencies.WorkPackageService.GetAllWorkPackages();
                var divisionList = await _materialServiceDependencies.DivisionService.GetAllDivision();
                var subDivisionList = await _materialServiceDependencies.SubDivisionService.GetAllSubDivision();
                var locationList = await _materialServiceDependencies
                    .LocationOperationService.GetAllLocationOperation();
                   
                var regionList = await _materialServiceDependencies.RegionService.GetAllRegions();
                
                
                var materilTypeList = await _materialServiceDependencies.MaterialTypeService.GetAllMaterialTypes();
                var categoryList = await _materialServiceDependencies.CategoryService.GetAllCategory();
                var supplierList = await _materialServiceDependencies.SupplierService.GetAllSupplier();
                var manufacturerList = await _materialServiceDependencies.ManufacturerService.GetAllManufacturer();
                var measurementList = await _materialServiceDependencies.MaterialMeasurementService.GetAllMaterialMeasurement();
                var contractorList = await _materialServiceDependencies.ContractorService.GetAllContractor();

                // Create validation lists sheet
                var validationSheet = worksheet.Workbook.AddWorksheet("ValidationLists");
                validationSheet.Hide();

                // Add validation data to hidden sheet and create named ranges
                int currentRow = 1;
                AddValidationList(validationSheet, "Projects", ref currentRow, 
                    projectList.Select(x => x.ProjectName).ToList());

                AddValidationList(validationSheet, "WorkPackages", ref currentRow, 
                    workPackageList.Select(x => x.WorkPackageName).ToList());

                AddValidationList(validationSheet, "Systems", ref currentRow, new List<string> { "Water", "Sewerage" });
                
                
                AddValidationList(validationSheet, "Divisions", ref currentRow, 
                    divisionList.Select(x => x.DivisionName).Distinct().ToList());

                AddValidationList(validationSheet, "SubDivisions", ref currentRow,
                    subDivisionList.Select(x => x.SubDivisionName).Distinct().ToList());

                AddValidationList(validationSheet, "Locations", ref currentRow,
                    locationList.Select(x => x.LocationOperationName).Distinct().ToList());


                AddValidationList(validationSheet, "Regions", ref currentRow, 
                    regionList.Select(x => x.RegionName).Distinct().ToList());

                AddValidationList(validationSheet, "MaterialTypes", ref currentRow, 
                    materilTypeList.Select(x => x.TypeName).Distinct().ToList());

                AddValidationList(validationSheet, "Categories", ref currentRow, 
                    categoryList.Select(x => x.CategoryName).Distinct().ToList());

                AddValidationList(validationSheet, "Suppliers", ref currentRow, 
                    supplierList.Select(x => x.SupplierName).Distinct().ToList());

                AddValidationList(validationSheet, "Manufacturers", ref currentRow,
                    manufacturerList.Select(x => x.ManufacturerName).Distinct().ToList());

                AddValidationList(validationSheet, "Contractors", ref currentRow, 
                    contractorList.Select(x => x.ContractorName).Distinct().ToList());

                AddValidationList(validationSheet, "Measurements", ref currentRow, 
                    measurementList.Select(x => x.MeasurementName).ToList());

                AddValidationList(validationSheet, "Status", ref currentRow, new List<string> { "Purchased", "InUse", "UnderMaintanance", "Defective", "Expired", "RequiredLicense" });

                // Add data validation for each column
                for (int row = 2; row <= 100; row++)
                {
                    var range = worksheet.Range(row, 1, row, 1);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Projects").Ranges.First());

                    range = worksheet.Range(row, 2, row, 2);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("WorkPackages").Ranges.First());

                    

                    range = worksheet.Range(row, 3, row, 3);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Systems").Ranges.First());

                    range = worksheet.Range(row, 4, row, 4);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Divisions").Ranges.First());


                    range = worksheet.Range(row, 5, row, 5);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("SubDivisions").Ranges.First());

                    range = worksheet.Range(row, 6, row, 6);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Locations").Ranges.First());

                    range = worksheet.Range(row, 7, row, 7);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Regions").Ranges.First());

                    range = worksheet.Range(row, 8, row, 8);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("MaterialTypes").Ranges.First());

                    range = worksheet.Range(row, 9, row, 9);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Categories").Ranges.First());

                    range = worksheet.Range(row, 10, row, 10);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Suppliers").Ranges.First());

                    range = worksheet.Range(row, 11, row, 11);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Manufacturers").Ranges.First());

                    range = worksheet.Range(row, 12, row, 12);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Contractors").Ranges.First());

                    range = worksheet.Range(row, 15, row, 15);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Measurements").Ranges.First());

                    range = worksheet.Range(row, 24, row, 24);
                    range.SetDataValidation().List(worksheet.Workbook.NamedRanges.NamedRange("Status").Ranges.First());
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

            }
            return isSuccess;
        }

        private void AddValidationList(IXLWorksheet sheet, string name, ref int startRow, List<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                sheet.Cell(startRow + i, 1).Value = values[i];
            }
            var range = sheet.Range(startRow, 1, startRow + values.Count - 1, 1);
            sheet.Workbook.NamedRanges.Add(name, range);
            startRow += values.Count + 1;
        }

        public BulkUploadResponseDto BulkUpload(Stream fileStream,string path, bool isRehabilitation)
        {
            folderPath = path;
            var bulkUploadResponse=ReadExcelForValidation(fileStream, isRehabilitation);
            if (bulkUploadResponse.IsSuccess)
            {
                //Add Record in BulkUpload Details
                BulkUploadDetailsDto bulkUploadDetail = AddBulkUploadDetails(bulkUploadResponse.Materials.Count);
                //Add records in Material
                bulkUploadResponse.Materials = AddMaterials(bulkUploadResponse.Materials, (Guid)bulkUploadDetail.Id);
                bulkUploadResponse.BulkUploadDetails = bulkUploadDetail;
            }
            return bulkUploadResponse;               
        }

        private BulkUploadResponseDto ReadExcelForValidation(Stream fileStream,bool isRehabilitation)
        {

            BulkUploadResponseDto bulkUploadResponse = new BulkUploadResponseDto();

            try
            {
                using (var workbook = new Workbook())
                {
                    workbook.LoadFromStream(fileStream);
                    
                    var worksheet = workbook.Worksheets[0];

                    int startRow = 2;
                    int endRow = worksheet.LastDataRow;

                    int endCol = worksheet.LastColumn;

                    var validationList = new List<BulkUploadValidationDto>();
                    var materialListDto=new List<MaterialDto>();
                    for (int row = startRow; row <= endRow; row++)
                    {
                        var validation = new BulkUploadValidationDto();
                        var materialDto = new MaterialDto();
                        var projectName = worksheet.Range[row, 1].Value.Trim();
                        validation = CheckFieldValidation("Project Name", projectName, "A" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        
                        //
                        var workPackageName = worksheet.Range[row, 2].Value.Trim();
                        validation = CheckFieldValidation("WorkPackage Name", workPackageName, "B" + row);
                        if(!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var workPackage = _materialServiceDependencies.WorkPackageService.GetWorkPackageIdByName(workPackageName);
                            if(workPackage != null)
                            {
                                materialDto.WorkPackageId = (Guid)workPackage.Id;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Work Package is not exists in database";
                                validation.FieldValue = workPackageName;
                                validation.FieldName = "Work Package";
                                validation.FieldAddress = "B" + row;
                                validationList.Add(validation);
                            }
                        }
                        //System
                        var system = worksheet.Range[row, 3].Value.Trim();
                        validation = CheckFieldValidation("System", system, "C" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            materialDto.System = system;
                        }

                        
                        //Division
                        var divisionName = worksheet.Range[row, 4].Value.Trim();
                        Guid divisionId= Guid.Empty;
                        validation = CheckFieldValidation("Division", divisionName, "D" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var division = _materialServiceDependencies.DivisionService
                                .GetDivisionIdByName(divisionName);
                            if (division != null)
                            {
                                materialDto.DivisionId =division.Id;
                                materialDto.Division = division;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Division is not exists in database";
                                validation.FieldValue = divisionName;
                                validation.FieldName = "Division";
                                validation.FieldAddress = "D" + row;
                                validationList.Add(validation);
                            }
                        }
                        

                        //SubDivision
                        var subDivisionName = worksheet.Range[row, 5].Value.Trim();
                        validation = CheckFieldValidation("SubDivision", subDivisionName, "E" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var subDivision = _materialServiceDependencies.SubDivisionService
                                .GetSubDivisionIdByName(subDivisionName);
                            if (subDivision != null)
                            {
                                materialDto.SubDivisionId = subDivision.Id;
                                materialDto.SubDivision = subDivision;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "SubDivision is not exists in database";
                                validation.FieldValue = divisionName;
                                validation.FieldName = "SubDivision";
                                validation.FieldAddress = "E" + row;
                                validationList.Add(validation);
                            }
                        }

                        //Location of Operation
                        var locationName = worksheet.Range[row, 6].Value.Trim();
                        validation = CheckFieldValidation("Location", locationName, "F" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var locationOperation = _materialServiceDependencies.LocationOperationService.GetLocationIdByName(locationName, system);
                            if (locationOperation != null)
                            {
                                materialDto.LocationId = (Guid)locationOperation.Id;
                                materialDto.Location = locationOperation;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Location is not exists in database";
                                validation.FieldValue = locationName;
                                validation.FieldName = "Location";
                                validation.FieldAddress = "F" + row;
                                validationList.Add(validation);
                            }
                        }
                        //Region
                        var regionName = worksheet.Range[row, 7].Value.Trim();
                        validation = CheckFieldValidation("Region", regionName, "G" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var region = _materialServiceDependencies.RegionService.GetRegionIdByName(regionName);
                            if (region != null)
                            {
                                materialDto.RegionId = (Guid)region.Id;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Region is not exists in database";
                                validation.FieldValue = regionName;
                                validation.FieldName = "Region";
                                validation.FieldAddress = "G" + row;
                                validationList.Add(validation);
                            }
                        }
                        //Material Type
                        var materialTypeName = worksheet.Range[row, 8].Value.Trim();
                        validation = CheckFieldValidation("Material Type", materialTypeName, "H" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var materialType = _materialServiceDependencies.MaterialTypeService.GetMaterialTypeIdByName(materialTypeName);
                            if (materialType != null)
                            {
                                materialDto.TypeId = materialType.Id;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Material Type is not exists in database";
                                validation.FieldValue = materialTypeName;
                                validation.FieldName = "Material Type";
                                validation.FieldAddress = "H" + row;
                                validationList.Add(validation);
                            }
                        }
                        //Category
                        var categoryName = worksheet.Range[row, 9].Value.Trim();
                        validation = CheckFieldValidation("Category", categoryName, "I" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var category = _materialServiceDependencies.CategoryService.GetCategoryIdByName(categoryName);
                            if (category != null)
                            {
                                materialDto.CategoryId = (Guid)category.Id;
                                materialDto.Category=category;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Category is not exists in database";
                                validation.FieldValue = categoryName;
                                validation.FieldName = "Category";
                                validation.FieldAddress = "I" + row;
                                validationList.Add(validation);
                            }
                        }
                        //Supplier
                        var supplierName = worksheet.Range[row, 10].Value.Trim();
                        validation = CheckFieldValidation("Supplier", supplierName, "J" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var supplier = _materialServiceDependencies.SupplierService.GetSupplierIdByName(supplierName);
                            if (supplier != null)
                            {
                                materialDto.SupplierId = (Guid)supplier.Id;
                            }
                            else
                            {
                                validation = new BulkUploadValidationDto();
                                validation.ValidationMessage = "Supplier is not exists in database";
                                validation.FieldValue = supplierName;
                                validation.FieldName = "Supplier";
                                validation.FieldAddress = "J" + row;
                                validationList.Add(validation);
                            }
                        }
                        //Manufacturer
                        var manufacturerName = worksheet.Range[row, 11].Value.Trim();
                        validation = CheckFieldValidation("Manufacturer", manufacturerName, "K" + row);
                        if (!validation.IsValidationSucess)
                        {
                            validationList.Add(validation);
                        }
                        else
                        {
                            var manufacturer = _materialServiceDependencies.ManufacturerService.GetManufacturerIdByName(manufacturerName);
                            if (manufacturer != null)
                            {
                                materialDto.ManufacturerId = (Guid)manufacturer.Id;
                            }
                            else
                            {
                                validation=new BulkUploadValidationDto();
                                validation.ValidationMessage = "Manufacturer is not exists in database";
                                validation.FieldValue = manufacturerName;
                                validation.FieldName = "Manufacturer";
                                validation.FieldAddress = "K" + row;
                                validationList.Add(validation);
                            }
                        }
                        //Contractor
                        var contractorName = worksheet.Range[row, 12].Value.Trim();
                        var contractor = _materialServiceDependencies.ContractorService.GetContractorIdByName(contractorName);
                        if (contractor != null)
                        {
                            materialDto.ContractorId = (Guid)contractor.Id;
                        }
                        else
                        {
                            validation = new BulkUploadValidationDto();
                            validation.ValidationMessage = "Contractor Name is not exists in database";
                            validation.FieldValue = contractorName;
                            validation.FieldName = "contractorName";
                            validation.FieldAddress = "L" + row;
                            validationList.Add(validation);
                        }

                        //Material Name
                        var materialName = worksheet.Range[row, 13].Value.Trim(); //Free Text Fields
                        validation = CheckFieldValidation("Material Name", materialName, "M" + row);
                        if (validation.IsValidationSucess)
                        {
                            materialDto.MaterialName = materialName;
                        }
                        else
                        {
                            validationList.Add(validation);
                        }    
                        
                        //Material QTY
                        var materialQty = worksheet.Range[row, 14].Value.Trim();
                        validation = CheckFieldValidation("Material Quantity", materialQty, "N" + row);
                        if (validation.IsValidationSucess)
                        {
                            materialDto.MaterialQty = Convert.ToInt32(materialQty);                           
                        }
                        else
                        {
                            validationList.Add(validation);
                        }
                        //Measuremnt
                        var measurementName = worksheet.Range[row, 15].Value.Trim();
                        var measurement = _materialServiceDependencies.MaterialMeasurementService.GetMeasuremetByName(measurementName);
                        if (measurement != null)
                        {
                            materialDto.MeasurementId = (Guid)measurement.Id;
                        }
                        else
                        {
                            validation = new BulkUploadValidationDto();
                            validation.ValidationMessage = "Measurement is not exists in database";
                            validation.FieldValue = measurementName;
                            validation.FieldName = "Measurement";
                            validation.FieldAddress = "O" + row;
                            validationList.Add(validation);
                        }
                        
                        //Model Number
                        var modelNumber = worksheet.Range[row, 16].Value.Trim();
                        validation = CheckFieldValidation("Model Number", modelNumber, "P" + row);
                        if (validation.IsValidationSucess)
                        {
                            materialDto.ModelNumber = modelNumber;
                        }
                        else
                        {
                            validationList.Add(validation);
                        }
                       
                        //Tag Number
                        var tagNumber = worksheet.Range[row, 17].Value.Trim();
                        validation = CheckFieldValidation("Tag Number", tagNumber, "Q" + row);
                        if (validation.IsValidationSucess)
                        {
                            materialDto.TagNumber = tagNumber;
                        }
                        else
                        {
                            validationList.Add(validation);
                        }
                        

                        //Purchase Date
                        string purchaseDate = worksheet.Range[row, 18].Value.Trim();
                        validation =CheckDateValidation("Purchase Date", purchaseDate, "R"+row);
                        if (validation.IsValidationSucess)
                        {
                            if (!string.IsNullOrEmpty(purchaseDate))
                            {
                                materialDto.PurchaseDate = Convert.ToDateTime(purchaseDate);
                            }                           
                        }
                        else
                        {
                            validationList.Add (validation);
                        }

                        //Commissioning Date
                        var commissioningDate = worksheet.Range[row, 19].Value.Trim();
                        validation = CheckDateValidation("Commissioning Date", commissioningDate, "S"+row);
                        if (validation.IsValidationSucess)
                        {
                            if (!string.IsNullOrEmpty(commissioningDate))
                            {
                                materialDto.CommissioningDate = Convert.ToDateTime(commissioningDate);
                            }

                                
                        }
                        else
                        {
                            validationList.Add(validation);
                        }

                        //Supply Date
                        string dateOfSupply = worksheet.Range[row, 20].Value.Trim();
                        validation = CheckDateValidation("Date of Supply", dateOfSupply, "T" + row);
                        if (validation.IsValidationSucess)
                        {
                            if (!string.IsNullOrEmpty(dateOfSupply))
                            {
                                materialDto.DateOfSupply = Convert.ToDateTime(dateOfSupply);
                            }
                            
                        }
                        else
                        {
                            validationList.Add(validation);
                        }

                        //YearOfInstallation
                        string installationDate = worksheet.Range[row, 21].Value.Trim();
                        validation = CheckDateValidation("YearOfInstallation", installationDate, "U"+row);
                        if (validation.IsValidationSucess)
                        {
                            if (!string.IsNullOrEmpty(installationDate))
                            {
                                materialDto.YearOfInstallation = Convert.ToDateTime(installationDate);
                            }
                            
                        } 
                        else
                        {
                            validationList.Add(validation);
                        }

                        //DesignLife Date
                        var designLifeDate = worksheet.Range[row, 22].Value.Trim();
                        validation = CheckDateValidation("DesignLife Date", designLifeDate, "V"+row);
                        if (validation.IsValidationSucess)
                        {
                            if (!string.IsNullOrEmpty(designLifeDate))
                            {
                                materialDto.DesignLifeDate = Convert.ToDateTime(designLifeDate);
                            }
                            
                        }
                        else
                        {
                            validationList.Add(validation);
                        }

                        //EndPeriodLife Date
                        var endPeriodLifeDate = worksheet.Range[row, 23].Value.Trim();
                        validation = CheckDateValidation("EndPeriodLife Date", endPeriodLifeDate, "W"+row);
                        if (validation.IsValidationSucess)
                        {
                            if (!string.IsNullOrEmpty(endPeriodLifeDate))
                            {
                                materialDto.EndPeriodLifeDate = Convert.ToDateTime(endPeriodLifeDate);
                            }
                           
                        }
                        else
                        {
                            validationList.Add(validation);
                        }
                        
                        //Material Status
                        var materialStatus = worksheet.Range[row, 24].Value.Trim();
                        materialDto.MaterialStatus = materialStatus;

                        //Remoarks
                        var remarks = worksheet.Range[row, 25].Value.Trim();
                        materialDto.Remarks = remarks;

                        //Add Column Material Code 
                        if (validation.IsValidationSucess)
                        {
                            if (isRehabilitation)
                            {
                                materialDto.RehabilitationMaterialCode = "R-" + GenerateMaterialCode(materialDto);
                                materialDto.IsRehabilitation = true;
                                worksheet.Range["Z1"].Value = "Material Rehabilitation Code";
                            }
                            else
                            {
                                materialDto.MaterialCode = GenerateMaterialCode(materialDto);
                                materialDto.IsRehabilitation = false;
                                worksheet.Range["Z1"].Value = "Material Code";
                            }
                            
                            
                            
                            worksheet.Range["Z1"].Style.Font.IsBold = true;
                            worksheet.Range["Z1"].Style.KnownColor = ExcelColors.Yellow;
                            worksheet.Range["Z1"].ColumnWidth = 20;
                            
                            
                            worksheet.Range["Z"+row.ToString()].Value = materialDto.MaterialCode;
                           
                        }
                        
                        //Check Duplicate Record in Database
                        if (IsRecordExistsInDatabase(materialDto))
                        {
                            validation = new BulkUploadValidationDto()
                            {
                                ValidationMessage = "Record already exists in database with Material Name: "
                                + materialDto.MaterialName + " and Tag Number: " + materialDto.TagNumber,
                                IsValidationSucess=false,
                                FieldName = "Material Name, Tag Number",
                                FieldAddress="Row No:"+row.ToString(),
                            };
                            validationList.Add(validation);
                        }
                        else
                        {
                            materialListDto.Add(materialDto);
                        }
                        
                    }

                    //Check Validation is Success or Not and Save File         
                    if(validationList.Count > 0)
                    {
                        bulkUploadResponse.ResponseMessage = "Please check below validations, correct it and upload file again";
                        bulkUploadResponse.IsSuccess = false;
                        bulkUploadResponse.Validations = validationList;
                    }
                    else
                    {
                        GenerateAndSaveBulkUpladFile(bulkUploadResponse, workbook, materialListDto);
                    }
                }
                
            }
            catch (Exception ex) 
            {
                bulkUploadResponse.ResponseMessage=ex.Message;
                bulkUploadResponse.IsSuccess=false;
                bulkUploadResponse.Validations = new List<BulkUploadValidationDto>();
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return bulkUploadResponse;
        }

        private async void GenerateAndSaveBulkUpladFile(BulkUploadResponseDto bulkUploadResponse, Workbook workbook, List<MaterialDto> materialListDto)
        {
            try
            {
                bulkUploadResponse.ResponseMessage = "Records Uploaded Successfully,Please download the file to confirm.";
                bulkUploadResponse.IsSuccess = true;
                bulkUploadResponse.Materials = materialListDto;
                bulkUploadResponse.Validations = new List<BulkUploadValidationDto>();

                newFileName = Guid.NewGuid().ToString() + "_" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString()
                       + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + ".xlsx";

                // Save to an Excel file
                string uploadedFilePath = folderPath + newFileName;
                workbook.SaveToFile(uploadedFilePath, ExcelVersion.Version2016);
                bulkUploadResponse.UploadedFile = newFileName;
                // Dispose resources
                workbook.Dispose();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            
        }

        private bool IsRecordExistsInDatabase(MaterialDto materialDto)
        {
            bool isRecordExistsInDatabase = false;
            try
            {
                var existingMaterialDto = _bulkUploadRepository.Query()
                    .Where(x => x.TagNumber == materialDto.TagNumber 
                    && x.MaterialName==materialDto.MaterialName).FirstOrDefault();
                if (existingMaterialDto != null)
                {
                    isRecordExistsInDatabase=true;
                }
            }
            catch(Exception ex)
            {

            }
            return isRecordExistsInDatabase;
        }

        private BulkUploadValidationDto? CheckFieldValidation(string fieldName, string fieldValue, string cellNo)
        {
            BulkUploadValidationDto validation = new BulkUploadValidationDto();
            if (string.IsNullOrEmpty(fieldValue))
            {
                validation.FieldName = fieldName;
                validation.FieldValue = fieldValue;
                validation.FieldAddress = cellNo;
                validation.ValidationMessage = fieldName + " is Required";
                validation.IsValidationSucess = false;
                
            }
            else
            {
                validation.IsValidationSucess=true;
            }
        return validation;
        }           

        private BulkUploadValidationDto CheckDateValidation(string fieldName, string fieldValue, string cellNo)
        {
            BulkUploadValidationDto validation = new BulkUploadValidationDto();
            validation.IsValidationSucess = true;
            DateTime date;
            try
            {
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    //validation.FieldName = fieldName;
                    //validation.FieldValue=fieldValue;
                    //validation.FieldAddress= cellNo;
                    //validation.ValidationMessage = fieldName + "is Required";
                    //validation.IsValidationSucess = false;

                    date = Convert.ToDateTime(fieldValue);
                    validation.IsValidationSucess = true;
                    validation.FieldValue = Convert.ToString(date);
                }
                
                
                

            }
            catch(Exception ex)
            {
                validation.FieldName = fieldName;
                validation.FieldValue = fieldValue;
                validation.FieldAddress = cellNo;
                validation.ValidationMessage = ex.Message;
                validation.IsValidationSucess = false;
            }
            return validation;
        }
        
        private  List<MaterialDto> AddMaterials(List<MaterialDto> materials, Guid fileBatchId)
        {
            List<MaterialDto> materialDtoList=new List<MaterialDto>();
            try
            {
                foreach (MaterialDto materialDto in materials)
                {
                    var material = _mapper.Map<Material>(materialDto);
                    
                    
                    material.CreatedDate= DateTime.Now;
                    material.CreatedBy = userName;
                    material.IsDeleted = false;                   
                    material.BuilkUploadDetailId = fileBatchId;
                    var addedMaterial = _bulkUploadRepository.Add(material);
                    if (addedMaterial != null)
                    {
                        materialDtoList.Add(_mapper.Map<MaterialDto>(addedMaterial));
                    }
                    
                }

            }
            catch(Exception ex)
            {

            }
            return materialDtoList;
        }
        private BulkUploadDetailsDto AddBulkUploadDetails(int uploadedCount)
        {
            var bulkUploadDetailDto = new BulkUploadDetailsDto()
            {
                FileName = newFileName,
                FilePath = folderPath,
                NoOfRecords = uploadedCount,
                CreatedBy = userName,
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                 TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
            };
            return _bulkUploadDetailService.AddBulkUploadDetail(bulkUploadDetailDto);           
        }
        
        public string GenerateMaterialCode(MaterialDto materialDto)
        {
            // Get current date and time
            DateTime now = DateTime.Now;
            string minutes = now.Minute.ToString("D2");
            string hours = now.Hour.ToString("D2");

            // Get system (first 2 characters)
            string systemPrefix = string.IsNullOrEmpty(materialDto.System) ? "WA" : materialDto.System.Substring(0, 2).ToUpper();

            // Get location (division, subdivision, or location)
            string locationPart = GetLocationPart(materialDto);

            // Get category (3 letters)
            string categoryPart = "CAT";
            if (!string.IsNullOrEmpty(materialDto.Category.CategoryName))
            {
                categoryPart = GetFirstThreeLetters(materialDto.Category.CategoryName);
            }

            // Combine all parts
            return $"{systemPrefix}-{locationPart}-{categoryPart}-{hours}{minutes}".ToUpper();
        }

        private string GetLocationPart(MaterialDto materialDto)
        {
            string locationPart = string.Empty;

            // Use division
            if (materialDto.Division!=null)
            {
                locationPart = GetFirstThreeLetters(materialDto.Division.DivisionName);
            }
            // Use subdivision
            else if (materialDto.SubDivision!=null)
            {
                locationPart = GetFirstThreeLetters(materialDto.SubDivision.SubDivisionName);
            }
            // Use location
            else if (materialDto.Location != null)
            {
                locationPart = GetFirstThreeLetters(materialDto.Location.LocationOperationName);
            }

            // If no location part was set, use a default
            if (string.IsNullOrEmpty(locationPart))
            {
                locationPart = "LOC";
            }

            return locationPart;
        }

        // Helper method to get first three letters from a string
        private string GetFirstThreeLetters(string str)
        {
            // Return "XXX" if str is null or empty
            if (string.IsNullOrEmpty(str)) return "XXX";

            // Remove spaces and special characters
            string cleanStr = new string(str.Where(char.IsLetterOrDigit).ToArray());

            // Get first three letters, pad with 'X' if needed
            return (cleanStr.Substring(0, Math.Min(3, cleanStr.Length)) + "XXX").Substring(0, 3);
        }
    }
}
