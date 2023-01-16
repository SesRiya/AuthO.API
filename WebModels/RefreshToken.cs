﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
    public record RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
