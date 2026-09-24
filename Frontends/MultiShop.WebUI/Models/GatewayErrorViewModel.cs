namespace MultiShop.WebUI.Models;

public class GatewayErrorViewModel
{
    public int StatusCode { get; set; }
    public string Title => StatusCode switch
    {
        403 => "Bu işlem için yetkin yok",
        404 => "Aradığın kayıt bulunamadı",
        _ => "Şu anda bağlantı kurulamıyor"
    };
    public string Message => StatusCode switch
    {
        403 => "Hesabının bu sayfaya veya işleme erişim yetkisi bulunmuyor.",
        404 => "Kayıt kaldırılmış veya bağlantı değişmiş olabilir.",
        _ => "İşlemin sonucunu doğrulayamadık. Biraz sonra tekrar kontrol edebilirsin."
    };
}
