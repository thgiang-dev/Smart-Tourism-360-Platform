using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Infrastructure.Storage;

namespace SmartTourism360.Infrastructure.Services
{
    public class StorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinIoOptions _options;

        public StorageService(IMinioClient minioClient, IOptions<MinIoOptions> options)
        {
            _minioClient = minioClient;
            _options = options.Value;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads")
        {
            await EnsureBucketExistsAsync();

            var extension = Path.GetExtension(fileName);
            var uniqueName = $"{Guid.NewGuid()}{extension}";
            var objectName = $"media/{folder}/{DateTime.UtcNow:yyyyMMdd}/{uniqueName}";

            // Ensure stream is positioned at the beginning
            if (fileStream.CanSeek && fileStream.Position > 0)
            {
                fileStream.Position = 0;
            }

            var putArgs = new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putArgs);

            return objectName;
        }

        private async Task EnsureBucketExistsAsync()
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(_options.BucketName);
                bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
                if (!found)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(_options.BucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs);

                    // Set policy to public (read-only for anonymous)
                    string policyJson = $@"{{
                        ""Version"": ""2012-10-17"",
                        ""Statement"": [
                            {{
                                ""Action"": [
                                    ""s3:GetObject""
                                ],
                                ""Effect"": ""Allow"",
                                ""Principal"": {{
                                    ""AWS"": [
                                        ""*""
                                    ]
                                }},
                                ""Resource"": [
                                    ""arn:aws:s3:::{_options.BucketName}/*""
                                ]
                            }}
                        ]
                    }}";
                    var setPolicyArgs = new SetPolicyArgs()
                        .WithBucket(_options.BucketName)
                        .WithPolicy(policyJson);
                    await _minioClient.SetPolicyAsync(setPolicyArgs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MinIO Seeding/Upload Warning] Error checking or creating bucket: {ex.Message}");
            }
        }

        public async Task<bool> DeleteFileAsync(string storagePath)
        {
            try
            {
                var removeArgs = new RemoveObjectArgs()
                    .WithBucket(_options.BucketName)
                    .WithObject(storagePath);

                await _minioClient.RemoveObjectAsync(removeArgs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetPublicUrl(string storagePath)
        {
            var protocol = _options.Secure ? "https" : "http";
            var host = !string.IsNullOrEmpty(_options.PublicEndpoint) ? _options.PublicEndpoint : _options.Endpoint;
            
            return $"{protocol}://{host}/{_options.BucketName}/{storagePath}";
        }
    }
}
