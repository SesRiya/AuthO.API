﻿namespace AuthenticationServer.API.Models
{
    public record RefreshToken
    {
        public Guid Id { get; set; }    
        public string RefToken { get; set; }

        public Guid UserId  { get; set; }
    }
}
