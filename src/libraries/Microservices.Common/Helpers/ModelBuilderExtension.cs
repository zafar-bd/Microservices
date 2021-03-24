using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Common.Helpers
{
    public static class ModelBuilderExtension
    {
        public static void ManageNonEntities(this ModelBuilder modelBuilder)
        {
            var ignorableEntities = typeof(IgnorableEntity)
                 .Assembly
                 .GetTypes()
                 .Where(x => typeof(IgnorableEntity).IsAssignableFrom(x))
                 .ToList();

            ignorableEntities.ForEach(vm =>
            {
                modelBuilder.Entity(vm).HasNoKey();
                //modelBuilder.Ignore(vm);
            });
        }

        public static void ManageDecimalPrecision(this IEnumerable<IMutableEntityType> entities)
        {
            entities
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)
                         || p.ClrType == typeof(decimal?))
                .ToList()
                .ForEach(p =>
                {
                    p.SetPrecision(18);
                    p.SetScale(2);
                });
        }

        public static void ManageDeletingBehaviorRestrictions(this IEnumerable<IMutableEntityType> entities)
        {
            entities
               .SelectMany(e => e.GetForeignKeys())
               .ToList()
               .ForEach(relationship => relationship.DeleteBehavior = DeleteBehavior.Restrict);
        }
    }
}
