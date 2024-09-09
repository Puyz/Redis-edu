# Caching ve Redis

## Caching Nedir?
Yazılım süreçlerinde verilere daha hızlı erişebilmek için bu verilerin bellekte saklanması olayına caching denir.

## Caching Yaklaşımları
**In-Memory Caching:** Verileri uygulamanın çalıştığı bilgisayarın RAM'inde cache'leyen yaklaşımdır.

**Distributed Caching:** Verileri birden fazla fiziksel makinede cache'leyen ve böylece verileri farklı noktalarda tutarak tek bir noktada saklamaktan daha güvenli bir davranış sergileyen yaklaşımdır. Bu yaklaşımla veriler bölünerek farklı makinelere dağıtılmaktadır. Haliyle büyük veri setleri için daha uygun ve ideal bir yaklaşımdır.

## Redis
**Redis(Remote Dictionary Server);** open source olan ve bellekte veri yapılarını yüksek performanslı bir şekilde cache'lemek için kullanılan bir veritabanıdır. Caching işlemlerinin yanında message broker olarak da kullanılabilmektedir. Yapısal olarak key-value veri modelinde çalışmaktadır ve NoSQL veritabanıdır.