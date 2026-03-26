-- ============================================================
--  PATCH: Contraseña admin + Empleados por servicio
--  Ejecutar conectado a la BD 'proyecto'
-- ============================================================
SET NOCOUNT ON;

DECLARE @NOW   datetime2       = GETUTCDATE();
DECLARE @ZERO  uniqueidentifier = '00000000-0000-0000-0000-000000000000';

-- IDs fijos de roles y sucursales (coinciden con seed.sql)
DECLARE @ROL_ADMIN   uniqueidentifier = '6A44FFF2-D955-44FE-B3F7-72BACAA6E095';
DECLARE @ROL_AGENTE  uniqueidentifier = '1FA34B54-7C12-4842-900C-84E9DB92C11D';

DECLARE @SUC1  uniqueidentifier = 'D0000001-0000-0000-0000-000000000001';
DECLARE @SUC2  uniqueidentifier = 'D0000002-0000-0000-0000-000000000002';

DECLARE @VEN1_3 uniqueidentifier = 'A0000003-0000-0000-0000-000000000003'; -- Central V-03
DECLARE @VEN2_1 uniqueidentifier = 'A0000004-0000-0000-0000-000000000004'; -- Norte V-01
DECLARE @VEN2_2 uniqueidentifier = 'A0000005-0000-0000-0000-000000000005'; -- Norte V-02

-- ─────────────────────────────────────────────────────────────
-- 1. CAMBIAR CONTRASEÑA DE ADMIN a texto plano
--    (LoginEmpleado compara PasswordHash == Password sin hash)
-- ─────────────────────────────────────────────────────────────
UPDATE [dbo].[Usuario]
SET    PasswordHash = 'admin123',
       ModifiedDate = @NOW
WHERE  Nombre = 'admin' AND Eliminado = 0;

PRINT 'Contraseña de admin actualizada a: admin123';

-- ─────────────────────────────────────────────────────────────
-- 2. NUEVOS USUARIOS AGENTE (uno por servicio sin cobertura)
-- ─────────────────────────────────────────────────────────────
--  Ana Martínez  → Reclamos        (Sucursal Central  V-03)
--  Pedro Soto    → Caja + Créditos (Sucursal Norte    V-01)
--  Laura Vega    → Consulta        (Sucursal Norte    V-02)
--  Superadmin    → igual que admin, contraseña fácil
-- ─────────────────────────────────────────────────────────────
MERGE [dbo].[Usuario] AS tgt
USING (VALUES
    ('AnaM',       'ana.martinez@banco.com',  '1234', @ROL_AGENTE),
    ('PedroS',     'pedro.soto@banco.com',    '1234', @ROL_AGENTE),
    ('LauraV',     'laura.vega@banco.com',    '1234', @ROL_AGENTE),
    ('superadmin', 'superadmin@banco.com',    'super123', @ROL_ADMIN)
) AS src (Nombre, Email, PasswordHash, RolId)
ON tgt.Email = src.Email AND tgt.Eliminado = 0
WHEN NOT MATCHED THEN
    INSERT (UsuarioId, Nombre, Email, PasswordHash, RolId,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), src.Nombre, src.Email, src.PasswordHash, src.RolId,
            @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, PasswordHash = src.PasswordHash, RolId = src.RolId;

-- ─────────────────────────────────────────────────────────────
-- 3. EMPLEADOS vinculados a los nuevos usuarios agente
-- ─────────────────────────────────────────────────────────────
DECLARE @USR_ANA   uniqueidentifier;
DECLARE @USR_PEDRO uniqueidentifier;
DECLARE @USR_LAURA uniqueidentifier;

SELECT @USR_ANA   = UsuarioId FROM [dbo].[Usuario] WHERE Email = 'ana.martinez@banco.com' AND Eliminado = 0;
SELECT @USR_PEDRO = UsuarioId FROM [dbo].[Usuario] WHERE Email = 'pedro.soto@banco.com'   AND Eliminado = 0;
SELECT @USR_LAURA = UsuarioId FROM [dbo].[Usuario] WHERE Email = 'laura.vega@banco.com'   AND Eliminado = 0;

