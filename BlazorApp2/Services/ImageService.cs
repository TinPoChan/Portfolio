using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

public class ImageService
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;
    private readonly string _customDomain;

    public ImageService(IOptions<AWSSettings> awsOptions)
    {
        if (string.IsNullOrEmpty(awsOptions.Value.BucketName) || string.IsNullOrEmpty(awsOptions.Value.ServiceURL))
            throw new InvalidOperationException("AWS settings must be fully configured.");

        var config = new AmazonS3Config
        {
            ServiceURL = awsOptions.Value.ServiceURL,
            ForcePathStyle = true
        };
        _s3Client = new AmazonS3Client(awsOptions.Value.AccessKey, awsOptions.Value.SecretKey, config);
        _bucketName = awsOptions.Value.BucketName;
        _customDomain = awsOptions.Value.CustomDomain ?? throw new InvalidOperationException("Custom domain must be configured.");
    }

    public async Task<string> UploadImage(IBrowserFile file)
    {
        var newFileName = CreateUniqueFileName(file.Name);
        using var stream = file.OpenReadStream(maxAllowedSize: 104857600);
        var response = await UploadFileStream(stream, newFileName, file.ContentType);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            return $"https://{_customDomain}/{newFileName}";
        }
        else
        {
            throw new Exception($"Failed to upload image. Status: {response.HttpStatusCode}");
        }
    }

    private string CreateUniqueFileName(string originalName) => Guid.NewGuid().ToString() + "_" + originalName;

    private async Task<PutObjectResponse> UploadFileStream(Stream stream, string fileName, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = stream,
            ContentType = contentType,
            DisablePayloadSigning = true,
            CannedACL = S3CannedACL.PublicRead
        };

        return await _s3Client.PutObjectAsync(putRequest);
    }
}
