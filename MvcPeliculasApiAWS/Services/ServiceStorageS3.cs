using Amazon.S3;
using Amazon.S3.Model;

namespace MvcPeliculasApiAWS.Services
{
    public class ServiceStorageS3
    {
        private string BucketName;
        private IAmazonS3 ClientS3;

        public ServiceStorageS3(IConfiguration configuration
            , IAmazonS3 clientS3)
        {
            this.ClientS3 = clientS3;
            this.BucketName = configuration.GetValue<string>
                ("AWS:S3BucketName");
        }

        public async Task<bool> UploadFileAsync
            (string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = this.BucketName,
                Key = fileName,
                InputStream = stream
            };
            PutObjectResponse response = await
                this.ClientS3.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
