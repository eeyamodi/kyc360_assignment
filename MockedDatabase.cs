using ky360datawebapi.models;

namespace ky360datawebapi
{
    public class MockedDatabase
    {
        private static readonly Random random = new();
        private readonly List<Entity> _entities;

        public MockedDatabase()
        {
            _entities = new List<Entity>(random.Next(1, 50));
            for (int i = 0; i < _entities.Capacity; i++)
            {
                _entities.Add(EntityDataGenerator.GenerateEntity());
            }
        }

        public IEnumerable<Entity> GetEntities()
        {
            return _entities;
        }

        public Entity? GetEntityById(string id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void UpdateEntity(Entity entity)
        {
            var index = _entities.FindIndex(e => e.Id == entity.Id);
            if (index != -1)
            {
                _entities[index] = entity;
            }
        }

        public void DeleteEntity(Entity entity)
        {
            _entities.RemoveAll(e => e.Id == entity.Id);
        }
    }
}