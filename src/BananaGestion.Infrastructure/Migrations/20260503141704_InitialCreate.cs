using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BananaGestion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "box_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CapacidadKilos = table.Column<int>(type: "integer", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_box_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "financial_transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReferenciaId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenciaTipo = table.Column<string>(type: "text", nullable: true),
                    ComprobanteUrl = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "harvest_calendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Semana = table.Column<int>(type: "integer", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    ColorCinta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    color_nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_harvest_calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Hectareas = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: true),
                    Latitud = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitud = table.Column<decimal>(type: "numeric", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Unidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StockMinimo = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    StockActual = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Rol = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "harvest_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroOrden = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HarvestCalendarId = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaEmbarque = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PdfUrl = table.Column<string>(type: "text", nullable: true),
                    Cliente = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    Procesada = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_harvest_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_harvest_orders_harvest_calendars_HarvestCalendarId",
                        column: x => x.HarvestCalendarId,
                        principalTable: "harvest_calendars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "task_configs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    TipoLabor = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    FrecuenciaDias = table.Column<int>(type: "integer", nullable: true),
                    InsumoCantidad = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    InsumoId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequiereFoto = table.Column<bool>(type: "boolean", nullable: false),
                    RequiereFirma = table.Column<bool>(type: "boolean", nullable: false),
                    RequiereGps = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_configs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_configs_products_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_task_configs_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "harvest_cosechas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HarvestCalendarId = table.Column<Guid>(type: "uuid", nullable: true),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SemanaEncinte = table.Column<int>(type: "integer", nullable: false),
                    AnoEncinte = table.Column<int>(type: "integer", nullable: false),
                    SemanaCosecha = table.Column<int>(type: "integer", nullable: false),
                    AnoCosecha = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    CantidadRacimos = table.Column<int>(type: "integer", nullable: false),
                    ColorCinta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_harvest_cosechas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_harvest_cosechas_harvest_calendars_HarvestCalendarId",
                        column: x => x.HarvestCalendarId,
                        principalTable: "harvest_calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_harvest_cosechas_lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_harvest_cosechas_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "harvest_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HarvestCalendarId = table.Column<Guid>(type: "uuid", nullable: true),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CantidadRacimosEmbolsados = table.Column<int>(type: "integer", nullable: false),
                    RacimosPerdidos = table.Column<int>(type: "integer", nullable: true),
                    ColorCinta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    semana_encinte = table.Column<int>(type: "integer", nullable: false),
                    ano_encinte = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_harvest_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_harvest_records_harvest_calendars_HarvestCalendarId",
                        column: x => x.HarvestCalendarId,
                        principalTable: "harvest_calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_harvest_records_lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_harvest_records_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventory_movements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    StockAnterior = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    StockNuevo = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Referencia = table.Column<string>(type: "text", nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_movements_lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_inventory_movements_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_movements_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Mensaje = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Leida = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaLeida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notifications_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_box_details",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HarvestOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CantidadPlanificada = table.Column<int>(type: "integer", nullable: false),
                    CantidadEmpacada = table.Column<int>(type: "integer", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_box_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_box_details_box_types_BoxTypeId",
                        column: x => x.BoxTypeId,
                        principalTable: "box_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_box_details_harvest_orders_HarvestOrderId",
                        column: x => x.HarvestOrderId,
                        principalTable: "harvest_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskConfigId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaProgramada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCompletada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_assignments_lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_assignments_task_configs_TaskConfigId",
                        column: x => x.TaskConfigId,
                        principalTable: "task_configs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_assignments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "harvest_box_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HarvestRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    PesoTotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_harvest_box_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_harvest_box_records_box_types_BoxTypeId",
                        column: x => x.BoxTypeId,
                        principalTable: "box_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_harvest_box_records_harvest_records_HarvestRecordId",
                        column: x => x.HarvestRecordId,
                        principalTable: "harvest_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskAssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FotoUrl = table.Column<string>(type: "text", nullable: true),
                    FirmaUrl = table.Column<string>(type: "text", nullable: true),
                    Latitud = table.Column<decimal>(type: "numeric(10,8)", precision: 10, scale: 8, nullable: true),
                    Longitud = table.Column<decimal>(type: "numeric(11,8)", precision: 11, scale: 8, nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    CantidadInsumoUtilizado = table.Column<int>(type: "integer", precision: 10, scale: 2, nullable: true),
                    TaskConfigId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_logs_task_assignments_TaskAssignmentId",
                        column: x => x.TaskAssignmentId,
                        principalTable: "task_assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_logs_task_configs_TaskConfigId",
                        column: x => x.TaskConfigId,
                        principalTable: "task_configs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_box_types_Codigo",
                table: "box_types",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_harvest_box_records_BoxTypeId",
                table: "harvest_box_records",
                column: "BoxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_box_records_HarvestRecordId",
                table: "harvest_box_records",
                column: "HarvestRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_calendars_Semana_Ano",
                table: "harvest_calendars",
                columns: new[] { "Semana", "Ano" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_harvest_cosechas_HarvestCalendarId",
                table: "harvest_cosechas",
                column: "HarvestCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_cosechas_LoteId",
                table: "harvest_cosechas",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_cosechas_UserId",
                table: "harvest_cosechas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_orders_HarvestCalendarId",
                table: "harvest_orders",
                column: "HarvestCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_orders_NumeroOrden",
                table: "harvest_orders",
                column: "NumeroOrden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_harvest_records_HarvestCalendarId",
                table: "harvest_records",
                column: "HarvestCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_records_LoteId",
                table: "harvest_records",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_harvest_records_UserId",
                table: "harvest_records",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_LoteId",
                table: "inventory_movements",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_ProductId",
                table: "inventory_movements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_UserId",
                table: "inventory_movements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_lotes_Codigo",
                table: "lotes",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_UserId",
                table: "notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_order_box_details_BoxTypeId",
                table: "order_box_details",
                column: "BoxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_order_box_details_HarvestOrderId",
                table: "order_box_details",
                column: "HarvestOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_products_Codigo",
                table: "products",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_assignments_LoteId",
                table: "task_assignments",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_task_assignments_TaskConfigId",
                table: "task_assignments",
                column: "TaskConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_task_assignments_UserId",
                table: "task_assignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_task_configs_InsumoId",
                table: "task_configs",
                column: "InsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_task_configs_ProductId",
                table: "task_configs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_task_logs_TaskAssignmentId",
                table: "task_logs",
                column: "TaskAssignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_logs_TaskConfigId",
                table: "task_logs",
                column: "TaskConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "financial_transactions");

            migrationBuilder.DropTable(
                name: "harvest_box_records");

            migrationBuilder.DropTable(
                name: "harvest_cosechas");

            migrationBuilder.DropTable(
                name: "inventory_movements");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "order_box_details");

            migrationBuilder.DropTable(
                name: "task_logs");

            migrationBuilder.DropTable(
                name: "harvest_records");

            migrationBuilder.DropTable(
                name: "box_types");

            migrationBuilder.DropTable(
                name: "harvest_orders");

            migrationBuilder.DropTable(
                name: "task_assignments");

            migrationBuilder.DropTable(
                name: "harvest_calendars");

            migrationBuilder.DropTable(
                name: "lotes");

            migrationBuilder.DropTable(
                name: "task_configs");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
