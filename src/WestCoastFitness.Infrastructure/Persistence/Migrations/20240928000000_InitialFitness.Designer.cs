using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WestCoastFitness.Infrastructure.Persistence;

#nullable disable

namespace WestCoastFitness.Infrastructure.Persistence.Migrations;

[DbContext(typeof(FitnessDbContext))]
[Migration("20240928000000_InitialFitness")]
partial class InitialFitness
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("fitness").HasAnnotation("ProductVersion", "8.0.8");
    }
}
