using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IMaterialBulkUploadService
    {
        Task<bool> GenrateMaterialUploadTemplate(string path);
        BulkUploadResponseDto BulkUpload(Stream fileStream, string filePath,bool isRehabilitation);
    }
}
