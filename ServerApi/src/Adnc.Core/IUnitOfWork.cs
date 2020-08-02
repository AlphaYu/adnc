using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Core
{
    public interface IUnitOfWork
    {
        //
        // 摘要:
        //     Gets the current Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction
        //     being used by the context, or null if no transaction is in use.
        //     This property will be null unless one of the 'BeginTransaction' or 'UseTransaction'
        //     methods has been called, some of which are available as extension methods installed
        //     by EF providers. No attempt is made to obtain a transaction from the current
        //     DbConnection or similar.
        //     For relational databases, the underlying DbTransaction can be obtained using
        //     the 'Microsoft.EntityFrameworkCore.Storage.GetDbTransaction' extension method
        //     on the returned Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.
       IDbContextTransaction CurrentTransaction { get; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether or not a transaction will be created
        //     automatically by Microsoft.EntityFrameworkCore.DbContext.SaveChanges if none
        //     of the 'BeginTransaction' or 'UseTransaction' methods have been called.
        //     Setting this value to false will also disable the Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy
        //     for Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     The default value is true, meaning that SaveChanges will always use a transaction
        //     when saving changes.
        //     Setting this value to false should only be done with caution since the database
        //     could be left in a corrupted state if SaveChanges fails.
       bool AutoTransactionsEnabled { get;}
        //
        // 摘要:
        //     Returns the name of the database provider currently in use. The name is typically
        //     the name of the provider assembly. It is usually easier to use a sugar method
        //     such as 'IsSqlServer()' instead of calling this method directly.
        //     This method can only be used after the Microsoft.EntityFrameworkCore.DbContext
        //     has been configured because it is only then that the provider is known. This
        //     means that this method cannot be used in Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)
        //     because this is where application code sets the provider to use as part of configuring
        //     the context.
       string ProviderName { get; }

        //
        // 摘要:
        //     Starts a new transaction.
        //
        // 返回结果:
        //     A Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction that represents
        //     the started transaction.
       IDbContextTransaction BeginTransaction();
        //
        // 摘要:
        //     Asynchronously starts a new transaction.
        //
        // 参数:
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // 返回结果:
        //     A task that represents the asynchronous transaction initialization. The task
        //     result contains a Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction
        //     that represents the started transaction.
       Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        //
        // 摘要:
        //     Determines whether or not the database is available and can be connected to.
        //     Note that being able to connect to the database does not mean that it is up-to-date
        //     with regard to schema creation, etc.
        //
        // 返回结果:
        //     True if the database is available; false otherwise.
       bool CanConnect();
        //
        // 摘要:
        //     Determines whether or not the database is available and can be connected to.
        //     Note that being able to connect to the database does not mean that it is up-to-date
        //     with regard to schema creation, etc.
        //
        // 参数:
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // 返回结果:
        //     True if the database is available; false otherwise.
       Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
        //
        // 摘要:
        //     Applies the outstanding operations in the current transaction to the database.
       void CommitTransaction();
        //
        // 摘要:
        //     Creates an instance of the configured Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy.
        //
        // 返回结果:
        //     An Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy instance.
       //IExecutionStrategy CreateExecutionStrategy();
        //
        // 摘要:
        //     Ensures that the database for the context exists. If it exists, no action is
        //     taken. If it does not exist then the database and all its schema are created.
        //     If the database exists, then no effort is made to ensure it is compatible with
        //     the model for this context.
        //     Note that this API does not use migrations to create the database. In addition,
        //     the database that is created cannot be later updated using migrations. If you
        //     are targeting a relational database and using migrations, you can use the DbContext.Database.Migrate()
        //     method to ensure the database is created and all migrations are applied.
        //
        // 返回结果:
        //     True if the database is created, false if it already existed.
       //bool EnsureCreated();
        //
        // 摘要:
        //     Asynchronously ensures that the database for the context exists. If it exists,
        //     no action is taken. If it does not exist then the database and all its schema
        //     are created. If the database exists, then no effort is made to ensure it is compatible
        //     with the model for this context.
        //     Note that this API does not use migrations to create the database. In addition,
        //     the database that is created cannot be later updated using migrations. If you
        //     are targeting a relational database and using migrations, you can use the DbContext.Database.Migrate()
        //     method to ensure the database is created and all migrations are applied.
        //
        // 参数:
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // 返回结果:
        //     A task that represents the asynchronous save operation. The task result contains
        //     true if the database is created, false if it already existed.
       //Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default);
        //
        // 摘要:
        //     Ensures that the database for the context does not exist. If it does not exist,
        //     no action is taken. If it does exist then the database is deleted.
        //     Warning: The entire database is deleted, and no effort is made to remove just
        //     the database objects that are used by the model for this context.
        //
        // 返回结果:
        //     True if the database is deleted, false if it did not exist.
       //bool EnsureDeleted();
        //
        // 摘要:
        //     Asynchronously ensures that the database for the context does not exist. If it
        //     does not exist, no action is taken. If it does exist then the database is deleted.
        //     Warning: The entire database is deleted, and no effort is made to remove just
        //     the database objects that are used by the model for this context.
        //
        // 参数:
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // 返回结果:
        //     A task that represents the asynchronous save operation. The task result contains
        //     true if the database is deleted, false if it did not exist.
       //Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default);
        //
        // 摘要:
        //     Determines whether the specified object is equal to the current object.
        //
        // 参数:
        //   obj:
        //     The object to compare with the current object.
        //
        // 返回结果:
        //     true if the specified object is equal to the current object; otherwise, false.
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public override bool Equals(object obj);
        //
        // 摘要:
        //     Serves as the default hash function.
        //
        // 返回结果:
        //     A hash code for the current object.
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public override int GetHashCode();
        //
        // 摘要:
        //     Discards the outstanding operations in the current transaction.
       void RollbackTransaction();
        //
        // 摘要:
        //     Returns a string that represents the current object.
        //
        // 返回结果:
        //     A string that represents the current object.
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public override string ToString();
    }
}
