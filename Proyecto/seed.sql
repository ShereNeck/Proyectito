-- ============================================================
--  SEED: Banco Argentum - Datos de Prueba
--  Fecha: 2026-03-25
--  Incluye corrección de tipo en Rol.Estado (bit → nvarchar)
-- ============================================================
SET NOCOUNT ON;

-- ─────────────────────────────────────────────────────────────
-- 0.  FIX: Rol.Estado (bit en DB  →  nvarchar(20) en código)
-- ─────────────────────────────────────────────────────────────
IF EXISTS (
    SELECT 1
    FROM   sys.columns c
    JOIN   sys.tables  t ON c.object_id = t.object_id
    WHERE  t.name = 'Rol' AND c.name = 'Estado'
      AND  c.system_type_id = 104          -- 104 = bit
)
BEGIN
    -- Quitar la DEFAULT constraint del bit
    DECLARE @con nvarchar(200);
    SELECT  @con = dc.name
    FROM    sys.default_constraints dc
    JOIN    sys.columns c
              ON dc.parent_object_id = c.object_id
             AND dc.parent_column_id = c.column_id
    JOIN    sys.tables t ON c.object_id = t.object_id
    WHERE   t.name = 'Rol' AND c.name = 'Estado';

    IF @con IS NOT NULL
        EXEC('ALTER TABLE [Rol] DROP CONSTRAINT [' + @con + ']');

    -- Convertir la columna
    ALTER TABLE [dbo].[Rol] ALTER COLUMN [Estado] nvarchar(20) NOT NULL;

    -- Actualizar datos existentes: 1/'1' → 'Activo', 0/'0' → 'Inactivo'
    UPDATE [dbo].[Rol] SET [Estado] = 'Activo' WHERE [Estado] IN ('1','True');
    UPDATE [dbo].[Rol] SET [Estado] = 'Inactivo' WHERE [Estado] IN ('0','False');

    -- Nueva DEFAULT
    ALTER TABLE [dbo].[Rol] ADD DEFAULT ('Activo') FOR [Estado];

    PRINT 'FIX aplicado: Rol.Estado cambiado de bit a nvarchar(20)';
END
ELSE
    PRINT 'Rol.Estado ya es nvarchar - no se requiere fix.';

GO

-- ─────────────────────────────────────────────────────────────
-- Variables de GUIDs fijos para reproducibilidad
-- ─────────────────────────────────────────────────────────────
DECLARE @ROL_ADMIN    uniqueidentifier = '6A44FFF2-D955-44FE-B3F7-72BACAA6E095';
DECLARE @ROL_AGENTE   uniqueidentifier = '1FA34B54-7C12-4842-900C-84E9DB92C11D';
DECLARE @ROL_CLIENTE  uniqueidentifier = 'C12D3F72-ED87-44CC-BC6D-B57648132317';

DECLARE @USR_ADMIN    uniqueidentifier = 'E77CE240-5A36-479D-9696-8E1915B78E1E';
DECLARE @USR_LESLIE   uniqueidentifier = '173AFDED-50EC-4515-804C-5AAF4B1DC5C7';
DECLARE @USR_AGENTE   uniqueidentifier = 'F8373211-9459-4E6F-9515-86CABF1AC4E1';
DECLARE @USR_EMILIO   uniqueidentifier = 'A2D13175-1140-4D96-BE1D-1A9D9FFD76DD';
DECLARE @USR_TEST     uniqueidentifier = 'AAB2A9A3-3AD9-445F-98D3-183F5D735EE7';
DECLARE @USR_MARIA    uniqueidentifier = 'B1000001-0000-0000-0000-000000000001';

DECLARE @EMP_LESLIE   uniqueidentifier = 'B0000001-0000-0000-0000-000000000001';
DECLARE @EMP_AGENTE   uniqueidentifier = 'B0000002-0000-0000-0000-000000000002';

DECLARE @SUC1         uniqueidentifier = 'D0000001-0000-0000-0000-000000000001';
DECLARE @SUC2         uniqueidentifier = 'D0000002-0000-0000-0000-000000000002';

