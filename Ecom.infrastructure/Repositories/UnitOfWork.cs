using AutoMapper;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Ecom.core.services;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer redis;
        private readonly IImageManagmentService _imageManagmentService;

        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailServices emailServices;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;

        public ICategoryRepository categoryRepository { get; }
        public IPhotoRepository photoRepository { get; }

        public IProductRepository productRepository { get; }

        public ICustomerBasketRepository customerBasket {  get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext context, IImageManagmentService imageManagmentService, IMapper mapper,
            IConnectionMultiplexer redis, UserManager<AppUser> userManager = null, IEmailServices emailServices = null, SignInManager<AppUser> signInManager = null, IGenerateToken token = null)
        {
            _context = context;
            _imageManagmentService = imageManagmentService;
            _mapper = mapper;
            this.redis = redis;
            _userManager = userManager;
            this.emailServices = emailServices;
            this.signInManager = signInManager;
            this.token = token;
            categoryRepository = new CategoryRepository(_context);
            photoRepository = new PhotoRepository(_context);
            productRepository = new ProductRepository(_context, _mapper, _imageManagmentService);
            customerBasket = new CustomerBasketRepository(redis);
            Auth = new AuthRepository(userManager, emailServices, signInManager,token);
        }
    }
}
