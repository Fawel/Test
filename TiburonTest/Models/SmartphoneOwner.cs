using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TiburonTest.Models
{
    public class SmartphoneOwner
    {
        public string Gender { get; set; }
        public int Count { get; set;}

    }
    public class Smartphones
    {
        public bool Owned { get; set; }
        public string BrandName { get; set; }
    }
    public class FinalViewModel
    {
        public List<SmartphoneOwner> Smartphoneowner { get; set; }
        public List<string> Genders { get; set; }
        public string Brand { get; set; }
        [Required(ErrorMessage ="Укажите пол")]
        public string Gender { get; set; }
        [Brand_AtLeastOneChecked(ErrorMessage ="Выберите хотя бы один бренд")]
        public List<Smartphones> BrandsOwned { get; set; }
    }
    public class Brand_AtLeastOneChecked : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            List<Smartphones> validatelist = (List<Smartphones>)value;
            for (int i = 0; i < validatelist.Count; i++)
            {
                if (validatelist[i].Owned == true)
                    return true;
            }
            return false;
        }
    }

}