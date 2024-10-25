using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;


namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        QLBANSACHEntities db = new QLBANSACHEntities();
        public ActionResult Index()
        {
            var lsSach = db.SACHes.OrderByDescending(s => s.NgayCapNhat).Take(6).ToList();
            return View(lsSach);
        }

        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in db.CHUDEs select cd;
            return PartialView(listChuDe);
        }

        public ActionResult NXBPartial()
        {
            var listNXB = from nxb in db.NHAXUATBANs select nxb;
            return PartialView(listNXB);
        }
        public class NavViewModel
        {
            public List<CHUDE> ChuDeList { get; set; }
            public List<NHAXUATBAN> NXBList { get; set; }
        }

        public ActionResult SlidePartial()
        {
            return PartialView();
        }

        public ActionResult NavbarPartial()
        {
            var listChuDe = db.CHUDEs.ToList();
            var listNXB = db.NHAXUATBANs.ToList();

            var model = new NavViewModel
            {
                ChuDeList = listChuDe,
                NXBList = listNXB
            };

            return PartialView(model);
        }


        public ActionResult SachBanNhieuPartical()
        {
            var listSachBanNhieu = db.SACHes.OrderByDescending(s => s.SoLuongBan).Take(6).ToList();
            return PartialView(listSachBanNhieu);
        }

        public ActionResult SachTheoChuDe(int id)
        {
            var lsCD = db.SACHes.Where(s => s.MaCD == id).ToList();
            return View("Index", lsCD);
        }

        public ActionResult SachTheoNXB(int id)
        {
            var lsNXB = db.SACHes.Where(s => s.MaNXB == id).ToList();
            return View("Index", lsNXB);
        }

        public ActionResult TimKiem(string keyword)
        {
            var lsSach = db.SACHes.Where(s => s.TenSach.Contains(keyword) || s.MoTa.Contains(keyword)).ToList();
            return View("Index", lsSach);
        }
        public ActionResult ChitietSach(int id)
        {
            var ctSach = db.SACHes.FirstOrDefault(s => s.MaSach == id);
            return View(ctSach);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            //Gan cac gia tri nguoi dung nhap du lieu cho cac bien
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return RedirectToAction("DangNhap", "SachOnline");
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View();
        }
    }
} 