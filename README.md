Simpra Final Project
Dijital ürünler satış yapılabilen bir e-ticaret sitesi geliştirilmiştir. Proje N-Layer Architecture baz alınarak geliştilmiş olup, .Net Core 6 teknolojisi kullanıldı.

Projeyi çalıştırmak için :
Projenin ana dizininde terminal açılır. "docker-compose up -d"
Proje kestrel üzerinden local'de ayağa kaldırılır.
Postman Dokümantasyon:
Postman ile gerekli dokümantasyon hazırlanmıştır.Örnek request ve örnek responslarda eklenmiştir. Her bir Controller ile ilgili açıklama yapılmıştır.

https://documenter.getpostman.com/view/20405564/2s93z6cNwT

Projede Kullanılan Teknolojiler ve Frameworkler
IDE : Visual Studio 2022
DB: MsSQL
Languages: C#
Entity Framework Core ORM(Code first)
Generic Repository Desing Pattern
Fluent Validation
N-Layer Architecture
AutoMapper
Redis
RabbitMQ
Serilog
Autofac
Jwt
Identitiy User
Unit Test ve Moq
Projenin Detay ve İçerikleri
Projenin Database’i MsSQL kullanılarak geliştirildi.
Projenin backend’i ‘Web API’ projesi olup, NLayer Architecture yapısına uygun olarak dizayn edildi.
RESTAPI PRINCIPLES VE CRUD Operations kullanıldı. (Model kullanarak GetAll, GetById , Put , Post , Delete methodlarini icen bir controller implement edildi. )
Generic Repository Design Pattern ve Unit of Work Design Pattern uygulandı.
Fluent Validation ve AutoMapper kullanıldı.
CustomResponse ve Middleware kullanıldı.
Put ve Post apilerin de model validation hazirlandı.
SOLID Prensiplerine uygun tasarım yapıldı.
RabbitMQ ile asenkron iletişim sağlandı. Basket Controllerda "CheckOut" endpointi ile asenkron olarak Order oluşturmak için mesaj gönderiyoruz.Bu mesajı "CreateOrderMessageCommandConsumer" sınıfında yakalayıp orderservice "CreateOrderAsync" metoduna gönderiyoruz.Ayrıca senkron olarak da OrderController'da "Save" endpointi ile ordermodel oluşturabiliyoruz. Kod tekrarından kaçınmak için 2 tarafta orderservice "CreateOrderAsync" metodunu kullanır.
Redis basket tarafında kullanıcının sepete eklediği ürünleri güncellemek, görüntülemek ve silmek için kullanılır.(Basket Service ve Redis Service)
Serilog kütüphenesi kullanıldı.
Autofac kullanıldı. Autofact'in "RegisterAssemblyTypes" kullanarak sonu repository ya da service ile biten tüm classları DI Container'a ekliyoruz.
Unit Test kodları Moq kütüphanesi kullanılılarak yazıldı(OrderService).
Kredi kartı bilgilerini hashlemek için BCrypt.Net kullanıldı.
Projenin Yapısı
N-Layer Architecture
N-katmanlı (N-layer) mimari, bir yazılım uygulamasını mantıksal olarak katmanlara bölen bir yaklaşımdır. Her katman belirli bir sorumluluğu yerine getirir ve daha yüksek seviyeli katmanlar, daha düşük seviyeli katmanlara bağımlı olur.

Core Layer(Simpra.Core)
N-katmanlı mimaride,core katmanı, mimarinin ortasında yer alır.Core katmanı,sistemin işlevselliğinin büyük bir kısmını içerir ve genellikle diğer katmanlarla doğrudan etkileşim halindedir.

1.Entity

Veritabanı tasarımı bağlamında "entity", veritabanında depolanan bilgilerin bir temsilcisidir.

2.Jwt

"JwtConfig" sınıfı, bir JWT yapılandırma dosyasındaki ayarları temsil etmek için kullanılır. Bu sınıfın özellikleri, JWT oluşturmak veya doğrulamak için kullanılan bazı değerleri içerir. "JwtConfig" sınıfı appsettings.json dosyasındaki "JwtConfig" bölümündeki değerleri temsil eder.

3.Logger

"RequestProfilerModel" sınıfı, Bu sınıf, bir web isteği (request) ve yanıt (response) için geliştirilmiş bir profil modelidir.web isteklerinin ve yanıtlarının izlenmesi veya profillemesi için kullanılabilir. İstek zamanı, yanıt zamanı, istek içeriği, yanıt içeriği ve ilgili HTTP bağlamı gibi bilgileri bu modelde saklayabilirsiniz. Bu bilgileri kullanarak performans analizi yapabilir, hataları takip edebilir veya istatistikler oluşturabiliriz.

