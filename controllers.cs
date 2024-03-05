using ky360datawebapi.models;
using Microsoft.AspNetCore.Mvc;

namespace ky360datawebapi
{
    public class EntitiesController : ControllerBase
    {
        private readonly MockedDatabase _database;

        public EntitiesController(MockedDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        [Route("api/entities")]
        public IActionResult GetEntities(string? search, string? gender, DateTime? startDate, DateTime? endDate, string[]? countries, string? sortBy, int page = 1, int pageSize = 10)
        {
            IEnumerable<Entity> entities = _database.GetEntities();
            // Note: These filter methods will change for actual database and will be handles using SQL queries
            if (!string.IsNullOrEmpty(search))
            {
                entities = entities.Where(e => e.Names.Any(n => n != null && (n.FirstName?.Contains(search, StringComparison.OrdinalIgnoreCase) == true || n.MiddleName?.Contains(search, StringComparison.OrdinalIgnoreCase) == true || n.Surname?.Contains(search, StringComparison.OrdinalIgnoreCase) == true)) || e.Addresses?.Any(a => a != null && (a.AddressLine?.Contains(search, StringComparison.OrdinalIgnoreCase) == true || a.Country?.Contains(search, StringComparison.OrdinalIgnoreCase) == true)) == true);
            }
            if (!string.IsNullOrEmpty(gender))
            {
                entities = entities.Where(e => e.Gender?.Equals(gender, StringComparison.OrdinalIgnoreCase) == true);
            }
            if (startDate.HasValue)
            {
                entities = entities.Where(e => e.Dates.Any(d => d != null && d.date >= startDate));
            }
            if (endDate.HasValue)
            {
                entities = entities.Where(e => e.Dates.Any(d => d != null && d.date <= endDate));
            }
            if (countries != null && countries.Length > 0)
            {
                entities = entities.Where(e => e.Addresses?.Any(a => a != null && countries.Contains(a.Country, StringComparer.OrdinalIgnoreCase)) == true);
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                string[] sortParams = sortBy.Split("");
                foreach (string param in sortParams)
                {
                    string field = param.Trim();
                    bool isDescending = field.StartsWith('-');
                    field = isDescending ? field.Substring(1) : field;

                    switch (field.ToLower())
                    {
                        case "id":
                            entities = isDescending ? entities.OrderByDescending(e => e.Id) : entities.OrderBy(e => e.Id);
                            break;
                    }
                }
            }
            IEnumerable<Entity> paginatedentries = entities.Skip((page - 1) * pageSize).Take(pageSize);
            int totalCount = entities.Count();
            return Ok(new PaginatedResponse<Entity>(paginatedentries,page, pageSize,totalCount));
        }

        [HttpGet]
        [Route("api/entity/{id}")]
        public IActionResult GetEntityById(string id)
        {
            Entity? entity = _database.GetEntityById(id);
            if(entity == null)
            {
                return NotFound("No such entity");
            }
            return Ok(entity);
        }

        [HttpPost]
        [Route("api/entity/create")]
        public IActionResult AddEntity([FromBody] Entity entity)
        {
            _database.AddEntity(entity);
            return Ok(entity);
        }

        [HttpPut]
        [Route("api/entity/{id}")]
        public IActionResult UpdateEntity(string id,[FromBody] Entity entity)
        {
            if(id != entity.Id)
            {
                return BadRequest("ID mismatch");
            }
            Entity? existing = _database.GetEntityById(entity.Id);
            if(existing == null)
            {
                return NotFound("Entity not found");
            }

            _database.UpdateEntity(entity);
            return NoContent();
        }

        [HttpDelete]
        [Route("api/entity/{id}")]
        public IActionResult DeleteEntity(string id)
        {
            Entity? entity = _database.GetEntityById(id);
            if(entity == null)
            {
                return NotFound("Entity not found");
            }
            _database.DeleteEntity(entity);
            return NoContent();
        }
    }

    public class PaginatedResponse<T>(IEnumerable<T> data, int page, int pageSize, int totalCount)
    {
        public IEnumerable<T> Data { get; set; } = data;
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = totalCount;
    }
}