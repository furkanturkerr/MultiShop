using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Tests;

public class BasketTotalDtoTests
{
    [Test]
    public void TotalPrice_SepetteUrunVarsa_UrunlerinToplaminiDoner()
    {
        var basket = new BasketTotalDto
        {
            BasketItems =
            [
                new BasketItemDto
                {
                    ProductPrice = 100,
                    Quantity = 2
                },
                new BasketItemDto
                {
                    ProductPrice = 50,
                    Quantity = 3
                }
            ]
        };

        var result = basket.TotalPrice;
        
        
        //Şu değeri kontrol et : Assert.That(...)
        //“350’ye eşit mi?” demek. : Is.EqualTo(350m)
        //m = decimal olduğunu belirtir
        Assert.That(result, Is.EqualTo(350m));
    }

    [Test]
    public void TotalPrice_SepetteUrunYoksa_0Doner()
    {
        var basket = new BasketTotalDto();
        
        var result = basket.TotalPrice;
        
        Assert.That(result, Is.Zero);
    }
}