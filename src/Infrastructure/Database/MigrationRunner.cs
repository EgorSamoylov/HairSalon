using FluentMigrator.Runner;

namespace Infrastructure.Database
{
    public class MigrationRunner
    {
        private readonly IMigrationRunner _migrationRunner;

        public MigrationRunner(IMigrationRunner migrationRunner)
        {
            _migrationRunner = migrationRunner;
        }

        public void Run()
        {
            _migrationRunner.MigrateUp();
        }
    }
}
