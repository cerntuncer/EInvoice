1. GÜN – 11 Temmuz 2025 Cuma
Stajımın ilk günüydü. Sabah şirkete giriş yaptıktan sonra İnsan Kaynakları departmanında görevli Merve Hanım ile staj kabul sürecine dair evrak işlemlerini tamamladım. SGK giriş bildirgesi, öğrenci belgesi, iletişim ve IBAN bilgilerimi eksiksiz olarak teslim ettim.
Evrak işlemlerinin ardından kurum içi kullanım için bir dizüstü bilgisayar teslim aldım. İlk kurulumlar teknik ekip tarafından yapıldı. Kullanıcı hesabım ile sisteme giriş yaptıktan sonra VPN kullanımı, güvenlik prosedürleri ve şirketin teknik destek portalı support.innova.com.tr üzerinden yardım talebi oluşturma süreçleri bana gösterildi.
Ardından çalışacağım birime geçtim ve bana mentorluk edecek olan Yusuf Bey ile tanıştım. İlk sohbetimizde .NET konusundaki bilgi seviyemden bahsettik. Temel seviyede bilgim olduğunu söyledim. Yusuf Bey, bu doğrultuda ilk birkaç günümü oryantasyon ve temel teknolojilere hazırlık süreci olarak planladı.
Günün teknik kısmında geliştirme ortamımı kurmaya başladık:
•	Visual Studio 2022 Community Edition ve gerekli .NET uzantılarını yükledik.
•	Microsoft SQL Server 2022 ve SQL Server Management Studio (SSMS) kurulumunu gerçekleştirdik.
•	.NET 8 SDK sistemime eklendi.
•	Git kuruldu ve GitHub hesabım ile bağlantısı yapıldı.
Yusuf Bey, staj süresince geliştireceğim projenin GitHub üzerinde tutulacağını söyledi. Projenin adı EInvoice olacak ve çok katmanlı (N-Tier Architecture) bir yapıda geliştirilecekti. Günün sonunda GitHub üzerinden boş bir repository oluşturarak bilgisayarıma klonladım ve ilk commit’i attım.
Bugün herhangi bir daily scrum toplantısına katılmadım, ancak Yusuf Bey bana bu toplantıların her gün sabah 09:15’te yapıldığını ve ertesi günden itibaren benim de katılacağımı söyledi. Böylece ekipteki diğer kişilerle tanışma fırsatım da olacaktı.
İlk günüm, hem şirket kültürünü hem de teknik araçları tanımam açısından çok verimli geçti. Bundan sonraki günlerde doğrudan geliştirme sürecine geçebilmek için tüm kurulum ve hazırlıkları tamamlamış oldum.
2. GÜN – 14 Temmuz 2025 Pazartesi
Bugün güne saat 09:15’te ekibin günlük daily scrum toplantısına katılarak başladım. Toplantı ofiste toplantı odasında yüz yüze yapıldı. Ekipteki herkes sırayla önceki gün ne yaptığını, o gün ne yapacağını ve varsa karşılaştığı sorunları paylaştı. Bu sayede hem ekibin birbirini sürekli güncel tuttuğunu hem de işlerin düzenli bir şekilde ilerlediğini gördüm. Ayrıca bu toplantıda ekipteki diğer yazılım geliştiricilerle tanıştım.
Daily toplantısının ardından mentorüm Yusuf Bey ile yüz yüze konuşarak staj boyunca üzerinde çalışacağım proje konusunu netleştirdik. Şirket, benden bir E-Fatura ve Finansal Yönetim Sistemi geliştirmemi istedi. Bu sistem sadece staj çalışması olarak kalmayacak, aynı zamanda şirket içinde demo sürüm olarak kullanılabilecek düzeyde olacaktı.
Öğleden sonra projeye başlamadan önce araştırma yaptım. Türkiye’de aktif olarak kullanılan e-fatura sistemlerini inceledim ve farklı firmaların geliştirdiği örnek projelere göz attım. Böylece sistemlerin nasıl çalıştığını, kullanıcıların hangi işlemleri sıklıkla yaptığını ve ekran tasarımlarının nasıl kurgulandığını gözlemledim. Özellikle e-fatura süreçlerinde dikkatimi çeken başlıca konular şunlar oldu:
•	Faturaların satır bazında ürün/hizmet ekleme mantığıyla hazırlanması
•	Müşteri bilgilerinin faturaya entegre edilmesi
•	Otomatik numaralandırma ve tarih kontrolü
•	Kullanıcı arayüzlerinde sade ama işlevsel menü tasarımları
Günün sonunda, Yusuf Bey ile yaptığımız görüşmede ertesi gün proje mimarisini planlamaya başlayacağımızı kararlaştırdık. Böylece işe başlamadan önce sistemin genel işleyişini anlamış ve hedefleri kafamda netleştirmiş oldum.
3. GÜN – 16 Temmuz 2025 Çarşamba
Bugün, sabah saat 09:15’te ofiste gerçekleştirilen günlük Daily Scrum toplantısına katılarak güne başladım. Toplantıda bir önceki gün yaptığım araştırmalardan ve proje için taslak fikirlerimden bahsettim. Ayrıca bugün projenin yazılım mimarisi ve temel veri yapısı olan veritabanı tasarımına başlayacağımız belirtildi. Yusuf Bey, “Bu sistemde en önemli kısım veritabanı, önce onu sağlam kuralım” dedi.
Üniversitede veritabanı dersi aldığım için temel mantığını biliyordum. Bu yüzden bilgisayar başına geçtiğimde tabloları önce kendim çıkarmam, sonrasında birlikte kontrol edeceğimiz söylendi. Ana tabloları yazmaya başladım. Genel tabloları oluştururken sorun yaşamadım ancak kullanıcı girişi için şifre, e-posta gibi alanları User tablosuna eklemiştim. Bu konuda emin olamadım. Yusuf Bey, bu bilgilerin UserCredential adında ayrı bir tabloda tutulması gerektiğini anlattı.
Bu tablonun şifre, giriş deneme sayısı, token gibi güvenlik verilerini barındıracağını ve User tablosu ile birebir ilişkili olacağını belirtti. Bunu daha önce hiç yapmadığım için önce User tablosundan bu alanları kaldırdım, ardından UserCredential tablosunu ekledim. Araştırma sonucunda şifrelerin hash olarak saklanması, token bilgisinin eklenmesi ve son giriş tarihi gibi detayların güvenlik açısından önemli olduğunu öğrendim.
Oluşturulan tablolar:
•	Person: Kişi bilgileri (ad, TCKN/VKN, vergi dairesi).
•	CustomerSupplier: Kişinin müşteri mi yoksa tedarikçi mi olduğunu tutuyor.
•	Addresses: Kişilerin adresleri. Birden fazla adres eklenebiliyor.
•	ProductsAndServices: Ürün ve hizmet listesi.
•	Invoices ve LineOfInvoices: Faturalar ve satırları.
•	Currents, Banks, Cases: Finans işlemleri için.
İlişkiler üzerinde çalışırken tabloların birbirine nasıl bağlanacağını netleştirdik. User ile UserCredential birebir (1-1), User ile Person birebir (1-1), Invoice ile LineOfInvoices bire çok (1-N) ilişkili olacak şekilde tasarlandı. CustomerSupplier – Person ve Addresses – Person ilişkileri bire çok (1-N) olacak. Banks ile Currents ve Cases ile Currents ise birebir (1-1) olacak.
Ayrıca veri bütünlüğünü sağlamak ve yanlış veri girişlerini önlemek için Foreign Key’lerin doğru tanımlanmasının önemi üzerinde durduk.
[ER diyagramı görseli buraya eklenecek]
Günün sonunda tüm tabloların ve ilişkilerin yer aldığı ER diyagramı tamamlandı. Bu süreçte UserCredential tablosu benim için en yeni ve öğretici kısım oldu. Başlangıçta kullanıcı girişi için gerekli alanları User tablosuna eklemiştim. Yusuf Bey’in yönlendirmesiyle bu alanları ayrı bir tabloda toplamanın mantığını öğrenmiş oldum.

