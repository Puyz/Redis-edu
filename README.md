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






