using Cenace.API.Infrastructure.Data;
using Cenace.API.Domain.Entities;
using ClosedXML.Excel;

namespace Cenace.API.Application.Services
{
    public class ExcelImportService
    {
        private readonly ApplicationDbContext _context;

        public ExcelImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ImportCapacityDemandFromExcelFolder(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.xlsx");

            foreach (var file in files)
            {
                using var workbook = new XLWorkbook(file);
                var worksheet = workbook.Worksheets.First();

                for (int row = 2; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    try
                    {
                        var demand = new CapacityDemand
                        {
                            Zone = worksheet.Cell(row, 1).GetString(),
                            Participant = worksheet.Cell(row, 2).GetString(),
                            Subaccount = worksheet.Cell(row, 3).GetString(),
                            DemandCapacityMW = worksheet.Cell(row, 4).GetDouble(),
                            AnnualPowerRequirementMW = worksheet.Cell(row, 5).GetDouble(),
                            EfficientAnnualRequirementMW = worksheet.Cell(row, 6).GetDouble(),
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.CapacityDemands.Add(demand);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading row {row} in file {file}: {ex.Message}");
                        continue;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