4. GÜN – 17 Temmuz 2025 Perşembe
Sabah daily’de dünkü ER yapısını kısaca hatırlattım. Bugün, veri tabanı şemasını kod tarafına yansıtmak için entity altyapısını hazırlamaya başladım. Tüm entitilerde ortak olan alanları her sınıfa tek tek yazmak yerine BaseEntity altında topladım ve tüm entity’lerin bu sınıftan kalıtım almasını sağladım. Böylece kod tekrarını önledim, standardı korudum; şirketin benimsediği SOLID prensiplerine uygun, okunabilir, sürdürülebilir, bakımı kolay ve genişletilebilir bir temel elde ettim.
Projede klasör yapılanmasını da netleştirdim: Entities ve Enumerations klasörlerini ayırdım (Resim 1.2’de görülmektedir). “Tip” niteliği taşıyan alanlar için enum kullanacağım; bu yaklaşım hem standartlara uyuyor hem de magic number kullanımını engelliyor.
 
BaseEntity (ortak alanlar)
•	Id
•	CreatedDate
•	UpdatedDate
•	Status (aktif/pasif)
BaseEntity kodu:
csharp
CopyEdit
namespace DatabaseAccessLayer.Models
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public Status Status { get; set; }
    }
}
Enum setleri (kullanım amacı)
•	Status (Active/Passive)
•	AddressType (Email, Şube, Fax, Phone, WebSite)
•	CurrencyType (TL, EUR, USD)
•	CurrentType (Case, Bank)
•	CustomerOrSupplierType (Customer, Supplier)
•	InvoiceSenario (EInvoice, EArchive, Paper)
•	InvoiceType (Purchase, Sales)
•	PersonType (User, CustomerOrSupplier)
•	UnitType (Adet, Kilogram, Gram, Litre, Mililitre, Metre, Santimetre)
•	UserType (NaturalPerson, LegalEntity)
Entity’lerde kullanım örneği
Entity’lerde BaseEntity’yi bu şekilde kullanacağım:
csharp
CopyEdit
public class Address : BaseEntity
{
    public AddressType AddressType { get; set; }

