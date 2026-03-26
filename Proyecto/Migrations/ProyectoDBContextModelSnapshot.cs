
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Proyecto.Data;

#nullable disable

namespace Proyecto.Migrations
{
    [DbContext(typeof(ProyectoDBContext))]
    partial class ProyectoDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Proyecto.Data.Entidades.AsignacionVentanilla", b =>
                {
                    b.Property<Guid>("AsignacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<Guid>("EmpleadoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Hora_Fin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Hora_Inicio")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("VentanillaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AsignacionId");

                    b.HasIndex("EmpleadoId");

                    b.HasIndex("VentanillaId");

                    b.ToTable("AsignacionVentanilla", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Cliente", b =>
                {
                    b.Property<Guid>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido_Cliente")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Activo");

                    b.Property<DateTime?>("Fecha_Nacimiento")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre_Cliente")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("ClienteId");

                    b.HasIndex("DNI")
                        .IsUnique();

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Cola", b =>
                {
                    b.Property<Guid>("ColaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClienteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Activa");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PrioridadId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ColaId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("PrioridadId");

                    b.ToTable("Cola", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Empleado", b =>
                {
                    b.Property<Guid>("EmpleadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Cargo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EmpleadoId");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("Empleado", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Modulo", b =>
                {
                    b.Property<Guid>("ModuloId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Controller")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Metodo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModulosAgrupadoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("ModuloId");

                    b.HasIndex("ModulosAgrupadoId");

                    b.ToTable("Modulo", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.ModulosAgrupados", b =>
                {
                    b.Property<Guid>("ModulosAgrupadosId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ModulosAgrupadosId");

                    b.ToTable("ModulosAgrupados", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.ModulosRoles", b =>
                {
                    b.Property<Guid>("ModulosRolesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanCreate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("CanUpdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModuloId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RolId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ModulosRolesId");

                    b.HasIndex("RolId");

                    b.HasIndex("ModuloId", "RolId")
                        .IsUnique();

                    b.ToTable("ModulosRoles", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Prioridad", b =>
                {
                    b.Property<Guid>("PrioridadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Peso")
                        .HasColumnType("int");

                    b.HasKey("PrioridadId");

                    b.ToTable("Prioridad", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Rol", b =>
                {
                    b.Property<Guid>("RolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Activo");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RolId");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Servicio", b =>
                {
                    b.Property<Guid>("ServicioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Activo");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre_Servicio")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Prefijo_Ticket")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("Tiempo_Estimado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(10);

                    b.HasKey("ServicioId");

                    b.ToTable("Servicio", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Sucursal", b =>
                {
                    b.Property<Guid>("SucursalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Activa");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("SucursalId");

                    b.ToTable("Sucursal", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Ticket", b =>
                {
                    b.Property<Guid>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ColaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado_Ticket")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Pendiente");

                    b.Property<DateTime?>("Hora_Atencion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Hora_Emision")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Hora_Finalizacion")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Numero_Ticket")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Posicion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("ServicioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SucursalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("VentanillaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TicketId");

                    b.HasIndex("ColaId");

                    b.HasIndex("Numero_Ticket")
                        .IsUnique();

                    b.HasIndex("ServicioId");

                    b.HasIndex("SucursalId");

                    b.HasIndex("VentanillaId");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Usuario", b =>
                {
                    b.Property<Guid>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<Guid>("RolId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UsuarioId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RolId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Ventanilla", b =>
                {
                    b.Property<Guid>("VentanillaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<string>("Estado_Ventanilla")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Cerrada");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Numero_Ventanilla")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("SucursalId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("VentanillaId");

                    b.HasIndex("SucursalId");

                    b.ToTable("Ventanilla", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.VentanillaServicio", b =>
                {
                    b.Property<Guid>("VentanillaServicioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Activo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ServicioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VentanillaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("VentanillaServicioId");

                    b.HasIndex("ServicioId");

                    b.HasIndex("VentanillaId", "ServicioId")
                        .IsUnique();

                    b.ToTable("VentanillaServicio", (string)null);
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.AsignacionVentanilla", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Empleado", "Empleado")
                        .WithMany("AsignacionVentanillas")
                        .HasForeignKey("EmpleadoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Ventanilla", "Ventanilla")
                        .WithMany("AsignacionVentanillas")
                        .HasForeignKey("VentanillaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Empleado");

                    b.Navigation("Ventanilla");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Cola", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Cliente", "Cliente")
                        .WithMany("Colas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Prioridad", "Prioridad")
                        .WithMany("Colas")
                        .HasForeignKey("PrioridadId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Prioridad");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Empleado", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Usuario", "Usuario")
                        .WithOne("Empleado")
                        .HasForeignKey("Proyecto.Data.Entidades.Empleado", "UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Modulo", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.ModulosAgrupados", "ModulosAgrupados")
                        .WithMany("Modulos")
                        .HasForeignKey("ModulosAgrupadoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ModulosAgrupados");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.ModulosRoles", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Modulo", "Modulo")
                        .WithMany("ModulosRoles")
                        .HasForeignKey("ModuloId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Rol", "Rol")
                        .WithMany("ModulosRoles")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Modulo");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Ticket", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Cola", "Cola")
                        .WithMany("Tickets")
                        .HasForeignKey("ColaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Servicio", "Servicio")
                        .WithMany("Tickets")
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Sucursal", "Sucursal")
                        .WithMany()
                        .HasForeignKey("SucursalId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Ventanilla", "Ventanilla")
                        .WithMany("Tickets")
                        .HasForeignKey("VentanillaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Cola");

                    b.Navigation("Servicio");

                    b.Navigation("Sucursal");

                    b.Navigation("Ventanilla");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Usuario", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Ventanilla", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Sucursal", "Sucursal")
                        .WithMany("Ventanillas")
                        .HasForeignKey("SucursalId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Sucursal");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.VentanillaServicio", b =>
                {
                    b.HasOne("Proyecto.Data.Entidades.Servicio", "Servicio")
                        .WithMany("VentanillaServicios")
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Proyecto.Data.Entidades.Ventanilla", "Ventanilla")
                        .WithMany("VentanillaServicios")
                        .HasForeignKey("VentanillaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Servicio");

                    b.Navigation("Ventanilla");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Cliente", b =>
                {
                    b.Navigation("Colas");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Cola", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Empleado", b =>
                {
                    b.Navigation("AsignacionVentanillas");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Modulo", b =>
                {
                    b.Navigation("ModulosRoles");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.ModulosAgrupados", b =>
                {
                    b.Navigation("Modulos");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Prioridad", b =>
                {
                    b.Navigation("Colas");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Rol", b =>
                {
                    b.Navigation("ModulosRoles");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Servicio", b =>
                {
                    b.Navigation("Tickets");

                    b.Navigation("VentanillaServicios");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Sucursal", b =>
                {
                    b.Navigation("Ventanillas");
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Usuario", b =>
                {
                    b.Navigation("Empleado")
                        .IsRequired();
                });

            modelBuilder.Entity("Proyecto.Data.Entidades.Ventanilla", b =>
                {
                    b.Navigation("AsignacionVentanillas");

                    b.Navigation("Tickets");

                    b.Navigation("VentanillaServicios");
                });
#pragma warning restore 612, 618
        }
    }
}
