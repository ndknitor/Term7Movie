﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITransactionService
    {
        Task<TransactionCreateResponse> CreateTransactionAsync(TransactionCreateRequest request, UserDTO user);
        Task ProcessPaymentAsync(MomoIPNRequest ipn);
    }
}
