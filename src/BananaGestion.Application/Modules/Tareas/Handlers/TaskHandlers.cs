using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Application.Modules.Tareas.Commands;
using BananaGestion.Application.Modules.Tareas.DTOs;
using BananaGestion.Application.Modules.Tareas.Queries;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;
using MediatR;

namespace BananaGestion.Application.Modules.Tareas.Handlers;

public class TaskConfigHandlers :
    IRequestHandler<GetTaskConfigsQuery, IEnumerable<TaskConfigDto>>,
    IRequestHandler<GetTaskConfigByIdQuery, TaskConfigDto>,
    IRequestHandler<CreateTaskConfigCommand, TaskConfigDto>,
    IRequestHandler<UpdateTaskConfigCommand, TaskConfigDto>,
    IRequestHandler<DeleteTaskConfigCommand, bool>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IRepository<Product> _productRepository;

    public TaskConfigHandlers(ITaskRepository taskRepository, IRepository<Product> productRepository)
    {
        _taskRepository = taskRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<TaskConfigDto>> Handle(GetTaskConfigsQuery request, CancellationToken cancellationToken)
    {
        var configs = await _taskRepository.GetAllAsync();
        return configs.Select(MapConfigToDto);
    }

    public async Task<TaskConfigDto> Handle(GetTaskConfigByIdQuery request, CancellationToken cancellationToken)
    {
        var config = await _taskRepository.GetByIdAsync(request.Id);
        if (config == null)
            throw new InvalidOperationException("Configuración no encontrada");
        return MapConfigToDto(config);
    }

    public async Task<TaskConfigDto> Handle(CreateTaskConfigCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<LaborType>(request.Request.TipoLabor, out var laborType))
            throw new InvalidOperationException("Tipo de labor inválido");

        if (!Enum.TryParse<TaskType>(request.Request.Tipo, out var taskType))
            throw new InvalidOperationException("Tipo de tarea inválido");

        var config = new TaskConfig
        {
            Nombre = request.Request.Nombre,
            Descripcion = request.Request.Descripcion,
            TipoLabor = laborType,
            Tipo = taskType,
            FrecuenciaDias = request.Request.FrecuenciaDias,
            InsumoCantidad = request.Request.InsumoCantidad,
            InsumoId = request.Request.InsumoId,
            RequiereFoto = request.Request.RequiereFoto,
            RequiereFirma = request.Request.RequiereFirma,
            RequiereGps = request.Request.RequiereGps
        };

        await _taskRepository.AddAsync(config);
        return MapConfigToDto(config);
    }

    public async Task<TaskConfigDto> Handle(UpdateTaskConfigCommand request, CancellationToken cancellationToken)
    {
        var config = await _taskRepository.GetByIdAsync(request.Id);
        if (config == null)
            throw new InvalidOperationException("Configuración no encontrada");

        if (!string.IsNullOrEmpty(request.Request.Nombre))
            config.Nombre = request.Request.Nombre;
        if (request.Request.Descripcion != null)
            config.Descripcion = request.Request.Descripcion;
        if (!string.IsNullOrEmpty(request.Request.TipoLabor) && 
            Enum.TryParse<LaborType>(request.Request.TipoLabor, out var laborType))
            config.TipoLabor = laborType;
        if (!string.IsNullOrEmpty(request.Request.Tipo) && 
            Enum.TryParse<TaskType>(request.Request.Tipo, out var taskType))
            config.Tipo = taskType;
        if (request.Request.FrecuenciaDias.HasValue)
            config.FrecuenciaDias = request.Request.FrecuenciaDias;
        if (request.Request.InsumoCantidad.HasValue)
            config.InsumoCantidad = request.Request.InsumoCantidad;
        if (request.Request.InsumoId.HasValue)
            config.InsumoId = request.Request.InsumoId;
        if (request.Request.RequiereFoto.HasValue)
            config.RequiereFoto = request.Request.RequiereFoto.Value;
        if (request.Request.RequiereFirma.HasValue)
            config.RequiereFirma = request.Request.RequiereFirma.Value;
        if (request.Request.RequiereGps.HasValue)
            config.RequiereGps = request.Request.RequiereGps.Value;
        if (request.Request.Activo.HasValue)
            config.Activo = request.Request.Activo.Value;

        await _taskRepository.UpdateAsync(config);
        return MapConfigToDto(config);
    }

    public async Task<bool> Handle(DeleteTaskConfigCommand request, CancellationToken cancellationToken)
    {
        await _taskRepository.DeleteAsync(request.Id);
        return true;
    }

    private TaskConfigDto MapConfigToDto(TaskConfig config) => new(
        config.Id, config.Nombre, config.Descripcion, config.TipoLabor.ToString(),
        config.Tipo.ToString(), config.FrecuenciaDias, config.InsumoCantidad,
        config.InsumoId, config.Insumo?.Nombre, config.RequiereFoto,
        config.RequiereFirma, config.RequiereGps, config.Activo
    );
}

