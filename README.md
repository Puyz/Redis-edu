# Caching ve Redis

## Caching Nedir?
Yazılım süreçlerinde verilere daha hızlı erişebilmek için bu verilerin bellekte saklanması olayına caching denir.

## Caching Yaklaşımları
**In-Memory Caching:** Verileri uygulamanın çalıştığı bilgisayarın RAM'inde cache'leyen yaklaşımdır.

**Distributed Caching:** Verileri birden fazla fiziksel makinede cache'leyen ve böylece verileri farklı noktalarda tutarak tek bir noktada saklamaktan daha güvenli bir davranış sergileyen yaklaşımdır. Bu yaklaşımla veriler bölünerek farklı makinelere dağıtılmaktadır. Haliyle büyük veri setleri için daha uygun ve ideal bir yaklaşımdır.



# In-Memory Cache

## Absolute & Sliding Expiration
**Absolute Time:** Cache'de ki datanın ne kadar tutulacağına dair net ömrünün belirtilmesidir. Belirtilen ömür sona erdiğinde cache direkt olarak temizlenir.

**Sliding Time:** Cache'lenmiş datanın memory'de belirtilen süre periyodu zarfında tutulmasını belirtir. Belirtilen süre periyodu içerisinde cache'e yapılan erişim neticesinde de datanın ömrü bir o kadar uzatılacaktır. Aksi taktirde belirtilen süre zarfında bir erişim söz konusu olmazsa cache temizlenecektir.

