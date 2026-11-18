using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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

                // 1. Validate all files first before uploading any to Azure
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
                }

                var connectionString = _config["FileStorage:ConnectionString"];
                var containerName = _config["FileStorage:ContainerName"];

                if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(containerName))
                {
                    return StatusCode(500, new { message = "Azure Blob Storage is not configured." });
                }

                var containerClient = new BlobContainerClient(connectionString, containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                var uploadedFilesList = new List<object>();

                // 2. Upload validated files to Azure Blob Storage
                foreach (var file in files)
                {
                    if (file.Length == 0) continue;

                    var extension = Path.GetExtension(file.FileName);
                    var safeFileName = $"{Guid.NewGuid():N}{extension}";

                    var blobClient = containerClient.GetBlobClient(safeFileName);
                    var blobHttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType };

                    await using (var stream = file.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
                    }

                    uploadedFilesList.Add(new
                    {
                        fileName = file.FileName,
                        fileUrl = safeFileName // Frontend နဲ့ Database မှာ မူလအတိုင်း file name ကိုပဲ သိမ်းထားနိုင်ပါတယ်
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

        [HttpGet("File/{fileUrl}")]
        public async Task<IActionResult> GetAttachmentFile(string fileUrl)
        {
            var connectionString = _config["FileStorage:ConnectionString"];
            var containerName = _config["FileStorage:ContainerName"];

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(containerName))
            {
                return StatusCode(500, new { message = "Storage is not configured." });
            }

            var safeFileName = Path.GetFileName(fileUrl);
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var blobClient = containerClient.GetBlobClient(safeFileName);

            if (!await blobClient.ExistsAsync())
            {
                return NotFound(new { message = "Attachment file not found on server." });
            }

            var extension = Path.GetExtension(safeFileName);
            var contentType = AllowedTypes.TryGetValue(extension, out var mimeSet)
                ? mimeSet.First()
                : "application/octet-stream";

            // Azure Blob မှ ဖိုင်ကို Stream အနေနဲ့ တိုက်ရိုက်ပြန်ပို့ပေးခြင်း
            var downloadResponse = await blobClient.DownloadStreamingAsync();
            return File(downloadResponse.Value.Content, contentType);
        }
    }
}

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace TMS_Proposal_Backend.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RequestAttachmentsController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        private static readonly Dictionary<string, HashSet<string>> AllowedTypes =
//            new(StringComparer.OrdinalIgnoreCase)
//            {
//                [".jpg"] = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg" },
//                [".jpeg"] = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg" },
//                [".png"] = new(StringComparer.OrdinalIgnoreCase) { "image/png" },
//                [".pdf"] = new(StringComparer.OrdinalIgnoreCase) { "application/pdf" },
//                [".xlsx"] = new(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
//                [".docx"] = new(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
//            };

//        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB per file

//        public RequestAttachmentsController(IConfiguration config)
//        {
//            _config = config;
//        }

//        [HttpPost("UploadMultiple")]
//        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
//        {
//            try
//            {
//                if (files == null || files.Count == 0)
//                {
//                    return BadRequest(new { message = "There are no file to upload" });
//                }

//                var storageFolder = _config["FileStorage:RequestAttachmentsPath"];
//                if (string.IsNullOrWhiteSpace(storageFolder))
//                {
//                    return StatusCode(500, new { message = "There is no verify file path" });
//                }

//                Directory.CreateDirectory(storageFolder);
//                var uploadedFilesList = new List<object>();

//                foreach (var file in files)
//                {
//                    if (file.Length == 0) continue;

//                    if (file.Length > MaxFileSizeBytes)
//                    {
//                        return BadRequest(new { message = $"File '{file.FileName}' exceeds 5 Mb" });
//                    }

//                    var extension = Path.GetExtension(file.FileName);
//                    if (!AllowedTypes.TryGetValue(extension, out var allowedMimeTypes) || !allowedMimeTypes.Contains(file.ContentType))
//                    {
//                        return BadRequest(new { message = $"Invalid File: {file.FileName}" });
//                    }

//                    var safeFileName = $"{Guid.NewGuid():N}{extension}";
//                    var filePath = Path.Combine(storageFolder, safeFileName);

//                    await using (var stream = new FileStream(filePath, FileMode.CreateNew))
//                    {
//                        await file.CopyToAsync(stream);
//                    }

//                    uploadedFilesList.Add(new
//                    {
//                        fileName = file.FileName,
//                        fileUrl = safeFileName
//                    });
//                }

//                return Ok(new
//                {
//                    message = "Files successfully uploaded",
//                    files = uploadedFilesList
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
//            }
//        }

//        [HttpGet("File/{fileUrl}")]
//        public IActionResult GetAttachmentFile(string fileUrl)
//        {
//            var storageFolder = _config["FileStorage:RequestAttachmentsPath"];
//            if (string.IsNullOrWhiteSpace(storageFolder))
//            {
//                return StatusCode(500, new { message = "Storage path is not configured." });
//            }

//            var safeFileName = Path.GetFileName(fileUrl);
//            var filePath = Path.Combine(storageFolder, safeFileName);

//            if (!System.IO.File.Exists(filePath))
//            {
//                return NotFound(new { message = "Attachment file not found on server." });
//            }

//            var extension = Path.GetExtension(safeFileName);
//            var contentType = AllowedTypes.TryGetValue(extension, out var mimeSet)
//                ? mimeSet.First()
//                : "application/octet-stream";

//            return PhysicalFile(filePath, contentType);
//        }
//    }
//}