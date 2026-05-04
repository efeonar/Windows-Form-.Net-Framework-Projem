using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace proje2
{
    public partial class Form1 : Form
    {
        // Ürün ve Fiyatlandırma Özellikleri (Yeni Gelişmiş Yapı)
        class Urun
        {
            public string Ad { get; set; }
            public string ResimUrl { get; set; }
            public decimal TabanFiyat { get; set; } // Standart (Küçük/Baz) fiyat

            // Farklı boyutlar için ek fiyatlar (e.g., "Orta": +30, "Büyük": +60)
            public Dictionary<string, decimal> BoyutFiyatlari { get; set; } = new Dictionary<string, decimal>();

            // Ekstra malzemeler ve fiyatları (e.g., "Ekstra Peynir": +20)
            public Dictionary<string, decimal> EkstraMalzemeler { get; set; } = new Dictionary<string, decimal>();
        }

        // Arayüzdeki kutucukları takip etmek için güncellenmiş sınıf
        class UrunKartArayuzu
        {
            public Urun Urun { get; set; }
            public PictureBox Pb { get; set; }
            public TextBox TxtFiyat { get; set; } // Artık fiyatı sadece gösterecek (Read-only)
            public ComboBox CmbBoyut { get; set; } // BOYUT SEÇİMİ
            public CheckedListBox ClbEkstralar { get; set; } // EKSTRA MALZEMELER
            public NumericUpDown NudAdet { get; set; }
        }

        List<Urun> menu = new List<Urun>();
        List<UrunKartArayuzu> kartlar = new List<UrunKartArayuzu>();
        Label lblToplam = new Label();

        public Form1()
        {
            // Form ayarları
            this.Text = "Gelişmiş Restoran Sipariş Sistemi";
            this.Size = new Size(800, 600); // Ekstra kutular için formu büyüttük
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 244, 248);

            MenuOlustur();
            ArayuzuCiz();
        }

        private void MenuOlustur()
        {
            // 1. BURGER (Sadece Boyut Seçenekli)
            Urun burger = new Urun { Ad = "Classic Burger", TabanFiyat = 220m, ResimUrl = "https://cdn-icons-png.flaticon.com/128/3075/3075977.png" };
            burger.BoyutFiyatlari.Add("Küçük", 0);
            burger.BoyutFiyatlari.Add("Orta (+30₺)", 30);
            burger.BoyutFiyatlari.Add("Büyük (+60₺)", 60);
            menu.Add(burger);

            // 2. CAJUN MIX (Sadece Boyut Seçenekli)
            Urun cajun = new Urun { Ad = "Cajun Mix Menü", TabanFiyat = 195.5m, ResimUrl = "https://cdn-icons-png.flaticon.com/128/1046/1046786.png" };
            cajun.BoyutFiyatlari.Add("Orta", 0); // Cajunda standart boy orta olsun
            cajun.BoyutFiyatlari.Add("Büyük (+40₺)", 40);
            menu.Add(cajun);

            // 3. SANDVİÇ (Senin seçtiğin ikonla) + Boyut + Ekstralar
            Urun sandwich = new Urun { Ad = "B.M.T. Sandviç", TabanFiyat = 160m, ResimUrl = "https://cdn-icons-png.flaticon.com/128/1625/1625062.png" };
            sandwich.BoyutFiyatlari.Add("Küçük", 0);
            sandwich.BoyutFiyatlari.Add("Orta (+25₺)", 25);
            sandwich.BoyutFiyatlari.Add("Büyük (+50₺)", 50);
            sandwich.EkstraMalzemeler.Add("Ekstra Peynir (+20₺)", 20);
            sandwich.EkstraMalzemeler.Add("Ekstra Sos (+10₺)", 10);
            sandwich.EkstraMalzemeler.Add("Acı Biber (+15₺)", 15);
            menu.Add(sandwich);

            // 4. PİZZA + Boyut + Çoklu Ekstralar
            Urun pizza = new Urun { Ad = "Büyük Boy Pizza", TabanFiyat = 250m, ResimUrl = "https://cdn-icons-png.flaticon.com/128/3132/3132693.png" };
            pizza.BoyutFiyatlari.Add("Orta", 0);
            pizza.BoyutFiyatlari.Add("Büyük (+60₺)", 60);
            pizza.EkstraMalzemeler.Add("Ekstra Peynir (+20₺)", 20);
            pizza.EkstraMalzemeler.Add("Zeytin (+10₺)", 10);
            pizza.EkstraMalzemeler.Add("Mantar (+15₺)", 15);
            pizza.EkstraMalzemeler.Add("Salam (+25₺)", 25);
            pizza.EkstraMalzemeler.Add("Mısır (+10₺)", 10);
            menu.Add(pizza);
        }

        private void ArayuzuCiz()
        {
            // 1. ÜST BAŞLIK
            Label lblBaslik = new Label();
            lblBaslik.Text = "LEZZET DÜNYASI MENÜSÜ";
            lblBaslik.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblBaslik.Dock = DockStyle.Top;
            lblBaslik.TextAlign = ContentAlignment.MiddleCenter;
            lblBaslik.Height = 60;
            lblBaslik.ForeColor = Color.FromArgb(44, 62, 80);
            this.Controls.Add(lblBaslik);

            // 2. ÜRÜN KARTLARI (FlowLayoutPanel)
            FlowLayoutPanel panelUrunler = new FlowLayoutPanel();
            panelUrunler.Dock = DockStyle.Fill;
            panelUrunler.AutoScroll = true;
            panelUrunler.Padding = new Padding(20);

            foreach (var urun in menu)
            {
                // Kartları dikey olarak uzattık (Size: 130x220 -> 160x350)
                Panel kart = new Panel();
                kart.Size = new Size(160, 350);
                kart.BackColor = Color.White;
                kart.Margin = new Padding(15);

                // Ürün Resmi
                PictureBox pb = new PictureBox();
                pb.Size = new Size(90, 90);
                pb.Location = new Point(35, 10);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                try { pb.Load(urun.ResimUrl); } catch { pb.BackColor = Color.LightGray; }
                kart.Controls.Add(pb);

                // Ürün Adı
                Label lblAd = new Label();
                lblAd.Text = urun.Ad;
                lblAd.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblAd.Location = new Point(0, 110);
                lblAd.Size = new Size(160, 25);
                lblAd.TextAlign = ContentAlignment.MiddleCenter;
                kart.Controls.Add(lblAd);

                // Fiyat Kutusu (Artık Read-only ve otomatik güncelleniyor)
                TextBox txtFiyat = new TextBox();
                txtFiyat.Text = urun.TabanFiyat.ToString("C2"); // Başlangıçta taban fiyatı göster
                txtFiyat.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                txtFiyat.ForeColor = Color.Green;
                txtFiyat.Location = new Point(25, 140);
                txtFiyat.Size = new Size(110, 25);
                txtFiyat.TextAlign = HorizontalAlignment.Center;
                txtFiyat.ReadOnly = true; // El ile değiştirilemez
                kart.Controls.Add(txtFiyat);

                // --- YENİ BOYUT SEÇİM KUTUSU (ComboBox) ---
                ComboBox cmbBoyut = new ComboBox();
                cmbBoyut.Location = new Point(15, 170);
                cmbBoyut.Size = new Size(130, 25);
                cmbBoyut.Font = new Font("Segoe UI", 9);
                cmbBoyut.DropDownStyle = ComboBoxStyle.DropDownList; // Yazı yazılamaz
                foreach (var boyut in urun.BoyutFiyatlari) cmbBoyut.Items.Add(boyut.Key);
                if (cmbBoyut.Items.Count > 0) cmbBoyut.SelectedIndex = 0; // İlk boyutu seçili getir
                kart.Controls.Add(cmbBoyut);

                // --- YENİ EKSTRA MALZEMELER LİSTESİ (CheckedListBox) ---
                CheckedListBox clbEkstralar = new CheckedListBox();
                clbEkstralar.Location = new Point(15, 205);
                clbEkstralar.Size = new Size(130, 80);
                clbEkstralar.Font = new Font("Segoe UI", 8);
                clbEkstralar.BorderStyle = BorderStyle.FixedSingle;
                clbEkstralar.CheckOnClick = true; // Tek tıkla işaretle/kaldır
                foreach (var ekstra in urun.EkstraMalzemeler) clbEkstralar.Items.Add(ekstra.Key);
                kart.Controls.Add(clbEkstralar);

                // Adet Seçici (NumericUpDown)
                NumericUpDown nudAdet = new NumericUpDown();
                nudAdet.Location = new Point(55, 300);
                nudAdet.Size = new Size(50, 25);
                nudAdet.Minimum = 0;
                nudAdet.Maximum = 20;
                nudAdet.Font = new Font("Segoe UI", 10);
                kart.Controls.Add(nudAdet);

                // Arayüz verilerini kaydediyoruz
                UrunKartArayuzu kartVerisi = new UrunKartArayuzu
                {
                    Urun = urun,
                    Pb = pb,
                    TxtFiyat = txtFiyat,
                    CmbBoyut = cmbBoyut,
                    ClbEkstralar = clbEkstralar,
                    NudAdet = nudAdet
                };
                kartlar.Add(kartVerisi);

                // --- FİYATI ANINDA GÜNCELLEME OLAYLARI (Event Handlers) ---
                // Boyut değişirse veya ekstra seçilirse fiyatı hesapla
                cmbBoyut.SelectedIndexChanged += (s, e) => KartFiyatiniGuncelle(kartVerisi);
                clbEkstralar.ItemCheck += (s, e) => Form.ActiveForm.BeginInvoke(new Action(() => KartFiyatiniGuncelle(kartVerisi))); // Seçim sonrası anında güncelleme için

                panelUrunler.Controls.Add(kart);
            }

            // 3. ALT KISIM (Hesapla Butonu)
            Panel altPanel = new Panel();
            altPanel.Dock = DockStyle.Bottom;
            altPanel.Height = 80;
            altPanel.BackColor = Color.White;

            Button btnHesapla = new Button();
            btnHesapla.Text = "HESAPLA VE SİPARİŞ VER";
            btnHesapla.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnHesapla.BackColor = Color.FromArgb(46, 204, 113);
            btnHesapla.ForeColor = Color.White;
            btnHesapla.FlatStyle = FlatStyle.Flat;
            btnHesapla.Size = new Size(270, 50);
            btnHesapla.Location = new Point(20, 15);
            btnHesapla.Cursor = Cursors.Hand;
            btnHesapla.Click += BtnHesapla_Click;
            altPanel.Controls.Add(btnHesapla);

            lblToplam.Text = "Toplam: 0,00 ₺";
            lblToplam.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblToplam.ForeColor = Color.DarkRed;
            lblToplam.AutoSize = true;
            lblToplam.Location = new Point(320, 25);
            altPanel.Controls.Add(lblToplam);

            this.Controls.Add(panelUrunler);
            this.Controls.Add(altPanel);
        }

        // --- YENİ YARDIMCI METOD: KART FİYATINI ANINDA GÜNCELLE ---
        private void KartFiyatiniGuncelle(UrunKartArayuzu kart)
        {
            decimal guncelBirimFiyat = kart.Urun.TabanFiyat;

            // 1. Seçili boyutun ek fiyatını ekle
            if (kart.CmbBoyut.SelectedItem != null)
            {
                string seciliBoyut = kart.CmbBoyut.SelectedItem.ToString();
                if (kart.Urun.BoyutFiyatlari.TryGetValue(seciliBoyut, out decimal boyutEki))
                {
                    guncelBirimFiyat += boyutEki;
                }
            }

            // 2. Seçili ekstraların fiyatlarını ekle
            foreach (var item in kart.ClbEkstralar.CheckedItems)
            {
                string seciliEkstra = item.ToString();
                if (kart.Urun.EkstraMalzemeler.TryGetValue(seciliEkstra, out decimal ekstraFiyati))
                {
                    guncelBirimFiyat += ekstraFiyati;
                }
            }

            // Kart üzerindeki fiyat kutusunu güncelle
            kart.TxtFiyat.Text = guncelBirimFiyat.ToString("C2");
        }

        // HESAPLA BUTONUNA TIKLANINCA ÇALIŞACAK KOD
        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            decimal toplamTutar = 0;
            string siparisOzeti = "Sipariş Özeti:\n\n";
            bool siparisVarMi = false;

            foreach (var kart in kartlar)
            {
                int adet = (int)kart.NudAdet.Value;

                if (adet > 0)
                {
                    decimal karttakiGuncelFiyat;
                    // Fiyat kutusunu read-only yaptık, o yüzden programın içindeki gerçek fiyatı hesaplayıp okumalıyız
                    if (decimal.TryParse(kart.TxtFiyat.Text, System.Globalization.NumberStyles.Currency, null, out karttakiGuncelFiyat))
                    {
                        decimal urunToplam = adet * karttakiGuncelFiyat;
                        toplamTutar += urunToplam;

                        // Özet kısmına detayları da yazalım
                        string detay = $" ({kart.CmbBoyut.Text}";
                        if (kart.ClbEkstralar.CheckedItems.Count > 0) detay += " + Ekstralar";
                        detay += ")";

                        siparisOzeti += $"{adet}x {kart.Urun.Ad}{detay} = {urunToplam:C2}\n";
                        siparisVarMi = true;
                    }
                }
            }

            lblToplam.Text = "Toplam: " + toplamTutar.ToString("C2");

            if (siparisVarMi)
            {
                siparisOzeti += $"\n-------------------\nGenel Toplam: {toplamTutar:C2}";
                MessageBox.Show(siparisOzeti, "Siparişiniz Alındı!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ekranı sıfırla
                foreach (var kart in kartlar)
                {
                    kart.NudAdet.Value = 0;
                }
                lblToplam.Text = "Toplam: 0,00 ₺";
            }
            else
            {
                MessageBox.Show("Lütfen önce ürünlerin altındaki kutulardan adet seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}