public class TaskAssignmentHandlers :
    IRequestHandler<GetPendingAssignmentsQuery, IEnumerable<TaskAssignmentDto>>,
    IRequestHandler<GetAssignmentsByDateRangeQuery, IEnumerable<TaskAssignmentDto>>,
    IRequestHandler<GetAssignmentWithDetailsQuery, TaskAssignmentDto>,
    IRequestHandler<GetOverdueAssignmentsQuery, IEnumerable<TaskAssignmentDto>>,
    IRequestHandler<CreateTaskAssignmentCommand, TaskAssignmentDto>,
    IRequestHandler<UpdateAssignmentStatusCommand, bool>
{
    private readonly ITaskRepository _taskRepository;

    public TaskAssignmentHandlers(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskAssignmentDto>> Handle(GetPendingAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var assignments = await _taskRepository.GetPendingAssignmentsAsync(request.UserId);
        return assignments.Select(MapAssignmentToDto);
    }

    public async Task<IEnumerable<TaskAssignmentDto>> Handle(GetAssignmentsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var assignments = await _taskRepository.GetAssignmentsByDateRangeAsync(request.Start, request.End);
        return assignments.Select(MapAssignmentToDto);
    }

    public async Task<TaskAssignmentDto> Handle(GetAssignmentWithDetailsQuery request, CancellationToken cancellationToken)
    {
        var assignment = await _taskRepository.GetAssignmentWithDetailsAsync(request.Id);
        if (assignment == null)
            throw new InvalidOperationException("Asignación no encontrada");
        return MapAssignmentToDto(assignment);
    }

    public async Task<IEnumerable<TaskAssignmentDto>> Handle(GetOverdueAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var assignments = await _taskRepository.GetOverdueAssignmentsAsync();
        return assignments.Select(MapAssignmentToDto);
    }

    public async Task<TaskAssignmentDto> Handle(CreateTaskAssignmentCommand request, CancellationToken cancellationToken)
    {
        var assignment = new TaskAssignment
        {
            TaskConfigId = request.Request.TaskConfigId,
            UserId = request.Request.UserId,
            LoteId = request.Request.LoteId,
            FechaProgramada = request.Request.FechaProgramada,
            Notas = request.Request.Notas
        };

        await _taskRepository.CreateAssignmentAsync(assignment);
        
        var fullAssignment = await _taskRepository.GetAssignmentWithDetailsAsync(assignment.Id);
        return MapAssignmentToDto(fullAssignment!);
    }

    public async Task<bool> Handle(UpdateAssignmentStatusCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<AssignmentStatus>(request.Status, out var status))
            throw new InvalidOperationException("Estado inválido");

        await _taskRepository.UpdateAssignmentStatusAsync(request.Id, status);
        return true;
    }

    private TaskAssignmentDto MapAssignmentToDto(TaskAssignment a) => new(
        a.Id, a.TaskConfigId, a.TaskConfig.Nombre, a.UserId,
        $"{a.User.Nombre} {a.User.Apellido}", a.LoteId, a.Lote.Nombre,
        a.FechaProgramada, a.FechaCompletada, a.Status.ToString(), a.Notas
    );
}

