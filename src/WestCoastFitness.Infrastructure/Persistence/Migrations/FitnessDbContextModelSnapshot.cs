using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WestCoastFitness.Infrastructure.Persistence;

#nullable disable

namespace WestCoastFitness.Infrastructure.Persistence.Migrations;

[DbContext(typeof(FitnessDbContext))]
partial class FitnessDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("fitness").HasAnnotation("ProductVersion", "8.0.8");
        // Runtime model is built from IEntityTypeConfiguration; this snapshot
        // exists so `dotnet ef` tooling recognises a migrations assembly.
    }
}