4.LogType

Bu sınıf, uygulamanızda belirli log türlerini tanımlamak ve kullanmak için kullanılabilir. Loglarınızı kaydetmek veya raporlamak için bu sabit değerleri kullanabilirsiniz.

5.Repository

Bu arabirim, genel bir veritabanı işlem arabirimini tanımlar ve genel CRUD (Create, Read, Update, Delete) işlemlerini destekler.Core katmanında Interfaceler yer almaktadır.

6.Service

Service arabirimleri, service davranışını ve işlevselliğini tanımlayan sözleşmelerdir.

7.UnitofWork

İş birimi deseni, bir dizi ilgili veritabanı işlemini tek bir iş birimi içinde gruplandırarak, bu işlemlerin aynı işlem oturumunda toplu olarak tamamlanmasını sağlar.

8.Attribute

Response için guid id üretmektedir.

9.RabbitMQ

RabbitMq için const olarak config değerleri yer almaktadır.

10.Role

Const olarak kullanıcı rolleri yer almaktadır.

11.Enum

Product ve order için status değerleri yer almaktadır.

Repository Layer(Simpra.Repository)
1.AppDbContext

Bu sınıf, bir veritabanı bağlantısı sağlar ve tablolara karşılık gelen DbSet'lerle etkileşimde bulunur. Ayrıca, SaveChangesAsync() ve SaveChanges() yöntemleri üzerine yazılarak, varlık değişiklikleri yapıldığında otomatik olarak "CreatedAt" ve "UpdatedAt" alanlarını günceller. Ayrıca, OnModelCreating() yöntemi aracılığıyla model yapılandırması tanımlanır. Bu sınıf genellikle uygulamanın veritabanı işlemlerini yönetmek için kullanılır.

2.Configurations

Her entity tipi için ayrı bir yapılandırma sınıfı oluşturarak daha ayrıntılı kontrol sağlandı. Bu sınıflara, "IEntityTypeConfiguration" arabirimini uyguladık ve "Configure" yöntemini içeririrler. Bu yöntem içinde, entity tablo adı, veri tipleri, zorunlulukları ve özellikleri, tabloların ilişkileri ayarlandı.

3.Migrations

4.Repositories

Generic Repository uygulandı:
public class GenericRepository : IGenericRepository where T : BaseEntity : Bu satırda GenericRepository sınıfı tanımlanır ve IGenericRepository arayüzünü uygular. T, BaseEntity sınıfından türeyen herhangi bir sınıf olabilir.

protected readonly AppDbContext _context; : Veritabanı bağlamına erişim sağlamak için AppDbContext nesnesi tanımlanır ve koruma düzeyi protected olarak ayarlanır. Bu nesne, sınıfın tüm yöntemlerinde kullanılabilir.

private readonly DbSet _dbSet; : DbSet nesnesi, T türündeki varlıkların veritabanındaki koleksiyonunu temsil eder. Veritabanı işlemleri bu DbSet nesnesi üzerinden gerçekleştirilir.

public GenericRepository(AppDbContext context) : GenericRepository sınıfının yapıcı metodu. Bu metot, AppDbContext örneğini alır ve _context ve _dbSet değişkenlerini ayarlar.

public async Task AddAsync(T entity) : Bir varlığı veritabanına eklemek için kullanılır. Veritabanı bağlamında _dbSet.AddAsync(entity) yöntemi çağrılır.

public async Task AddRangeAsync(IEnumerable entities) : Birden çok varlığı veritabanına eklemek için kullanılır. Veritabanı bağlamında _dbSet.AddRangeAsync(entities) yöntemi çağrılır.

public async Task AnyAsync(Expression<Func<T, bool>> expression) : Belirli bir koşulu sağlayan varlıkların olup olmadığını kontrol etmek için kullanılır. Veritabanı bağlamında _dbSet.AnyAsync(expression) yöntemi çağrılır.

public IQueryable GetAll() : Tüm varlıkları almak için kullanılır. AsNoTracking() yöntemi, varlıkların takip edilmediğini belirtir ve performansı artırır.

