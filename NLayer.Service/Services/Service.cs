﻿using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        /*
         * Bu serivs bussiness kodlarının olacağı yer
         * Repo ile haberleşerek veritabanı ile ilgli işlemleri yapılacak.
         * Save change yapılabilmesi için unit of work interface kullanılması gerekli, repoda save change unit of work kullanılacağı için yapılmadı
         */

        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IGenericRepository<T> genericRepository,IUnitOfWork unitOfWork)
        {
            _repository = genericRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            /*
             * entity buraya geldiğinde id değeri yok,yeni data kayıt ediliyor ve id sql server tarafından verilecek
             * SaveChange ile EFCore sql tarafında ilgili satır için verilen id değerini bu entity için id propertysine atar
             */
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
        {
            await _repository.AddRangeAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        //Remove metodunda save changes çağırılacağı için async mevcut
        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            _repository.RemoveRange(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }
    }
}
