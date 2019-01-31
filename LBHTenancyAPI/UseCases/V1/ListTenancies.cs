﻿using System;
using System.Collections.Generic;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Gateways.V1;

namespace LBHTenancyAPI.UseCases.V1
{
    public class ListTenancies : IListTenancies
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public ListTenancies(ITenanciesGateway tenanciesGateway)
        {
            this.tenanciesGateway = tenanciesGateway;
        }

        public Response Execute(List<string> tenancyRefs)
        {
            var response = new Response();
            var tenancies = tenanciesGateway.GetTenanciesByRefs(tenancyRefs);

            response.Tenancies = tenancies.ConvertAll(tenancy => new ResponseTenancy
                {
                    TenancyRef = tenancy.TenancyRef,
                    PropertyRef = tenancy.PropertyRef,
                    PaymentRef = tenancy.PaymentRef,
                    StartDate = FormatDate(tenancy.StartDate),
                    Tenure = tenancy.Tenure,
                    LastActionCode = tenancy.LastActionCode,
                    LastActionDate = FormatDate(tenancy.LastActionDate),
                    CurrentBalance = new Currency(tenancy.CurrentBalance),
                    ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                    PrimaryContactName = tenancy.PrimaryContactName,
                    PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                    PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                }
            );

            return response;
        }

        private static string FormatDate(DateTime? date)
        {
            return date.HasValue ? string.Format("{0:u}", date) : null;
        }

        public struct Response
        {
            public List<ResponseTenancy> Tenancies { get; set; }
        }

        public struct ResponseTenancy
        {
            public string TenancyRef { get; set; }
            public string PropertyRef { get; set; }
            public string Tenure { get; set; }
            public string LastActionCode { get; set; }
            public string LastActionDate { get; set; }
            public Currency CurrentBalance { get; set; }
            public string ArrearsAgreementStatus { get; set; }
            public string PrimaryContactName { get; set; }
            public string PrimaryContactShortAddress { get; set; }
            public string PrimaryContactPostcode { get; set; }
            public string PaymentRef { get; set; }
            public string StartDate { get; set; }
        }
    }
}
