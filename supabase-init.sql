CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE box_types (
    "Id" uuid NOT NULL,
    "Codigo" character varying(50) NOT NULL,
    "Descripcion" character varying(500) NOT NULL,
    "CapacidadKilos" integer,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_box_types" PRIMARY KEY ("Id")
);

CREATE TABLE financial_transactions (
    "Id" uuid NOT NULL,
    "Tipo" character varying(20) NOT NULL,
    "Categoria" character varying(100) NOT NULL,
    "Monto" numeric(12,2) NOT NULL,
    "Fecha" timestamp with time zone NOT NULL,
    "Descripcion" character varying(500),
    "ReferenciaId" uuid,
    "ReferenciaTipo" text,
    "ComprobanteUrl" text,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_financial_transactions" PRIMARY KEY ("Id")
);

CREATE TABLE harvest_calendars (
    "Id" uuid NOT NULL,
    "Semana" integer NOT NULL,
    "Ano" integer NOT NULL,
    "ColorCinta" character varying(50) NOT NULL,
    color_nombre character varying(50),
    "FechaInicio" timestamp with time zone NOT NULL,
    "FechaFin" timestamp with time zone NOT NULL,
    "Activo" boolean NOT NULL,
    CONSTRAINT "PK_harvest_calendars" PRIMARY KEY ("Id")
);

CREATE TABLE lotes (
    "Id" uuid NOT NULL,
    "Codigo" character varying(50) NOT NULL,
    "Nombre" character varying(200) NOT NULL,
    "Hectareas" numeric(10,4) NOT NULL,
    "Ubicacion" text,
    "Latitud" numeric,
    "Longitud" numeric,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    "Notas" text,
    CONSTRAINT "PK_lotes" PRIMARY KEY ("Id")
);

CREATE TABLE products (
    "Id" uuid NOT NULL,
    "Codigo" character varying(50) NOT NULL,
    "Nombre" character varying(200) NOT NULL,
    "Descripcion" text,
    "Unidad" character varying(50) NOT NULL,
    "StockMinimo" numeric(10,2) NOT NULL,
    "StockActual" numeric(10,2) NOT NULL,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_products" PRIMARY KEY ("Id")
);

CREATE TABLE users (
    "Id" uuid NOT NULL,
    "Email" character varying(256) NOT NULL,
    "PasswordHash" text NOT NULL,
    "Nombre" character varying(100) NOT NULL,
    "Apellido" character varying(100) NOT NULL,
    "Telefono" character varying(20),
    "Rol" integer NOT NULL,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    "UltimoLogin" timestamp with time zone,
    CONSTRAINT "PK_users" PRIMARY KEY ("Id")
);

CREATE TABLE harvest_orders (
    "Id" uuid NOT NULL,
    "NumeroOrden" character varying(100) NOT NULL,
    "HarvestCalendarId" uuid,
    "FechaEmbarque" timestamp with time zone NOT NULL,
    "PdfUrl" text,
    "Cliente" character varying(200),
    "Notas" text,
    "Procesada" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_harvest_orders" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_harvest_orders_harvest_calendars_HarvestCalendarId" FOREIGN KEY ("HarvestCalendarId") REFERENCES harvest_calendars ("Id")
);

CREATE TABLE task_configs (
    "Id" uuid NOT NULL,
    "Nombre" character varying(200) NOT NULL,
    "Descripcion" text,
    "TipoLabor" integer NOT NULL,
    "Tipo" integer NOT NULL,
    "FrecuenciaDias" integer,
    "InsumoCantidad" numeric(10,2),
    "InsumoId" uuid,
    "RequiereFoto" boolean NOT NULL,
    "RequiereFirma" boolean NOT NULL,
    "RequiereGps" boolean NOT NULL,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    "ProductId" uuid,
    CONSTRAINT "PK_task_configs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_task_configs_products_InsumoId" FOREIGN KEY ("InsumoId") REFERENCES products ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_task_configs_products_ProductId" FOREIGN KEY ("ProductId") REFERENCES products ("Id")
);

