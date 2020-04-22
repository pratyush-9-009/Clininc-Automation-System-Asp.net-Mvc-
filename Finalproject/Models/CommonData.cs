using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Finalproject.Models;

namespace Finalproject.Models
{
    public class CommonData
    {
        public List<SelectListItem> RoleNName()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (ProjectEntities1 ie = new ProjectEntities1())
            {
                var gdata = ie.RoleDetails.ToList();
                foreach (var item in gdata)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleId.ToString()

                    });
                }
            }
            return lst;
            /*RoleModel model = new RoleModel();
            model.listItems = lst;*/
        }

        public List<SelectListItem> DoctorSpecs()
        {
            List<SelectListItem> lst_d = new List<SelectListItem>();
            using (ProjectEntities1 ie = new ProjectEntities1())
            {
                var gdata = ie.SpecializedDatas.ToList();
                foreach (var item in gdata)
                {
                    lst_d.Add(new SelectListItem
                    {
                        Text = item.SpecializedName,
                        Value = item.SpecializedId.ToString()

                    });
                }
            }
            return lst_d;
        }

        public List<SelectListItem> DruGdetail()
        {
            List<SelectListItem> lst_d = new List<SelectListItem>();
            using (ProjectEntities1 ie = new ProjectEntities1())
            {
                var gdata = ie.Drugs.ToList();
                foreach (var item in gdata)
                {
                    lst_d.Add(new SelectListItem
                    {
                        Text = item.DrugName,
                        Value = item.DrugId.ToString()

                    });
                }
            }
            return lst_d;
        }

        public List<SelectListItem> DoctorApp()
        {
            List<SelectListItem> lst_d = new List<SelectListItem>();
            using (ProjectEntities1 ie = new ProjectEntities1())
            {
                
                var gdata = ie.Doctors.ToList();
                
                foreach (var item in gdata)
                {
                    var gdata2 = ie.SpecializedDatas.FirstOrDefault(a => a.SpecializedId == item.SpecializedId);
                    lst_d.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName + " [ " + gdata2.SpecializedName+" ]",
                        Value = item.DoctorId.ToString()

                    }) ;
                }
            }
            return lst_d;
        }

        public List<SelectListItem> SupplierNName()
        {
            List<SelectListItem> lst_s = new List<SelectListItem>();
            using (ProjectEntities1 ie = new ProjectEntities1())
            {
                var gdata = ie.Suppliers.ToList();
                foreach (var item in gdata)
                {
                    lst_s.Add(new SelectListItem
                    {
                        Text = item.FirstName+" "+item.LastName,
                        Value = item.SupplierId.ToString()

                    });
                }
            }
            return lst_s;
        }
    }
}