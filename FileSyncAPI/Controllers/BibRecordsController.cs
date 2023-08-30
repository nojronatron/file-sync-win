using FileSyncAPI.Helpers;
using FileSyncAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileSyncAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BibRecordsController : ControllerBase
    {
        private readonly IApiBibDataCollection _apiBibRecords;
        private readonly ILogger<BibRecordsController> _logger;
        private readonly IRecordLogger _recordLogger;

        public BibRecordsController(
            IApiBibDataCollection apiBibRecords, 
            ILogger<BibRecordsController> logger, 
            IRecordLogger recordLogger)
        {
            _apiBibRecords = apiBibRecords;
            _logger = logger;
            _recordLogger = recordLogger;
        }

        // POST api/<BibRecordsController>
        [HttpPost(Name = "PostBibRecords")]
        public IActionResult Create(ApiBibRecords values)
        {
            _logger.LogInformation("PostBibRecords called with arg {values}.", values);

            if (values.BibRecords != null && values.BibRecords.Count > 0)
            {
                foreach (ApiBibRecord value in values.BibRecords)
                {
                    _logger.LogInformation("Current value is {value}", value);
                    string convertedValue = ConvertBibRecord.Convert(value);
                    _logger.LogInformation("Converted value is {convertedValue}", convertedValue);

                    try
                    {
                        _apiBibRecords.Add(value);
                        _recordLogger.AddEntry(value);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Exception {e} thrown.", e);
                    }
                }
            } else
            {
                _logger.LogError("values.BibRecords was null or values.BibRecords.Count was 0.");
            }

            _recordLogger.FlushEntries();
            _logger.LogInformation("Exiting BibRecordsController POST route.");
            return Ok(new { message = "Bib records added." });
        }
    }
}
