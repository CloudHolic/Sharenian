﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Sharenian.Models;

public class ExcelManager
{
    public string FilePath { get; set; }

    public ExcelManager(string filePath = "")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FilePath = filePath;
    }

    public async Task WriteGuildToExcel(List<GuildInfo> guildList, IProgress<int> progress)
    {
        if (string.IsNullOrEmpty(FilePath))
            return;

        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("수로");
        worksheet.Cells[1, 1].Value = "순위";
        worksheet.Cells[1, 2].Value = "길드명";
        worksheet.Cells[1, 3].Value = "길드레벨";
        worksheet.Cells[1, 4].Value = "길드마스터";
        worksheet.Cells[1, 5].Value = "길드원 수";
        worksheet.Cells[1, 6].Value = "점수";
        worksheet.Cells[1, 7].Value = "노블포인트";
        
        var count = guildList.Count;
        await Task.Run(() => guildList.ForEach(x =>
            {
                worksheet.Cells[x.Order + 1, 1].Value = x.Order;
                worksheet.Cells[x.Order + 1, 2].Value = x.Name;
                worksheet.Cells[x.Order + 1, 3].Value = x.Level;
                worksheet.Cells[x.Order + 1, 4].Value = x.Master;
                worksheet.Cells[x.Order + 1, 5].Value = x.MemberCount;
                worksheet.Cells[x.Order + 1, 6].Value = x.Score;
                worksheet.Cells[x.Order + 1, 7].Value = x.Point;

                worksheet.Cells[x.Order + 1, 5].Style.Numberformat.Format = "#,##";

                progress.Report(1000 * x.Order / count);
            }));
        
        worksheet.Cells.AutoFitColumns();
        worksheet.Column(5).Width = 15;
        worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Cells[2, 6, count + 1, 6].Style.Font.Color.SetColor(Color.Red);

        await package.SaveAsAsync(FilePath);
    }

    public async Task WriteUserToExcel(List<UserInfo> userList, IProgress<int> progress)
    {
        if (string.IsNullOrEmpty(FilePath))
            return;

        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("무릉");
        worksheet.Cells[1, 1].Value = "닉네임";
        worksheet.Cells[1, 2].Value = "직업";
        worksheet.Cells[1, 3].Value = "레벨";
        worksheet.Cells[1, 4].Value = "경험치";

        var count = userList.Count;
        await Task.Run(() => userList.Select((x, i) => new
            {
                Item = x,
                Index = i
            })
            .ToList()
            .ForEach(x =>
            {
                var (user, index) = (x.Item, x.Index);

                worksheet.Cells[index + 2, 1].Value = user.NickName;
                worksheet.Cells[index + 2, 2].Value = user.Job;
                worksheet.Cells[index + 2, 3].Value = $"Lv.{user.Level}";
                worksheet.Cells[index + 2, 4].Value = user.ExpRate;

                progress.Report(1000 * index / count);
            }));

        worksheet.Cells.AutoFitColumns();
        worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        await package.SaveAsAsync(FilePath);
    }
}