using Adnc.Infra.IRepositories;
using Adnc.Infra.Repository.Mongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Adnc.Infra.Repository.Mongo.Configuration
{
    /// <summary>
    /// A configuration builder for this package.
    /// </summary>
    public class MongoConfigurationBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConfigurationBuilder" /> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public MongoConfigurationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the service collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Searches the specified assembly for type and registers them.
        /// * Registers all public implementations of <see cref="IMongoRepository{TEntity}"/> as each of their non-generic interfaces.
        /// * Adds entity configuration provided by implementations of <see cref="IMongoEntityConfiguration{TEntity}"/>.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public MongoConfigurationBuilder FromAssembly(Assembly assembly)
        {
            // Repositories.
            foreach (var (repositoryType, _) in GetWithGenericInterface(assembly, typeof(IMongoRepository<>)))
            {
                foreach (var i in repositoryType.GetInterfaces().Where(i => !i.IsGenericType))
                {
                    Services.AddTransient(i, repositoryType);
                }
            }

            // Configuration
            foreach (var (configurationType, entityType) in GetWithGenericInterface(assembly, typeof(IMongoEntityConfiguration<>)))
            {
                Services.AddTransient(
                    typeof(IMongoEntityConfiguration<>).MakeGenericType(entityType),
                    configurationType);
            }

            return this;
        }

        /// <summary>
        /// Registers all public implementations of <see cref="IMongoRepository{TEntity}"/> as each of their non-generic interfaces.
        /// Adds indexes defined in public implementations of <see cref="IMongoEntityConfiguration{TEntity}"/>.
        /// </summary>
        /// <returns></returns>
        public MongoConfigurationBuilder FromAssemblyContaining<T>() => FromAssembly(typeof(T).Assembly);

        private static IEnumerable<(Type type, Type genericType)> GetWithGenericInterface(Assembly assembly, Type genericTypeDefinition) =>
            assembly.ExportedTypes
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface)
                .Select(t =>
                {
                    var genericInterface = t.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition);

                    return genericInterface == null ? default : (t, genericInterface.GenericTypeArguments.First());
                })
                .Where(x => x != default);
    }
}