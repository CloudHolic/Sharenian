using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Sharenian.Models;

public class ExcelManager
{
    public string FilePath { get; set; }

    public ExcelManager(string filePath = "")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FilePath = filePath;
    }

    public async Task WriteExcel(List<Guild> guildList, IProgress<int> progress)
    {
        if (string.IsNullOrEmpty(FilePath))
            return;

        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("수로");
        worksheet.Cells[1, 1].Value = "순위";
        worksheet.Cells[1, 2].Value = "길드명";
        worksheet.Cells[1, 3].Value = "길드레벨";
        worksheet.Cells[1, 4].Value = "길드마스터";
        worksheet.Cells[1, 5].Value = "점수";
        worksheet.Cells[1, 6].Value = "노블포인트";
        
        var count = guildList.Count;
        await Task.Run(() => guildList.ForEach(x =>
            {
                worksheet.Cells[x.Order + 1, 1].Value = x.Order;
                worksheet.Cells[x.Order + 1, 2].Value = x.Name;
                worksheet.Cells[x.Order + 1, 3].Value = x.Level;
                worksheet.Cells[x.Order + 1, 4].Value = x.Master;
                worksheet.Cells[x.Order + 1, 5].Value = x.Score;
                worksheet.Cells[x.Order + 1, 6].Value = x.Point;

                progress.Report(100 * x.Order / count);
            }));

        await package.SaveAsAsync(FilePath);
    }
}