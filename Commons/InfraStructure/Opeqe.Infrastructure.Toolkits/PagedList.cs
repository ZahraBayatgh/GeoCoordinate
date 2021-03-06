﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Toolkits
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    /// Use return new PagedList<T>(query, pageIndex, pageSize);
    [Serializable]

    public class PagedList<T> : List<T>, IPagedList<T>
    {
        private async Task InitializeAsync(IMongoQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException("pageSize must be greater than zero");
            }

            TotalCount = totalCount ?? await source.CountAsync();
            source = totalCount == null ? source.Skip(pageIndex * pageSize).Take(pageSize) : source;
            AddRange(source);

            if (pageSize > 0)
            {
                TotalPages = TotalCount / pageSize;
                if (TotalCount % pageSize > 0)
                {
                    TotalPages++;
                }
            }

            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        private async Task InitializeAsync(IMongoCollection<T> source, FilterDefinition<T> filterdefinition, SortDefinition<T> sortdefinition, int pageIndex, int pageSize)
        {
            TotalCount = (int)source.CountDocuments(filterdefinition);
            AddRange(await source.Find(filterdefinition).Sort(sortdefinition).Skip(pageIndex * pageSize).Limit(pageSize).ToListAsync());
            if (pageSize > 0)
            {
                TotalPages = TotalCount / pageSize;
                if (TotalCount % pageSize > 0)
                {
                    TotalPages++;
                }
            }
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        private void Initialize(IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException("pageSize must be greater than zero");
            }

            TotalCount = totalCount ?? source.Count();

            if (pageSize > 0)
            {
                TotalPages = TotalCount / pageSize;
                if (TotalCount % pageSize > 0)
                {
                    TotalPages++;
                }
            }

            PageSize = pageSize;
            PageIndex = pageIndex;
            source = totalCount == null ? source.Skip(pageIndex * pageSize).Take(pageSize) : source;
            AddRange(source);
        }

        public static async Task<PagedList<T>> Create(IMongoCollection<T> source, FilterDefinition<T> filterdefinition, SortDefinition<T> sortdefinition, int pageIndex, int pageSize)
        {
            var pagelist = new PagedList<T>();
            await pagelist.InitializeAsync(source, filterdefinition, sortdefinition, pageIndex, pageSize);
            return pagelist;
        }

        public static async Task<PagedListData<T>> Create(IMongoQueryable<T> source, int pageIndex, int pageSize)
        {
            var pagelist = new PagedList<T>();
            await pagelist.InitializeAsync(source, pageIndex, pageSize);
            return new PagedListData<T>()
            {
                Data = pagelist,
                MetaData = new PagedListMeta()
                {
                    HasNextPage = pagelist.HasNextPage,
                    HasPreviousPage = pagelist.HasPreviousPage,
                    Skip = pagelist.PageIndex,
                    Take = pagelist.PageSize,
                    TotalCount = pagelist.TotalCount,
                    TotalPages = pagelist.TotalPages,
                }
            };

        }


        public PagedList()
        {
        }
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Initialize(source, pageIndex, pageSize);
        }

        public PagedList(IAggregateFluent<T> source, int pageIndex, int pageSize)
        {
            var range = source.Skip(pageIndex * pageSize).Limit(pageSize + 1).ToList();
            int total = range.Count > pageSize ? range.Count : pageSize;
            this.TotalCount = source.ToListAsync().Result.Count;
            if (pageSize > 0)
            {
                this.TotalPages = total / pageSize;
            }

            if (total % pageSize > 0)
            {
                TotalPages++;
            }

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(range.Take(pageSize));
        }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Initialize(source, pageIndex, pageSize, totalCount);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage => (PageIndex > 0);
        public bool HasNextPage => (PageIndex + 1 < TotalPages);
    }
    public class PagedListMeta
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public long TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public long TotalPages { get; set; }
    }

    public class PagedListData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PagedListMeta MetaData { get; set; }
    }









}
