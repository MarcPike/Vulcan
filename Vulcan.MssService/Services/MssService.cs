using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Vulcan.MssService.Protos;

namespace Vulcan.MssService
{
    public class MssService : MSSService.MSSServiceBase
    {
        private readonly ILogger<MssService> _logger;

        public MssService(ILogger<MssService> logger)
        {
            _logger = logger;
        }

        public override Task<InventoryVerificationResponse> inventoryVerification(InventoryVerificationRequest request, ServerCallContext context)
        {
            return base.inventoryVerification(request, context);
        }
    }
}