using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace AppService;
public sealed class LicensingService : IBackgroundTask
{
    private BackgroundTaskDeferral backgroundTaskDeferral;
    private AppServiceConnection appServiceconnection;

    public void Run(IBackgroundTaskInstance taskInstance)
    {
        // Get a deferral so that the service isn't terminated.
        this.backgroundTaskDeferral = taskInstance.GetDeferral();

        // Associate a cancellation handler with the background task.
        taskInstance.Canceled += OnTaskCanceled;

        // Retrieve the app service connection and set up a listener for incoming app service requests.
        var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
        appServiceconnection = details.AppServiceConnection;
        appServiceconnection.RequestReceived += OnRequestReceived;
    }

    private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {
        Console.WriteLine("Request received");

        var response = new ValueSet();
        response.Add("Status", "OK");
        var responseResult = await args.Request.SendResponseAsync(response);
        Console.WriteLine("Response sent");
    }

    private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
        if (this.backgroundTaskDeferral != null)
        {
            // Complete the service deferral.
            this.backgroundTaskDeferral.Complete();
        }
    }
}