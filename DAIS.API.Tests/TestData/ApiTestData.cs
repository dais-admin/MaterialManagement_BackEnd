using DAIS.CoreBusiness.Dtos;

namespace DAIS.API.Tests.TestData
{
    public static class ApiTestData
    {
        public static List<UserDto> GetUserDtoData()
        {
            return new List<UserDto>() {
                new UserDto()
                {
                    Id=Guid.NewGuid(),
                    UserName="Admin",
                    IsSuccess=true,
                    UserToken="xxxxxyyyxxxy5677yy6yy57t",
                    Message="Login Success",
                    UserType="Admin"
                },
                new UserDto()
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin",
                    IsSuccess = true,
                    UserToken = "",
                    Message = "Register Success",
                    UserType = "Admin"
                }
            };
        }
        public static LoginDto GetLoginDtoData()
        {
            return new LoginDto()
            {
                UserEmail = "admin@test.com",
                Password = "password",
            };
        }

        public static RegistrationDto GetRegistrationDtoData()
        {
            return new RegistrationDto()
            {
                UserName = "Admin",
                Password = "password",
                UserType = "Admin"
            };
        }

        public static List<MaterialDto> GetMaterialDtoData()
        {
            return new List<MaterialDto>() {
                new MaterialDto()
                {
                    Id=Guid.NewGuid(),
                    MaterialName="Pump",
                    MaterialCode="123_ABC_#1",
                    MaterialQty=1,
                    MaterialStatus="Purchesed",
                    System="Water",
                    TagNumber="#1232",
                    ModelNumber="A121",
                    LocationRFId="AA11AD",
                    PurchaseDate=DateTime.UtcNow,
                    YearOfInstallation=DateTime.UtcNow,
                    DesignLifeDate=DateTime.UtcNow,
                    EndPeriodLifeDate=DateTime.UtcNow,
                    TypeId=Guid.NewGuid(),
                    CategoryId=Guid.NewGuid(),
                    RegionId=Guid.NewGuid(),
                    LocationId=Guid.NewGuid(),
                    //DivisionId=Guid.NewGuid(),
                    SupplierId=Guid.NewGuid(),
                    ManufacturerId=Guid.NewGuid()
                },
                new MaterialDto()
                {

                }
            };
            
        }
        public static List<CategoryDto> GetCategoryDtoData()
        {
            return new List<CategoryDto>()
            {
                new CategoryDto()
                {
                    Id=Guid.NewGuid(),
                    CategoryName="ABC",
                    CategoryCode="123",
                    MaterialTypeName="pump"

                }

            };
        }
        public static List<RegionDto> GetRegionDtoData()
        {
            return new List<RegionDto>()
            {
                new RegionDto()
                {
                    Id=Guid.NewGuid(),
                    RegionName="PQR",
                    RegionCode="234"

                }
            };
        }
        public static List<DivisionDto>GetDivisionDtoData()
        {
            return new List<DivisionDto>()
            { 
                new DivisionDto()
                {
                    Id=Guid.NewGuid(),
                    DivisionName="XYZ",
                    DivisionCode="123"


                }

            };
        }
        public static List<LocationOperationDto> GetLocationOperationDtoData()
        {
            return new List<LocationOperationDto>()
            {
                 new LocationOperationDto()
                 {
                     Id=Guid.NewGuid(),  
                     LocationOperationName="ABC",
                     LocationOperationCode="123"

                 }
            };
        }
        public static List<ManufacturerDto> GetManufacturerDtoData()
        {
            return new List<ManufacturerDto>()
            {
                new ManufacturerDto()
                {
                    Id =Guid.NewGuid(),
                    ManufacturerName="PQR",
                    ManufacturerAddress="123",
                    ProductsDetails="ABC",
                    ImportantDetails="Good "
                    
                }
            };
        }
        public static List<SupplierDto> GetSupplierDtoData()
        {
            return new List<SupplierDto>()
            {
                new SupplierDto()
                {
                    Id=Guid.NewGuid(),
                    SupplierName="AVP",
                    SupplierAddress="VRW",
                    ProductsDetails="Microsoft",
                    Remarks="Good"

                }
            };
        }
        public static List<MaterialServiceProviderDto> GetMaterialServiceProviderDtoData()
        {
            return new List<MaterialServiceProviderDto>()
            {
                new MaterialServiceProviderDto()
                {
                    Id=Guid.NewGuid(),
                    ServiceProviderName="MNO",
                    Address="ASD",
                    //ContactNo="789",                    
                    ManufacturerId=Guid.NewGuid(),
                }
            };

        }
        public static List<DocumentMasterDto> GetDocumentMasterDtoData()
        {
            return new List<DocumentMasterDto>()
            {
                 new DocumentMasterDto()
                 {
                     Id=Guid.NewGuid(),
                     DocumentName="AKR"
                 }
            };
        }
        public static List<MaterialDocumentDto> GetMaterialDocumentDtoData()
        {
            return new List<MaterialDocumentDto>()
            { 
                new  MaterialDocumentDto()
                {
                        Id=Guid.NewGuid(),
                        DocumentName="ABC",
                        DocumentFileName="PQR",
                        DocumentFilePath="XYZ",
                        DocumentTypeId=Guid.NewGuid(),
                        MaterialId=Guid.NewGuid(),
                } 
            };
        }
        public static List<MaterialHardwareDto> GetMaterialHardwareDtoData()
        {
            return new List<MaterialHardwareDto>()
            {  
                new MaterialHardwareDto()
                {
                    Id=Guid.NewGuid(),
                    SupplierId=Guid.NewGuid(),
                    SerialNo=1,
                    Chipset="ABC",
                    DateOfManufacturer=DateTime.UtcNow,
                    NetworkDetails="XYZ",
                    DiskDetails="PQR",
                    BiosDetails="GHJ"

                }
            };
        }
        public static List<MaterialWarrantyDto> GetMaterialWarrantyDtoData()
        {
            return new List<MaterialWarrantyDto>
            {
                    new MaterialWarrantyDto()
                    {
                        Id=Guid.NewGuid(),
                        WarrantyStartDate = DateTime.UtcNow,
                        WarrantyEndDate=DateTime.UtcNow,
                        NoOfMonths=1,
                        ManufacturerId=Guid.NewGuid(),

                    }
            };

        }
        public static List<MaterialTypeDto> GetMaterialTypeDtoData()
        {

            return new List<MaterialTypeDto>()
            {
                new MaterialTypeDto()
                {
                    Id=Guid.NewGuid(),
                    TypeName="type",
                    TypeCode="123"

                }

            };
        }

    }
}
