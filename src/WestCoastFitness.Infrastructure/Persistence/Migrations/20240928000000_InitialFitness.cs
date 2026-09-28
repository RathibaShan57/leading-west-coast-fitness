using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestCoastFitness.Infrastructure.Persistence.Migrations;

public partial class InitialFitness : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "fitness");
        migrationBuilder.CreateTable(
            name: "locations",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                City = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                State = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                PostalCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                TimeZone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                CityPrivilegeTaxRate = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                StudioCapacity = table.Column<int>(type: "integer", nullable: false),
                OutdoorPatio = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_locations", x => x.Id));

        migrationBuilder.CreateTable(
            name: "members",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FirstName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                LastName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                HomeLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                JoinedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                WaiverSigned = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_members", x => x.Id));
        migrationBuilder.CreateIndex(name: "IX_members_Email", schema: "fitness", table: "members", column: "Email", unique: true);

        migrationBuilder.CreateTable(
            name: "plans",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Tier = table.Column<int>(type: "integer", nullable: false),
                MonthlyPriceUsd = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                IncludedClassesPerMonth = table.Column<int>(type: "integer", nullable: false),
                IncludesGuestPass = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_plans", x => x.Id));
        migrationBuilder.CreateIndex(name: "IX_plans_Code", schema: "fitness", table: "plans", column: "Code", unique: true);

        migrationBuilder.CreateTable(
            name: "trainers",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FullName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                Specialty = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                YearsExperience = table.Column<int>(type: "integer", nullable: false),
                HomeLocationId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_trainers", x => x.Id));

        migrationBuilder.CreateTable(
            name: "classes",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                Discipline = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                Intensity = table.Column<int>(type: "integer", nullable: false),
                Outdoor = table.Column<bool>(type: "boolean", nullable: false),
                LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                TrainerId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_classes", x => x.Id));

        migrationBuilder.CreateTable(
            name: "sessions",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FitnessClassId = table.Column<Guid>(type: "uuid", nullable: false),
                LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                StartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Capacity = table.Column<int>(type: "integer", nullable: false),
                BookedCount = table.Column<int>(type: "integer", nullable: false),
                Cancelled = table.Column<bool>(type: "boolean", nullable: false),
                CancellationReason = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_sessions", x => x.Id));

        migrationBuilder.CreateTable(
            name: "bookings",
            schema: "fitness",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CheckedInAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_bookings", x => x.Id));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "bookings", schema: "fitness");
        migrationBuilder.DropTable(name: "sessions", schema: "fitness");
        migrationBuilder.DropTable(name: "classes", schema: "fitness");
        migrationBuilder.DropTable(name: "trainers", schema: "fitness");
        migrationBuilder.DropTable(name: "plans", schema: "fitness");
        migrationBuilder.DropTable(name: "members", schema: "fitness");
        migrationBuilder.DropTable(name: "locations", schema: "fitness");
    }
}
