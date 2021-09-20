using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents entity ID
    /// </summary>
    /// <typeparam name="TEntityId">The type of the entity identifier.</typeparam>
    public class EntityId<TEntityId> : IEntityId<TEntityId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityId{TEntityId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public EntityId(TEntityId id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public TEntityId Id { get; set; }
    }
}
