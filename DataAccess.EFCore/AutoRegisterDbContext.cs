using DataAccess.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore
{
    public class AutoRegisterDbContext<TContext> : DbContext
        where TContext : IDataContext
    {
        private readonly TContext _context;

        public AutoRegisterDbContext(DbContextOptions options, TContext context) : base(options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            var types = _context.GetTypesToRegister();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttributes(typeof(EntityAttribute), true).FirstOrDefault() as EntityAttribute;
                if (attribute != null)
                {
                    var entity = modelBuilder.Entity(type);

                    if (!string.IsNullOrWhiteSpace(attribute.TableName))
                        entity.ToTable(attribute.TableName, attribute.Schema);
                    else
                        entity.HasNoKey();

                    var props = type.GetProperties()
                                    .Where(info => Attribute.IsDefined(info, typeof(CompositeKeyAttribute)))
                                    .Select(p => p.Name)
                                    .ToArray();

                    if (props.Any())
                        entity.HasKey(props);
                }
            }
        }
    }
}
