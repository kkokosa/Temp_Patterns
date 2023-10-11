using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Reports.Main.Strategy;

namespace Reports.Console.Strategy.Strategies;

internal class GoogleSheetStrategy : ISpreadsheetStrategy
{
    private SheetsService? _sheetsService;
    private string? _spreadsheetId;
    private int _newSheetIndex;
    readonly List<Request> _requests = new List<Request>();

    public void Export(string title, List<List<object>> data)
    {
        Initialize();
        AddSpreadsheet(title);
        AddSheet(title);
        for (int i = 0; i < data.Count; i++)
        for (int j = 0; j < data[i].Count; j++)
            SetCellValue(i + 1, j + 1, data[i][j]);
        Save();
    }

    private void Initialize()
    {
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

        _sheetsService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "GoogleSheetExample",
        });
    }

    private void AddSpreadsheet(string title)
    {
        var spreadsheet = new Spreadsheet()
        {
            Properties = new SpreadsheetProperties()
            {
                Title = title
            }
        };
        _spreadsheetId = _sheetsService.Spreadsheets.Create(spreadsheet).Execute().SpreadsheetId;
    }

    private void AddSheet(string title)
    {
        // Add new sheet
        AddSheetRequest addSheetRequest = new AddSheetRequest
        {
            Properties = new SheetProperties
            {
                Title = title
            }
        };
        BatchUpdateSpreadsheetRequest batchUpdateRequest = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request>
            {
                new Request { AddSheet = addSheetRequest }
                }
        };
        var res = _sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, _spreadsheetId).Execute();
        _newSheetIndex = res.Replies.First().AddSheet.Properties.SheetId!.Value;
    }

    private void SetCellValue(int row, int column, object value)
    {
        _requests.Add(PrepareRequest(_newSheetIndex, row, column, value));
    }

    private void Save()
    {
        BatchUpdateSpreadsheetRequest secondBatchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = _requests };
        _sheetsService.Spreadsheets.BatchUpdate(secondBatchUpdateRequest, _spreadsheetId).Execute();
    }

    private static Request PrepareRequest(int sheetIndex, int row, int column, object value)
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
                                        StringValue = value.ToString()
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