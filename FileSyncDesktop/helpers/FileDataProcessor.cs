using FileSyncDesktop.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FileSyncDesktop.Helpers
{
    public class FileDataProcessor : IFileDataProcessor
    {
        private static string pattern = @"\d{1,3}\t(OUT|IN|DROP)\t\d{4}\t\d{1,2}\t\w{2}";
        private IBibRecordCollection _bibRecordCollection;
        private IRmzLogger _logger;
        private readonly int _delay = 500;
        private readonly string _localRecordsLog = "LocalRecordsLog.txt";

        public FileDataProcessor(IBibRecordCollection bibRecordCollection, IRmzLogger logger)
        {
            _bibRecordCollection = bibRecordCollection;
            _logger = logger;
        }

        internal delegate Task<bool> AsyncProcessFile(string fileName);
        /// <summary>
        /// Processes a file of data, line by line, and adds BibRecords to the BibRecordCollection.
        /// Returns true if the file was processed without error, false otherwise.
        /// ProcessFile() does not distinguish between a file that has data matching the Regex pattern and one that does not.
        /// </summary>
        /// <param name="fileName"></param>
        public Library.Helpers.BibRecordModels ProcessFile(string fileName)
        {
            _logger.Data("FileDataProcessor.ProcessFile", "Called!");
            var bibRecords = new Library.Helpers.BibRecordModels();

            AsyncProcessFile asyncPF = async (string _filename) =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        // delay before reading the file for about 500 milliseconds
                        Thread.Sleep(_delay);
                        string[] dataLines = File.ReadAllLines(_filename);

                        foreach (var dataLine in dataLines)
                        {
                            Match match = Regex.Match(dataLine, pattern, RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                _logger.Data("FileDataProcessor.ProcessFile", $"Bib entry detected.");
                                string[] data = dataLine.Split('\t');
                                int bibNumber = int.Parse(data[0]);
                                string action = data[1];
                                string bibTimeOfDay = data[2];
                                int dayOfMonth = int.Parse(data[3]);
                                string shortLocation = data[4];
                                var temp = new Library.Models.BibRecordModel()
                                {
                                    BibNumber = bibNumber,
                                    Action = action,
                                    BibTimeOfDay = bibTimeOfDay,
                                    DayOfMonth = dayOfMonth,
                                    Location = shortLocation
                                };

                                bibRecords.BibRecords.Add(temp);
                                _logger.Data("FileDataProcessor.ProcessFile", $"Bib entry added to local list.");
                            }
                        }

                        _bibRecordCollection.AddRange(bibRecords.BibRecords);
                        string processedFilesCount = $"Processed {bibRecords.BibRecords.Count} bibs.";
                        _logger.Data("ProcessFile", processedFilesCount);
                        _logger.Flush();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Data("FileDataProcessor", $"Error! {ex.Message}");
                        _logger.Flush();
                        return false;
                    }
                });
            };

            var result = asyncPF(fileName).Result;
            WriteBibRecordsToLocalLog(bibRecords);
            return bibRecords;
        }

        private void WriteBibRecordsToLocalLog(Library.Helpers.BibRecordModels records)
        {
            if (records.BibRecords.Count < 1)
            {
                return;
            }

            DirectoryInfo rootDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo logFileInfo = new FileInfo(Path.Combine(rootDir.FullName, _localRecordsLog));

            using (StreamWriter sw = logFileInfo.AppendText())
            {
                foreach (var record in records.BibRecords)
                {
                    var entry = $"{record}";
                    sw.WriteLine(entry);
                }
            }
        }
    }
}
