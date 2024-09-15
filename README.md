# Caching ve Redis

## Caching Nedir?
Yazılım süreçlerinde verilere daha hızlı erişebilmek için bu verilerin bellekte saklanması olayına caching denir.

## Caching Yaklaşımları
**In-Memory Caching:** Verileri uygulamanın çalıştığı bilgisayarın RAM'inde cache'leyen yaklaşımdır.

**Distributed Caching:** Verileri birden fazla fiziksel makinede cache'leyen ve böylece verileri farklı noktalarda tutarak tek bir noktada saklamaktan daha güvenli bir davranış sergileyen yaklaşımdır. Bu yaklaşımla veriler bölünerek farklı makinelere dağıtılmaktadır. Haliyle büyük veri setleri için daha uygun ve ideal bir yaklaşımdır.

## Redis
**Redis(Remote Dictionary Server);** open source olan ve bellekte veri yapılarını yüksek performanslı bir şekilde cache'lemek için kullanılan bir veritabanıdır. Caching işlemlerinin yanında message broker olarak da kullanılabilmektedir. Yapısal olarak key-value veri modelinde çalışmaktadır ve NoSQL veritabanıdır.

### Redis'te ki Veri Türleri
**String:** Redis'in en temel, en basit veri türüdür. Metinsel değerlerle birlikte her türlü veriyi saklamak için kullanılır. Hatta binary olarak resim, dosya vs. verileri de saklayabilmektedir.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| SET | Ekleme | SET isim Ömer |
| GET | Okuma | GET isim |
| GETRANGE | Karakter aralığı okuma | GETRANGE isim 2 4 |
| INCR & INCRBY | Arttırma | INCR sayi |
| DECR & DECRBY | Azaltma | DECR sayi |
| APPEND | Üzerine ekleme | APPEND isim Temel |

 **List:** Değerleri koleksiyonel olarak tutan bir türdür.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| LPUSH | Başa veri ekleme | LPUSH renkler mor |
| LRANGE | Verileri listeleme | LRANGE renkler 0 -1 |
| RPUSH | Sona veri ekleme | RPUSH renkler turuncu |
| LPOP | İlk veriyi çıkarma | LPOP renkler  |
| RPOP | Son veriyi çıkarma | RPOP renkler  |
| LINDEX | İndex'e göre veriyi getirme | LINDEX renkler 2  |

**Set:** Verileri rastgele bir düzende unique bir biçimde tutan veri türüdür.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| SADD | Ekleme | LPUSH renkler mor |
| SREM | Silme | LRANGE renkler 0 -1 |
| SISMEMBER | Arama | RPUSH renkler turuncu |
| SINTER | İki set'teki kesişimi getirir | SINTER renkler1 renkler2  |
| SCARD | Eleman sayısını getirir | SCARD renkler  |

**Sorted Set:** Set'in düzenli bir şekilde tutan versiyonudur.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| ZADD | Ekleme | ZADD teams 5 TEAM-A |
| ZRANGE | Getirme | ZRANGE teams 0 2 & ZRANGE teams 0 -1 WITHSCORES|
| ZREM | Silme | ZREM teams TEAM-B |
| ZREVRANK | Sıralama öğrenme | ZREVRANK teams TEAM-A |

**Hash:** Key-Value formatında veri tutan türdür.
| KOMUT             | İŞLEV  | ÖRNEK |
| ---------- | ---------- | ----------|
| HMSET & HSET | Ekleme | ZHMSET sozluk pen kalem |
| HMGET & HGET | Getirme | HMGET sozluk pen |
| HDEL | Silme | HDEL sozluk pen |
| HGETALL | Tümünü getirme | HGETALL sozluk	 |

**Streams:** Log gibi hareket eden bir veri türüdür. Streams, event'lerin oluştukları sırayla kaydedilmelerini ve daha sonra işlenmelerini sağlar.

**Geospatial Indexes:** Coğrafi koordinatlann saklanmasını sağlayan veri türüdür.


### Replication Davranışı Nedir?
Replication, bir redis sunucusundaki tüm verisel yapının farklı bir sunucu tarafından birebir modellenmesi/çoğaltılması/replike edilmesidir. Bu sayede verilerin güvencesini sağlayabilir ve bir kopyasını saklayabilmek için önlemler alabiliriz.

#### Replication Davranışındaki Temel Terminolojiler Nelerdir?
**Master:** Replication davranışında modellenecek/replikası alınacak olan sunucuya **master** adını veriyoruz. Yani ana redis sunucumuzdur.

**Slave:** Master'ın replikasını alan sunucuya ise **slave** adını veriyoruz. Slaveler, master'a subscribe olup masterda veri güncellendikçe kendilerini update eder ve orjinal veriye sahip olur.

****!**** *Replication özelliğinde master ve slave arasında kurulan bir bağlantı üzerinden master'daki tüm değişiklikler anlık olarak slave sunuculara aktarılıyor olacaktır. Bu bağlantı koptuğu taktirde otomatik olarak yeniden sağlanılarak, verisel güvence sergilenmeye çalışılacaktır. Haliyle bu davranış sayesinde master sunucuda olabilecek arıza veya kesinti durumlarında sorumluluğu slave sunucuların otomatik devralmasıyla kesintisiz bir hizmet sağlayabiliyor olacağız. Eğer ki, master ile slave arasında verisel bir eşitleme durumu tam olarak gerçekleşmemişse Redis bunun olabilmesi için talepte bulunacak ve masterdan güncel verilerin slave'e aktarılması için kaynak tüketimine devam edecektir.*

****!**** *Bir masterın birden fazla replikasyonu da olabilir. Böylece, birden fazla slave sunucunun olmasıyla yüksek kullanılabilirlik, yedekleme ve kurtarma, veri ölçeklendirme ve coğrafi olarak dağıtılmış sistemler gibi senaryolarda yararlı çalışmalar gerçekleştirebiliriz.*

****!**** *Ek olarak, slave sunuculan test süreçlerinde kullanabilir ve verisel güvenceyi sağlayabiliriz.*

#### Replication Uygulanması

Replication davranışını sergileyebilmek için öncelikle Redis sunucularını ayağa kaldırmamız gerekmektedir.

```bash
  docker run -p 1333:6379 —name redis-master -d redis
```
```bash
  docker run -p 1334:6379 —name redis-slave -d redis
```

Ardından master'la slave sunucusu arasındaki replication ilişkisini sağlayabilmek için master'ın IP'sini elde etmemiz gerekmektedir. Bunun için aşağıdaki talimatı çalıştıralım.

```bash
  docker inspect -f "{{ .NetworkSettings.IPAddress }}" redis-master
```

Son olarak master ile slave arasında replication ilişkisini oluşturalım. 

*<master-ip> <master-port>*

```bash
  docker exec -it redis-slave redis-cli slaveof 172.17.0.3 1333
```

# In-Memory Cache

## Absolute & Sliding Expiration
**Absolute Time:** Cache'de ki datanın ne kadar tutulacağına dair net ömrünün belirtilmesidir. Belirtilen ömür sona erdiğinde cache direkt olarak temizlenir.

**Sliding Time:** Cache'lenmiş datanın memory'de belirtilen süre periyodu zarfında tutulmasını belirtir. Belirtilen süre periyodu içerisinde cache'e yapılan erişim neticesinde de datanın ömrü bir o kadar uzatılacaktır. Aksi taktirde belirtilen süre zarfında bir erişim söz konusu olmazsa cache temizlenecektir.

![Expiration](https://github.com/user-attachments/assets/fe0895e2-5aff-4e5d-a19a-dea56921fba1)





