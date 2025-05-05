using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Domain.Entites
{
    public class User:IDP.Domain.Entites.BaseEntites.BaseEntity
    {
        public required string FullName { get; set; }
        public required string CodeNumber { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Salt { get; set; }
        
    }
}
