using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Migrations
{
    [Migration(202503181541)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("employees")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("first_name").AsString(100).NotNullable()
                .WithColumn("last_name").AsString(100).NotNullable()
                .WithColumn("phone_number").AsString(24).NotNullable()
                .WithColumn("email").AsString(100).NotNullable()
                .WithColumn("position").AsString(255).Nullable();

            Create.Table("clients")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("first_name").AsString(100).NotNullable()
                .WithColumn("last_name").AsString(100).NotNullable()
                .WithColumn("phone_number").AsString(24).NotNullable()
                .WithColumn("email").AsString(100).NotNullable()
                .WithColumn("note").AsString(255).Nullable();

            Create.Table("amenities")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("service_name").AsString(100).NotNullable()
                .WithColumn("description").AsString(255).NotNullable()
                .WithColumn("author_id").AsInt32().NotNullable().ForeignKey("employees", "id")
                .WithColumn("price").AsInt32().NotNullable()
                .WithColumn("duration_minutes").AsInt32().NotNullable();

            Create.Table("appointments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("client_id").AsInt32().NotNullable().ForeignKey("clients", "id")
                .WithColumn("employee_id").AsInt32().NotNullable().ForeignKey("employees", "id")
                .WithColumn("amenity_id").AsInt32().NotNullable().ForeignKey("amenities", "id")
                .WithColumn("appointment_datetime").AsDateTime().NotNullable()
                .WithColumn("notes").AsInt32().Nullable();

            Insert.IntoTable("employees")
                .Row(new
                {
                    first_name = "Fedor",
                    last_name = "Grach",
                    phone_number = "+7 980 528 24 89",
                    email = "mr.fedorGrach@mail.ru",
                    position = "hairdresser"
                });

            Insert.IntoTable("clients")
                .Row(new
                {
                    first_name = "George",
                    last_name = "Vasiliev",
                    phone_number = "+7 970 533 31 05",
                    email = "mr.GeorgeVasiliev@mail.ru",
                    note = "Подравнять виски"
                });

            Insert.IntoTable("amenities")
                .Row(new
                {
                    service_name = "Площадка",
                    description = "Волосы приподнимают и стригут ровно, как бы площадкой",
                    author_id = 1,
                    price = 800,
                    duration_minutes = 40
                });

            Insert.IntoTable("appointments")
                .Row(new
                {
                    client_id = 1,
                    empployee_id = 1,
                    amenity_id = 1,
                    appointment_datetime = DateTime.Now,
                    notes = "Клиент просит, чтобы подравняли виски и задушевно поговорили"
                });
        }

        public override void Down() // откат миграции 
        {
            Delete.Table("appointments");
            Delete.Table("amenities");
            Delete.Table("clients");
            Delete.Table("employees");
        }
    }
}