![Expiration](https://github.com/user-attachments/assets/fe0895e2-5aff-4e5d-a19a-dea56921fba1)

# Redis
**Redis(Remote Dictionary Server);** open source olan ve bellekte veri yapılarını yüksek performanslı bir şekilde cache'lemek için kullanılan bir veritabanıdır. Caching işlemlerinin yanında message broker olarak da kullanılabilmektedir. Yapısal olarak key-value veri modelinde çalışmaktadır ve NoSQL veritabanıdır.

## Redis'te ki Veri Türleri
**String:** Redis'in en temel, en basit veri türüdür. Metinsel değerlerle birlikte her türlü veriyi saklamak için kullanılır. Hatta binary olarak resim, dosya vs. verileri de saklayabilmektedir.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| `SET` | Ekleme | SET isim Ömer |
| `GET` | Okuma | GET isim |
| `GETRANGE` | Karakter aralığı okuma | GETRANGE isim 2 4 |
| `INCR & INCRBY` | Arttırma | INCR sayi |
| `DECR & DECRBY` | Azaltma | DECR sayi |
| `APPEND` | Üzerine ekleme | APPEND isim Temel |

 **List:** Değerleri koleksiyonel olarak tutan bir türdür.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| `LPUSH` | Başa veri ekleme | LPUSH renkler mor |
| `LRANGE` | Verileri listeleme | LRANGE renkler 0 -1 |
| `RPUSH` | Sona veri ekleme | RPUSH renkler turuncu |
| `LPOP` | İlk veriyi çıkarma | LPOP renkler  |
| `RPOP` | Son veriyi çıkarma | RPOP renkler  |
| `LINDEX` | İndex'e göre veriyi getirme | LINDEX renkler 2  |

**Set:** Verileri rastgele bir düzende unique bir biçimde tutan veri türüdür.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| `SADD` | Ekleme | LPUSH renkler mor |
| `SREM` | Silme | LRANGE renkler 0 -1 |
| `SISMEMBER` | Arama | RPUSH renkler turuncu |
| `SINTER` | İki set'teki kesişimi getirir | SINTER renkler1 renkler2  |
| `SCARD` | Eleman sayısını getirir | SCARD renkler  |

**Sorted Set:** Set'in düzenli bir şekilde tutan versiyonudur.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| `ZADD` | Ekleme | ZADD teams 5 TEAM-A |
| `ZRANGE` | Getirme | ZRANGE teams 0 2 & ZRANGE teams 0 -1 WITHSCORES|
| `ZREM` | Silme | ZREM teams TEAM-B |
| `ZREVRANK` | Sıralama öğrenme | ZREVRANK teams TEAM-A |

**Hash:** Key-Value formatında veri tutan türdür.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| `HMSET & HSET` | Ekleme | ZHMSET sozluk pen kalem |
| `HMGET & HGET` | Getirme | HMGET sozluk pen |
| `HDEL` | Silme | HDEL sozluk pen |
| `HGETALL` | Tümünü getirme | HGETALL sozluk	 |

**Streams:** Log gibi hareket eden bir veri türüdür. Streams, event'lerin oluştukları sırayla kaydedilmelerini ve daha sonra işlenmelerini sağlar.

**Geospatial Indexes:** Coğrafi koordinatlann saklanmasını sağlayan veri türüdür.


## Replication Davranışı Nedir?
Replication, bir redis sunucusundaki tüm verisel yapının farklı bir sunucu tarafından birebir modellenmesi/çoğaltılması/replike edilmesidir. Bu sayede verilerin güvencesini sağlayabilir ve bir kopyasını saklayabilmek için önlemler alabiliriz.

### Replication Davranışındaki Temel Terminolojiler Nelerdir?
**Master:** Replication davranışında modellenecek/replikası alınacak olan sunucuya **master** adını veriyoruz. Yani ana redis sunucumuzdur.

**Slave:** Master'ın replikasını alan sunucuya ise **slave** adını veriyoruz. Slaveler, master'a subscribe olup masterda veri güncellendikçe kendilerini update eder ve orjinal veriye sahip olur. **Readonly yapıdadır.**

****!**** *Replication özelliğinde master ve slave arasında kurulan bir bağlantı üzerinden master'daki tüm değişiklikler anlık olarak slave sunuculara aktarılıyor olacaktır. Bu bağlantı koptuğu taktirde otomatik olarak yeniden sağlanılarak, verisel güvence sergilenmeye çalışılacaktır. Haliyle bu davranış sayesinde master sunucuda olabilecek arıza veya kesinti durumlarında sorumluluğu slave sunucuların otomatik devralmasıyla kesintisiz bir hizmet sağlayabiliyor olacağız. Eğer ki, master ile slave arasında verisel bir eşitleme durumu tam olarak gerçekleşmemişse Redis bunun olabilmesi için talepte bulunacak ve masterdan güncel verilerin slave'e aktarılması için kaynak tüketimine devam edecektir.*

****!**** *Bir masterın birden fazla replikasyonu da olabilir. Böylece, birden fazla slave sunucunun olmasıyla yüksek kullanılabilirlik, yedekleme ve kurtarma, veri ölçeklendirme ve coğrafi olarak dağıtılmış sistemler gibi senaryolarda yararlı çalışmalar gerçekleştirebiliriz.*

****!**** *Ek olarak, slave sunuculan test süreçlerinde kullanabilir ve verisel güvenceyi sağlayabiliriz.*

### Replication Uygulanması

* Replication davranışını sergileyebilmek için öncelikle Redis sunucularını ayağa kaldırmamız gerekmektedir.

```bash
  docker run -p 1333:6379 —name redis-master -d redis
```
```bash
  docker run -p 1334:6379 —name redis-slave -d redis
```

* Ardından master'la slave sunucusu arasındaki replication ilişkisini sağlayabilmek için master'ın IP'sini elde etmemiz gerekmektedir. Bunun için aşağıdaki talimatı çalıştıralım.

```bash
  docker inspect -f "{{ .NetworkSettings.IPAddress }}" redis-master
```
veya
```bash
  docker inspect -f "{{ range .NetworkSettings.Networks }}{{.IPAddress}}{{end}}" redis-master
```
* Son olarak master ile slave arasında replication ilişkisini oluşturalım. 

*<master-ip> <master-port>*

```bash
  docker exec -it redis-slave redis-cli slaveof 172.17.0.3 1333
```


## Redis Sentinel Nedir?
Redis Sentinel yapılanması, Redis veritabanı için yüksek kullanılabilirlik sağlamak amacıyla geliştirilmiş yönetim servisidir.

Redis Sentinel, master/slave replikasyon sistemi üzerinde çalışan bir yönetim servisidir. Sentinel, Redis veritabanının sağlığını izler ve herhangi bir problem/kesinti vs. meydana geldiği taktirde otomatik olarak failover(yük devretme) işlemlerini gerçekleştirerek farklı bir sunucu üzerinden Redis hizmetinin kaldığı yerden devam etmesini sağlar.

Şu durumlarda kullanmayı tercih edebiliriz;

**Redis sunucusunun arızalanması:** Redis sunucusu arızalandığı taktirde Redis Sentinel servisi ile farklı bir sunucu üzerinden Redis çalışmalarına devam edebilir ve böylece kesintisiz hizmet vermeyi sürdürebiliriz.

**Bakım ve güncelleme süreçlerinde:** Bakım ve güncelleme süreçlerinde Redis sunucusu geçici olarak çalışmaz hale gelmesi durumunda Sentinel ile farklı bir sunucu üzerinden hizmetin kesintisiz verilmesini sağlayabiliriz.

**Yüksek trafik:** Yüksek trafiğin olduğu durumlarda Redis sunucusunun performansı yetmeyebilir ve Redis gelen taleplere gecikmeli sonuçlar dönebilir. Böyle durumlarda Sentinel ile daha performanslı çalışmalar gerçekleştirebiliriz.

**Yedekleme ve geri yükleme:** Sentinel servisi ile yedekleme ve geri yükleme süreçlerini sorunsuz bir şekilde tamamlanmasını sağlayabiliriz.

**!** *Yani Redis veritabanının sürekli olarak çalışması gerektiği durumlarda Sentinel yapılanması kullanılmaktadır.*

### Sentinel
Redis veritabanının sağlığını izleyen ve otomatik failover İşlemerİnİ gerçekleştiren bir yönetim servisidir.

Sentinel, Redis veritabanının yüksek kullanılabilirliğini sağlamak ve herhangi bir kesintiye mahal vermeksizin sürdürülebilir kılmak için otomatik olarak çalışmakta ve Redis master sunucusunun sağlıklı olup olmadığını sürekli olarak gözlemlemektedir. Master sunucusunda bir problem ya da kesinti meydana geldiği taktirde sentinel sunucusu yedek(slave) Redis sunuculardan birinin yükseltilmesi ve ana bilgisayar yerine master yapılması işinden sorumludur.

Redis Sentinel, sisteme eklenen tüm slave sunucular hakkında bilgi toplamakta ve aralarından bir master seçer.

Sentinel sunucusu, Redis Sentinel yapılanmasının merkezi bileşenidir.

### Failover
Herhangi bir slave'in mevcut master yerine geçip master olmasına **failover** denmektedir. 

Sentinel sunucusu, failover işlemi gerçekleştirdiği taktirde yeni master'ın IP değerini diğer slave'lere ileterek tüm sunucuların senkronize olmasını sağlamaktadır. Failover işlemi ardından mevcut master'da slave konumuna geçecektir.


### Tek Sentinel Sunuculu Mimari
Tek Sentinel sunuculu mimaride slave sunucular, master sunucunun replikasyonuyla çalışır ve ihtiyaç halinde master sunucu olabilir. Sentinel sunucusu, master ve slave sunucuların IP adreslerini bilir ve senkronizasyonu sağlar.

### Birden Fazla Sentinel Sunuculu Mimari
Birden fazla sentinel sunucusu olduğunda, lideri belirlemek için sentinel sunucuları kendi aralarında oylama yapar. Bu oylama sonucunda otomatik olarak bir lider seçilir. Leader’ı belirlemek için ekstra yapılandırmaya gerek yoktur, sentinel sunucularının iletişim halinde olmaları yeterlidir.

**!!!** *Tek sentinel sunuculu konfigürasyon ile çoklu sentinel sunucu konfigürasyonu arasında **fark bulunmamaktadır.***

### Örnek Birden Fazla Sentinel Sunuculu Mimari Oluşturma

**Adım 1:** Öncelikle ortak bir ağ oluşturalım.
```bash
docker network create redis-network
```

**Adım 2:** Redis master sunucusunu oluşturalım.
```bash
docker run -d --name redis-master -p 1333:6379 --network redis-network redis redis-server
```

**Adım 3:** 3 adet slave sunucu oluşturalım.
```bash
docker run -d --name redis-slave-1 -p 1334:6379 --network redis-network redis redis-server --slaveof redis-master 6379
docker run -d --name redis-slave-2 -p 1335:6379 --network redis-network redis redis-server --slaveof redis-master 6379
docker run -d --name redis-slave-3 -p 1336:6379 --network redis-network redis redis-server --slaveof redis-master 6379
```

**Adım 4:** Sentinel sunucularını ayağa kaldırmadan önce sentinel sunucunun yapılandırmasını sağlamalıyız. Bunun için bilgisayarın herhangi bir dizinine “sentinel.conf” adında dosya oluşturalım ve aşağıdaki komutları dosya içerisine kaydedelim.

.conf dosyası içerisinde yer alan mymaster isimlendirmesini takma bir isimdir. İstediğimiz ismi verebiliriz.

```conf
# Sentinel tarafından izlenecek Master sunucusu
# <IP> <PORT> <Sentinel Sunucu Sayısı> 
sentinel monitor mymaster 172.18.0.2 1333 3

# Master sunucunun tepki vermemesi durumunda Sentinel'in bekleme süresi
sentinel down-after-milliseconds mymaster 5000

# Master sunucunun yeniden yapılandırılması için Sentinel'in beklemesi gereken süre
sentinel failover-timeout mymaster 10000

# Sentinel tarafından eş zamanlı olarak kullanılacak slave sayısı
sentinel parallel-syncs mymaster 3
```

**!** Docker üzerinde redis master sunucunun ip adresini bulmak için aşağıdaki komutu çalıştırabiliriz.
```bash
docker inspect -f "{{ range .NetworkSettings.Networks }}{{.IPAddress}}{{end}}" redis-master
```


**Adım 5:** 3 adet sentinel sunucu ayağa kaldıralım. Sentinel default port 26379'dur.
```bash
docker inspect -f "{{ range .NetworkSettings.Networks }}{{.IPAddress}}{{end}}" redis-master
```
#### Örnek Sentinel Sunucusu Oluşturma
```bash
docker run -d --name redis-sentinel-1 -p 1250:26379 --network redis-network -v C:\Redis\sentinel.conf:/usr/local/etc/redis/sentinel.conf redis redis-sentinel /usr/local/etc/redis/sentinel.conf
docker run -d --name redis-sentinel-2 -p 1251:26379 --network redis-network -v C:\Redis\sentinel.conf:/usr/local/etc/redis/sentinel.conf redis redis-sentinel /usr/local/etc/redis/sentinel.conf
docker run -d --name redis-sentinel-3 -p 1252:26379 --network redis-network -v C:\Redis\sentinel.conf:/usr/local/etc/redis/sentinel.conf redis redis-sentinel /usr/local/etc/redis/sentinel.conf
```



