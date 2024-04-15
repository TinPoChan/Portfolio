using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

public class ImageService
{
    private readonly AmazonS3Client _s3Client;
    private readonly string? _bucketName;
    private readonly string? _customDomain;

    public ImageService(IOptions<AWSSettings> awsOptions)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = awsOptions.Value.ServiceURL,
            ForcePathStyle = true // Important for S3 compatible services like R2
        };
        _s3Client = new AmazonS3Client(awsOptions.Value.AccessKey, awsOptions.Value.SecretKey, config);
        _bucketName = awsOptions.Value.BucketName;
        _customDomain = awsOptions.Value.CustomDomain;
    }

    public async Task<string> UploadImage(IBrowserFile file)
    {
        var customDomain = _customDomain;
        var newFileName = Guid.NewGuid().ToString() + "_" + file.Name;

        using var stream = file.OpenReadStream(maxAllowedSize: 104857600);
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = newFileName,
            InputStream = stream,
            ContentType = file.ContentType,
            DisablePayloadSigning = true,
            CannedACL = S3CannedACL.PublicRead // Set the ACL if you want the image to be publicly accessible
        };

        var response = await _s3Client.PutObjectAsync(putRequest);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            //return $"{_s3Client.Config.ServiceURL}/{_bucketName}/{newFileName}";
            return $"https://{customDomain}/{newFileName}";
        }
        else
        {
            throw new Exception("Failed to upload image");
        }
    }
}
