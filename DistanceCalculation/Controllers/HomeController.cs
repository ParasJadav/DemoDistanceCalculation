using DistanceCalculation.Models;
using DistanceCalculation.Repository;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace DistanceCalculation.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            AddressModel address = new AddressModel();
            address.DistanceFrom = string.Empty;
            return View(address);
        }

        [HttpPost]
        public JsonResult GetLocationNameByKeyword(string SearchedString)
        {
            IEnumerable<Locations> filteredlocations = DistanceRepository.GetLocations();
            if (filteredlocations != null && filteredlocations.Count() > 0)
            {
                filteredlocations = filteredlocations.Where(a => a.Name.Contains(SearchedString)).ToList();
            }
            
            return Json(filteredlocations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDistanceByAddress(string selectedAddress)
        {
            IEnumerable<Locations> listoflocations = DistanceRepository.GetLocations();
            List<DistanceModels> listDistance = new List<DistanceModels>();
            if (listoflocations != null && listoflocations.Count() > 0)
            {
                List<string> lstAddresses = listoflocations.Where(a => a.Name != selectedAddress).Select(a => a.Name).ToList();
                string joinedAddress = string.Join("|", lstAddresses);
                listDistance = DistanceRepository.Calculate(selectedAddress, joinedAddress);
            }
            return Json(listDistance, JsonRequestBehavior.AllowGet);
        }

        

    }
}