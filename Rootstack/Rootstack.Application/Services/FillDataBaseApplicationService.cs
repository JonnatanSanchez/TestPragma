using AutoMapper;
using GoogleScraping.Domain.ServiceAgent;
using GoogleScraping.Models;

namespace GoogleScraping.Application.Services
{
    public class FillDataBaseApplicationService : IFillDataBaseApplicationService
    {
        private readonly IFillDataBaseDomainService fillDataBaseDomainService;
        private readonly IMapper mapper;

        public FillDataBaseApplicationService(IFillDataBaseDomainService fillDataBaseDomainService, IMapper mapper)
        {
            this.fillDataBaseDomainService = fillDataBaseDomainService;
            this.mapper = mapper;
        }

        public ResponseViewModel FillDataBase()
        {
            return fillDataBaseDomainService.FillDataBase();
        }
    }
}