public class TaskLogHandlers :
    IRequestHandler<GetTaskLogQuery, TaskLogDto?>,
    IRequestHandler<CreateTaskLogCommand, TaskLogDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IBlobStorageService _blobService;
    private readonly IEmailService _emailService;

    public TaskLogHandlers(
        ITaskRepository taskRepository,
        IInventoryRepository inventoryRepository,
        IBlobStorageService blobService,
        IEmailService emailService)
    {
        _taskRepository = taskRepository;
        _inventoryRepository = inventoryRepository;
        _blobService = blobService;
        _emailService = emailService;
    }

    public async Task<TaskLogDto?> Handle(GetTaskLogQuery request, CancellationToken cancellationToken)
    {
        var log = await _taskRepository.GetTaskLogAsync(request.AssignmentId);
        if (log == null) return null;
        return MapLogToDto(log);
    }

    public async Task<TaskLogDto> Handle(CreateTaskLogCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _taskRepository.GetAssignmentWithDetailsAsync(request.Request.TaskAssignmentId);
        if (assignment == null)
            throw new InvalidOperationException("Asignación no encontrada");

        string? fotoUrl = null;
        string? firmaUrl = null;

        if (request.Foto != null)
        {
            fotoUrl = await _blobService.UploadAndConvertToTifAsync(request.Foto, "foto.jpg");
        }

        if (request.Firma != null)
        {
            firmaUrl = await _blobService.UploadFileAsync(request.Firma, "firma.png", "image/png");
        }

        var log = new TaskLog
        {
            TaskAssignmentId = request.Request.TaskAssignmentId,
            FotoUrl = fotoUrl,
            FirmaUrl = firmaUrl,
            Latitud = request.Request.Latitud,
            Longitud = request.Request.Longitud,
            Observaciones = request.Request.Observaciones,
            CantidadInsumoUtilizado = request.Request.CantidadInsumoUtilizado
        };

        await _taskRepository.CreateTaskLogAsync(log);

        if (assignment.TaskConfig.TipoLabor == LaborType.Fumigacion || 
            assignment.TaskConfig.TipoLabor == LaborType.Fertilizacion)
        {
            if (assignment.TaskConfig.InsumoId.HasValue && request.Request.CantidadInsumoUtilizado.HasValue)
            {
                var movement = new InventoryMovement
                {
                    ProductId = assignment.TaskConfig.InsumoId.Value,
                    LoteId = assignment.LoteId,
                    Tipo = "Salida",
                    Cantidad = request.Request.CantidadInsumoUtilizado.Value,
                    Referencia = $"Tarea: {assignment.TaskConfig.Nombre}",
                    Notas = $"Aplicado en lote {assignment.Lote.Nombre}"
                };
                await _inventoryRepository.CreateMovementAsync(movement);

                var product = await _inventoryRepository.GetProductByIdAsync(assignment.TaskConfig.InsumoId.Value);
                if (product != null && product.StockActual <= product.StockMinimo)
                {
                    var admins = await _taskRepository.GetPendingAssignmentsAsync();
                    foreach (var admin in admins.Where(a => a.User.Rol == UserRole.Administrador))
                    {
                        await _emailService.SendLowStockAlertAsync(
                            admin.User.Email, product.Nombre, product.StockActual, product.StockMinimo);
                    }
                }
            }
        }

        await _taskRepository.UpdateAssignmentStatusAsync(assignment.Id, AssignmentStatus.Completada);

        return MapLogToDto(log);
    }

    private TaskLogDto MapLogToDto(TaskLog log) => new(
        log.Id, log.TaskAssignmentId, log.FechaRegistro, log.FotoUrl, log.FirmaUrl,
        log.Latitud, log.Longitud, log.Observaciones, log.CantidadInsumoUtilizado
    );
}
