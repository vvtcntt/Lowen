using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LOWEN.Models.Mapping
{
    public class tblProductTagMap : EntityTypeConfiguration<tblProductTag>
    {
        public tblProductTagMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(200);

            this.Property(t => t.Tag)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("tblProductTag");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idp).HasColumnName("idp");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Tag).HasColumnName("Tag");
        }
    }
}
