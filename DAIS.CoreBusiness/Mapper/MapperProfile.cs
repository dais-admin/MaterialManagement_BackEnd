using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Dtos.Reports;
using DAIS.DataAccess.Entities;

namespace DAIS.CoreBusiness.Mapper
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<ProjectDto,Project>().ReverseMap();
            CreateMap<WorkPackageDto,WorkPackage>().ReverseMap();
            CreateMap<MaterialTypeDto, MaterialType>().ReverseMap();            
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<RegionDto, Region>().ReverseMap();
            CreateMap<LocationOperationDto, LocationOperation>().ReverseMap();
            CreateMap<DivisionDto, Division>().ReverseMap();
            CreateMap<SupplierDto, Supplier>().ReverseMap();
            CreateMap<DocumentMasterDto,DocumentType>().ReverseMap();
            CreateMap<MaterialDocumentDto,MaterialDocument>().ReverseMap();
            CreateMap<MaterialWarrantyDto, MaterialWarranty>().ReverseMap();
            CreateMap<MaterialHardwareDto, MaterialHardware>().ReverseMap();
            CreateMap<MaterialSoftwareDto, MaterialSoftware>().ReverseMap();
            CreateMap<MaterialApprovalDto,MaterialApproval>().ReverseMap();
            CreateMap<MaterialMeasuremetDto,MaterialMeasurement>().ReverseMap(); 
            CreateMap<ContractorDto, Contractor>().ReverseMap();
            CreateMap<AgencyDto, Agency>().ReverseMap();
            CreateMap<SubDivisionDto, SubDivision>().ReverseMap();
            CreateMap<BulkUploadDetailsDto,BulkUploadDetail>().ReverseMap();
            CreateMap<MaterialMaintenaceDto, MaterialMaintenance>().ReverseMap();

            CreateMap<Material, MaterialDto>().ReverseMap();
            CreateMap<MaterialDto, Material>()
                .ForMember(dest => dest.Manufacturer, option => option.Ignore())
                .ForMember(dest => dest.MaterialType, option => option.Ignore())
                .ForMember(dest => dest.Category, option => option.Ignore())
                .ForMember(dest => dest.Region, option => option.Ignore())
                .ForMember(dest => dest.SubDivision, option => option.Ignore())
                .ForMember(dest => dest.Location, option => option.Ignore())
                .ForMember(dest => dest.Supplier, option => option.Ignore());

            CreateMap<ManufacturerDto, Manufacturer>().ReverseMap();
            CreateMap<MaterialServiceProviderDto, MaterialServiceProvider>().ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id)).ReverseMap();

            CreateMap<MaterialIssueRecieveVoucher, MaterialIssueReceiveDto>()
                .ForMember(dest => dest.VoucherNo, opt => opt.MapFrom(src => src.VoucherNo))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.VoucherDate))
                .ForMember(dest => dest.Remarks, opt => opt.Ignore());

            CreateMap<MaterialIssueReceiveDto, MaterialIssueRecieveVoucher>()
                .ForMember(dest => dest.VoucherNo, opt => opt.MapFrom(src => src.VoucherNo))
                .ForMember(dest => dest.VoucherDate, opt => opt.MapFrom(src => src.Date));

        }

    }
}
