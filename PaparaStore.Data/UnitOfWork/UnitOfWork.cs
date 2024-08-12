using PaparaStore.Data.Domain;
using PaparaStore.Data.GenericRepository;
using PaparaStore.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly PaparaStoreDbContext dbContext;

    public IGenericRepository<User> UserRepository { get; }

    public IGenericRepository<Wallet> WalletRepository {get;}

    public IGenericRepository<WalletTransaction> WalletTransactionRepository { get; }

    public IGenericRepository<UserContact> UserContactRepository { get; }

    public IGenericRepository<Product> ProductRepository { get; }
    public IGenericRepository<ProductCode> ProductCodeRepository { get; }

    public IGenericRepository<Category> CategoryRepository { get; }
    public IGenericRepository<Order> OrderRepository { get; }
    public IGenericRepository<OrderDetail> OrderDetailRepository { get; }
    public IGenericRepository<Cart> CartRepository { get; }
    public IGenericRepository<Coupon> CouponRepository { get; }
    public IGenericRepository<CartProduct> CartProductRepository { get; }
    public IGenericRepository<CategoryProduct> CategoryProductRepository { get; }
    public UnitOfWork(PaparaStoreDbContext dbContext)
        {
            this.dbContext = dbContext; 
            WalletRepository = new GenericRepository<Wallet>(this.dbContext);
            UserRepository = new GenericRepository<User>(this.dbContext);
            WalletTransactionRepository = new GenericRepository<WalletTransaction>(this.dbContext);
            UserContactRepository = new GenericRepository<UserContact>(this.dbContext);
            ProductRepository = new GenericRepository<Product>(this.dbContext);
            ProductCodeRepository = new GenericRepository<ProductCode>(this.dbContext);
            CategoryRepository = new GenericRepository<Category>(this.dbContext);
            OrderRepository = new GenericRepository<Order>(this.dbContext);
            OrderDetailRepository = new GenericRepository<OrderDetail>(this.dbContext);
            CartRepository = new GenericRepository<Cart>(this.dbContext);
            CouponRepository = new GenericRepository<Coupon>(this.dbContext);
            CartProductRepository = new GenericRepository<CartProduct>(this.dbContext);
            CategoryProductRepository = new GenericRepository<CategoryProduct>(this.dbContext);
    }

    public async Task Complete()
    {
              await dbContext.SaveChangesAsync();
    }
    public async Task CompleteWithTransaction()
    {
        using (var dbTransaction = await dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await dbContext.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                Console.WriteLine(ex);
                throw;
            }
        }
    }

    public void Dispose()
    {
      
    }
}
