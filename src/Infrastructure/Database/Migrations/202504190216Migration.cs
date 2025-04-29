using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Migrations
{
    [Migration(202504190216)]
    public class _202504190216Migration : Migration

    {
        public override void Up()
        {
            if (Schema.Table("appointments").Column("amenity_id").Exists())
            {
                Rename.Column("amenity_id")
                  .OnTable("appointments")
                  .To("service_id");
            }
        }

        public override void Down()
        {
            if (Schema.Table("appointments").Column("service_id").Exists())
            {
                Rename.Column("service_id")
                  .OnTable("appointments")
                  .To("amenity_id");
            }
        }
    }
}
