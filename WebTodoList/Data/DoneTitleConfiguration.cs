using System;
using Microsoft.EntityFrameworkCore;
using WebTodoList.Models;

namespace WebTodoList.Data
{
    public class DoneTitleConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public DoneTitleConfiguration()
        {
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TodoItem> builder)
        {
            builder.ToTable("DoneTitles");
            builder.HasKey(st => st.Done);
            builder.Property(st => st.Done)
            .HasColumnType("nvarchar(20)")
            .HasConversion(
                st => st.ToString(),
                st => (TodoStatus)Enum.Parse(typeof(TodoStatus), st));
            builder.Property(st => st.Title)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);
        }
    }
}
