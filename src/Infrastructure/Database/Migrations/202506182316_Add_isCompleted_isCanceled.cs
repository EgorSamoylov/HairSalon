using FluentMigrator;

namespace Infrastructure.Database.Migrations
{
    [Migration(202506182316)]
    public class _202506182316Migration : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("appointments").Column("is_completed").Exists())
            {
                Alter.Table("appointments")
                    .AddColumn("is_completed").AsBoolean().NotNullable().WithDefaultValue(false);
            }

            if (!Schema.Table("appointments").Column("is_cancelled").Exists())
            {
                Alter.Table("appointments")
                    .AddColumn("is_cancelled").AsBoolean().NotNullable().WithDefaultValue(false);
            }
        }

        public override void Down()
        {
            if (Schema.Table("appointments").Column("is_completed").Exists())
            {
                Delete.Column("is_completed").FromTable("appointments");
            }

            if (Schema.Table("appointments").Column("is_cancelled").Exists())
            {
                Delete.Column("is_cancelled").FromTable("appointments");
            }
        }
    }
}