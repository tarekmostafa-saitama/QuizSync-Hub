﻿

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities;

public class ApplicationUser :IdentityUser
{
    public string FullName { get; set; }

    public List<Game> Games { get; set; }

}
