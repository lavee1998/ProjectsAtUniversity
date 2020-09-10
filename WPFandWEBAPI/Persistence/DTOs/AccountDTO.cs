﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistence.DTOs
{
    public class LoginDto
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}