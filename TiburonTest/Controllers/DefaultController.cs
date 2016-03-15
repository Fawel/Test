using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiburonTest.Models;
using System.Threading.Tasks;

namespace TiburonTest.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            DB workWithDB = new DB();
            FinalViewModel Owner = new FinalViewModel
            {
                BrandsOwned=new List<Smartphones>(workWithDB.GetBrandList()),
                Genders=new List<string>(workWithDB.GetGenders())
            };
            return View(Owner);
        }

        [HttpPost]
        public ActionResult Index(FinalViewModel _owner)
        {
            DB workWithDB = new DB();
            
            if (ModelState.IsValid)
            {
                List<FinalViewModel> res = new List<FinalViewModel>();
                List<Smartphones> brands = new List<Smartphones>(workWithDB.GetBrandList());
                ViewBag.genders = workWithDB.GetGenders();
                foreach (var item in _owner.BrandsOwned)
                {
                    if (item.Owned == true)
                    {
                        workWithDB.UpdateCount(item.BrandName, _owner.Gender);
                    }
                }

                foreach (var item in brands)
                {
                    {
                        res.Add(new FinalViewModel { Brand = item.BrandName, Smartphoneowner = workWithDB.GetBrandUsers(item.BrandName) });

                    }
                }
                return PartialView("result", res);
            }
            else
            {
                _owner.Genders = new List<string>(workWithDB.GetGenders());

                return PartialView(_owner);
            }
        }
    }
}