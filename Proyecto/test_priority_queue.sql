-- ============================================================
--  TEST: Priority queue correctness
--  Agent: agente / agente123  (Central V-02 → Créditos)
--  Expected call order when pressing "Siguiente" 6 times:
--    1. CR-VIP-1   (VIP,  emitted 30 min ago)
--    2. CR-VIP-2   (VIP,  emitted 10 min ago)
--    3. CR-PREF-1  (Pref, emitted 25 min ago)
--    4. CR-PREF-2  (Pref, emitted  8 min ago)
--    5. CR-NORM-1  (Norm, emitted 20 min ago)
--    6. CR-NORM-2  (Norm, emitted  5 min ago)
-- ============================================================
USE proyecto;
SET NOCOUNT ON;

DECLARE @NOW  datetime2        = GETUTCDATE();
DECLARE @ZERO uniqueidentifier = '00000000-0000-0000-0000-000000000000';

-- Fixed IDs (from seed.sql)
DECLARE @SRV_CREDITO  uniqueidentifier = 'E0000002-0000-0000-0000-000000000002';
DECLARE @SUC1         uniqueidentifier = 'D0000001-0000-0000-0000-000000000001';
DECLARE @PRI_NORMAL   uniqueidentifier = 'F0000001-0000-0000-0000-000000000001';
DECLARE @PRI_PREFER   uniqueidentifier = 'F0000002-0000-0000-0000-000000000002';
DECLARE @PRI_VIP      uniqueidentifier = 'F0000003-0000-0000-0000-000000000003';

-- Guest client (created by TicketService when no real client exists)
DECLARE @CLI_GUEST uniqueidentifier;
SELECT @CLI_GUEST = ClienteId FROM [dbo].[Cliente] WHERE DNI = '00000000' AND Eliminado = 0;

