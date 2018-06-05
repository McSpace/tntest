using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tntest.Controllers
{

    // public interface IFormFile
    // {
    //     string ContentType { get; }
    //     string ContentDisposition { get; }
    //     IHeaderDictionary Headers { get; }
    //     long Length { get; }
    //     string Name { get; }
    //     string FileName { get; }
    //     Stream OpenReadStream();
    //     void CopyTo(Stream target);
    //     Task CopyToAsync(Stream target, CancellationToken cancellationToken);
    // }



    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }




        // POST api/values
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<string> UploadFile()
        {
            var file = Request.Form.Files[0];
            var filePath = Path.GetTempFileName();
            filePath = Path.ChangeExtension(filePath, "jpeg");
            if (file.Length > 0)
            {
                
             using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
                
            }

            return Ok(new { Ok = "1" });
        }


        [HttpPost("UploadFiles"), DisableRequestSizeLimit]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream, CancellationToken.None);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }

    }
}
