using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SachOnline.Models
{
    public class GioHang
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien 
        {
            get { return iSoLuong * dDonGia; }
        }

    public GioHang(int ms)
        {
        iMaSach = ms;
            // Truy vấn lấy sách
         var sach = db.SACHes.FirstOrDefault(s => s.MaSach == ms);
            sTenSach = sach.TenSach;
            sAnhBia = sach.AnhBia;
            dDonGia = double.Parse(sach.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}