using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Gateways.Arrears;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class CreateArrearsAgreementUseCase : ICreateArrearsAgreementUseCase
    {
        private readonly IArrearsAgreementGateway _arrearsAgreementGateway;

        public CreateArrearsAgreementUseCase(IArrearsAgreementGateway arrearsAgreementGateway)
        {
            _arrearsAgreementGateway = arrearsAgreementGateway;
        }

        public async Task<IExecuteWrapper<CreateArrearsAgreementResponse>> ExecuteAsync(CreateArrearsAgreementRequest request, CancellationToken cancellationToken)
        {
            //validate
            if(request == null || !request.Validate().IsValid)
                return new ExecuteWrapper<CreateArrearsAgreementResponse>(request?.Validate());
            //execute business logic
            var webServiceRequest = new ArrearsAgreementRequest
            {
                Agreement = request?.AgreementInfo,
                PaymentSchedule = request?.PaymentSchedule?.ToArray()
            };
            var response = await _arrearsAgreementGateway.CreateArrearsAgreementAsync(webServiceRequest,cancellationToken).ConfigureAwait(false);
            //marshall response

            return response as IExecuteWrapper<CreateArrearsAgreementResponse>;
        }
    }
}