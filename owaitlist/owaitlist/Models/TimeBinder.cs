using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace owaitlist.Models
{
    public class TimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider
                .GetValue(key);

            if (valueProviderResult == null ||
                string.IsNullOrEmpty(valueProviderResult
                    .AttemptedValue))
            {
                return null;
            }

            bindingContext.ModelState
                .SetModelValue(key, valueProviderResult);

            var hours = ((string[])valueProviderResult.RawValue)[0];
            var minutes = ((string[])valueProviderResult.RawValue)[1];

            var time = new TimeSpan(Convert.ToInt32(hours),
                Convert.ToInt32(minutes), 0);

            return time;
        }
    }
}