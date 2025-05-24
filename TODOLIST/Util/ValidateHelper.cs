using Microsoft.AspNetCore.Mvc;
using TODOLIST.Service;

namespace TODOLIST.Util
{
    public class ValidateHelper
    {
        public static List<T> SetUserId<T>(IEnumerable<T> items, int userId) where T : IUserIdentifiable
        {
            foreach (var item in items)
            {
                item.USER_ID = userId;
            }
            return items.ToList();
        }
    }
}