DECLARE @VEN1_1       uniqueidentifier = 'A0000001-0000-0000-0000-000000000001';
DECLARE @VEN1_2       uniqueidentifier = 'A0000002-0000-0000-0000-000000000002';
DECLARE @VEN1_3       uniqueidentifier = 'A0000003-0000-0000-0000-000000000003';
DECLARE @VEN2_1       uniqueidentifier = 'A0000004-0000-0000-0000-000000000004';
DECLARE @VEN2_2       uniqueidentifier = 'A0000005-0000-0000-0000-000000000005';

DECLARE @SRV_CAJA     uniqueidentifier = 'E0000001-0000-0000-0000-000000000001';
DECLARE @SRV_CREDITO  uniqueidentifier = 'E0000002-0000-0000-0000-000000000002';
DECLARE @SRV_INVERSION uniqueidentifier = 'E0000003-0000-0000-0000-000000000003';
DECLARE @SRV_CONSULTA uniqueidentifier = 'E0000004-0000-0000-0000-000000000004';
DECLARE @SRV_RECLAMO  uniqueidentifier = 'E0000005-0000-0000-0000-000000000005';

DECLARE @PRI_NORMAL   uniqueidentifier = 'F0000001-0000-0000-0000-000000000001';
DECLARE @PRI_PREFER   uniqueidentifier = 'F0000002-0000-0000-0000-000000000002';
DECLARE @PRI_VIP      uniqueidentifier = 'F0000003-0000-0000-0000-000000000003';

DECLARE @CLI_EMILIO   uniqueidentifier = 'C0000001-0000-0000-0000-000000000001';
DECLARE @CLI_TEST     uniqueidentifier = 'C0000002-0000-0000-0000-000000000002';
DECLARE @CLI_MARIA    uniqueidentifier = 'C0000003-0000-0000-0000-000000000003';

DECLARE @COLA1        uniqueidentifier = 'CC000001-0000-0000-0000-000000000001';
DECLARE @COLA2        uniqueidentifier = 'CC000002-0000-0000-0000-000000000002';
DECLARE @COLA3        uniqueidentifier = 'CC000003-0000-0000-0000-000000000003';

DECLARE @MODG1        uniqueidentifier = 'AA100001-0000-0000-0000-000000000001';
DECLARE @MODG2        uniqueidentifier = 'AA100002-0000-0000-0000-000000000002';
DECLARE @MODG3        uniqueidentifier = 'AA100003-0000-0000-0000-000000000003';

DECLARE @MOD1         uniqueidentifier = 'BB200001-0000-0000-0000-000000000001';
DECLARE @MOD2         uniqueidentifier = 'BB200002-0000-0000-0000-000000000002';
DECLARE @MOD3         uniqueidentifier = 'BB200003-0000-0000-0000-000000000003';
DECLARE @MOD4         uniqueidentifier = 'BB200004-0000-0000-0000-000000000004';
DECLARE @MOD5         uniqueidentifier = 'BB200005-0000-0000-0000-000000000005';
DECLARE @MOD6         uniqueidentifier = 'BB200006-0000-0000-0000-000000000006';

DECLARE @NOW          datetime2 = GETUTCDATE();
DECLARE @ZERO         uniqueidentifier = '00000000-0000-0000-0000-000000000000';

