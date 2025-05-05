using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Command.User
{
    public record UserCommand :IRequest<bool>
    {
        [Required(ErrorMessage = "این داده الزامی است")]
        [MinLength(4)]
        public required string FullName { get; set; }
        public required string CodeNumber { get; set; }
    }
}
