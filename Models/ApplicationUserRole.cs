﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CodersCupAward.Models;

public partial class ApplicationUserRole
{
    public int ApplicationUserRoleId { get; set; }

    public int ApplicationUserId { get; set; }

    public int ApplicationRolesId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }

    public virtual ApplicationRoles ApplicationRoles { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
}