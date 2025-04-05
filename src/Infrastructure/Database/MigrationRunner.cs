using FluentMigrator.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
