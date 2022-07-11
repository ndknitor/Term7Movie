using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class TheaterUpdateRequest
    {
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int Id { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [MaxLength(100, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        public string Name { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        [MaxLength(200, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_MAX_LENGTH)]
        public string Address { set; get; }
        [Required(ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_REQUIRED)]
        public bool Status { set; get; }
    }
}
