using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OOAP_lab8.Models;

namespace OOAP_lab8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoController : ControllerBase
    {
        [HttpGet("breadth")]
        public IActionResult GetGeoTreeBreadth()
        {
            var tree = BuildGeoTree();
            var iterator = new BreadthFirstIterator(tree);

            var names = iterator.Select(node => node.Name).ToList();
            return Ok(names);
        }

        private GeoNode BuildGeoTree()
        {
            var country = new GeoNode("Україна");

            var oblast1 = new GeoNode("Львівська область");
            var oblast2 = new GeoNode("Київська область");

            var rayon1 = new GeoNode("Львівський район");
            var rayon2 = new GeoNode("Бродівський район");

            var city1 = new GeoNode("Львів");
            var city2 = new GeoNode("Броди");

            var street1 = new GeoNode("вул. Шевченка");
            var street2 = new GeoNode("вул. Франка");

            var rayon3 = new GeoNode("Києво-Святошинський район");
            var city3 = new GeoNode("Ірпінь");
            var street3 = new GeoNode("вул. Центральна");

            city1.AddChild(street1);
            city2.AddChild(street2);
            city3.AddChild(street3);

            rayon1.AddChild(city1);
            rayon2.AddChild(city2);
            rayon3.AddChild(city3);

            oblast1.AddChild(rayon1);
            oblast1.AddChild(rayon2);
            oblast2.AddChild(rayon3);

            country.AddChild(oblast1);
            country.AddChild(oblast2);

            return country;
        }
    }
}
