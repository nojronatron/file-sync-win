using FileSyncDesktop.Collections;
using FileSyncDesktop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FileSyncDesktop.Helpers
{
    public class FileDataProcessor : IFileDataProcessor
    {
        private static string pattern = @"\d{1,3}\t(OUT|IN|DROP)\t\d{4}\t\d{1,2}\t\w{2}";
        private IBibRecordCollection _bibRecordCollection;
        private ILogger _logger;

        public FileDataProcessor(IBibRecordCollection bibRecordCollection, ILogger logger)
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
        public Library.Helpers.BibRecords ProcessFile(string fileName)
        {
            _logger.Data("FileDataProcessor.ProcessFile", "Called!");
            var bibRecords = new Library.Helpers.BibRecords();

            AsyncProcessFile asyncPF = async (string _filename) =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        // delay before reading the file for about 500 milliseconds
                        Thread.Sleep(500);
                        string[] dataLines = File.ReadAllLines(_filename);

                        foreach (var dataLine in dataLines)
                        {
                            Match match = Regex.Match(dataLine, pattern, RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                _logger.Data("FileDataProcessor.ProcessFile:", $"Regex Match in {dataLine}");
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
                                    ShortLocation = shortLocation
                                };

                                bibRecords.bibRecords.Add(temp);
                                _logger.Data("FileDataProcessor.ProcessFile:", $"Bib entry added: {temp}");
                            }
                        }

                        _bibRecordCollection.AddRange(bibRecords.bibRecords);
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
            return bibRecords;
        }
    }
}