    [Required]
    [MaxLength(200)]
    public string Text { get; set; }

    public long PersonId { get; set; }

    [ForeignKey("PersonId")]
    public Person Person { get; set; }
}
Günün sonunda BaseEntity ve tüm enum yapıları tamamlandı; klasör düzeni oturdu (Resim 1.2). Yarın, şemaya göre tüm entity sınıflarını bu standartla yazıp bitireceğim.
5. GÜN – 18 Temmuz 2025 Cuma
Daily’de, dünkü veritabanı şema çalışmalarının ardından bugün entity sınıflarını yazdığımı paylaştım.
Tüm entity’lerde daha önce oluşturduğum enum tiplerini (AddressType, UserType, CustomerOrSupplierType, CurrencyType, PersonType, InvoiceType, InvoiceSenario, UnitType) kullandım.
BaseEntity sınıfından kalıtım alarak Id, CreatedDate, UpdatedDate, Status gibi ortak alanları her entity’de otomatik hale getirdim.
İlişkileri entity tarafında Data Annotations ile tanımladım:
•	[Required] → Zorunlu alan
•	[MaxLength] → Maksimum uzunluk
•	[ForeignKey] → FK tanımı
•	[Column(TypeName = "decimal(18,2)")] → Ondalık hassasiyet
Bugün ayrıca, bire-bir (1-1), bire-çok (1-N) ve çoktan-bire (N-1) ilişkilerin entity tarafında nasıl yazıldığını inceledim ve uyguladım.
 
1️⃣ Bire-bir (One-to-One) İlişki
Tanım:
Her iki tarafta da tekil navigation bulunur, FK bağımlı tarafta olur. Genelde FK alanı unique olmalıdır.
Örneğim: User ↔ UserCredential
Elle yazdığım kod:
public class UserCredential : BaseEntity
{
    public long UserId { get; set; } // User ile 1-1 ilişki

    public string Provider { get; set; } = "Local";
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? SecurityStamp { get; set; }

    public User User { get; set; } = null!;
}
Bu yapıda User ile UserCredential arasında bire-bir ilişki kuruldu. Resim 1.1’de görülmektedir.
 
