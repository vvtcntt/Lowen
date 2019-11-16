using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LOWEN.Models.Mapping
{
    public class tblGroupPriceMap : EntityTypeConfiguration<tblGroupPrice>
    {
        public tblGroupPriceMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.GroupName)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("tblGroupPrice");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.Sale).HasColumnName("Sale");
            this.Property(t => t.Ord).HasColumnName("Ord");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreate).HasColumnName("DateCreate");
        }
    }
}
