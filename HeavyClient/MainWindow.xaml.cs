using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HeavyClient.Config;
using Microsoft.Win32;
using Map = HeavyClient.Data.ViewModels.Map;

namespace HeavyClient
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> routeSearches = new List<string>();
        public static uint lineCounter = 1;

        public MainWindow()
        {
            InitializeComponent();
            SetMenu();
        }

        private void SetMenu()
        {
            var logs = new MenuItem
            {
                Header = "Logs",
                FontSize = 15
            };
            var quit = new MenuItem
            {
                Header = "Quit",
                FontSize = 15
            };
            quit.Click += On_Quit_Click;

            var export = new MenuItem
            {
                Header = "Export",
                FontSize = 15
            };
            export.Click += On_Export;

            var mainItem = new MenuItem
            {
                Header = "Menu",
                FontSize = 15
            };

            mainItem.Items.Add(logs);
            mainItem.Items.Add(export);
            mainItem.Items.Add(quit);

            menu.Items.Add(mainItem);
            mainMenu.NavigationUIVisibility = NavigationUIVisibility.Automatic;
        }

        private void On_Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void On_Export(object sender, RoutedEventArgs e)
        {
            var statsToSave = Map.statsToSave;
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx;*.csv;*.xlsm;*.xls";
            saveFileDialog.Title = "Save Datasheet";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                var filename = saveFileDialog.FileName;
                try
                {
                    SaveExelFile(filename, statsToSave, routeSearches);
                    MessageBox.Show("Statistics were successfully saved", "OK", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, null, MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }

                if (!File.Exists(filename))
                    MessageBox.Show("Could'nt create File, an error has occured", null, MessageBoxButton.OKCancel,
                        MessageBoxImage.Error);
            }
        }

        private void SaveExelFile(string filename, List<StationStatistics> stationStatistics,
            List<string> routeSearches)
        {
            using (var spreadSheet = SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = spreadSheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart2.Worksheet = new Worksheet(new SheetData());

                for (var letter = 'A'; letter <= 'E'; letter++)
                    if (letter == 'A')
                        InsertDataAtCell("Departure", letter.ToString(), 1, worksheetPart, spreadSheet);
                    else if (letter == 'B')
                        InsertDataAtCell("Arrival", letter.ToString(), 1, worksheetPart, spreadSheet);
                    else if (letter == 'C')
                        InsertDataAtCell("Distance", letter.ToString(), 1, worksheetPart, spreadSheet);
                    else if (letter == 'D')
                        InsertDataAtCell("Duration", letter.ToString(), 1, worksheetPart, spreadSheet);
                    else if (letter == 'E')
                        InsertDataAtCell("Searched AT", letter.ToString(), 1, worksheetPart, spreadSheet);

                for (var i = 1; i < routeSearches.Count; i += 2)
                {
                    lineCounter++;
                    var dep = routeSearches[i - 1].Split('-')[0];
                    var arr = routeSearches[i - 1].Split('-')[1];
                    var distance = routeSearches[i].Split('-')[0];
                    var searchAt = routeSearches[i].Split('-')[1];
                    var duration = routeSearches[i].Split('-')[2];

                    for (var letter = 'A'; letter <= 'E'; letter++)
                        if (letter == 'A')
                            InsertDataAtCell(dep, letter.ToString(), lineCounter, worksheetPart, spreadSheet);
                        else if (letter == 'B')
                            InsertDataAtCell(arr, letter.ToString(), lineCounter, worksheetPart, spreadSheet);
                        else if (letter == 'C')
                            InsertDataAtCell(distance, letter.ToString(), lineCounter, worksheetPart, spreadSheet);
                        else if (letter == 'D')
                            InsertDataAtCell(duration, letter.ToString(), lineCounter, worksheetPart, spreadSheet);
                        else if (letter == 'E')
                            InsertDataAtCell(searchAt, letter.ToString(), lineCounter, worksheetPart, spreadSheet);
                }

                lineCounter = 1;

                for (var letter = 'A'; letter <= 'F'; letter++)
                    if (letter == 'A')
                        InsertDataAtCell("Number", letter.ToString(), lineCounter, worksheetPart2, spreadSheet);
                    else if (letter == 'B')
                        InsertDataAtCell("Name", letter.ToString(), lineCounter, worksheetPart2, spreadSheet);
                    else if (letter == 'C')
                        InsertDataAtCell("Contract", letter.ToString(), lineCounter, worksheetPart2, spreadSheet);
                    else if (letter == 'D')
                        InsertDataAtCell("Adress", letter.ToString(), lineCounter, worksheetPart2, spreadSheet);
                    else if (letter == 'E')
                        InsertDataAtCell("Occurence", letter.ToString(), lineCounter, worksheetPart2, spreadSheet);
                    else if (letter == 'F')
                        InsertDataAtCell("Type", letter.ToString(), lineCounter, worksheetPart2, spreadSheet);

                for (var i = 0; i < stationStatistics.Count; i++)
                {
                    lineCounter++;

                    for (var letter = 'A'; letter <= 'F'; letter++)
                        if (letter == 'A')
                            InsertDataAtCell(stationStatistics[i].station.number.ToString(), letter.ToString(),
                                lineCounter, worksheetPart2, spreadSheet);
                        else if (letter == 'B')
                            InsertDataAtCell(stationStatistics[i].station.name, letter.ToString(), lineCounter,
                                worksheetPart2, spreadSheet);
                        else if (letter == 'C')
                            InsertDataAtCell(stationStatistics[i].station.contractName, letter.ToString(), lineCounter,
                                worksheetPart2, spreadSheet);
                        else if (letter == 'D')
                            InsertDataAtCell(stationStatistics[i].station.address, letter.ToString(), lineCounter,
                                worksheetPart2, spreadSheet);
                        else if (letter == 'E')
                            InsertDataAtCell(stationStatistics[i].occurence.ToString(), letter.ToString(), lineCounter,
                                worksheetPart2, spreadSheet);
                        else if (letter == 'F')
                            InsertDataAtCell(stationStatistics[i].type.ToString(), letter.ToString(), lineCounter,
                                worksheetPart2, spreadSheet);
                }

                var sheets = spreadSheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet
                {
                    Id = spreadSheet.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Searches"
                };

                var sheet2 = new Sheet
                {
                    Id = spreadSheet.WorkbookPart.GetIdOfPart(worksheetPart2),
                    SheetId = 2,
                    Name = "Most Visited Stations"
                };
                sheets.Append(sheet);
                sheets.Append(sheet2);
            }
        }

        private void InsertDataAtCell(string data, string column, uint row, WorksheetPart worksheetPart,
            SpreadsheetDocument spreadSheet)
        {
            // Get the SharedStringTablePart. If it does not exist, create a new one.
            SharedStringTablePart shareStringPart;
            if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            else
                shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();

            // Insert the text into the SharedStringTablePart.
            var index = InsertSharedStringItem(data, shareStringPart);

            var cell = InsertCellInWorksheet(column, row, worksheetPart);

            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            // Save the new worksheet.
            worksheetPart.Worksheet.Save();
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row {RowIndex = rowIndex};
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();

            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Cell refCell = null;
            foreach (var cell in row.Elements<Cell>())
                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }

            var newCell = new Cell {CellReference = cellReference};
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }

        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null) shareStringPart.SharedStringTable = new SharedStringTable();

            var i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (var item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text) return i;

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }
    }
}