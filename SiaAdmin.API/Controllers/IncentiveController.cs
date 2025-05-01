using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateIncentive;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLogByUser;
using SiaAdmin.Application.Features.Queries.Incentive.GetAllIncentice;
 
namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncentiveController : BaseApiController
    {
        /// <summary>
        /// Hediye Listesini döndürür.
        /// </summary>
        [HttpGet("IncentiveList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetIncentiveList([FromQuery]GetAllIncenticeRequest getAllIncentiveRequest)
        {
            var response =await Mediator.Send(getAllIncentiveRequest);
            return Ok(response);
        }
        /// <summary>
        /// Kullanıcı hediye isteği
        /// Token bilgisi gerekir
        /// </summary>
        [HttpGet("RequestIncentive")]
        [Authorize]
        public async Task<IActionResult> RequestIncentivesByUser([FromQuery] RequestIncentive incentive)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new CreateSurveyLogByUserRequest()
                { InternalGuid = Guid.Parse(userGuid), IncentiveId = incentive.IncentiveID });
            return Ok();
        }
        /// <summary>
        /// Tüm hediyelerin açıklamaları ile birlikte listesini getirir
        /// </summary>
        [HttpGet("GetAllIncentiveList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllIncentiveList()
        {
            var incentiveList = StaticIncentiveList();
            return Ok(incentiveList);
        }

        public class RequestIncentive
        {
            public int IncentiveID { get; set; }
        }

        public class Incentive
        {
            public int Id { get; set; }
            public string Header { get; set; }
            public int Point { get; set; }
            public string Description { get; set; }

        }

        List<Incentive> StaticIncentiveList()
        {
            var incentive = new List<Incentive>();
            incentive.Add(new Incentive()
            {
                Id = 1,
                Header = "Papara Hediye Kart Talebi",
                Point = 5000,
                Description = "Biriktirdiğiniz 5.000 puan karşılığında Papara uygulaması içerisinde 15 farklı markada kullanabileceğiniz 50 TL değerinde hediye kart kazanabilirsiniz. Bir hafta içerisinde en az 50 TL en fazla ise 500 TL hediye kart talebinde bulunabilirsiniz. Hediye kartlarınızın geçerlilik süresi 1 yıldır. Papara hesabınıza iletilen hediye kartlar 1 yıl içerisinde kullanılmazsa geçersiz sayılır. Papara dünyasında hediye kartınızı kullanabileceğiniz sektörler ise; yeme& içme, giyim, aksesuar, market alışverişi, hobi& eğlence, kozmetik & bakım, elektronik eşyalar, ev dekorasyonu, ulaşım & tatil, eğlence sektörleridir. DİKKAT: Papara’nın hediye kart hizmetinden yararlanmak için Papara hesabı açmanız gerekmektedir. Talep ettiğiniz hediye bir hafta içerisinde kayıtlı telefon numaranız olan Papara hesabına otomatik yüklenecektir. Kazandığınız hediye kartınızı sadece online alışverişlerinizde kullanabilirsiniz. Kayıtlı numaranızın yanlış olması, farklı bir Papara kullanıcısına ait olması ya da Papara hesabınızın olmaması durumunda hediyenin teslim edilmeme sorumluluğu tamamen paneliste aittir. Sanal hediye kartı ile alakalı tüm şikayet ve yorumlarınızı da sialive@sia-insight.com adresine iletmenizi rica ederiz."
            });
            incentive.Add(new Incentive()
            {
                Id = 2,
                Header = "Günlük 1GB Bedava Internet Paketi",
                Point = 500,
                Description = "Günlük 1GB Bedava Internet paketi için kullanım şartları: Turkcell, Türk Telekom, Vodafone operatörlerinde geçerli Bedava Interneti 1 gün süresince kullanabilirsiniz. 24 saat içinde kullanılmayan bedava internet hakkınız ertesi güne dervretmez."
            });
            incentive.Add(new Incentive()
            {
                Id = 3,
                Header = "APP Store ve Itunes Hediye Kartı 100 TL",
                Point = 9250,
                Description = "Bir kart, binbir hediye. Tek bir kartla onlara dünyalar kadar eğlence hediye edebilirsiniz. App Store’dan milyonlarca uygulama. Apple Arcade’deki çığır açıcı uygulamalara erişim imkanı. Apple Music ile 60 milyondan fazla şarkı. Ve Apple Books’tan olağanüstü bir kitap ve sesli kitap seçkisi. App Store ve iTunes Hediye Kartı’nda binbir hediye saklı."
            });
            incentive.Add(new Incentive()
            {
                Id = 4,
                Header = "Boyner Hediye Çeki 50 TL",
                Point = 4750,
                Description = "SiaLive tarafından iletilecek kişiye özel tek kullanımlık kod ile hediye çeki kullanımı sağlanır. Oluşturulan hediye çeki; 31 Aralık 2023 tarihine kadar Outlet mağazaları hariç YKM ve BOYNER mağazalarında ve tüm ürünlerde geçerlidir. Aynı şekilde boyner.com.tr online alışverişlerinde kullanılabilir. Kelebek kampanyası dahil tüm indirim ve kampanyalarda son fiyatlardan geçerlidir.(Çek ve ürün hediyesi kampanyası hariç). Hediye çeki ile yapılan alışverişteki ürünlerin iadesi halinde, hediye çeki bedeli iade edilen ürün bedelinden düşülerek işlem yapılır. Hediye çeki karşılığı para iadesi yapılamaz. Hediye çeki iadeye tabi değildir."
            });
            incentive.Add(new Incentive()
            {
                Id = 5,
                Point = 2700,
                Header = "Google Play 25 TL değerinde Sanal Hediye Kartı",
                Description = "Bir hediye, sonsuz eğlence. Google Play'de kitaplar, dergiler ile sevdiğiniz Android uygulamaları ve oyunları var. Google Play hediyelerini saniyeler içinde kullanabilirsiniz. Kredi kartı gerekmez. "
            });
            incentive.Add(new Incentive()
            {
                Id = 6,
                Point = 4750,
                Header = "Google Play 50 TL değerinde Sanal Hediye Kartı",
                Description = "Bir hediye, sonsuz eğlence. Google Play'de kitaplar, dergiler ile sevdiğiniz Android uygulamaları ve oyunları var. Google Play hediyelerini saniyeler içinde kullanabilirsiniz. Kredi kartı gerekmez. "
            });
            incentive.Add(new Incentive()
            {
                Id = 7,
                Point = 9250,
                Header = "Google Play 100 TL değerinde Sanal Hediye Kartı",
                Description = "Bir hediye, sonsuz eğlence. Google Play'de kitaplar, dergiler ile sevdiğiniz Android uygulamaları ve oyunları var. Google Play hediyelerini saniyeler içinde kullanabilirsiniz. Kredi kartı gerekmez. "
            });
            incentive.Add(new Incentive()
            {
                Id = 8,
                Point = 2700,
                Header = "Migros 25 TL değerinde Sanal Hediye Çeki",
                Description = "Migros Sanal Hediye Çeki, 16 haneli alısveriş hakkı sağlayan numerik bir koddur.Migros Sanal Hediye Çekleri; Migros, 5M Migros, Macrocenter ve Migros Jet mağazalarında kasiyere beyan edilerek kullanılabilir.Migros Sanal Hediye Çekleri internet üzerinden yapılacak olan alısverislerde kullanılamamaktadır. Migros Sanal Hediye Çekleri tek kullanımlık olup bölünerek kullanılamaz. Alışveriş sonunda çek içinde kalan bakiye için para üstü, hediye çeki vb. talep edilemez. Bir alışverişte birden fazla Migros Sanal Hediye Çeki kullanılabilir.Migros Sanal Hediye Çekleri teslim tarihinden itibaren 2 yıl süre ile geçerlidir."
            });
            incentive.Add(new Incentive()
            {
                Id = 9,
                Point = 4750,
                Header = "Migros 50 TL değerinde Sanal Hediye Çeki",
                Description = "Migros Sanal Hediye Çeki, 16 haneli alısveriş hakkı sağlayan numerik bir koddur.Migros Sanal Hediye Çekleri; Migros, 5M Migros, Macrocenter ve Migros Jet mağazalarında kasiyere beyan edilerek kullanılabilir.Migros Sanal Hediye Çekleri internet üzerinden yapılacak olan alısverislerde kullanılamamaktadır. Migros Sanal Hediye Çekleri tek kullanımlık olup bölünerek kullanılamaz. Alışveriş sonunda çek içinde kalan bakiye için para üstü, hediye çeki vb. talep edilemez. Bir alışverişte birden fazla Migros Sanal Hediye Çeki kullanılabilir.Migros Sanal Hediye Çekleri teslim tarihinden itibaren 2 yıl süre ile geçerlidir."
            });
            incentive.Add(new Incentive()
            {
                Id = 10,
                Point = 9250,
                Header = "Migros 100 TL değerinde Sanal Hediye Çeki",
                Description = "Migros Sanal Hediye Çeki, 16 haneli alısveriş hakkı sağlayan numerik bir koddur.Migros Sanal Hediye Çekleri; Migros, 5M Migros, Macrocenter ve Migros Jet mağazalarında kasiyere beyan edilerek kullanılabilir.Migros Sanal Hediye Çekleri internet üzerinden yapılacak olan alısverislerde kullanılamamaktadır. Migros Sanal Hediye Çekleri tek kullanımlık olup bölünerek kullanılamaz. Alışveriş sonunda çek içinde kalan bakiye için para üstü, hediye çeki vb. talep edilemez. Bir alışverişte birden fazla Migros Sanal Hediye Çeki kullanılabilir.Migros Sanal Hediye Çekleri teslim tarihinden itibaren 2 yıl süre ile geçerlidir."
            });
            incentive.Add(new Incentive()
            {
                Id = 11,
                Point = 18500,
                Header = "Netflix 200 TL değerinde Sanal Hediye Kartı",
                Description = "Bilgisayar, cep telefonu, tablet ve akıllı televizyon (Smart TV) gibi çeşitli cihazlar üzerinden izleyebileceğiniz, aynı zamanda her ay güncellenen yerli/yabancı film ve dizilere erişebilme imkanı bu kartla size hediye."
            });
            incentive.Add(new Incentive()
            {
                Id = 12,
                Point = 18500,
                Header = "WWF-Türkiye e-sertifika",
                Description = "WWF-Türkiye (Doğal Hayatı Koruma Vakfı) doğal kaynakların aşırı tüketimi, kirlilik, yasadışı avcılık, doğa tahribatı, iklim krizi gibi insan kaynaklı sorunlarla mücadele ederek doğal yaşam alanlarının azalması ve türlerin kaybıyla sonuçlanan tehditleri durdurmayı amaçlamaktadır. 40 yılı aşkın süredir ülkemizde faaliyetlerine devam eden WWF-Türkiye (Doğal Hayatı Koruma Vakfı), Büyük Menderes Su Koruyuculuğu, Türkiye’nin Canı Projesi, Yeşil Deniz Kaplumbağaları ve Yunus Türlerini Koruması, Kaş Kekova Deniz Koruma Alanı Programı, Sürdürülebilir Balıkçılık Projesi gibi birçok ulusal ve uluslararası doğa koruma projesi ile insanın doğayla uyum içinde yaşadığı bir gelecek için çalışmaktadır. SiaLive katılımcısı olarak, kazanmış olduğunuz puanlar karşılığında bağışta bulunabilir, değişime ufak bir katkıda bulunabilirsiniz."
            });
            incentive.Add(new Incentive()
            {
                Id = 13,
                Point = 0,
                Header = "Hediyeler Hakkında Genel Bilgilendirme",
                Description = "Ödül programı kapsamında sunulan ürünlerin/hizmetlerin ve/veya ödemelerin yapıldığı üçüncü kişilerin kendi şartları ve koşulları bulunabilir. Lütfen bir ödül seçmeden önce bu ürünlerin/hizmetlerin kullanım şartlarını ve koşullarını dikkatli bir şekilde gözden geçiriniz. Bazı ödül prosedürleri için kişisel bilgileriniz istenebilir ve ödül programımızdaki iş ortaklarımızla bu bilgileri paylaşmamız gerekebilir. Örneğin cep telefonu veri paketi yüklemek için telefon numaranız gerekecektir. Bu Kullanım Şartları ve Koşullarını kabul ederek, bu tür amaçlar için kişisel bilgilerin toplanmasını, işlenmesini ve/veya ifşa edilmesini kabul etmiş bulunursunuz. Tüm kişisel bilgiler Gizlilik Politikamızda belirtilen şartlara/koşullara tabidir. Puanınıza karşılık gelen hediyenizi oluşturmak için, Profilim -> Puanım sayfasında yer alan Yeni Hediye Oluştur işlemini seçmeniz gerekmektedir. Hediye ürün görselleri temsilidir. Daha fazla bilgi için İletişim sayfasından bizimle temasa geçebilirsiniz. Sia Live, Hediye Kataloğundaki ürünleri istediği zaman ve önceden herhangi bir bildirime gerek olmaksızın, kaldırma, ürün özelliklerini ve puan bilgilerini değiştirme hakkını saklı tutar."
            });
            return incentive;
        }
    }
}
