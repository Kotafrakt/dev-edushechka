﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        private const string _paymentAddProcedure = "dbo.Payment_Insert";
        private const string _paymentDeleteProcedure = "dbo.Payment_Delete";
        private const string _paymentSelectByIdProcedure = "dbo.Payment_SelectById";
        private const string _paymentAllByUserIdProcedure = "dbo.Payment_SelectAllByUserId";
        private const string _paymentUpdateProcedure = "dbo.Payment_Update";
        private const string _addPaymentsProcedure = "[dbo].[Insert_Payments]";
        private const string _paymentType = "[dbo].[PaymentType]";

        public PaymentRepository() { }

        public int AddPayment(PaymentDto paymentDto)
        {
            return _connection.QuerySingle<int>(
                _paymentAddProcedure,
                new
                {
                    userId = paymentDto.User.Id,
                    paymentDto.Date,
                    paymentDto.Sum,
                    paymentDto.IsPaid
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeletePayment(int id)
        {
            _connection.Execute(
                _paymentDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public PaymentDto GetPayment(int id)
        {
            PaymentDto result = default;
            return _connection
                .Query<PaymentDto, UserDto, PaymentDto>(
                _paymentSelectByIdProcedure,
                (payment, user) =>
                {
                    if (result == null)
                    {
                        result = payment;
                        result.User = user;
                    }
                    return result;
                },
                new { id },
                splitOn: "Id",
                    commandType: CommandType.StoredProcedure
            )
            .FirstOrDefault();
        }

        public List<PaymentDto> GetPaymentsByUser(int userId)
        {
            var paymentDictionary = new Dictionary<int, PaymentDto>();

            return _connection
                .Query<PaymentDto, UserDto, PaymentDto>(
                    _paymentAllByUserIdProcedure,
                    (payment, user) =>
                    {
                        PaymentDto result;

                        if (!paymentDictionary.TryGetValue(payment.Id, out result))
                        {
                            result = payment;
                            result.User = user;
                            paymentDictionary.Add(payment.Id, result);
                        }

                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public void UpdatePayment(PaymentDto paymentDto)
        {
            _connection.Execute(
                _paymentUpdateProcedure,
                new
                {
                    paymentDto.Date,
                    paymentDto.Sum,
                    paymentDto.IsPaid
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public void AddPayments(List<PaymentDto> payments)
        {
            var table = new DataTable();
            table.Columns.Add("Date");
            table.Columns.Add("Sum");
            table.Columns.Add("UserId");
            table.Columns.Add("IsPaid");
            table.Columns.Add("IsDeleted");

            foreach (var bill in payments)
            {
                table.Rows.Add(bill.Date, bill.Sum, bill.User.Id, bill.IsPaid, bill.IsDeleted);
            }
            _connection.Execute(
                _addPaymentsProcedure,
                new { tblPayment = table.AsTableValuedParameter(_paymentType) },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}