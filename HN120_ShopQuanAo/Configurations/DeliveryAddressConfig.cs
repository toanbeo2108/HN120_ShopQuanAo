using HN120_ShopQuanAo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Configurations
{
    public class DeliveryAddressConfig : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.HasKey(p => p.DeliveryAddressID); // Set khóa chính
            builder.HasOne(p => p.User).WithMany(p => p.DeliveryAddress).HasForeignKey(p => p.UserID);
        }
    }
}
