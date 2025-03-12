using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerAPIWithEF.Models;

public partial class CustomerV1
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }
}