CREATE TABLE harvest_cosechas (
    "Id" uuid NOT NULL,
    "HarvestCalendarId" uuid,
    "LoteId" uuid NOT NULL,
    "UserId" uuid,
    "Fecha" timestamp with time zone NOT NULL,
    "SemanaEncinte" integer NOT NULL,
    "AnoEncinte" integer NOT NULL,
    "SemanaCosecha" integer NOT NULL,
    "AnoCosecha" integer NOT NULL,
    "Estado" integer NOT NULL,
    "CantidadRacimos" integer NOT NULL,
    "ColorCinta" character varying(50) NOT NULL,
    "Notas" text,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_harvest_cosechas" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_harvest_cosechas_harvest_calendars_HarvestCalendarId" FOREIGN KEY ("HarvestCalendarId") REFERENCES harvest_calendars ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_harvest_cosechas_lotes_LoteId" FOREIGN KEY ("LoteId") REFERENCES lotes ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_harvest_cosechas_users_UserId" FOREIGN KEY ("UserId") REFERENCES users ("Id") ON DELETE SET NULL
);

CREATE TABLE harvest_records (
    "Id" uuid NOT NULL,
    "HarvestCalendarId" uuid,
    "LoteId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Fecha" timestamp with time zone NOT NULL,
    "CantidadRacimosEmbolsados" integer NOT NULL,
    "RacimosPerdidos" integer,
    "ColorCinta" character varying(50) NOT NULL,
    "Notas" text,
    "FechaCreacion" timestamp with time zone NOT NULL,
    semana_encinte integer NOT NULL,
    ano_encinte integer NOT NULL,
    CONSTRAINT "PK_harvest_records" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_harvest_records_harvest_calendars_HarvestCalendarId" FOREIGN KEY ("HarvestCalendarId") REFERENCES harvest_calendars ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_harvest_records_lotes_LoteId" FOREIGN KEY ("LoteId") REFERENCES lotes ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_harvest_records_users_UserId" FOREIGN KEY ("UserId") REFERENCES users ("Id") ON DELETE RESTRICT
);

CREATE TABLE inventory_movements (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "LoteId" uuid,
    "UserId" uuid,
    "Tipo" character varying(20) NOT NULL,
    "Cantidad" numeric(10,2) NOT NULL,
    "StockAnterior" numeric(10,2) NOT NULL,
    "StockNuevo" numeric(10,2) NOT NULL,
    "Referencia" text,
    "Notas" text,
    "Fecha" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_inventory_movements" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_inventory_movements_lotes_LoteId" FOREIGN KEY ("LoteId") REFERENCES lotes ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_inventory_movements_products_ProductId" FOREIGN KEY ("ProductId") REFERENCES products ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_inventory_movements_users_UserId" FOREIGN KEY ("UserId") REFERENCES users ("Id") ON DELETE SET NULL
);

CREATE TABLE notifications (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Titulo" character varying(200) NOT NULL,
    "Mensaje" text NOT NULL,
    "Tipo" character varying(50) NOT NULL,
    "Leida" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    "FechaLeida" timestamp with time zone,
    CONSTRAINT "PK_notifications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_notifications_users_UserId" FOREIGN KEY ("UserId") REFERENCES users ("Id") ON DELETE CASCADE
);

CREATE TABLE order_box_details (
    "Id" uuid NOT NULL,
    "HarvestOrderId" uuid NOT NULL,
    "BoxTypeId" uuid NOT NULL,
    "CantidadPlanificada" integer NOT NULL,
    "CantidadEmpacada" integer NOT NULL,
    "Notas" text,
    CONSTRAINT "PK_order_box_details" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_order_box_details_box_types_BoxTypeId" FOREIGN KEY ("BoxTypeId") REFERENCES box_types ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_order_box_details_harvest_orders_HarvestOrderId" FOREIGN KEY ("HarvestOrderId") REFERENCES harvest_orders ("Id") ON DELETE RESTRICT
);

CREATE TABLE task_assignments (
    "Id" uuid NOT NULL,
    "TaskConfigId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "LoteId" uuid NOT NULL,
    "FechaProgramada" timestamp with time zone NOT NULL,
    "FechaCompletada" timestamp with time zone,
    "Status" integer NOT NULL,
    "Notas" text,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_task_assignments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_task_assignments_lotes_LoteId" FOREIGN KEY ("LoteId") REFERENCES lotes ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_task_assignments_task_configs_TaskConfigId" FOREIGN KEY ("TaskConfigId") REFERENCES task_configs ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_task_assignments_users_UserId" FOREIGN KEY ("UserId") REFERENCES users ("Id") ON DELETE RESTRICT
);