2️⃣ Bire-çok (One-to-Many) İlişki
Tanım:
Üst entity koleksiyon tutar, alt entity’de FK ve tekil navigation bulunur.
Örneğim: Person → Addresses
Elle yazdığım kod:
public class Address : BaseEntity
{
    public AddressType AddressType { get; set; }

    [Required]
    [MaxLength(200)]
    public string Text { get; set; }

    public long PersonId { get; set; }

    [ForeignKey("PersonId")]
    public Person Person { get; set; }
}
Person sınıfında ICollection<Address> bulunur. Böylece bir kişi birden fazla adrese sahip olabilir. Resim 1.2’de görülmektedir.
 
3️⃣ Çoktan-bire (Many-to-One) İlişki
Tanım:
Alt tarafta FK ve tekil navigation vardır; üst tarafta koleksiyon olabilir veya olmayabilir.
Örneğim: Invoice → CustomerSupplier ve Invoice → Current
Elle yazdığım kod:
public class Invoice : BaseEntity
{
    public InvoiceType Type { get; set; }
    public int No { get; set; }
    public DateTime Date { get; set; }
    public InvoiceSenario Senario { get; set; }

    public long CurrentId { get; set; }
    [ForeignKey("CurrentId")]
    public Current Current { get; set; }

    public long CustomerSupplierId { get; set; }
    [ForeignKey("CustomerSupplierId")]
    public CustomerSupplier CustomerSupplier { get; set; }

    public ICollection<LineOfInvoice> LineOfInvoices { get; set; } = new List<LineOfInvoice>();
}
Burada her fatura sadece bir Current ve bir CustomerSupplier ile ilişkili olabilir, ancak bir Current veya CustomerSupplier birden fazla fatura ile ilişkilendirilebilir. Resim 1.3’te görülmektedir.
 
✅ Bugünlük Özet:
•	Tüm entity’lerde enum tipleri ve ortak alanları kullandım.
•	Bire-bir, bire-çok ve çoktan-bire ilişkilerin entity tarafında nasıl tanımlandığını öğrendim.
•	Elle yazdığım örnekleri kodladım ve ilgili sınıfların ekran görüntülerini aldım.
📌 Yarın: Fluent API ile ilişkileri netleştirip Map dosyalarını oluşturacağım.
6. GÜN – 21 Temmuz 2025 Pazartesi
Bugün, entity’lerde kurduğum ilişkileri Fluent API ile yazdım ve hepsini Options klasörü altındaki Map dosyalarına ayırdım (Resim 1.3’te görülmektedir).
Amaç; ilişki türlerini (1-1, 1-N, N-1) netleştirmek ve silme davranışlarını (Cascade / Restrict) doğru kullanmaktı.
 
Fluent API’de İlişkiler 
Bire-çok (1-N):
Üst tarafta koleksiyon, alt tarafta FK + tekil navigation.
Örn: Person → Addresses, Invoice → LineOfInvoices
csharp
CopyEdit
builder.HasOne(a => a.Person)
       .WithMany(p => p.Addresses)
       .HasForeignKey(a => a.PersonId);
Bire-bir (1-1):
Her iki tarafta tekil navigation; FK genelde bağımlı tarafta.
Örn: User ↔ UserCredential, Bank ↔ Current, Case ↔ Current
csharp
CopyEdit
builder.HasOne(uc => uc.User)
       .WithOne(u => u.UserCredential)
       .HasForeignKey<UserCredential>(x => x.UserId);
Çok-tan-bire (N-1):
Alt tarafta FK + tekil navigation; üst tarafta isteğe bağlı koleksiyon.
Örn: LineOfInvoice → Invoice, LineOfInvoice → ProductAndService
csharp
CopyEdit
builder.HasOne(li => li.Invoice)
       .WithMany(i => i.LineOfInvoices)
       .HasForeignKey(li => li.InvoiceId);
 
