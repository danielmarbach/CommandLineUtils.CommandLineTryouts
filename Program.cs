using System;

namespace CommandLineTryouts
{
    using McMaster.Extensions.CommandLineUtils;

    // usage:
    //  migrate-timeouts preview|migrate|abort ravendb --raven-specific-options rabbitmq --target-specific-options
    //  migrate-timeouts preview|migrate|abort sqlp --raven-specific-options sqlt --target-specific-options
    //  migrate-timeouts ravendb preview --raven-specific-options --target-specific-options
    //  migrate-timeouts ravendb migrate --raven-specific-options --target-specific-options [--cutoff-time] [--endpoint-filter]
    //  migrate-timeouts ravendb abort --raven-specific-options
    //  migrate-timeouts sqlp preview --sqlp-specific-options --target-specific-options
    //  migrate-timeouts sqlp migrate --sqlp-specific-options --target-specific-options [--cutoff-time] [--endpoint-filter]
    //  migrate-timeouts sqlp abort --sqlp-specific-options
    //
    // Examples:
    //  ravendb preview --serverUrl http://localhost:8080 --databaseName raven-timeout-test --prefix TimeoutDatas --ravenVersion 4 --target amqp://guest:guest@localhost:5672
    //  sqlp preview --source \"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyTestDB;Integrated Security=True;\" --dialect MsSqlServer --target amqp://guest:guest@localhost:5672
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "migrate-timeouts"
            };

            var verboseOption = app.Option("--verbose", "Show verbose output", CommandOptionType.NoValue, true);

            var connectionStringOption = new CommandOption($"--target", CommandOptionType.SingleValue)
            {
                Description = "The connection string for the target transport"
            };

            var rabbitMQConnectionStringOption = new CommandOption($"--target", CommandOptionType.SingleValue)
            {
                Description = "The connection string for the target transport"
            };

            var sqlSourceDialectOption = new CommandOption($"--dialect", CommandOptionType.SingleValue)
            {
                Description = "The SQL dialect"
            };

            var rabbitMqTargetUseTlsOption = new CommandOption($"--use-tls", CommandOptionType.SingleValue)
            {
                Description = ""
            };

            var ravenDBSourceServerUrlOption = new CommandOption($"--server-url", CommandOptionType.SingleValue)
            {
                Description = ""
            };

            var ravenDBSourceDatabaseNameOption = new CommandOption($"--database-name", CommandOptionType.SingleValue)
            {
                Description = ""
            };

            var sqlPOptions = new[]
            {
                connectionStringOption,
                sqlSourceDialectOption
            };

            var ravenDBOptions = new[]
            {
                ravenDBSourceServerUrlOption,
                ravenDBSourceDatabaseNameOption
            };

            var rabbitMqOptions = new[]
            {
                rabbitMQConnectionStringOption,
                rabbitMqTargetUseTlsOption
            };

            var sqlTOptions = new[]
            {
                connectionStringOption,
                sqlSourceDialectOption
            };

            app.HelpOption(inherited: true);

            app.Command("migrate", migrateCommand =>
            {
                // sources
                migrateCommand.Command("sqlp", sqlpCommand =>
                {
                    sqlpCommand.Options.AddRange(sqlPOptions);

                    sqlpCommand.Command("rabbitmq", rabbitmqCommand =>
                    {
                        rabbitmqCommand.Options.AddRange(rabbitMqOptions);

                        rabbitmqCommand.OnExecute(() =>
                        {
                            Console.WriteLine("Specify a subcommand");
                            app.ShowHelp();
                            return 1;
                        });
                    });

                    sqlpCommand.Command("sqlt", sqlTCommand =>
                    {
                        sqlTCommand.Options.AddRange(sqlTOptions);

                        sqlTCommand.OnExecute(() =>
                        {
                            Console.WriteLine("Specify a subcommand");
                            app.ShowHelp();
                            return 1;
                        });
                    });

                    sqlpCommand.OnExecute(() =>
                    {
                        Console.WriteLine("Specify a subcommand");
                        app.ShowHelp();
                        return 1;
                    });
                });

                migrateCommand.Command("ravendb", ravenDBCommand =>
                {
                    ravenDBCommand.Options.AddRange(ravenDBOptions);

                    ravenDBCommand.Command("rabbitmq", rabbitmqCommand =>
                    {
                        rabbitmqCommand.Options.AddRange(rabbitMqOptions);

                        rabbitmqCommand.OnExecute(() =>
                        {
                            Console.WriteLine("Specify a subcommand");
                            app.ShowHelp();
                            return 1;
                        });
                    });

                    ravenDBCommand.Command("sqlt", sqlTCommand =>
                    {
                        sqlTCommand.Options.AddRange(sqlTOptions);

                        sqlTCommand.OnExecute(() =>
                        {
                            Console.WriteLine("Specify a subcommand");
                            app.ShowHelp();
                            return 1;
                        });
                    });

                    ravenDBCommand.OnExecute(() =>
                    {
                        Console.WriteLine("Specify a subcommand");
                        app.ShowHelp();
                        return 1;
                    });
                });

                migrateCommand.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    app.ShowHelp();
                    return 1;
                });
            });

            app.OnExecute(() =>
            {
                Console.WriteLine("Specify a subcommand");
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }
    }
}
