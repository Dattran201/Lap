using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null ) 
            { 
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
         }

        private int TongSoLuong()
        {
            int tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            foreach (var item in lstGioHang)
            {
                tong += item.iSoLuong;
            }
            return tong;
        }

        private double TongTien()
        {
            double tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            foreach (var item in lstGioHang)
            {
                tong += item.dThanhTien;
            }
            return tong;
        }

        public ActionResult ThemMoiGioHang(int id, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.FirstOrDefault(s => s.iMaSach == id);
            if (sp != null)
            {
                lstGioHang.Add(new GioHang(id));
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
    }
}