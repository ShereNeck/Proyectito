-- ============================================================
--  FIX: Reemplaza CreateBy / ModifiedBy NULL → Guid.Empty
--  Ejecutar conectado a la BD 'proyecto'
-- ============================================================
SET NOCOUNT ON;

DECLARE @ZERO uniqueidentifier = '00000000-0000-0000-0000-000000000000';

UPDATE [dbo].[Rol]                  SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Rol]                  SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Usuario]              SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Usuario]              SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Empleado]             SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Empleado]             SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Sucursal]             SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Sucursal]             SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Servicio]             SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Servicio]             SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Prioridad]            SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Prioridad]            SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Ventanilla]           SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Ventanilla]           SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[VentanillaServicio]   SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[VentanillaServicio]   SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[AsignacionVentanilla] SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[AsignacionVentanilla] SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Cliente]              SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Cliente]              SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Cola]                 SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Cola]                 SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Ticket]               SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Ticket]               SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[Modulo]               SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[Modulo]               SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[ModulosAgrupados]     SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[ModulosAgrupados]     SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;
UPDATE [dbo].[ModulosRoles]         SET CreateBy  = @ZERO WHERE CreateBy  IS NULL;
UPDATE [dbo].[ModulosRoles]         SET ModifiedBy = @ZERO WHERE ModifiedBy IS NULL;

PRINT 'Todos los NULL en CreateBy/ModifiedBy reemplazados por Guid.Empty.';
