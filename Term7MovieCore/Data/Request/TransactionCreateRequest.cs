﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.ValidationAttributes;

namespace Term7MovieCore.Data.Request
{
    public class TransactionCreateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public long ShowtimeId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public int TheaterId { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [UniqueLongIdList(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_INVALID_FIELD)]
        public IEnumerable<long> IdList { set; get; }
    }
}
