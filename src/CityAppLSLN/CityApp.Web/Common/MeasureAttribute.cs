using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Common
{
    public class MeasureAttribute : ActionFilterAttribute
    {
        private readonly ILogger<MeasureAttribute> logger;

        public MeasureAttribute(ILogger<MeasureAttribute> logger) => this.logger = logger;

        public MeasureOutputTypes MeasureOutputType { get; set; }
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await next();

            stopWatch.Stop();

            if (context.Controller is not Controller controller) return;

            var message = $"Action took ";

            switch (MeasureOutputType)
            {
                case MeasureOutputTypes.MINUTES:
                    message += stopWatch.Elapsed.Minutes + " min.";
                    break;
                case MeasureOutputTypes.SECONDS:
                    message += stopWatch.Elapsed.Seconds + " s.";
                    break;
                case MeasureOutputTypes.MILISECONDS:
                    message += stopWatch.Elapsed.Milliseconds + " ms.";
                    break;
                default:
                    message += stopWatch.ElapsedMilliseconds + " ms.";
                    break;
            }

            controller.ViewBag.MeasureMessage = message;  
            logger.LogInformation(message);
        }
    }

    public enum MeasureOutputTypes
    {
        MILISECONDS=0,
        SECONDS,
        MINUTES
    }
}