using NLog;
using System;
using System.Data.Entity;
using Dal.Repositories;

namespace Dal
{
    public class UnitOfWork : IDisposable
    {
        private readonly Logger logger;
        private readonly EfDbContext context;
        private CategoryRepository categoryRepository;
        private MemberRepository memberRepository;
        private ProductRepository productRepository;
        private RoleRepository roleRepository;
        private TaxCategoryRepository taxCategoryRepository;
        private TicketLineRepository ticketLineRepository;
        private TicketRepository ticketRepository;
        private MemberCardRepository memberCardRepository;
        private CheckoutSheetRepository checkoutSheetRepository;
        private TransactionRepository transactionRepository;
        private bool disposed;

        public UnitOfWork()
        {
            this.logger = LogManager.GetCurrentClassLogger();
            this.context = new EfDbContext();
        }

        public CategoryRepository CategoryRepository
        {
            get
            {
                return this.categoryRepository ?? (this.categoryRepository = new CategoryRepository(this.context));
            }
        }

        public MemberRepository MemberRepository
        {
            get
            {
                return this.memberRepository ?? (this.memberRepository = new MemberRepository(this.context));
            }
        }

        public ProductRepository ProductRepository
        {
            get
            {
                return this.productRepository ?? (this.productRepository = new ProductRepository(this.context));
            }
        }

        public RoleRepository RoleRepository
        {
            get
            {
                return this.roleRepository ?? (this.roleRepository = new RoleRepository(this.context));
            }
        }

        public TaxCategoryRepository TaxCategoryRepository
        {
            get
            {
                return this.taxCategoryRepository ?? (this.taxCategoryRepository = new TaxCategoryRepository(this.context));
            }
        }

        public TicketLineRepository TicketLineRepository
        {
            get
            {
                return this.ticketLineRepository ?? (this.ticketLineRepository = new TicketLineRepository(this.context));
            }
        }

        public TicketRepository TicketRepository
        {
            get
            {
                return this.ticketRepository ?? (this.ticketRepository = new TicketRepository(this.context));
            }
        }

        public MemberCardRepository MemberCardRepository
        {
            get
            {
                return this.memberCardRepository ?? (this.memberCardRepository = new MemberCardRepository(this.context));
            }
        }

        public CheckoutSheetRepository CheckoutSheetRepository
        {
            get
            {
                return this.checkoutSheetRepository ?? (this.checkoutSheetRepository = new CheckoutSheetRepository(this.context));
            }
        }

        public TransactionRepository TransactionsRepository
        {
            get
            {
                return this.transactionRepository ?? (this.transactionRepository = new TransactionRepository(this.context));
            }
        }

        public void Detach(object o)
        {
            this.context.Entry(o).State = EntityState.Detached;
        }

        public void Save()
        {
            this.context.SaveChanges();
            this.logger.Debug("Changes saved to database");
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
                this.context.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            this.logger.Debug("DbContext destroyed");
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}