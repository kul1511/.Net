using System;
using System.Collections.Generic;

namespace ProductEFAPI.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? City { get; set; }

    public int? Salary { get; set; }
}
