# MultiShop RabbitMQ akışı

Sipariş başarıyla oluşturulduğunda `Order API`, `OrderCreatedEvent` mesajını RabbitMQ'ya gönderir. Mesaj `multishop.events` exchange'i üzerinden `order.created` routing key'iyle `multishop.order.created` kuyruğuna gider. `MultiShop.OrderCreatedConsumer` Worker projesi bu kuyruğu dinler.

```text
Order API (Producer)
        |
        | OrderCreatedEvent
        v
RabbitMQ Exchange -> Queue -> OrderCreatedConsumer (Consumer)
```

## Çalıştırma

RabbitMQ container'ını başlat:

```bash
docker compose -f docker-compose.infrastructure.yml up -d rabbitmq
```

Yönetim ekranı: `http://localhost:15672`

- Kullanıcı: `multishop`
- Parola: `multishop_dev`

Ardından Rider'da şu iki projeyi çalıştır:

1. `MultiShop.Order.WebApi`
2. `MultiShop.OrderCreatedConsumer`

## Temel kavramlar

- **Producer:** Mesajı gönderen uygulama. Bu projede Order API.
- **Exchange:** Mesajın hangi kuyruğa gideceğine karar verir.
- **Routing key:** Exchange'in yönlendirmede kullandığı anahtar. Burada `order.created`.
- **Queue:** Consumer hazır olana kadar mesajın beklediği posta kutusu.
- **Consumer:** Kuyruktaki mesajı okuyan Worker.
- **ACK:** Consumer mesajı başarıyla işlediğini RabbitMQ'ya bildirir; RabbitMQ mesajı kuyruktan siler.

Yerel ayarlar `appsettings.json` içindedir. Docker veya sunucu ortamında aynı değerler `RabbitMQ__HostName`, `RabbitMQ__UserName` ve `RabbitMQ__Password` çevre değişkenleriyle değiştirilebilir.

Bu başlangıç sürümünde Outbox Pattern yoktur. Sipariş kaydedildikten sonra RabbitMQ erişilemezse sipariş korunur ancak olay daha sonra otomatik gönderilmez.
