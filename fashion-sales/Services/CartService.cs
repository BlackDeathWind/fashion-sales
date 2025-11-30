using System.Text.Json;
using fashion_sales.Models.ViewModels;

namespace fashion_sales.Services;

public interface ICartService
{
    IList<CartItemViewModel> GetCart(ISession session);
    void SaveCart(ISession session, IList<CartItemViewModel> items);
}

public class CartService : ICartService
{
    private const string CartKey = "CART_ITEMS";

    public IList<CartItemViewModel> GetCart(ISession session)
    {
        var json = session.GetString(CartKey);
        if (string.IsNullOrEmpty(json))
        {
            return new List<CartItemViewModel>();
        }

        return JsonSerializer.Deserialize<IList<CartItemViewModel>>(json) ?? new List<CartItemViewModel>();
    }

    public void SaveCart(ISession session, IList<CartItemViewModel> items)
    {
        var json = JsonSerializer.Serialize(items);
        session.SetString(CartKey, json);
    }
}


