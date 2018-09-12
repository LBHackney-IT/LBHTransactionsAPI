using System;
using System.Collections.Generic;
using AgreementService;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class APIError
    {
        public bool IsValid { get; set; }
        public IList<ExecutionError> Errors { get; set; }
        public IList<ExecutionError> ValidationErrors { get; set; }

        public APIError(RequestValidationResponse validationResponse)
        {
            if (validationResponse == null)
                IsValid = false;
            else
            {
                IsValid = validationResponse.IsValid;
                ValidationErrors = validationResponse.ValidationErrors;
            }
        }

        public APIError(Exception ex)
        {
            Errors = new List<ExecutionError> { new ExecutionError(ex) };
        }

        public APIError(WebResponse response)
        {
            Errors = new List<ExecutionError>
            {
                new ExecutionError(response)
            };
        }
    }
}