using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            IActionResult result;
            var entity = _context.CelestialObjects.SingleOrDefault(a => a.Id == id);
            if (entity != null)
            {
                AddSatelites(entity);

                result = Ok(entity);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.CelestialObjects;
            if ((list != null) && list.Any())
            {
                foreach (var celestialObject in list)
                {
                    AddSatelites(celestialObject);
                }
            }

            return Ok(list);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            IActionResult result;
            var list = _context.CelestialObjects.Where(a => a.Name == name);
            if ((list != null) && list.Any())
            {
                foreach (var celestialObject in list)
                {
                    AddSatelites(celestialObject);
                }

                result = Ok(list);
            }
            else
            {
                result = NotFound();
            }

            return result;

        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject celestialObject)
        {
            IActionResult result;
            var entity = _context.CelestialObjects.SingleOrDefault(a => a.Id == id);
            if (entity != null)
            {
                entity.Name = celestialObject.Name;
                entity.OrbitalPeriod = celestialObject.OrbitalPeriod;
                entity.OrbitedObjectId = celestialObject.OrbitedObjectId;

                _context.Update(entity);
                _context.SaveChanges();
                result = NoContent();
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            IActionResult result;
            var entity = _context.CelestialObjects.SingleOrDefault(a => a.Id == id);
            if (entity != null)
            {
                entity.Name = name;

                _context.Update(entity);
                _context.SaveChanges();
                result = NoContent();
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            IActionResult result;
            var entityList = _context.CelestialObjects.Where(a => a.Id == id || a.OrbitedObjectId == id);
            if ((entityList != null) && entityList.Any())
            {

                _context.RemoveRange(entityList);
                _context.SaveChanges();
                result = NoContent();
            }
            else
            {
                result = NotFound();
            }

            return result;
        }


        #region private helper methods
        private void AddSatelites(CelestialObject entity)
        {
            Debug.Assert(entity != null, "Entity must not be null");

            var satelites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == entity.Id);
            if (satelites != null && satelites.Any())
            {
                if (entity.Satellites == null)
                {
                    entity.Satellites = new List<CelestialObject>();
                }

                foreach (var satelite in satelites)
                {
                    entity.Satellites.Add(satelite);
                }
            }
        }
        #endregion
    }
}
