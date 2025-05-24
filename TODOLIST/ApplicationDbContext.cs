using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Xml;
using TODOLIST.Domain;

namespace TODOLIST
{
    /// <summary>
    /// アプリケーションデータベースコンテキストクラス
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todolist> Todolists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var environmentConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            if (!string.IsNullOrEmpty(environmentConnectionString))
            {
                // 環境変数の接続文字列を優先する
                options.UseSqlServer(environmentConnectionString);
            }
            else
            {
                // 環境変数がない場合、appsettings.json を読み込む
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        // OnModelCreating メソッドでエンティティの設定を行う
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // 必要に応じてベースクラスの OnModelCreating を呼び出す

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<User>().HasData(
            new User { USER_ID = 1, USER_NAME = "Taro", EMAIL_ADDRESS = "Taro@gmail.com", PASSWORD = "rewkwekwe" },
            new User { USER_ID = 2, USER_NAME = "Jiro", EMAIL_ADDRESS = "Jiro@gmail.com", PASSWORD = "erkewjwek" }
            );

            modelBuilder.Entity<Todolist>(entity =>
            {
                // 自動生成されるように設定
                entity.Property(t => t.TODOLIST_ID)
                      .ValueGeneratedOnAdd();
            });


            modelBuilder.Entity<Todolist>().HasData(
             new Todolist { TODOLIST_ID = 1,
                            USER_ID = 1,
                            TITLE = "GETについて",
                            CONTENT = "内容1",
                            CREATED_DATE = new DateTime(2022,4,11),
                            UPDATED_DATE = new DateTime(2022,5,5)
             },
             new Todolist { TODOLIST_ID = 2,
                            USER_ID = 2,
                            TITLE = "PUTについて",
                            CONTENT = "内容2",
                            CREATED_DATE = new DateTime(2022,2,1),
                            UPDATED_DATE = new DateTime(2022,2,24)
             }
            );

        }
    }
}