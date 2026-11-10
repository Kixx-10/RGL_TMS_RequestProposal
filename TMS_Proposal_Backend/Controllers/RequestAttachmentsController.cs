using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS_Proposal_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestAttachmentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        private static readonly Dictionary<string, HashSet<string>> AllowedTypes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".jpg"] = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg" },
                [".jpeg"] = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg" },
                [".png"] = new(StringComparer.OrdinalIgnoreCase) { "image/png" },
                [".pdf"] = new(StringComparer.OrdinalIgnoreCase) { "application/pdf" },
                [".xlsx"] = new(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                [".docx"] = new(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
            };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB per file

        public RequestAttachmentsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("UploadMultiple")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest(new { message = "There are no file to upload" });
                }

                var storageFolder = _config["FileStorage:RequestAttachmentsPath"];
                if (string.IsNullOrWhiteSpace(storageFolder))
                {
                    return StatusCode(500, new { message = "There is no verify file path" });
                }

                Directory.CreateDirectory(storageFolder);
                var uploadedFilesList = new List<object>();

                foreach (var file in files)
                {
                    if (file.Length == 0) continue;

                    if (file.Length > MaxFileSizeBytes)
                    {
                        return BadRequest(new { message = $"File '{file.FileName}' exceeds 5 Mb" });
                    }

                    var extension = Path.GetExtension(file.FileName);
                    if (!AllowedTypes.TryGetValue(extension, out var allowedMimeTypes) || !allowedMimeTypes.Contains(file.ContentType))
                    {
                        return BadRequest(new { message = $"Invalid File: {file.FileName}" });
                    }

                    var safeFileName = $"{Guid.NewGuid():N}{extension}";
                    var filePath = Path.Combine(storageFolder, safeFileName);

                    await using (var stream = new FileStream(filePath, FileMode.CreateNew))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedFilesList.Add(new
                    {
                        fileName = file.FileName,
                        fileUrl = safeFileName
                    });
                }

                return Ok(new
                {
                    message = "Files successfully uploaded",
                    files = uploadedFilesList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}