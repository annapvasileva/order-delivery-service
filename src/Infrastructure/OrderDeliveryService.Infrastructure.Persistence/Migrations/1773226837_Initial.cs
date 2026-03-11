using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace OrderDeliveryService.Infrastructure.Persistence.Migrations;

[Migration(1773226837, "Initial")]
public class InitialMigration : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
                           create table if not exists orders
                           (
                               order_id         bigint primary key generated always as identity,
                           
                               senders_city             text                        not null,
                               senders_address          text                        not null,
                               recipients_city          text                        not null,
                               recipients_address       text                        not null,
                               cargo_weight             decimal(12,3)               not null,
                               cargo_collection_date    timestamp with time zone    not null
                           );
                           """,
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
                           drop table if exists orders;
                           """,
        });
    }

    public string ConnectionString => throw new NotSupportedException();
}