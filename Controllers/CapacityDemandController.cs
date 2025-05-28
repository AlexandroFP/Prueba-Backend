using Microsoft.AspNetCore.Mvc;
using Cenace.API.Application.Services;
using Cenace.API.Domain.Entities;
using Cenace.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Cenace.API.Domain.Dtos;

namespace Cenace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CapacityDemandController : ControllerBase
    {
        private readonly ExcelImportService _excelImportService;
        private readonly ExcelDownloadService _excelDownloadService;
        private readonly ApplicationDbContext _context;

        public CapacityDemandController(ExcelImportService excelImportService, ExcelDownloadService excelDownloadService, ApplicationDbContext context)
        {
            _excelImportService = excelImportService;
            _excelDownloadService = excelDownloadService;
            _context = context;
        }

        // GET: api/CapacityDemand
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CapacityDemand>>>> GetCapacityDemands()
        {
            try
            {
                var demands = await _context.CapacityDemands.ToListAsync();

                if (demands == null || !demands.Any())
                {
                    return NotFound(new ApiResponse<IEnumerable<CapacityDemand>>
                    {
                        Success = false,
                        Message = "No se encontraron datos.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<IEnumerable<CapacityDemand>>
                {
                    Success = true,
                    Message = "Datos obtenidos exitosamente.",
                    Data = demands
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CapacityDemand>>
                {
                    Success = false,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                });
            }
        }

        // DELETE: api/CapacityDemand/delete-all
        [HttpDelete("delete-all")]
        public async Task<ActionResult<string>> DeleteAllCapacityDemands()
        {
            try
            {
                var allDemands = await _context.CapacityDemands.ToListAsync();
                _context.CapacityDemands.RemoveRange(allDemands);
                await _context.SaveChangesAsync();
                return Ok("Todos los registros fueron eliminados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar los datos: {ex.Message}");
            }
        }

        // POST: api/CapacityDemand/download-data
        [HttpPost("download-data")]
        public async Task<IActionResult> DownloadData()
        {
            try
            {
                var downloader = HttpContext.RequestServices.GetRequiredService<ExcelDownloadService>();
                await downloader.DownloadExcelsAsync();

                var importer = HttpContext.RequestServices.GetRequiredService<ExcelImportService>();
                var folderPath = downloader.GetDownloadPath();
                await importer.ImportCapacityDemandFromExcelFolder(folderPath);

                return Ok("Los datos fueron descargados e importados exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al refrescar datos: {ex.Message}");
            }
        }

    }
}
