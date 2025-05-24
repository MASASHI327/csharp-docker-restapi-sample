using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOLIST.Domain;
using TODOLIST.Util;

namespace TODOLISTTEST
{
    public class ValidateHelperTest
    {
        /// <summary>
        /// 正常系
        /// </summary>
        [Fact]
        public void SetUserId_SetsUserIdForAllItems()
        {

            var items = new List<Todolist>
        {
            new() {
                    TODOLIST_ID = 1,
                    CONTENT = "内容1",
                    CREATED_DATE = DateTime.Parse("2025-04-11"),
                    TITLE = "GETについて",
                    UPDATED_DATE = DateTime.Parse("2026-04-11"),
                    USER_ID = 5
                  },
            new() {
                    TODOLIST_ID = 2,
                    CONTENT = "内容2",
                    CREATED_DATE = DateTime.Parse("2025-08-11"),
                    TITLE = "GETについて",
                    UPDATED_DATE = DateTime.Parse("2026-10-11"),
                    USER_ID = 8
                   }
        };
            int expectedUserId = 42;
            var result = ValidateHelper.SetUserId(items, expectedUserId);

            Assert.All(result, item => Assert.Equal(expectedUserId, item.USER_ID));
        }
    }
}

