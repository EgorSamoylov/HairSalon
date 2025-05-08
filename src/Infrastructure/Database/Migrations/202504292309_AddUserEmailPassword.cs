using FluentMigrator;

namespace Infrastructure.Database.Migrations
{
    [Migration(202504292309)]
    public class AddUserEmailPassword : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE TYPE user_role AS ENUM ('User', 'Admin')");

            Alter.Table("users")
                .AddColumn("password_hash").AsString(255).Nullable()
                .AddColumn("role").AsCustom("user_role").WithDefaultValue("User");
        }

        public override void Down()
        {
            Delete.Column("password_hash")
                .Column("role")
                .FromTable("users");

            Execute.Sql("DROP TYPE user_role");
        }
    }
}