-- ─────────────────────────────────────────────────────────────
-- 1. ROLES
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Rol] AS tgt
USING (VALUES
    (@ROL_ADMIN,   'Administrador', 'Acceso total al sistema',                  'Activo'),
    (@ROL_AGENTE,  'Agente',        'Empleado de ventanilla',                   'Activo'),
    (@ROL_CLIENTE, 'Cliente',       'Usuario cliente del sistema de turnos',    'Activo')
) AS src (RolId, Nombre, Descripcion, Estado)
ON tgt.RolId = src.RolId
WHEN NOT MATCHED THEN
    INSERT (RolId, Nombre, Descripcion, Estado, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.RolId, src.Nombre, src.Descripcion, src.Estado, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, Descripcion = src.Descripcion, Estado = src.Estado, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 2. USUARIOS
--    MERGE por Email (índice único) para no conflictuar con
--    UsuarioIds ya existentes en la BD.
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Usuario] AS tgt
USING (VALUES
    -- (Nombre, Email, PasswordHash, RolId)
    ('admin',   'admin@banco.com',         '$2a$11$S92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9IIC/.og/a3LWqXiGJXey', @ROL_ADMIN),
    ('Leslie',  'leslie@banco.com',        '1234',      @ROL_AGENTE),
    ('agente',  'agente@banco.com',        'agente123', @ROL_AGENTE),
    ('EmilioA', 'emilioarg355@gmail.com',  '1234',      @ROL_CLIENTE),
    ('test',    'test@test.com',           'hola123',   @ROL_CLIENTE),
    ('MariaL',  'maria.lopez@gmail.com',   'maria123',  @ROL_CLIENTE)
) AS src (Nombre, Email, PasswordHash, RolId)
ON tgt.Email = src.Email AND tgt.Eliminado = 0
WHEN NOT MATCHED THEN
    INSERT (UsuarioId, Nombre, Email, PasswordHash, RolId, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), src.Nombre, src.Email, src.PasswordHash, src.RolId, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, PasswordHash = src.PasswordHash, RolId = src.RolId;