public async Task<List> GetAllWithIncludeAsync(params string[] includes) : Belirli bir varlık için ilişkili varlıkları dahil ederek tüm varlıkları almak için kullanılır. "includes" parametresi ile dahil edilecek varlık ilişkileri belirtilir. Bu yöntem, varlıkların yüksek miktarda sorgulanmasına neden olabileceğinden performansı etkileyebilir.

public async Task GetByIdAsync(int id) : Belirli bir kimlik (id) değerine sahip varlığı almak için kullanılır. Veritabanı bağlamında _dbSet.FindAsync(id) yöntemi çağrılır.

public async Task GetByIdWithIncludeAsync(int id, params string[] includes) : Belirli bir kimlik (id) değerine sahip varlığı ilişkili varlıkları dahil ederek almak için kullanılır. İçerdiği varlıkları belirtmek için "includes" parametresi kullanılır. İlk olarak, _dbSet üzerinde bir sorgu oluşturulur. Sonra, "includes" parametresinde belirtilen ilişkili varlıkları sorguya dahil etmek için Aggregate() yöntemi kullanılır. Sonuç olarak, varlık kimliğiyle eşleşen ilk varlık FirstOrDefaultAsync() yöntemiyle alınır ve geri döndürülür.

public void Remove(T entity) : Bir varlığı veritabanından kaldırmak için kullanılır. Veritabanı bağlamında _dbSet.Remove(entity) yöntemi çağrılır.

public void RemoveRange(IEnumerable entities) : Birden çok varlığı veritabanından kaldırmak için kullanılır. Veritabanı bağlamında _dbSet.RemoveRange(entities) yöntemi çağrılır.

public void Update(T entity) : Bir varlığı güncellemek için kullanılır. Veritabanı bağlamında _dbSet.Update(entity) yöntemi çağrılır.

public IQueryable Where(Expression<Func<T, bool>> expression) : Belirli bir koşulu sağlayan varlıkları almak için kullanılır. Veritabanı bağlamında _dbSet.Where(expression) yöntemi çağrılır.

public IEnumerable WhereWithInclude(Expression<Func<T, bool>> expression, params string[] includes) : Belirli bir koşulu sağlayan varlıkları ilişkili varlıkları dahil ederek almak için kullanılır. "includes" parametresi ile dahil edilecek varlık ilişkileri belirtilir. İlk olarak, _dbSet üzerinde bir sorgu oluşturulur. Ardından, belirtilen koşulu sağlayan varlıkları Where() yöntemiyle sorguya ekler. Son olarak, "includes" parametresinde belirtilen ilişkili varlıkları sorguya dahil etmek için Aggregate() yöntemi kullanılır. Sorgunun sonucu liste olarak döndürülür.

CategoryRepository, CouponRepository, OrderRepository ve ProductRepository GenericRepository'den kalıtım almıştır.
5.SEED

CategorySeed ve ProductSeed olmak üzere 2'ye ayırılır.

6.UnitOfWork

UnitOfWork, veritabanı işlemlerini tek bir iş birimi olarak gruplamayı ve daha sonra bu iş birimini tamamlamayı veya geri almayı sağlar.

UnitOfWork sınıfının bir kurucu metodu vardır ve bu metot, bir AppDbContext örneği alır. AppDbContext örneği, Entity Framework veya benzeri bir ORM (nesne ilişkisel eşlem) aracılığıyla veritabanına erişimi sağlar.
CompleteAsync metodu, _context.SaveChangesAsync() yöntemini çağırarak asenkron olarak değişiklikleri veritabanına kaydeder.
Complete metodu, _context.SaveChanges() yöntemini çağırarak senkron olarak değişiklikleri veritabanına kaydeder.
CompleteWithTransaction metodu, veritabanı işlemlerini bir işlem (transaction) içinde gerçekleştirir. _context.Database.BeginTransaction() yöntemi çağırılarak bir işlem başlatılır. Ardından, _context.SaveChanges() yöntemi çağırılarak değişiklikler veritabanına kaydedilir ve işlem Commit() yöntemiyle onaylanır. Herhangi bir hata durumunda, işlem geri alınır (Rollback() yöntemi) ve bir hata fırlatılır.
CompleteWithTransactionAsync metodu, asenkron olarak veritabanı işlemlerini bir işlem içinde gerçekleştirir. _context.Database.BeginTransactionAsync() yöntemi çağırılarak bir işlem başlatılır. Ardından, _context.SaveChangesAsync() yöntemi çağırılarak değişiklikler veritabanına kaydedilir ve işlem CommitAsync() yöntemiyle onaylanır. Herhangi bir hata durumunda, işlem geri alınır (RollbackAsync() yöntemi) ve bir hata fırlatılır.
Dispose metodu, IDisposable arabirimini uygulayarak çağırılır. Bu metot, Clean metodunu çağırarak ve GC.SuppressFinalize(this) yöntemiyle bellek temizliği yapar. Clean metodu, disposing parametresine bağlı olarak _context örneğini temizler.
Shema Layer(Simpra.Shema)
1.Entitylerin request ve response modellerini barındırır.

