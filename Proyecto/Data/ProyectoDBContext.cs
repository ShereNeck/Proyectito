using Proyecto.Data.Entidades;
using Proyecto.Data.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Data
{
    public class ProyectoDBContext : DbContext
    {
        public ProyectoDBContext(DbContextOptions<ProyectoDBContext> options)
            : base(options) { }


        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ModulosAgrupados> ModulosAgrupados { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<ModulosRoles> ModulosRoles { get; set; }


        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Ventanilla> Ventanillas { get; set; }
        public DbSet<VentanillaServicio> VentanillaServicios { get; set; }


        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<AsignacionVentanilla> AsignacionVentanillas { get; set; }


        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cola> Colas { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolConfig());
            modelBuilder.ApplyConfiguration(new UsuarioConfig());
            modelBuilder.ApplyConfiguration(new ModulosAgrupadosConfig());
            modelBuilder.ApplyConfiguration(new ModuloConfig());
            modelBuilder.ApplyConfiguration(new ModulosRolesConfig());
            modelBuilder.ApplyConfiguration(new SucursalConfig());
            modelBuilder.ApplyConfiguration(new ServicioConfig());
            modelBuilder.ApplyConfiguration(new PrioridadConfig());
            modelBuilder.ApplyConfiguration(new ClienteConfig());
            modelBuilder.ApplyConfiguration(new EmpleadoConfig());
            modelBuilder.ApplyConfiguration(new VentanillaConfig());
            modelBuilder.ApplyConfiguration(new VentanillaServicioConfig());
            modelBuilder.ApplyConfiguration(new AsignacionVentanillaConfig());
            modelBuilder.ApplyConfiguration(new ColaConfig());
            modelBuilder.ApplyConfiguration(new TicketConfig());
        }

        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AuditEntities()
        {
            var entries = ChangeTracker.Entries<EntidadBase>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreateDate = now;
                    entry.Entity.ModifiedDate = now;
                    entry.Entity.Eliminado = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = now;
                    entry.Property(x => x.CreateDate).IsModified = false;
                    entry.Property(x => x.CreateBy).IsModified = false;
                }
            }
        }
    }
}
