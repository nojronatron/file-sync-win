using FileSyncAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileSyncAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BibRecordsController : ControllerBase
    {
        private readonly IApiBibDataCollection _apiBibRecords;
        private readonly ILogger<BibRecordsController> _logger;

        public BibRecordsController(IApiBibDataCollection apiBibRecords, ILogger<BibRecordsController> logger)
        {
            _apiBibRecords = apiBibRecords;
            _logger = logger;
        }

        // POST api/<BibRecordsController>
        [HttpPost(Name = "PostBibRecords")]
        public IActionResult Create(IEnumerable<ApiBibRecord> values)
        {
            _logger.LogInformation("PostBibRecords called with arg {values}.", values);

            foreach (var value in values)
            {
                _logger.LogInformation("Current value is {value}", value);
                try
                {
                    _apiBibRecords.Add(value);
                }
                catch (Exception e)
                {
                    _logger.LogError("Exception {e} thrown.", e);
                }
            }

            //string apiRecords = _apiBibRecords.ToString();
            //_logger.LogInformation("apiBibRecords to string returned:", apiRecords);
            _logger.LogInformation("Exiting BibRecordsController POST route.");
            return Ok(new { message = "Bib records added." });
        }

    }
}