-- Leer los IDs reales de los agentes para usarlos en Empleados
SELECT @USR_LESLIE = UsuarioId FROM [dbo].[Usuario] WHERE Email = 'leslie@banco.com'  AND Eliminado = 0;
SELECT @USR_AGENTE = UsuarioId FROM [dbo].[Usuario] WHERE Email = 'agente@banco.com'  AND Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 3. SUCURSALES
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Sucursal] AS tgt
USING (VALUES
    (@SUC1, 'Sucursal Central', 'Av. Independencia 1500, Tegucigalpa', '2222-1000', 'Activa'),
    (@SUC2, 'Sucursal Norte',   'Blvd. Morazán 890, Tegucigalpa',      '2222-2000', 'Activa')
) AS src (SucursalId, Nombre, Direccion, Telefono, Estado)
ON tgt.SucursalId = src.SucursalId
WHEN NOT MATCHED THEN
    INSERT (SucursalId, Nombre, Direccion, Telefono, Estado, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.SucursalId, src.Nombre, src.Direccion, src.Telefono, src.Estado, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, Direccion = src.Direccion, Telefono = src.Telefono,
               Estado = src.Estado, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 4. SERVICIOS
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Servicio] AS tgt
USING (VALUES
    (@SRV_CAJA,      'Caja y Pagos',       'CA', 'Pago de servicios, retiros y depósitos', 10, 'Activo'),
    (@SRV_CREDITO,   'Créditos',           'CR', 'Solicitud y gestión de préstamos',        20, 'Activo'),
    (@SRV_INVERSION, 'Inversiones',        'IN', 'Apertura y consulta de inversiones',      15, 'Activo'),
    (@SRV_CONSULTA,  'Consulta General',   'CG', 'Información de productos y servicios',    10, 'Activo'),
    (@SRV_RECLAMO,   'Reclamos',           'RC', 'Atención de quejas y reclamos',           15, 'Activo')
) AS src (ServicioId, Nombre_Servicio, Prefijo_Ticket, Descripcion, Tiempo_Estimado, Estado)
ON tgt.ServicioId = src.ServicioId
WHEN NOT MATCHED THEN
    INSERT (ServicioId, Nombre_Servicio, Prefijo_Ticket, Descripcion, Tiempo_Estimado, Estado,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.ServicioId, src.Nombre_Servicio, src.Prefijo_Ticket, src.Descripcion,
            src.Tiempo_Estimado, src.Estado, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre_Servicio = src.Nombre_Servicio, Prefijo_Ticket = src.Prefijo_Ticket,
               Descripcion = src.Descripcion, Tiempo_Estimado = src.Tiempo_Estimado,
               Estado = src.Estado, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 5. PRIORIDADES
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Prioridad] AS tgt
USING (VALUES
    (@PRI_NORMAL, 'Normal',       'Atención en orden de llegada',                    1),
    (@PRI_PREFER, 'Preferencial', 'Adultos mayores, embarazadas, personas con discapacidad', 2),
    (@PRI_VIP,    'VIP',          'Clientes con cuentas premium',                    3)
) AS src (PrioridadId, Nombre, Descripcion, Peso)
ON tgt.PrioridadId = src.PrioridadId
WHEN NOT MATCHED THEN
    INSERT (PrioridadId, Nombre, Descripcion, Peso, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.PrioridadId, src.Nombre, src.Descripcion, src.Peso, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, Descripcion = src.Descripcion, Peso = src.Peso, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 6. VENTANILLAS
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Ventanilla] AS tgt
USING (VALUES
    -- Sucursal Central
    (@VEN1_1, 'V-01', 'Abierta',  @SUC1),
    (@VEN1_2, 'V-02', 'Abierta',  @SUC1),
    (@VEN1_3, 'V-03', 'Cerrada',  @SUC1),
    -- Sucursal Norte
    (@VEN2_1, 'V-01', 'Abierta',  @SUC2),
    (@VEN2_2, 'V-02', 'Cerrada',  @SUC2)
) AS src (VentanillaId, Numero_Ventanilla, Estado_Ventanilla, SucursalId)
ON tgt.VentanillaId = src.VentanillaId
WHEN NOT MATCHED THEN
    INSERT (VentanillaId, Numero_Ventanilla, Estado_Ventanilla, SucursalId,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.VentanillaId, src.Numero_Ventanilla, src.Estado_Ventanilla, src.SucursalId,
            @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Numero_Ventanilla = src.Numero_Ventanilla, Estado_Ventanilla = src.Estado_Ventanilla,
               SucursalId = src.SucursalId, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 7. VENTANILLA - SERVICIO  (qué servicios atiende cada ventanilla)
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[VentanillaServicio] AS tgt
USING (VALUES
    -- Sucursal Central V-01: Caja y Consulta
    (NEWID(), @VEN1_1, @SRV_CAJA,      1),
    (NEWID(), @VEN1_1, @SRV_CONSULTA,  1),
    -- Sucursal Central V-02: Créditos e Inversiones
    (NEWID(), @VEN1_2, @SRV_CREDITO,   1),
    (NEWID(), @VEN1_2, @SRV_INVERSION, 1),
    -- Sucursal Central V-03: Reclamos
    (NEWID(), @VEN1_3, @SRV_RECLAMO,   1),
    -- Sucursal Norte V-01: Caja y Créditos
    (NEWID(), @VEN2_1, @SRV_CAJA,      1),
    (NEWID(), @VEN2_1, @SRV_CREDITO,   1),
    -- Sucursal Norte V-02: Consulta y Reclamos
    (NEWID(), @VEN2_2, @SRV_CONSULTA,  1),
    (NEWID(), @VEN2_2, @SRV_RECLAMO,   1)
) AS src (VentanillaServicioId, VentanillaId, ServicioId, Activo)
ON tgt.VentanillaId = src.VentanillaId AND tgt.ServicioId = src.ServicioId
WHEN NOT MATCHED THEN
    INSERT (VentanillaServicioId, VentanillaId, ServicioId, Activo,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.VentanillaServicioId, src.VentanillaId, src.ServicioId, src.Activo,
            @ZERO, @NOW, @ZERO, @NOW, 0);

-- ─────────────────────────────────────────────────────────────
-- 8. EMPLEADOS  (MERGE por UsuarioId — índice único en la tabla)
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Empleado] AS tgt
USING (VALUES
    ('Leslie',  'García',    'Cajera',            @USR_LESLIE),
    ('Carlos',  'Rodríguez', 'Ejecutivo de Caja', @USR_AGENTE)
) AS src (Nombre, Apellido, Cargo, UsuarioId)
ON tgt.UsuarioId = src.UsuarioId AND tgt.Eliminado = 0
WHEN NOT MATCHED THEN
    INSERT (EmpleadoId, Nombre, Apellido, Cargo, UsuarioId,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), src.Nombre, src.Apellido, src.Cargo, src.UsuarioId,
            @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, Apellido = src.Apellido, Cargo = src.Cargo;

