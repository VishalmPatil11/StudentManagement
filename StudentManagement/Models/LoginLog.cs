using System;

namespace StudentManagement.Models
{
    public class LoginLog
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public bool Successful { get; set; }
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
    }
}