CREATE TABLE harvest_box_records (
    "Id" uuid NOT NULL,
    "HarvestRecordId" uuid NOT NULL,
    "BoxTypeId" uuid NOT NULL,
    "Cantidad" integer NOT NULL,
    "PesoTotal" numeric(10,2),
    "Notas" text,
    CONSTRAINT "PK_harvest_box_records" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_harvest_box_records_box_types_BoxTypeId" FOREIGN KEY ("BoxTypeId") REFERENCES box_types ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_harvest_box_records_harvest_records_HarvestRecordId" FOREIGN KEY ("HarvestRecordId") REFERENCES harvest_records ("Id") ON DELETE RESTRICT
);

CREATE TABLE task_logs (
    "Id" uuid NOT NULL,
    "TaskAssignmentId" uuid NOT NULL,
    "FechaRegistro" timestamp with time zone NOT NULL,
    "FotoUrl" text,
    "FirmaUrl" text,
    "Latitud" numeric(10,8),
    "Longitud" numeric(11,8),
    "Observaciones" text,
    "CantidadInsumoUtilizado" integer,
    "TaskConfigId" uuid,
    CONSTRAINT "PK_task_logs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_task_logs_task_assignments_TaskAssignmentId" FOREIGN KEY ("TaskAssignmentId") REFERENCES task_assignments ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_task_logs_task_configs_TaskConfigId" FOREIGN KEY ("TaskConfigId") REFERENCES task_configs ("Id")
);

CREATE UNIQUE INDEX "IX_box_types_Codigo" ON box_types ("Codigo");

CREATE INDEX "IX_harvest_box_records_BoxTypeId" ON harvest_box_records ("BoxTypeId");

CREATE INDEX "IX_harvest_box_records_HarvestRecordId" ON harvest_box_records ("HarvestRecordId");

CREATE UNIQUE INDEX "IX_harvest_calendars_Semana_Ano" ON harvest_calendars ("Semana", "Ano");

CREATE INDEX "IX_harvest_cosechas_HarvestCalendarId" ON harvest_cosechas ("HarvestCalendarId");

CREATE INDEX "IX_harvest_cosechas_LoteId" ON harvest_cosechas ("LoteId");

CREATE INDEX "IX_harvest_cosechas_UserId" ON harvest_cosechas ("UserId");

CREATE INDEX "IX_harvest_orders_HarvestCalendarId" ON harvest_orders ("HarvestCalendarId");

CREATE UNIQUE INDEX "IX_harvest_orders_NumeroOrden" ON harvest_orders ("NumeroOrden");

CREATE INDEX "IX_harvest_records_HarvestCalendarId" ON harvest_records ("HarvestCalendarId");

CREATE INDEX "IX_harvest_records_LoteId" ON harvest_records ("LoteId");

CREATE INDEX "IX_harvest_records_UserId" ON harvest_records ("UserId");

CREATE INDEX "IX_inventory_movements_LoteId" ON inventory_movements ("LoteId");

CREATE INDEX "IX_inventory_movements_ProductId" ON inventory_movements ("ProductId");

CREATE INDEX "IX_inventory_movements_UserId" ON inventory_movements ("UserId");

CREATE UNIQUE INDEX "IX_lotes_Codigo" ON lotes ("Codigo");

CREATE INDEX "IX_notifications_UserId" ON notifications ("UserId");

CREATE INDEX "IX_order_box_details_BoxTypeId" ON order_box_details ("BoxTypeId");

CREATE INDEX "IX_order_box_details_HarvestOrderId" ON order_box_details ("HarvestOrderId");

CREATE UNIQUE INDEX "IX_products_Codigo" ON products ("Codigo");

CREATE INDEX "IX_task_assignments_LoteId" ON task_assignments ("LoteId");

CREATE INDEX "IX_task_assignments_TaskConfigId" ON task_assignments ("TaskConfigId");

CREATE INDEX "IX_task_assignments_UserId" ON task_assignments ("UserId");

CREATE INDEX "IX_task_configs_InsumoId" ON task_configs ("InsumoId");

CREATE INDEX "IX_task_configs_ProductId" ON task_configs ("ProductId");

CREATE UNIQUE INDEX "IX_task_logs_TaskAssignmentId" ON task_logs ("TaskAssignmentId");

CREATE INDEX "IX_task_logs_TaskConfigId" ON task_logs ("TaskConfigId");

CREATE UNIQUE INDEX "IX_users_Email" ON users ("Email");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260503141704_InitialCreate', '8.0.10');

COMMIT;