MERGE [dbo].[Empleado] AS tgt
USING (VALUES
    ('Ana',   'Martínez', 'Especialista en Reclamos', @USR_ANA),
    ('Pedro', 'Soto',     'Cajero Sucursal Norte',    @USR_PEDRO),
    ('Laura', 'Vega',     'Consultora',               @USR_LAURA)
) AS src (Nombre, Apellido, Cargo, UsuarioId)
ON tgt.UsuarioId = src.UsuarioId AND tgt.Eliminado = 0
WHEN NOT MATCHED THEN
    INSERT (EmpleadoId, Nombre, Apellido, Cargo, UsuarioId,
            CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), src.Nombre, src.Apellido, src.Cargo, src.UsuarioId,
            @ZERO, @NOW, @ZERO, @NOW, 0)
WHEN MATCHED THEN
    UPDATE SET Nombre = src.Nombre, Apellido = src.Apellido, Cargo = src.Cargo;

-- ─────────────────────────────────────────────────────────────
-- 4. ASIGNACIONES a ventanillas (solo si no tiene asignación activa)
-- ─────────────────────────────────────────────────────────────
DECLARE @EMP_ANA   uniqueidentifier;
DECLARE @EMP_PEDRO uniqueidentifier;
DECLARE @EMP_LAURA uniqueidentifier;

SELECT @EMP_ANA   = EmpleadoId FROM [dbo].[Empleado] WHERE UsuarioId = @USR_ANA   AND Eliminado = 0;
SELECT @EMP_PEDRO = EmpleadoId FROM [dbo].[Empleado] WHERE UsuarioId = @USR_PEDRO AND Eliminado = 0;
SELECT @EMP_LAURA = EmpleadoId FROM [dbo].[Empleado] WHERE UsuarioId = @USR_LAURA AND Eliminado = 0;

-- Ana → Central V-03 (Reclamos)
IF @EMP_ANA IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM [dbo].[AsignacionVentanilla]
    WHERE EmpleadoId = @EMP_ANA AND Hora_Fin IS NULL AND Eliminado = 0)
    INSERT INTO [dbo].[AsignacionVentanilla]
        (AsignacionId, Hora_Inicio, Hora_Fin, EmpleadoId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), @NOW, NULL, @EMP_ANA, @VEN1_3, @ZERO, @NOW, @ZERO, @NOW, 0);

-- Pedro → Norte V-01 (Caja + Créditos)
IF @EMP_PEDRO IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM [dbo].[AsignacionVentanilla]
    WHERE EmpleadoId = @EMP_PEDRO AND Hora_Fin IS NULL AND Eliminado = 0)
    INSERT INTO [dbo].[AsignacionVentanilla]
        (AsignacionId, Hora_Inicio, Hora_Fin, EmpleadoId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), @NOW, NULL, @EMP_PEDRO, @VEN2_1, @ZERO, @NOW, @ZERO, @NOW, 0);

-- Laura → Norte V-02 (Consulta + Reclamos)
IF @EMP_LAURA IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM [dbo].[AsignacionVentanilla]
    WHERE EmpleadoId = @EMP_LAURA AND Hora_Fin IS NULL AND Eliminado = 0)
    INSERT INTO [dbo].[AsignacionVentanilla]
        (AsignacionId, Hora_Inicio, Hora_Fin, EmpleadoId, VentanillaId,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (NEWID(), @NOW, NULL, @EMP_LAURA, @VEN2_2, @ZERO, @NOW, @ZERO, @NOW, 0);

-- ─────────────────────────────────────────────────────────────
-- 5. Abrir ventanilla V-03 Central (estaba Cerrada)
-- ─────────────────────────────────────────────────────────────
UPDATE [dbo].[Ventanilla]
SET    Estado_Ventanilla = 'Abierta', ModifiedDate = @NOW
WHERE  VentanillaId = @VEN1_3;

-- ─────────────────────────────────────────────────────────────
-- RESUMEN
-- ─────────────────────────────────────────────────────────────
PRINT '============================================';
PRINT 'PATCH completado.';
PRINT '';
PRINT '  ADMIN (acceso total)';
PRINT '  - admin       / admin123   → Panel Admin';
PRINT '  - superadmin  / super123   → Panel Admin';
PRINT '';
PRINT '  AGENTES por servicio';
PRINT '  - Leslie  / 1234     → Caja y Consulta  (Central V-01)';
PRINT '  - agente  / agente123→ Créditos e Inv.  (Central V-02)';
PRINT '  - AnaM    / 1234     → Reclamos          (Central V-03)';
PRINT '  - PedroS  / 1234     → Caja y Créditos   (Norte   V-01)';
PRINT '  - LauraV  / 1234     → Consulta y Rec.   (Norte   V-02)';
PRINT '============================================';
