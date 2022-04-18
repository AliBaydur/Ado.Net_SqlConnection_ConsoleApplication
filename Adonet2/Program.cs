using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adonet2
{
    class Program
    {
        public static SqlConnection cnn = new SqlConnection(@"Server=ALI\SQLEXPRESS; Database=ETicaretFilm; Trusted_Connection=True;");
        static void Main(string[] args)
        {
            KayitGetirAdapter();
            Console.WriteLine("Lütfen yapmak istediğiniz işlemi seçiniz...");
            Console.WriteLine("WHERE (W)\t INSERT(I \t UPDATE(U)\t DELETE (D)");
            string islem = Console.ReadLine();
            if (islem.ToUpper() == "W")
            {
                Console.WriteLine("Lütfen Bir ID giriniz.");
                KayitGetirReader(Console.ReadLine());
            }
            else if (islem.ToUpper() == "I")
            {
                KayitEkle();
            }
            else if (islem.ToUpper() == "U")
            {
                Console.WriteLine("Lütfen KategoriID giriniz.");
                string id = Console.ReadLine();
                KayitGuncelle(id);
                KayitGetirReader(id);
            }
            else if (islem.ToUpper()== "D")
            {
                Console.WriteLine("Lütfen ID Giriniz.");
                string id = Console.ReadLine();
                KayitSil(id);                
            }
            Console.Read();
        }

        public static void KayitGetirAdapter()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Tbl_Urun", cnn);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = cmd;
            da.Fill(ds);

            Console.WriteLine("ID\tAdi\tKodu\tFiyat");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine(ds.Tables[0].Rows[i][0] + "\t" + ds.Tables[0].Rows[i][4] + "\t" + ds.Tables[0].Rows[i][3] + "\t" + ds.Tables[0].Rows[i][5]);
            }
            
        }

        public static void KayitGetirReader(string id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Tbl_Urun WHERE UrunID =@Id",cnn);
            cmd.Parameters.AddWithValue("@Id", id);
            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("ID\tAdi\tKodu\tFiyat");
            while (dr.Read())
            {
                Console.WriteLine(dr[0] + "\t" + dr[4] + "\t" + dr[3] + "\t" + dr[5].ToString());
            }
            dr.Close();
            cnn.Close();
        }

        public static void KayitEkle()
        {
            string  kodu, adi;
            int fiyat, kategori, altkategori;
            Console.WriteLine("Lütfen Ürün Kodunu Giriniz.");
            kodu = Console.ReadLine();
            Console.WriteLine("Lütfen Ürün Adını Giriniz.");
            adi = Console.ReadLine();
            Console.WriteLine("Lütfen Ürün Fiyatını Giriniz.");
            fiyat = int.Parse(Console.ReadLine());
            Console.WriteLine("Lütfen Ürün KategoriId Giriniz.");
            kategori = int.Parse(Console.ReadLine());
            Console.WriteLine("Lütfen Ürün AltKategoriId Giriniz.");
            altkategori = int.Parse(Console.ReadLine());
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Tbl_Urun(UrunKodu,UrunAdi,UrunFiyat,KategoriID,AltKategoriID) VALUES ('" + kodu + "','" + adi + "', "+ fiyat +","+ kategori +","+altkategori+")";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;
            cnn.Open();

            int kayit = cmd.ExecuteNonQuery();
            cnn.Close();

            Console.WriteLine(kayit.ToString() + "Adet Kayıt Eklendi..");
        }

        public static void KayitGuncelle(string id)
        {
            string adi, kodu;
            Console.WriteLine("Ürün Adını Giriniz:");
            adi = Console.ReadLine();
            Console.WriteLine("Ürün Kodunu Giriniz:");
            kodu = Console.ReadLine();

            SqlCommand cmd = new SqlCommand("UPDATE Tbl_Urun SET UrunAdi='" + adi + "',UrunKodu='" + kodu + "' WHERE UrunID = " + id + "", cnn);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            Console.WriteLine("Veri başarılı şekilde güncellendi...");
        }

        public static void KayitSil(string id)
        {
            SqlCommand cmd = new SqlCommand("SP_UrunlerDelete", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            Console.WriteLine("Veri Başarılı Bir Şekilde Silindi...");
        }
    }
}