-- Leer los IDs reales de los empleados para las asignaciones
SELECT @EMP_LESLIE = EmpleadoId FROM [dbo].[Empleado] WHERE UsuarioId = @USR_LESLIE AND Eliminado = 0;
SELECT @EMP_AGENTE = EmpleadoId FROM [dbo].[Empleado] WHERE UsuarioId = @USR_AGENTE AND Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 9. ASIGNACION VENTANILLA  (empleados activos en ventanilla)
-- ─────────────────────────────────────────────────────────────
IF @EMP_LESLIE IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM [dbo].[AsignacionVentanilla]
    WHERE EmpleadoId = @EMP_LESLIE AND Hora_Fin IS NULL AND Eliminado = 0)
    INSERT INTO [dbo].[AsignacionVentanilla]
        (AsignacionId, Hora_Inicio, Hora_Fin, EmpleadoId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), @NOW, NULL, @EMP_LESLIE, @VEN1_1, @ZERO, @NOW, @ZERO, @NOW, 0);

IF @EMP_AGENTE IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM [dbo].[AsignacionVentanilla]
    WHERE EmpleadoId = @EMP_AGENTE AND Hora_Fin IS NULL AND Eliminado = 0)
    INSERT INTO [dbo].[AsignacionVentanilla]
        (AsignacionId, Hora_Inicio, Hora_Fin, EmpleadoId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), @NOW, NULL, @EMP_AGENTE, @VEN1_2, @ZERO, @NOW, @ZERO, @NOW, 0);

