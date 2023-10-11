using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Console
{
    internal class DailyReport : IReport
    {
        private ISalesRepository _salesRepository;

        public DailyReport(ISalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }

        public void GenerateReport(DateOnly dayDate)
        {
            // Initialize Google Sheets API
            UserCredential credential;
            using (var stream = new FileStream(@"d:\\github\\workshops\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                "user",
                    CancellationToken.None
                ).Result;
            }

            var sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleSheetExample",
            });

            // Create new spreadsheet
            var dayText = dayDate.ToString("yyyyMMdd");
            var title = $"Daily Sales Report - {dayText}";
            var spreadsheet = new Spreadsheet()
            {
                Properties = new SpreadsheetProperties()
                {
                    Title = title
                }
            };
            var spreadsheetId = sheetsService.Spreadsheets.Create(spreadsheet).Execute().SpreadsheetId;

            // Add new sheet
            AddSheetRequest addSheetRequest = new AddSheetRequest
            {
                Properties = new SheetProperties
                {
                    Title = dayText
                }
            };
            BatchUpdateSpreadsheetRequest batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request>
                {
                    new Request { AddSheet = addSheetRequest }
                }
            };
            var res = sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, spreadsheetId).Execute();
            //(res.Replies).Items[0]).AddSheet.Properties.Index
            var newSheetIndex = res.Replies.First().AddSheet.Properties.SheetId!.Value;

            // Get sales data for a specified day
            var sales = _salesRepository.GetSales(
                dayDate.ToDateTime(TimeOnly.MinValue),
                dayDate.ToDateTime(TimeOnly.MaxValue));

            List<IList<object>> data = sales
                .GroupBy(s => new { s.Date, s.ProductName })
                .Select(g => (IList<object>)new List<object> { g.Key.ProductName, g.Sum(s => s.Amount) })
                .ToList();

            List<Request> requests = new List<Request>();
            requests.Add(PrepareRequest(newSheetIndex, 0, 0, "Product"));
            requests.Add(PrepareRequest(newSheetIndex, 0, 1, "Amount"));
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    var value = data[i][j].ToString();
                    var request = PrepareRequest(newSheetIndex, i+1, j, value);
                    requests.Add(request);
                }
            }
            BatchUpdateSpreadsheetRequest secondBatchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = requests };
            sheetsService.Spreadsheets.BatchUpdate(secondBatchUpdateRequest, spreadsheetId).Execute();
        }

        private static Request PrepareRequest(int sheetIndex, int row, int column, string value)
        {
            return new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = new GridCoordinate
                    {
                        SheetId = sheetIndex,
                        RowIndex = row,
                        ColumnIndex = column
                    },
                    Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = new List<CellData> { 
                                new CellData
                                {
                                    UserEnteredValue = new ExtendedValue
                                    {
                                        StringValue = value
                                    }
                                }
                            }
                        }
                    },
                    Fields = "userEnteredValue"
                }
            };
        }
    }
}