2.Request Modeller: İstemci (client) tarafından sunucuya gönderilen isteği temsil eder. Bu model genellikle HTTP isteklerinde veya API çağrılarında kullanılır.

3.Response Modeller: Response modeli ise sunucudan gelen yanıtı temsil eder. Sunucu, isteği aldıktan sonra işler ve bir yanıt döner. Bu yanıt, işlem sonucunda sunucu tarafından oluşturulan veriyi ve gerekli durum kodunu içerir.

Service Layer (Simpra.Service)
1.Fluent Validation : .NET tabanlı bir kütüphanedir ve doğrulama (validation) kurallarını tanımlamak ve uygulamak için kullanılır. Fluent Validation, genellikle giriş doğrulaması (input validation) veya veri geçerliliği kontrolü gibi senaryolarda kullanılır. Örneğin, bir kullanıcının bir formu doldurduğu bir senaryoda, kullanıcının girdiği verilerin geçerli olup olmadığını doğrulamak için Fluent Validation kullanılabilir.

2.Mapper:

Mapper, bir veri modelini başka bir veri modeline dönüştürmek veya eşlemek için kullanılan bir bileşendir. Genellikle farklı veri yapıları arasında bilgilerin aktarılması veya dönüştürülmesi gerektiğinde kullanılır.

3.Exceptions:

Exception, C# programlama dilinde bir hata veya istisnai durumu temsil eden bir sınıftır. Bir exception, programın normal akışını değiştirerek hatanın tespit edilmesini, yönetilmesini ve işlenmesini sağlar.

Client-side Exception, istemci tarafında (örneğin, bir masaüstü uygulaması veya web tarayıcısı) ortaya çıkan hataları temsil eder. Bu tür hatalar genellikle kullanıcı etkileşimine veya istemcinin donanım veya yazılım sınırlamalarına bağlı olarak ortaya çıkar.

NotFound Exception, C# dilinde belirli bir kaynağın bulunamaması durumunda fırlatılan bir exception türüdür. Genellikle veritabanı işlemleri veya dosya işlemleri gibi kaynaklara erişim sırasında kullanılır.

4.Messages:

RabbitMQ ile mesaj gönderebilmek için "CreateOrderMessageCommand" oluşturulmuştur.

5.Response:

Custom Response :Bu sınıf, başarı durumunu, veriyi ve hataları saklamak için kullanılır ve genel olarak API isteklerine yanıt olarak döndürülür. "Data" adında bir özellik, generic olarak belirlenen türde veri taşımak için kullanılır. "StatusCode" adında bir özellik, durum kodunu temsil eder ve [JsonIgnore] özniteliği ile serileştirme işlemlerinde görmezden gelinir. "Errors" adında bir özellik, bir dize listesi alarak hataları temsil eder. Bu sınıfın ayrıca bir dizi statik metodu vardır:

"Success" metodu, bir başarı durumunu temsil eder ve "statusCode" ve "data" parametrelerini alır. Bu metot, "CustomResponse" türünde bir nesne oluşturarak başarı durumunu ve veriyi içerir. İkinci bir "Success" metodu, sadece "statusCode" parametresini alır ve başarı durumunu temsil eder, ancak veri içermez. "Fail" metodu, bir hata durumunu temsil eder ve "statusCode" ve "errors" parametrelerini alır. Bu metot, "CustomResponse" türünde bir nesne oluşturarak hata durumunu ve hata listesini içerir. İkinci bir "Fail" metodu, bir hata durumunu temsil eder ve "statusCode" ve "error" parametrelerini alır. Bu metot, yalnızca tek bir hatayı içeren bir "CustomResponse" nesnesi oluşturur.

"NoContent" sınıfını tanımlar. Ancak, sınıfın içeriği boştur ve herhangi bir özellik veya davranışa sahip değildir.

6.Service

