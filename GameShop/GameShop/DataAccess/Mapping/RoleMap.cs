using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameShop.Models;
using System.Data.Entity.ModelConfiguration;

namespace GameShop.DataAccess.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("Roles");
            HasKey(x => x.Id);
        }
    }
}