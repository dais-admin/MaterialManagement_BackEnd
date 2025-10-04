﻿using DAIS.CoreBusiness.Dtos;

namespace DAIS.CoreBusiness.Interfaces
{
    public interface IDesignDocumentService
    {
        Task<DesignDocumentDto> AddDesignDocumentAsync(DesignDocumentDto designDocumentDto);
        Task<DesignDocumentDto> UpdateDesignDocumentAsync(DesignDocumentDto designDocumentDto);
        Task DeleteDesignDocumentAsync(Guid Id);
        Task<DesignDocumentDto> GetDesignDocumentByIdAsync(Guid Id);
        Task<List<DesignDocumentDto>> GetAllDesignlDocumentAsync();

        DesignDocumentDto GetDesignDocumentIdByName(string name);
    }
}
