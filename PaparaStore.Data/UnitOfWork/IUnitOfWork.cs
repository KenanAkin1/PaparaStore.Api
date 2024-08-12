using PaparaStore.Data.GenericRepository;
using PaparaStore.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Data.UnitOfWork;

public interface IUnitOfWork
{
    Task Complete();
    Task CompleteWithTransaction();


    IGenericRepository<Wallet> WalletRepository { get; }
    IGenericRepository<WalletTransaction> WalletTransactionRepository { get; }
    IGenericRepository<UserContact> UserContactRepository { get; }
    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Product> ProductRepository { get; }
    IGenericRepository<ProductCode> ProductCodeRepository { get; }
    IGenericRepository<Category> CategoryRepository { get; }
    IGenericRepository<Order> OrderRepository { get; }
    IGenericRepository<OrderDetail> OrderDetailRepository { get; }
    IGenericRepository<Cart> CartRepository { get; }
    IGenericRepository<Coupon> CouponRepository { get; }


    IGenericRepository<CartProduct> CartProductRepository { get; }
    IGenericRepository<CategoryProduct> CategoryProductRepository { get; }

}
