using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Grpc.Net.Client;
using Vulcan.MssService.Protos;

namespace Vulcan.MssService.Client
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: TestMssInventoryVerification [HostName] [Port] [User Id] [TODO-Other params!]");
                return;
            }

            var hostName = args[0];
            var serverPort = args[1];
            var userId = Convert.ToInt32(args[2]);

            var stockItemId = ConsoleUtil.GetIntegerValue("StockItemId", 0);
            int productId = 0;
            if (stockItemId == 0)
            {
                productId = ConsoleUtil.GetIntegerValue("ProductId", 0);
            }

            var partNumberId = ConsoleUtil.GetIntegerValue("PartNumberId", 0);

            var group = ConsoleUtil.GetStringValue("Group");
            var sizeFrom = ConsoleUtil.GetStringValue("Size From");
            var sizeTo = ConsoleUtil.GetStringValue("Size To");
            var grade = ConsoleUtil.GetStringValue("Grade");
            var millId = ConsoleUtil.GetIntegerValue("MillId", 0);
            var castNumber = ConsoleUtil.GetStringValue("CastNumber");
            var incReserved = ConsoleUtil.GetBoolValue("Include Reserved"); 
            var lengthMin = ConsoleUtil.GetStringValue("Length Min");
            var lengthMax = ConsoleUtil.GetStringValue("Length Max");
            var weightMin = ConsoleUtil.GetStringValue("Weight Min");
            var weightMax = ConsoleUtil.GetStringValue("Weight Max");

            var salesItemId = ConsoleUtil.GetIntegerValue("Sales Item Id", 0);
            var baseSpecId = ConsoleUtil.GetIntegerValue("Base Spec Id", 0);
            var partSpecId = ConsoleUtil.GetIntegerValue("Part Spec Id", 0);
            var addSpec1Id = ConsoleUtil.GetIntegerValue("Additional Spec Id 1", 0);
            var addSpec2Id = ConsoleUtil.GetIntegerValue("Additional Spec Id 2", 0);
            var addSpec3Id = ConsoleUtil.GetIntegerValue("Additional Spec Id 3", 0);
            var addSpec4Id = ConsoleUtil.GetIntegerValue("Additional Spec Id 4", 0);

            var inventoryVerificationSelectionParameters = new InventoryVerificationSelectionParameters()
            {
                StockItemId = stockItemId,
                PartNumberId = partNumberId,
                ProductId = productId,
                Group = group,
                Grade = grade,
                SizeFrom = sizeFrom,
                SizeTo = sizeTo,
                MillId = millId,
                CastNumber = castNumber,
                IncludeReserved = incReserved,
                LengthMinimum = lengthMin,
                LengthMaximum = lengthMax
            };

            var inventoryVerificationSpecificationParameters = new InventoryVerificationSpecificationParameters()
            {
                SalesItemId = salesItemId,
                BaseSpecificationId = baseSpecId,
                PartSpecificationId = partSpecId,
                AdditionalSpecification1Id = addSpec1Id,
                AdditionalSpecification2Id = addSpec2Id,
                AdditionalSpecification3Id = addSpec3Id,
                AdditionalSpecification4Id = addSpec4Id
            };
            
            
            var resultFilter = new InventoryVerificationResultFilterParameters();

            var stockStatusInput = ConsoleUtil.GetIntegerValue("Stock Status (0 = Both, 1 = Pass, 2 = Fail)", 0);
            if ((stockStatusInput == 0) || (stockStatusInput > 2))
            {
                resultFilter.StockTestStatus =
                    InventoryVerificationResultFilterParameters.Types.StockTestStatus.TestBoth;
            }
            else if (stockStatusInput == 1)
            {
                resultFilter.StockTestStatus =
                    InventoryVerificationResultFilterParameters.Types.StockTestStatus.TestPass;
            }
            else if (stockStatusInput == 2)
            {
                resultFilter.StockTestStatus =
                    InventoryVerificationResultFilterParameters.Types.StockTestStatus.TestFail;
            }

            var testStatusInput = ConsoleUtil.GetIntegerValue("Result Status (0 = Both, 1 = Fail)", 0);
            if ((testStatusInput == 0) || (testStatusInput > 1))
            {
                resultFilter.TestResultStatus =
                    InventoryVerificationResultFilterParameters.Types.TestResultStatus.ResultBoth;
            }
            else
            {
                resultFilter.TestResultStatus =
                    InventoryVerificationResultFilterParameters.Types.TestResultStatus.ResultFail;

            }

            var inventoryVerificationInput =
                ConsoleUtil.GetIntegerValue(
                    "Inventory Verification Type (0 = AN, 1 = AN/HT, 2 = AN/HT/TE, 3 = AN/TE, 4 = HT/TE)", 2);
            if (inventoryVerificationInput == 0)
            {
                resultFilter.InventoryVerification =
                    InventoryVerificationResultFilterParameters.Types.InventoryVerification.An;
            }
            else if (inventoryVerificationInput == 1)
            {
                resultFilter.InventoryVerification =
                    InventoryVerificationResultFilterParameters.Types.InventoryVerification.AnHt;
            }
            else if ((inventoryVerificationInput == 2) || (inventoryVerificationInput > 4))
            {
                resultFilter.InventoryVerification =
                    InventoryVerificationResultFilterParameters.Types.InventoryVerification.AnHtTe;
            }
            else if (inventoryVerificationInput == 3)
            {
                resultFilter.InventoryVerification =
                    InventoryVerificationResultFilterParameters.Types.InventoryVerification.AnTe;
            }
            else if (inventoryVerificationInput == 4)
            {
                resultFilter.InventoryVerification =
                    InventoryVerificationResultFilterParameters.Types.InventoryVerification.HtTe;
            }



            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;


            using var grpcChannel = GrpcChannel.ForAddress($"http://{hostName}:{serverPort}", new GrpcChannelOptions {HttpHandler = httpHandler});
            var grpcClient = new MSSService.MSSServiceClient(grpcChannel);

            var request = new Request()
            {
                UserId = userId,
                Version = 0
            };

            var verificationRequest =
                new InventoryVerificationRequest()
                {
                    Request = request,
                    Selections = inventoryVerificationSelectionParameters,
                    ServiceConnectionId = 1,
                    Specifications = inventoryVerificationSpecificationParameters,
                    ResultFilters = resultFilter
                };
            var cancelToken = new CancellationToken();
            try
            {
                var reply = 
                    await grpcClient.inventoryVerificationAsync(verificationRequest, null, null, cancelToken);
                Console.WriteLine(reply.Status);
                foreach (var inventoryVerificationStockLine in reply.StockLine)
                {
                    Console.WriteLine(inventoryVerificationStockLine.StockNumber);
                }
                Console.WriteLine(reply.Status);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine($"  --> {e.InnerException.Message}");
                }

            }
        }
    }
}
