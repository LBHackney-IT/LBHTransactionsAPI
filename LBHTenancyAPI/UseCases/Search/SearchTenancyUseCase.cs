using System;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways.Search;
using LBHTenancyAPI.Infrastructure.Exceptions;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using LBHTenancyAPI.UseCases.Search.Models;
using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.Search
{
    public class SearchTenancyUseCase : ISearchTenancyUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public SearchTenancyUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        public async Task<SearchTenancyResponse> ExecuteAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            //validate
            if (request == null)
                throw new BadRequestException();

            var validationResponse = request.Validate(request);
            if (!validationResponse.IsValid)
                throw new BadRequestException(validationResponse);

            var response = await _searchGateway.SearchTenanciesAsync(request, cancellationToken).ConfigureAwait(false);

            //tenancy could have no attached contacts
            if (response == null)
                return new SearchTenancyResponse();

            //Create real response
            var useCaseResponse = new SearchTenancyResponse
            {
                Tenancies = response.ConvertAll(tenancy => new TenancySummary
                {
                    TenancyRef = tenancy.TenancyRef,
                    PropertyRef = tenancy.PropertyRef,
                    Tenure = tenancy.Tenure,
                    LatestTenancyAction = new LatestTenancyAction
                    {
                        LastActionCode = tenancy.LastActionCode,
                        LastActionDate = String.Format("{0:u}", tenancy.LastActionDate)
                    },
                    CurrentBalance = tenancy.CurrentBalance.ToString("C"),
                    ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                    PrimaryContact = new PrimaryContact
                    {
                        Name = tenancy.PrimaryContactName,
                        ShortAddress = tenancy.PrimaryContactShortAddress,
                        Postcode = tenancy.PrimaryContactPostcode
                    }
                })
            };

            return useCaseResponse;
        }
    }
}