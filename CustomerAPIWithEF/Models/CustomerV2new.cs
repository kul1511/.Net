using System;
using System.Collections.Generic;

namespace CustomerAPIWithEF.Models;

public partial class CustomerV2new
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;
}
