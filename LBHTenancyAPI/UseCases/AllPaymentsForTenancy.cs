﻿using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class AllPaymentsForTenancy : IListAllPayments
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public AllPaymentsForTenancy(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public PaymentTransactionResponse Execute(string tenancyRef)
        {
            var response = new PaymentTransactionResponse();
            var paymentTransaction = tenanciesGateway.GetPaymentTransactionsByTenancyRef(tenancyRef);

            response.PaymentTransactions = paymentTransaction.ConvertAll(paymentTrans => new PaymentTransaction()
                {
                    Ref= paymentTrans.TransactionsRef,
                    Amount= paymentTrans.TransactionAmount.ToString("C"),
                    Date = string.Format("{0:u}", paymentTrans.TransactionDate),
                    Type = paymentTrans.TransactionType,
                    PropertyRef = paymentTrans.PropertyRef
                }
            );

            return response;
        }

        public struct PaymentTransactionResponse
        {
            public List<PaymentTransaction> PaymentTransactions { get; set; }
        }

        public struct PaymentTransaction
        {
            public string Ref { get; set; }
            public string Date { get; set; }
            public string PropertyRef{ get; set; }
            public string Type { get; set; }
            public string Amount { get; set; }
        }
    }
}