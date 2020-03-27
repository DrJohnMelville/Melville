using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Melville.Linq.Statistics.FileWriter
{
  public sealed class ExcelFileWriter: IDisposable
  {
    private List<(string Name, IList<IList> Cells)> Sheets = 
      new List<(string, IList<IList>)>();
    private readonly Stream outputStream;

    public ExcelFileWriter(String outputStream) : this(File.Create(outputStream))
    {
    }

    public ExcelFileWriter(Stream outputStream)
    {
      this.outputStream = outputStream;
    }

    public void AddPage(string name, IList<IList> cells) => Sheets?.Add((name, cells));
    public void AddObjectPage<T>(string name, IEnumerable<T> cells) => 
      AddPage(name, ObjectTableFormatter.Dump(cells).ToList());

    public void Save()
    {
      if (Sheets == null) return;
      using (var pack = new ExcelPackage(outputStream))
      {
        foreach (var sheet in Sheets)
        {
          var ws = pack.Workbook.Worksheets.Add(sheet.Name);
          for (int i = 0; i < sheet.Cells.Count; i++)
          {
            for (int j = 0; j < sheet.Cells[i].Count; j++)
            {
              ws.Cells[i + 1, j + 1].Value = sheet.Cells[i][j];
            }
          }
       }
       pack.Save();
      }
        Sheets = null;
    }

    void IDisposable.Dispose()
    {
      Save();
      outputStream.Dispose();
    }
  }
}