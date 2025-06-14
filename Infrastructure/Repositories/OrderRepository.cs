﻿using Domain.Context;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository,  IDisposable
    {
        private readonly OMDbContext _context;
        private bool _disposed = false;

        public OrderRepository(OMDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
                    .Take(1000) // Limit to 100 orders for performance
                .ToListAsync();
        }

        public async Task<int> CreateOrder(Order order, List<OrderProduct> products)
        {
            await _context.Orders.AddAsync(order);
            await _context.OrderProducts.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
