using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CityApp.Generator
{
    public class GenerateNews
    {
        private readonly INewsRepository newsRepository;

        public GenerateNews(INewsRepository newsRepository) => this.newsRepository = newsRepository;

        [FunctionName("GenerateNews")]
        public IActionResult RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("Generating news");

            string name = req.Query["count"];

            int count = 100;
            if (!string.IsNullOrEmpty(name))
                count = int.Parse(name);

            //add https://random-data-api.com/api/ to the calls - check documentation
            string content =
                "Lorem ipsum dolor sit amet. Vel quibusdam consequuntur aut culpa dolorem et quas aliquid non deleniti nulla At consectetur error aut eveniet optio. Qui ratione quia ad tempora quasi At earum commodi est quos dolore vel repellendus enim eos dolores commodi eum incidunt magni. In quis obcaecati et rerum galisum Id dolorum eos distinctio sint aut veritatis impedit in eaque doloremque. 33 eaque quidem et expedita nesciunt ab temporibus galisum eum impedit. Et dolor dolores 33 harum aspernatur in beatae error eos ipsam error aut fugiat nesciunt a enim quas. Cum beatae facilis quo molestias nesciunt est porro odio. Ut eligendi illum et nisi reprehenderit aut nesciunt voluptate et nemo voluptatem qui sint corrupti sed praesentium enim et inventore amet.Id consequatur porro Nam impedit et eveniet repellat consectetur dignissimos non voluptatem obcaecati et facilis esse! Aut quas quibusdam et inventore debitis vel accusamus odit est quia exercitationem non autem amet. Qui quia rerum id quisquam distinctio ex fugit quia quo similique impedit non quam repellat. Sed incidunt quas id dolore galisum in quia aspernatur id minima consequatur. Non amet dolores quo repudiandae consequuntur non possimus aliquid est ipsa quae hic autem velit in molestias saepe ut perferendis ullam. Eos autem illum ab blanditiis amet ab distinctio quidem hic omnis officiis! Rem quidem dolore sed asperiores consequatur est quae repellat aut fugiat totam quo iure porro. Quo voluptate illum aut quis excepturi ut amet libero sed sunt voluptatem aut pariatur atque rem velit deleniti. Ea consequatur quod ex dolor labore et esse omnis ut mollitia alias et optio veniam id voluptatem maxime.Rem cumque eius nam debitis dolore qui voluptatibus vero eos iusto laudantium et atque aperiam. Et dolor galisum est consequatur amet Sit facere est voluptatibus quod qui earum commodi et rerum voluptatum? Ut quibusdam doloremque ut quasi porro ad ratione quis. Ut accusamus sunt et consequuntur dolores vel sunt inventore 33 facilis dolores vel ipsam delectus aut placeat autem ut adipisci asperiores. Aut provident soluta non maxime perspiciatis vel delectus beatae quo blanditiis praesentium vel earum velit voluptas assumenda ea mollitia doloribus?";
            for (int counter = 0; counter < count; counter++)
            {
                string title = RandomGenerator.GenerateString(15);
                string shortDescription = RandomGenerator.GenerateString(5);
                log.LogInformation($"Adding {counter} to the database");
                var entity = new News
                {
                    Content = content, Title = title, ExternalLink = "https://beyondlocalhost.tech",
                    ShortDescription = shortDescription
                };
                newsRepository.Insert(entity);
            }

            return new OkObjectResult($"Generated {name} items");
        }
    }
}