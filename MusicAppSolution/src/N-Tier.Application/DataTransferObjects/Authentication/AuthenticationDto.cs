using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.DataTransferObjects.Authentication;

public record AuthenticationDto(string email, string password);

