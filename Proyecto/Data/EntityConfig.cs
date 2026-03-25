using Proyecto.Data.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Proyecto.Data.EntityConfig
{
    public class RolConfig : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Rol");
            builder.HasKey(x => x.RolId);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Descripcion).HasMaxLength(300);
            builder.Property(x => x.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("Activo");
        }
    }

    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(x => x.UsuarioId);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(512);

            builder.HasOne(x => x.Rol)
                   .WithMany(x => x.Usuarios)
                   .HasForeignKey(x => x.RolId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class EmpleadoConfig : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("Empleado");
            builder.HasKey(x => x.EmpleadoId);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Apellido).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Cargo).IsRequired().HasMaxLength(100);

            builder.HasIndex(x => x.UsuarioId).IsUnique();

            builder.HasOne(x => x.Usuario)
                   .WithOne(x => x.Empleado)
                   .HasForeignKey<Empleado>(x => x.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ModulosAgrupadosConfig : IEntityTypeConfiguration<ModulosAgrupados>
    {
        public void Configure(EntityTypeBuilder<ModulosAgrupados> builder)
        {
            builder.ToTable("ModulosAgrupados");
            builder.HasKey(x => x.ModulosAgrupadosId);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(200);
        }
    }

    public class ModuloConfig : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.ToTable("Modulo");
            builder.HasKey(x => x.ModuloId);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Controller).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Metodo).IsRequired().HasMaxLength(50);

            builder.HasOne(x => x.ModulosAgrupados)
                   .WithMany(x => x.Modulos)
                   .HasForeignKey(x => x.ModulosAgrupadoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ModulosRolesConfig : IEntityTypeConfiguration<ModulosRoles>
    {
        public void Configure(EntityTypeBuilder<ModulosRoles> builder)
        {
            builder.ToTable("ModulosRoles");
            builder.HasKey(x => x.ModulosRolesId);
            builder.Property(x => x.Descripcion).HasMaxLength(200);
            builder.Property(x => x.CanRead).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CanCreate).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CanUpdate).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CanDelete).IsRequired().HasDefaultValue(false);

            builder.HasIndex(x => new { x.ModuloId, x.RolId }).IsUnique();

            builder.HasOne(x => x.Modulo)
                   .WithMany(x => x.ModulosRoles)
                   .HasForeignKey(x => x.ModuloId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Rol)
                   .WithMany(x => x.ModulosRoles)
                   .HasForeignKey(x => x.RolId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class SucursalConfig : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> builder)
        {
            builder.ToTable("Sucursal");
            builder.HasKey(x => x.SucursalId);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Direccion).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Telefono).HasMaxLength(20);
            builder.Property(x => x.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("Activa");
        }
    }

    public class ServicioConfig : IEntityTypeConfiguration<Servicio>
    {
        public void Configure(EntityTypeBuilder<Servicio> builder)
        {
            builder.ToTable("Servicio");
            builder.HasKey(x => x.ServicioId);
            builder.Property(x => x.Nombre_Servicio).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Prefijo_Ticket).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Descripcion).HasMaxLength(300);
            builder.Property(x => x.Tiempo_Estimado).IsRequired().HasDefaultValue(10);
            builder.Property(x => x.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("Activo");
        }
    }

    public class PrioridadConfig : IEntityTypeConfiguration<Prioridad>
    {
        public void Configure(EntityTypeBuilder<Prioridad> builder)
        {
            builder.ToTable("Prioridad");
            builder.HasKey(x => x.PrioridadId);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Descripcion).HasMaxLength(300);
            builder.Property(x => x.Peso).IsRequired();
        }
    }

    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");
            builder.HasKey(x => x.ClienteId);
            builder.Property(x => x.DNI).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.DNI).IsUnique();
            builder.Property(x => x.Nombre_Cliente).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Apellido_Cliente).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Fecha_Nacimiento).IsRequired(false);
            builder.Property(x => x.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("Activo");
        }
    }

    public class VentanillaConfig : IEntityTypeConfiguration<Ventanilla>
    {
        public void Configure(EntityTypeBuilder<Ventanilla> builder)
        {
            builder.ToTable("Ventanilla");
            builder.HasKey(x => x.VentanillaId);
            builder.Property(x => x.Numero_Ventanilla).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Estado_Ventanilla).IsRequired().HasMaxLength(20).HasDefaultValue("Cerrada");

            builder.HasOne(x => x.Sucursal)
                   .WithMany(x => x.Ventanillas)
                   .HasForeignKey(x => x.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class VentanillaServicioConfig : IEntityTypeConfiguration<VentanillaServicio>
    {
        public void Configure(EntityTypeBuilder<VentanillaServicio> builder)
        {
            builder.ToTable("VentanillaServicio");
            builder.HasKey(x => x.VentanillaServicioId);
            builder.Property(x => x.Activo).IsRequired().HasDefaultValue(true);

            builder.HasIndex(x => new { x.VentanillaId, x.ServicioId }).IsUnique();

            builder.HasOne(x => x.Ventanilla)
                   .WithMany(x => x.VentanillaServicios)
                   .HasForeignKey(x => x.VentanillaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Servicio)
                   .WithMany(x => x.VentanillaServicios)
                   .HasForeignKey(x => x.ServicioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class AsignacionVentanillaConfig : IEntityTypeConfiguration<AsignacionVentanilla>
    {
        public void Configure(EntityTypeBuilder<AsignacionVentanilla> builder)
        {
            builder.ToTable("AsignacionVentanilla");
            builder.HasKey(x => x.AsignacionId);
            builder.Property(x => x.Hora_Inicio).IsRequired();
            builder.Property(x => x.Hora_Fin).IsRequired(false);

            builder.HasOne(x => x.Empleado)
                   .WithMany(x => x.AsignacionVentanillas)
                   .HasForeignKey(x => x.EmpleadoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Ventanilla)
                   .WithMany(x => x.AsignacionVentanillas)
                   .HasForeignKey(x => x.VentanillaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ColaConfig : IEntityTypeConfiguration<Cola>
    {
        public void Configure(EntityTypeBuilder<Cola> builder)
        {
            builder.ToTable("Cola");
            builder.HasKey(x => x.ColaId);
            builder.Property(x => x.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("Activa");

            builder.HasOne(x => x.Prioridad)
                   .WithMany(x => x.Colas)
                   .HasForeignKey(x => x.PrioridadId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cliente)
                   .WithMany(x => x.Colas)
                   .HasForeignKey(x => x.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Ticket");
            builder.HasKey(x => x.TicketId);
            builder.Property(x => x.Numero_Ticket).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.Numero_Ticket).IsUnique();
            builder.Property(x => x.Hora_Emision).IsRequired();
            builder.Property(x => x.Hora_Atencion).IsRequired(false);
            builder.Property(x => x.Hora_Finalizacion).IsRequired(false);
            builder.Property(x => x.Estado_Ticket).IsRequired().HasMaxLength(20).HasDefaultValue("Pendiente");
            builder.Property(x => x.Posicion).IsRequired().HasDefaultValue(0);

            builder.HasOne(x => x.Cola)
                   .WithMany(x => x.Tickets)
                   .HasForeignKey(x => x.ColaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Servicio)
                   .WithMany(x => x.Tickets)
                   .HasForeignKey(x => x.ServicioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sucursal)
                   .WithMany()
                   .HasForeignKey(x => x.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Ventanilla)
                   .WithMany(x => x.Tickets)
                   .HasForeignKey(x => x.VentanillaId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
