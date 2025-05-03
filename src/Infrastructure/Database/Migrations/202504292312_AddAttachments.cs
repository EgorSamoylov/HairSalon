using FluentMigrator;
using System.Data;

namespace Infrastructure.Database.Migrations
{
    [Migration(202504292312)]
    public class Migration_202504260913_AddAttachments : Migration
    {
        public override void Up()
        {
            Create.Table("attachments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("file_name").AsString().NotNullable()
                .WithColumn("stored_path").AsString().NotNullable()
                .WithColumn("content_type").AsString().NotNullable()
                .WithColumn("size").AsInt64().NotNullable()
                .WithColumn("created_at").AsDateTime().NotNullable();

            Alter.Table("users")
                .AddColumn("logo_attachment_id").AsInt32().Nullable();

            Create.ForeignKey("fk_users_logo_attachment_id")
                .FromTable("users").ForeignColumn("logo_attachment_id")
                .ToTable("attachments").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.SetNull);
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_users_logo_attachment_id").OnTable("users");
            Delete.Column("logo_attachment_id").FromTable("users");
            Delete.Table("attachments");
        }
    }
}
