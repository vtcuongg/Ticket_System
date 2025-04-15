
using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Options;
using TicketSystem.Helper;
using Amazon;

namespace TicketSystem.Service
{
    public class S3Service : IS3Service
    {
        private readonly AwsS3Settings _settings;
        private readonly IAmazonS3 _s3Client;

        public S3Service(IOptions<AwsS3Settings> settings)
        {
            _settings = settings.Value;
            _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));
        }
        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                var uri = new Uri(fileUrl);
                var key = uri.AbsolutePath.TrimStart('/'); // "attachments/..."
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _settings.BucketName,
                    Key = key
                };

                var response = await _s3Client.DeleteObjectAsync(deleteRequest);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    throw new Exception("Failed to delete file from S3.");
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception("Error deleting file from S3: " + ex.Message, ex);
            }
        }
        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            try
            {
                using var stream = file.OpenReadStream();

                var request = new PutObjectRequest
                {
                    BucketName = _settings.BucketName,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{fileName}";
                }

                throw new Exception("Failed to upload file to S3.");
            }
            catch (AmazonS3Exception ex)
            {
                // Có thể log thêm lỗi ở đây
                throw new Exception("Error uploading file to S3: " + ex.Message, ex);
            }
        }
    }
}