BaseService :Bu temel hizmet sınıfı, genel CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemlerini gerçekleştirmek için kullanılır. IGenericRepository ve IUnitOfWork bağımlılıklarını enjekte eder ve bu bağımlılıkları kullanarak ilgili işlemleri gerçekleştirir. Hataların kaydedilmesi ve uygun istisna mesajlarının fırlatılması için Serilog kütüphanesi kullanılır.
AddAsync: Bu metot, T tipinde bir varlık (entity) eklemek için kullanılır. Verilen varlığı veritabanına ekler, birim işlemi tamamlanır ve eklenen varlığı geri döndürür. Eğer bir hata oluşursa, AddAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

AddRangeAsync: Bu metot, IEnumerable türünde birden fazla varlığı toplu olarak eklemek için kullanılır. Verilen varlıkları veritabanına ekler, birim işlemi tamamlanır ve eklenen varlıkları geri döndürür. Eğer bir hata oluşursa, AddRangeAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

AnyAsync: Bu metot, belirli bir koşula uyan varlık olup olmadığını kontrol etmek için kullanılır. Verilen ifadeye (expression) uyan bir varlık varsa true, yoksa false döndürür. Eğer bir hata oluşursa, AnyAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

GetAllAsync: Bu metot, tüm varlıkları IEnumerable türünde geri döndürür. Varlıklar veritabanından alınırken, GetAllAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

GetByIdAsync: Bu metot, belirli bir id değerine sahip varlığı getirmek için kullanılır. Eğer varlık bulunamazsa, NotFoundException fırlatılır. Eğer başka bir hata oluşursa, GetByIdAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

RemoveAsync: Bu metot, verilen varlığı veritabanından kaldırmak için kullanılır. Birim işlemi tamamlanır. Eğer bir hata oluşursa, RemoveAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

RemoveRangeAsync: Bu metot, verilen varlık koleksiyonunu toplu olarak veritabanından kaldırmak için kullanılır. Birim işlemi tamamlanır. Eğer bir hata oluşursa, RemoveRangeAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

UpdateAsync: Bu metot, verilen varlığı güncellemek için kullanılır. Önce, varlığın veritabanında var olup olmadığı kontrol edilir. Varlık bulunamazsa NotFoundException fırlatılır. Eğer başka bir hata oluşursa, UpdateAsync Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

Where: Bu metot, belirli bir koşula uyan varlıkları sorgulamak için kullanılır. Verilen ifadeye uyan varlıkları IQueryable türünde geri döndürür. Eğer bir hata oluşursa, Where Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

WhereWithInclude: Bu metot, belirli bir koşula uyan varlıkları sorgulamak ve ilişkili varlıkları dahil etmek için kullanılır. Verilen ifadeye uyan varlıkları ve belirtilen ilişkili varlıkları IEnumerable türünde geri döndürür. Eğer bir hata oluşursa, WhereWithInclude Exception başlığıyla bir hata günlüğe kaydedilir ve bir Exception fırlatılır.

AuthenticationService,BasketService,CouponService, OrderService,ProductService,RedisService,UserService,CategoryService hazırlandı.

Simpra.Api
1.Modules: Autofac kullanarak bağımlılık enjeksiyonunu yapılandıran ve hizmetlerin ve depoların modülünü tanımlayan RepoServiceModule sınıfını içerir. Bu yapılandırma, projede kullanılan IRepository ve IService uygulamalarının otomatik olarak kaydedilmesini ve çözünürlük yapısının oluşturulmasını sağlar. Bu sayede, herhangi bir sınıfın bağımlılıkları çözümlenirken uygun IRepository ve IService uygulamaları otomatik olarak enjekte edilebilir.

2.Middleware: RequestLoggingAndErrorHandlerMiddleware: Hataları handle edebilmek ve loglama uygulayabilmek için uygulandı.

3.logs: Log dosyaları yer alır aynı zamanda console'da da gösteririr.

4.helper: BCrypt.Net. paketi kullanarak kredi kartı bilgilerini hashler.

5.Consumer: RabbitMQ'dan gönderilen mesajı yakalamaya yarar.

6.Extensions: Db, Jwt, RabbitMQ, Redis için extension metotları yazılmıştır. Ayrıca "MigrateAndSeedUserExtension" ile program ayağa kalktığında database'e henüz yansımamış güncel migrationları database'e yansıtıyor eğer sistemde kullanıcı yok ise default olarak 1'er adet admin ve user role sahip kullanıcı oluşturuyor.

7.Controller: AuthenticationController, BasketController, CategoryController, CouponController, ProductController, OrderController, UserController,

8.Settings : Options pattern kullanırak doldurmak için Redis Setting isminde sınıf tanımlanmıştır.
