using AutoMapper;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class MaterialServiceInfrastructure
    {
        public IGenericRepository<Material> GenericRepository { get; }
        public IMapper Mapper { get; }
        public ILogger<MaterialService> Logger { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public MaterialServiceInfrastructure(
            IGenericRepository<Material> genericRepository,
            IMapper mapper,
            ILogger<MaterialService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            GenericRepository = genericRepository;
            Mapper = mapper;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
        }

    }
}
