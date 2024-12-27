using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.DataTransferObjects.Authentication;

public record RefreshTokenDto(string accessToken,
    string refreshToken);

