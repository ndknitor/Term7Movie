﻿using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class ShowtimeFilterRequest : ParentFilterRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int? TheaterId { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = Constants.CONSTRAINT_REQUEST_MESSAGE_GREATER_THAN_ZERO)]
        public int? MovieId { set; get; }
        public DateTime? Date { set; get; }

        public bool IsNotShowedYet { set; get; } = true;
        public bool IsIncludeMovie { set; get; } = true;
        public bool IsIncludeTicketType { set; get; } = true;
    }
}
