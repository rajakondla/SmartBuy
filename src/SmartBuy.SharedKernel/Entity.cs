using System;

namespace SmartBuy.SharedKernel
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        public TId Id { get; protected set; }

        protected Entity(TId id) : this()
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("Id value cannot be a default value");
            }
            Id = id;
        }

        public Entity()
        {
        }

        public override bool Equals(object other)
        {
            var entity = other as Entity<TId>;

            if (entity != null)
            {
                return this.Equals(entity);
            }

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }
    }
}
