using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Alter.Table("clients")
                .AddColumn("logo_attachment_id").AsInt32().Nullable();

            Alter.Table("employees")
                .AddColumn("logo_attachment_id").AsInt32().Nullable();

            Create.ForeignKey("fk_clients_logo_attachment_id")
                .FromTable("clients").ForeignColumn("logo_attachment_id")
                .ToTable("attachments").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.SetNull);

            Create.ForeignKey("fk_employees_logo_attachment_id")
                .FromTable("employees").ForeignColumn("logo_attachment_id")
                .ToTable("attachments").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.SetNull);
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_clients_logo_attachment_id").OnTable("clients");
            Delete.Column("logo_attachment_id").FromTable("clients");
            Delete.ForeignKey("fk_employees_logo_attachment_id").OnTable("employees");

            Delete.Column("logo_attachment_id").FromTable("employees");
            Delete.Table("attachments");
        }
    }
}
