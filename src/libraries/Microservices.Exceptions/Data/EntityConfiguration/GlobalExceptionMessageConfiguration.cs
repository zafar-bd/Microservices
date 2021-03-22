using Microservices.Exceptions.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Order.Data.EntityConfiguration
{
    public class GlobalExceptionMessageConfiguration : IEntityTypeConfiguration<GlobalExceptionMessage>
    {
        public void Configure(EntityTypeBuilder<GlobalExceptionMessage> builder)
        {
        }
    }
}