-- ─────────────────────────────────────────────────────────────
-- 10. CLIENTES  (perfiles de cliente, distintos de Usuarios)
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Cliente] AS tgt
USING (VALUES
    (@CLI_EMILIO, '08012004001122', 'Emilio',  'Argueta',  '1990-05-14', 'Activo'),
    (@CLI_TEST,   '08012004003344', 'Usuario', 'Prueba',   '1985-11-20', 'Activo'),
    (@CLI_MARIA,  '08012004005566', 'María',   'López',    '1992-03-08', 'Activo')
) AS src (ClienteId, DNI, Nombre_Cliente, Apellido_Cliente, Fecha_Nacimiento, Estado)
ON tgt.ClienteId = src.ClienteId
WHEN NOT MATCHED THEN
    INSERT (ClienteId, DNI, Nombre_Cliente, Apellido_Cliente, Fecha_Nacimiento, Estado,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.ClienteId, src.DNI, src.Nombre_Cliente, src.Apellido_Cliente,
            src.Fecha_Nacimiento, src.Estado, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET DNI = src.DNI, Nombre_Cliente = src.Nombre_Cliente,
               Apellido_Cliente = src.Apellido_Cliente, Fecha_Nacimiento = src.Fecha_Nacimiento,
               Estado = src.Estado, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 11. COLAS
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Cola] AS tgt
USING (VALUES
    (@COLA1, 'Activa', @PRI_NORMAL, @CLI_EMILIO),
    (@COLA2, 'Activa', @PRI_PREFER, @CLI_TEST),
    (@COLA3, 'Activa', @PRI_VIP,    @CLI_MARIA)
) AS src (ColaId, Estado, PrioridadId, ClienteId)
ON tgt.ColaId = src.ColaId
WHEN NOT MATCHED THEN
    INSERT (ColaId, Estado, PrioridadId, ClienteId,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.ColaId, src.Estado, src.PrioridadId, src.ClienteId,
            @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Estado = src.Estado, PrioridadId = src.PrioridadId,
               ClienteId = src.ClienteId, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 12. TICKETS  (varios estados para probar el flujo completo)
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM [dbo].[Ticket] WHERE Numero_Ticket = 'CA-001')
    INSERT INTO [dbo].[Ticket]
        (TicketId, Numero_Ticket, Hora_Emision, Hora_Atencion, Hora_Finalizacion,
         Estado_Ticket, Posicion, ColaId, ServicioId, SucursalId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES
        (NEWID(), 'CA-001', DATEADD(MINUTE,-40,@NOW), DATEADD(MINUTE,-30,@NOW), DATEADD(MINUTE,-20,@NOW),
         'Atendido', 1, @COLA1, @SRV_CAJA, @SUC1, @VEN1_1,
         @ZERO, @NOW, @ZERO, @NOW, 0);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ticket] WHERE Numero_Ticket = 'CR-001')
    INSERT INTO [dbo].[Ticket]
        (TicketId, Numero_Ticket, Hora_Emision, Hora_Atencion, Hora_Finalizacion,
         Estado_Ticket, Posicion, ColaId, ServicioId, SucursalId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES
        (NEWID(), 'CR-001', DATEADD(MINUTE,-20,@NOW), DATEADD(MINUTE,-10,@NOW), NULL,
         'En atención', 2, @COLA2, @SRV_CREDITO, @SUC1, @VEN1_2,
         @ZERO, @NOW, @ZERO, @NOW, 0);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ticket] WHERE Numero_Ticket = 'CA-002')
    INSERT INTO [dbo].[Ticket]
        (TicketId, Numero_Ticket, Hora_Emision, Hora_Atencion, Hora_Finalizacion,
         Estado_Ticket, Posicion, ColaId, ServicioId, SucursalId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES
        (NEWID(), 'CA-002', DATEADD(MINUTE,-15,@NOW), NULL, NULL,
         'En espera', 3, @COLA3, @SRV_CAJA, @SUC1, NULL,
         @ZERO, @NOW, @ZERO, @NOW, 0);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ticket] WHERE Numero_Ticket = 'CG-001')
    INSERT INTO [dbo].[Ticket]
        (TicketId, Numero_Ticket, Hora_Emision, Hora_Atencion, Hora_Finalizacion,
         Estado_Ticket, Posicion, ColaId, ServicioId, SucursalId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES
        (NEWID(), 'CG-001', DATEADD(MINUTE,-10,@NOW), NULL, NULL,
         'En espera', 4, @COLA1, @SRV_CONSULTA, @SUC1, NULL,
         @ZERO, @NOW, @ZERO, @NOW, 0);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ticket] WHERE Numero_Ticket = 'IN-001')
    INSERT INTO [dbo].[Ticket]
        (TicketId, Numero_Ticket, Hora_Emision, Hora_Atencion, Hora_Finalizacion,
         Estado_Ticket, Posicion, ColaId, ServicioId, SucursalId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES
        (NEWID(), 'IN-001', DATEADD(MINUTE,-5,@NOW), NULL, NULL,
         'En espera', 5, @COLA2, @SRV_INVERSION, @SUC2, NULL,
         @ZERO, @NOW, @ZERO, @NOW, 0);

-- ─────────────────────────────────────────────────────────────
-- 13. MODULOS AGRUPADOS  (grupos del menú lateral Admin)
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[ModulosAgrupados] AS tgt
USING (VALUES
    (@MODG1, 'Gestión de Usuarios'),
    (@MODG2, 'Gestión Operativa'),
    (@MODG3, 'Reportes y Configuración')
) AS src (ModulosAgrupadosId, Descripcion)
ON tgt.ModulosAgrupadosId = src.ModulosAgrupadosId
WHEN NOT MATCHED THEN
    INSERT (ModulosAgrupadosId, Descripcion, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.ModulosAgrupadosId, src.Descripcion, @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Descripcion = src.Descripcion, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 14. MODULOS
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Modulo] AS tgt
USING (VALUES
    (@MOD1, 'Usuarios',     'Admin',   'Index',  @MODG1),
    (@MOD2, 'Roles',        'Admin',   'Roles',  @MODG1),
    (@MOD3, 'Sucursales',   'Admin',   'Sucursales', @MODG2),
    (@MOD4, 'Servicios',    'Admin',   'Servicios',  @MODG2),
    (@MOD5, 'Ventanillas',  'Admin',   'Ventanillas',@MODG2),
    (@MOD6, 'Reportes',     'Admin',   'Reportes',   @MODG3)
) AS src (ModuloId, Nombre, Controller, Metodo, ModulosAgrupadoId)
ON tgt.ModuloId = src.ModuloId
WHEN NOT MATCHED THEN
    INSERT (ModuloId, Nombre, Controller, Metodo, ModulosAgrupadoId,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.ModuloId, src.Nombre, src.Controller, src.Metodo, src.ModulosAgrupadoId,
            @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, Controller = src.Controller, Metodo = src.Metodo,
               ModulosAgrupadoId = src.ModulosAgrupadoId, Eliminado = 0;

-- ─────────────────────────────────────────────────────────────
-- 15. MODULOS - ROLES  (permisos por rol)
-- ─────────────────────────────────────────────────────────────
-- Administrador: acceso total
MERGE [dbo].[ModulosRoles] AS tgt
USING (VALUES
    (NEWID(), 'Admin - Usuarios',    1,1,1,1, @MOD1, @ROL_ADMIN),
    (NEWID(), 'Admin - Roles',       1,1,1,1, @MOD2, @ROL_ADMIN),
    (NEWID(), 'Admin - Sucursales',  1,1,1,1, @MOD3, @ROL_ADMIN),
    (NEWID(), 'Admin - Servicios',   1,1,1,1, @MOD4, @ROL_ADMIN),
    (NEWID(), 'Admin - Ventanillas', 1,1,1,1, @MOD5, @ROL_ADMIN),
    (NEWID(), 'Admin - Reportes',    1,0,0,0, @MOD6, @ROL_ADMIN),
    -- Agente: solo lectura de Sucursales y Servicios
    (NEWID(), 'Agente - Sucursales', 1,0,0,0, @MOD3, @ROL_AGENTE),
    (NEWID(), 'Agente - Servicios',  1,0,0,0, @MOD4, @ROL_AGENTE),
    (NEWID(), 'Agente - Ventanillas',1,0,0,0, @MOD5, @ROL_AGENTE)
) AS src (ModulosRolesId, Descripcion, CanRead, CanCreate, CanUpdate, CanDelete, ModuloId, RolId)
ON tgt.ModuloId = src.ModuloId AND tgt.RolId = src.RolId
WHEN NOT MATCHED THEN
    INSERT (ModulosRolesId, Descripcion, CanRead, CanCreate, CanUpdate, CanDelete,
            ModuloId, RolId, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (src.ModulosRolesId, src.Descripcion, src.CanRead, src.CanCreate, src.CanUpdate,
            src.CanDelete, src.ModuloId, src.RolId, @ZERO, @NOW, @ZERO, @NOW, 0);

-- ─────────────────────────────────────────────────────────────
-- RESUMEN
-- ─────────────────────────────────────────────────────────────
PRINT '============================================';
PRINT 'SEED completado. Usuarios disponibles:';
PRINT '';
PRINT '  EMPLEADOS (login en /Account/Login?tab=empleado)';
PRINT '  - admin    / [password BCrypt]  → Administrador';
PRINT '  - Leslie   / 1234               → Agente';
PRINT '  - agente   / agente123          → Agente';
PRINT '';
PRINT '  CLIENTES (login en /Account/Login?tab=cliente)';
PRINT '  - EmilioA  / 1234               → Cliente';
PRINT '  - test     / hola123            → Cliente';
PRINT '  - MariaL   / maria123           → Cliente';
PRINT '';
PRINT '  SUCURSALES: Sucursal Central, Sucursal Norte';
PRINT '  SERVICIOS : Caja y Pagos, Créditos, Inversiones,';
PRINT '              Consulta General, Reclamos';
PRINT '  TICKETS   : CA-001 (Atendido), CR-001 (En atención),';
PRINT '              CA-002 / CG-001 / IN-001 (En espera)';
PRINT '============================================';
