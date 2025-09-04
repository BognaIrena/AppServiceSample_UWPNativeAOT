using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace newUWPAppServiceTest
{
    public static class AppServiceClient
    {

        const string AppServiceName = "DummyAppService";
        const string PackageFamilyName = "newUWPAppServiceTest_8tgegp95jbzft";
        private const string COMMAND = "Command";
        private const string DEVICEID = "DeviceID";
        private const string MEDIACODECNAME = "MediaCodecName";

        public static async Task<ValueSet> GetLicenseInfo(string codec, string deviceId)
        {
            ValueSet request = new ValueSet()
            {
                { COMMAND, "GetLicenseInfo" },
                { DEVICEID, deviceId },
                { MEDIACODECNAME, codec }
            };
            
            return await SendMessage(request);
        }

        public static async Task<ValueSet> SetCodec(string codec, string deviceId)
        {
            ValueSet request = new ValueSet()
            {
                { COMMAND, "SetCodec" },
                { DEVICEID, deviceId },
                { MEDIACODECNAME, codec },
                { "Token", string.Empty}
            };

            return await SendMessage(request);
        }

        public static async Task<ValueSet> SetProfile(string parameters)
        {
            ValueSet request = new ValueSet()
            {
                { COMMAND, "SetProfile" },
                { "ProfileParameters", parameters }
            };

            return await SendMessage(request);
        }
        public static async Task<ValueSet> SendMessage(ValueSet request)
        {
            AppServiceConnection connection = new AppServiceConnection();
            connection.PackageFamilyName = PackageFamilyName;
            connection.AppServiceName = AppServiceName;
            try
            {
                AppServiceConnectionStatus status = await connection.OpenAsync();

                if (status == AppServiceConnectionStatus.Success)
                {
                    AppServiceResponse response = await connection.SendMessageAsync(request);

                    if (response.Status == AppServiceResponseStatus.Success)
                    {
                        return response.Message;
                    }
                    else
                    {
                        Console.WriteLine($"App service request unsuccessful: {response.Status}");
                        var resp = new ValueSet();
                        resp.Add("Status", status);
                        return resp;
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to connect to app service: {status}");
                    var resp = new ValueSet();
                    resp.Add("Status", status);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"App service connection throw an exception: {ex.Message}");
                var resp = new ValueSet();
                resp.Add("Status", ex.Message);
                return resp;
            }
            finally
            {
                connection.Dispose();
            }
        }
    }
}
