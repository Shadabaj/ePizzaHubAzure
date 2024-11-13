using ePizzaHub.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ePizzaHub.UI.Helpers
{
    public abstract class BaseViewPage<TModel> : RazorPage<TModel>
    {
        public UserModel CurrentUser
        {
            get
            {
                if (User.Claims.Count() > 0)
                {
                    string userData = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData).Value;
                    var user = JsonConvert.DeserializeObject<UserModel>(userData);
                    return user;
                }
                return null;
            }
        }
    }
}