using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Application.DTOs.Profile
{
    public class UpdateProfileDto
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Email {  get; set; }
        public string? Avatar { get; set; }
    }
}