IF @CLI_GUEST IS NULL
BEGIN
    SET @CLI_GUEST = NEWID();
    INSERT INTO [dbo].[Cliente]
        (ClienteId, DNI, Nombre_Cliente, Apellido_Cliente, Fecha_Nacimiento, Estado,
         CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
    VALUES (@CLI_GUEST, '00000000', 'Invitado', 'Invitado', '1990-01-01', 'Activo',
            @ZERO, @NOW, @ZERO, @NOW, 0);
END

-- ── Step 1: clean existing test tickets ────────────────────
DELETE FROM [dbo].[Ticket]
WHERE Numero_Ticket IN ('CR-VIP-1','CR-VIP-2','CR-PREF-1','CR-PREF-2','CR-NORM-1','CR-NORM-2');

DELETE FROM [dbo].[Cola]
WHERE ColaId IN (
    SELECT c.ColaId FROM [dbo].[Cola] c
    INNER JOIN [dbo].[Cliente] cl ON cl.ClienteId = c.ClienteId
    WHERE cl.DNI = '00000000'
      AND NOT EXISTS (SELECT 1 FROM [dbo].[Ticket] t WHERE t.ColaId = c.ColaId)
);

-- ── Step 2: create Colas (one per ticket) ──────────────────
DECLARE @COLA_VIP1  uniqueidentifier = NEWID();
DECLARE @COLA_VIP2  uniqueidentifier = NEWID();
DECLARE @COLA_PRF1  uniqueidentifier = NEWID();
DECLARE @COLA_PRF2  uniqueidentifier = NEWID();
DECLARE @COLA_NRM1  uniqueidentifier = NEWID();
DECLARE @COLA_NRM2  uniqueidentifier = NEWID();

INSERT INTO [dbo].[Cola]
    (ColaId, Estado, PrioridadId, ClienteId, CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
VALUES
    (@COLA_VIP1, 'Activa', @PRI_VIP,    @CLI_GUEST, @ZERO, @NOW, @ZERO, @NOW, 0),
    (@COLA_VIP2, 'Activa', @PRI_VIP,    @CLI_GUEST, @ZERO, @NOW, @ZERO, @NOW, 0),
    (@COLA_PRF1, 'Activa', @PRI_PREFER, @CLI_GUEST, @ZERO, @NOW, @ZERO, @NOW, 0),
    (@COLA_PRF2, 'Activa', @PRI_PREFER, @CLI_GUEST, @ZERO, @NOW, @ZERO, @NOW, 0),
    (@COLA_NRM1, 'Activa', @PRI_NORMAL, @CLI_GUEST, @ZERO, @NOW, @ZERO, @NOW, 0),
    (@COLA_NRM2, 'Activa', @PRI_NORMAL, @CLI_GUEST, @ZERO, @NOW, @ZERO, @NOW, 0);

-- ── Step 3: insert tickets with staggered emission times ───
INSERT INTO [dbo].[Ticket]
    (TicketId, Numero_Ticket, Hora_Emision, Hora_Atencion, Hora_Finalizacion,
     Estado_Ticket, Posicion, ColaId, ServicioId, SucursalId, VentanillaId,
     CreateBy, CreateDate, ModifiedBy, ModifiedDate, Eliminado)
VALUES
    (NEWID(), 'CR-VIP-1',  DATEADD(MINUTE,-30,@NOW), NULL, NULL, 'En espera', 1, @COLA_VIP1, @SRV_CREDITO, @SUC1, NULL, @ZERO,@NOW,@ZERO,@NOW,0),
    (NEWID(), 'CR-VIP-2',  DATEADD(MINUTE,-10,@NOW), NULL, NULL, 'En espera', 2, @COLA_VIP2, @SRV_CREDITO, @SUC1, NULL, @ZERO,@NOW,@ZERO,@NOW,0),
    (NEWID(), 'CR-PREF-1', DATEADD(MINUTE,-25,@NOW), NULL, NULL, 'En espera', 3, @COLA_PRF1, @SRV_CREDITO, @SUC1, NULL, @ZERO,@NOW,@ZERO,@NOW,0),
    (NEWID(), 'CR-PREF-2', DATEADD(MINUTE, -8,@NOW), NULL, NULL, 'En espera', 4, @COLA_PRF2, @SRV_CREDITO, @SUC1, NULL, @ZERO,@NOW,@ZERO,@NOW,0),
    (NEWID(), 'CR-NORM-1', DATEADD(MINUTE,-20,@NOW), NULL, NULL, 'En espera', 5, @COLA_NRM1, @SRV_CREDITO, @SUC1, NULL, @ZERO,@NOW,@ZERO,@NOW,0),
    (NEWID(), 'CR-NORM-2', DATEADD(MINUTE, -5,@NOW), NULL, NULL, 'En espera', 6, @COLA_NRM2, @SRV_CREDITO, @SUC1, NULL, @ZERO,@NOW,@ZERO,@NOW,0);

-- ── Step 4: preview expected order ─────────────────────────
SELECT
    t.Numero_Ticket,
    p.Nombre          AS Prioridad,
    p.Peso,
    t.Hora_Emision,
    t.Estado_Ticket,
    ROW_NUMBER() OVER (ORDER BY p.Peso DESC, t.Hora_Emision ASC) AS ExpectedCallOrder
FROM [dbo].[Ticket] t
INNER JOIN [dbo].[Cola]      c ON c.ColaId      = t.ColaId
INNER JOIN [dbo].[Prioridad] p ON p.PrioridadId = c.PrioridadId
WHERE t.Numero_Ticket IN ('CR-VIP-1','CR-VIP-2','CR-PREF-1','CR-PREF-2','CR-NORM-1','CR-NORM-2')
  AND t.Estado_Ticket = 'En espera'
ORDER BY p.Peso DESC, t.Hora_Emision ASC;

PRINT 'Test tickets inserted. Log in as agente/agente123 and press Siguiente 6 times.';
PRINT 'Expected order: CR-VIP-1 → CR-VIP-2 → CR-PREF-1 → CR-PREF-2 → CR-NORM-1 → CR-NORM-2';
