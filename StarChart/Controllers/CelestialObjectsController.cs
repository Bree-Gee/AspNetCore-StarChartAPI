using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectsController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
