using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilters
{
    public class FilterKeywordActionFilter : IActionFilter
    {
        //Runs after the execution goes into the controller's method.
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Leave empty, do not keep throw new NotImplementedException().
        }

        //Runs before the execution goes into the controller's method.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool stopped = true;

            if(context.ActionArguments.Count == 0)
            {
                //problem - cannot continue.
                context.Result = new BadRequestResult();
                return; //Stop the action filter at this.
            }

            var userInput = context.ActionArguments.SingleOrDefault(x => x.Key == "keyword");
            if(userInput.Value != null)
            {
                if(userInput.Value.ToString().Trim() != "")
                {
                    //fine unless it is some kind of malicious injection.
                    return; //proceed smoothly.
                }
            }

            //problem - cannot continue
            context.Result = new ForbidResult();
            return;
        }
    }
}
