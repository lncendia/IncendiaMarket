using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.Data.Models
{
    public class ShopCart
    {
        // private readonly Db _db;
        //
        // public ShopCart(Db db)
        // {
        //     _db = db;
        // }
        // public string Id { get; set; }
        // public static ShopCart GetCart(IServiceProvider provider)
        // {
        //     ISession session = provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
        //     var context = provider.GetRequiredService<Db>();
        //     string shopCartId = session.GetString("CartId")?? Guid.NewGuid().ToString();
        //     session.SetString("CartId", shopCartId);
        //     return new ShopCart(context) {Id = shopCartId};
        // }
        //
        // public void AddToCart(Advertisement car)
        // {
        //     _db.Add(new ShopCartItem {Car = car, ShopCartId = Id});
        //     _db.SaveChanges();
        // }
        //
        // public List<ShopCartItem> GetCartItems()
        // {
        //     return _db.ShopCartItems.Where(_ => _.ShopCartId == Id).Include(_=>_.Car).ToList();
        // }

    }
}