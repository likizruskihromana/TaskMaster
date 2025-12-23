using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Application.DTOs.Auth
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiration {  get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