Silme Davranışları
•	Cascade: Üst kayıt silinince bağlı alt kayıtlar da otomatik silinir.
Örnek: Invoice → Current (Map’te OnDelete(DeleteBehavior.Cascade) kullandım).
Ayrıca satır tarafında Invoice → LineOfInvoices kurgusunda da bu mantık geçerli.
•	Restrict: Üst kayıt silinmeden önce alt kayıtların kaldırılması gerekir.
Örnek: Invoice → CustomerSupplier (Map’te OnDelete(DeleteBehavior.Restrict) kullandım).
📌 Kural:
“Alt kayıt üst olmadan anlamsızsa” → Cascade
“Alt kayıt korunmalı/bağımsız kalmalıysa” → Restrict
 
 
MyContext Sınıfı
Fluent API Map dosyalarını tanımladıktan sonra, Context katmanında MyContext sınıfını ekledim ve tüm Map’leri bağladım.
DbSet’ler, EF Core’da veritabanındaki tabloları temsil eden koleksiyonlardır.
Bu koleksiyonlar üzerinden sorgulama, ekleme, güncelleme ve silme işlemleri yapılır.
        MyContext sınıfımda :DbSet tanımları, Veritabanındaki tabloları temsil eder
        public DbSet<Address> Addresses { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AddressMap());
            modelBuilder.ApplyConfiguration(new BankMap());
            modelBuilder.ApplyConfiguration(new CaseMap());
            modelBuilder.ApplyConfiguration(new CurrentMap());
            modelBuilder.ApplyConfiguration(new CustomerSupplierMap());
            modelBuilder.ApplyConfiguration(new InvoiceMap());
            modelBuilder.ApplyConfiguration(new LineOfInvoiceMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserCredentialMap());
        }
    }
}
 
✅ Bugünlük Özet:
•	Tüm ilişkileri Fluent API ile netleştirdim.
•	Cascade / Restrict mantığını doğru yerlerde uyguladım.
•	Map klasörünü tamamladım.
•	MyContext’i oluşturarak tüm Map’leri bağladım.
📌 Yarın: Migration’a geçerek veritabanını oluşturacağım ve gerekirse seed verilerini eklemeye başlayacağım.
7. GÜN – 22 Temmuz 2025 Salı
Daily’de, dünkü Fluent API ve MyContext çalışmalarının ardından bugün SQL Server bağlantısı ve ilk migration işlemini yaptığımı paylaştım.
Adımlarım:
•	SQL Server Management Studio (SSMS) bağlantısı:
Öncelikle SSMS’i açarak localhost üzerinden Windows Authentication ile sunucuya bağlandım. Bu sayede migration sonrası oluşacak veritabanının tablolarını doğrudan görebileceğim ortamı hazırladım.
•	Migration oluşturma:
Visual Studio’da Package Manager Console üzerinden:
powershell
CopyEdit
Add-Migration InitialCreate
komutunu çalıştırdım. Bu komut, entity’ler ve Fluent API ayarlarıma göre tabloları oluşturacak SQL script’lerini üretti.
•	Veritabanını güncelleme:
Migration oluşturulduktan sonra:
powershell
CopyEdit
Update-Database
komutunu çalıştırarak script’leri SQL Server’a uyguladım.
•	Tabloların oluşumunu kontrol etme:
SSMS’te InvoiceDb → Tables altında entity’lerime karşılık gelen tüm tabloların eksiksiz olarak oluştuğunu gördüm:
Addresses, Banks, Cases, Currents, CustomersSuppliers, Invoices, LineOfInvoices, Person, ProductsAndServices, UserCredentials, Users
ve ayrıca migration geçmişini tutan __EFMigrationsHistory tablosu.
•	
Artık kod tarafındaki tüm model yapısı SQL Server üzerinde birebir şekilde oluştu.

 
 
Bugün itibarıyla DatabaseAccessLayer katmanını tamamen tamamladım. Böylece veri tabanı tarafı hazır; yarın iş katmanı (BusinessLogicLayer) kurulumuna geçebilirim.

 

resimde eklemiştim yazılar böyle devamınıda bu yazılara benzer şekilde yazalım ama bekle projede değişiklik yapıyoruz tam bitsin öyle diğer günleride yazarız ama şu an attıklarım kesin yazdım 13 günlük kısmı kaldı onlarıda proje tamamlanınca yazacağız 
