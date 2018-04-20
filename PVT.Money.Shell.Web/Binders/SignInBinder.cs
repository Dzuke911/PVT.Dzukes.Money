using Microsoft.AspNetCore.Mvc.ModelBinding;
using PVT.Money.Shell.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Binders
{
    public class SignInBinder :IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            ValueProviderResult result;
            //SignInModel model = new SignInModel();
            Type modelType = context.ModelType;
            object model = Activator.CreateInstance(modelType);
            ICollection<PropertyInfo> properties = modelType.GetProperties();
            foreach(PropertyInfo p in properties)
            {
                result = context.ValueProvider.GetValue(p.Name);
                if(p.PropertyType == typeof(string))
                    p.SetValue(model, result.FirstValue);
                if (p.PropertyType == typeof(bool))
                    p.SetValue(model, Convert.ToBoolean(result.FirstValue));
            }

            //ValueProviderResult result = context.ValueProvider.GetValue("Login");
            //PropertyInfo logInfo = modelType.GetProperty("Login");
            //logInfo.SetValue(model,result.FirstValue);
            //result = context.ValueProvider.GetValue("Password");
            //PropertyInfo passInfo = modelType.GetProperty("Password");
            //passInfo.SetValue(model, result.FirstValue);

            context.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
