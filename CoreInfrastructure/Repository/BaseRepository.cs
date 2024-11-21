using CoreInfrastructure.IRepository;
using Domains.Entities;
using Infrastructure.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreInfrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public BaseRepository(DbContext context, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }
        #region Contract Methods
        public async Task<T> AddAsyncAndGet(T entity)
        {
            _context.Set<T>().Add(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return await SaveChangesAsync();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveChangesAsync();
        }

        public async Task AddOrUpdateAsync(T entity, Expression<Func<T, bool>> specification)
        {
            if (await _context.Set<T>().AsNoTracking().AnyAsync(specification))
            {
                var keyProperty = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.FirstOrDefault();
                if (keyProperty != null)
                {
                    var existingEntity = await GetAsync(specification, true);
                    if (existingEntity != null)
                    {
                        var properties = _context.Model.FindEntityType(typeof(T))?.GetProperties();
                        foreach (var item in properties)
                        {
                            var valueFromNewEntity = entity.GetType().GetProperty(item.Name)?.GetValue(entity);
                            if (valueFromNewEntity != null && (valueFromNewEntity.ToString() != Guid.Empty.ToString()))
                            {
                                existingEntity.GetType().GetProperty(item.Name)?.SetValue(existingEntity, valueFromNewEntity);
                            }
                        }

                        _context.Set<T>().Update(existingEntity);
                    }
                }
            }
            else
            {
                await _context.Set<T>().AddAsync(entity);
            }
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> specification, bool trackingChanges)
        {
            var query = trackingChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();
            return await query.FirstOrDefaultAsync(specification);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> specification)
        {
            return await GetAsync(specification, true);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", int take = 15, int skip = 0)
        {
            var query = _context.Set<T>().Where(expression);
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            query = query.Skip(skip).Take(take);
            if (orderBy != null)
                query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> specification, bool trackingChanges = true)
        {
            var query = _context.Set<T>().Where(specification);
            if (!trackingChanges)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> specification)
        {
            return await FindByAsync(specification, true);
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> expression, bool trackingChanges = true)
        {
            var query = _context.Set<T>().Where(expression);
            if (!trackingChanges)
            {
                query = query.AsNoTracking();
            }
            return query.AsEnumerable();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> specification)
        {
            return FindBy(specification, true);
        }

        public long Count(Expression<Func<T, bool>> specification)
        {
            return _context.Set<T>().AsNoTracking().Count(specification);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> specification)
        {
            return await _context.Set<T>().AsNoTracking().CountAsync(specification);
        }

        public bool Exists(Expression<Func<T, bool>> specification)
        {
            return _context.Set<T>().AsNoTracking().Any(specification);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> specification)
        {
            return await _context.Set<T>().AsNoTracking().AnyAsync(specification);
        }

        public IEnumerable<T> GetFilteredElements(Expression<Func<T, bool>> specification,
             long pageIndex, int pageSize, IList<SortByInfo> orderByExpression = null)
        {
            var query = BuildPagingQuery(_context.Set<T>().Where(specification), pageIndex, pageSize, orderByExpression);
            return query.AsEnumerable();
        }

        public async Task<IEnumerable<T>> GetFilteredElementsAsync(Expression<Func<T, bool>> specification,
            long pageIndex, int pageSize, IList<SortByInfo> orderByExpression = null)
        {
            var query = BuildPagingQuery(_context.Set<T>().Where(specification), pageIndex, pageSize, orderByExpression);
            return await query.ToListAsync();
        }

        //public async Task<bool> UpdateChangesTracker()
        //{
        //    var changesTracking = _context.ChangeTracker.Entries();
        //    var iAuditClient = _serviceProvider.GetService<IAuditClient>();
        //    string currentUserID = GetClaimUserId();
        //    var baseAudits = new List<BaseAudit>();
        //    DateTimeOffset ActionTime = DateTimeOffset.UtcNow;

        //    foreach (var changedEntity in changesTracking)
        //    {
        //        if (changedEntity.Entity is IBaseEntity entity)
        //        {
        //            string userIDString = string.IsNullOrEmpty(entity.Author) ?
        //                        string.IsNullOrEmpty(currentUserID) ? Guid.Empty.ToString() : currentUserID
        //                        : entity.Author;

        //            switch (changedEntity.State)
        //            {
        //                case EntityState.Added:
        //                    entity.CreatedDate = ActionTime;
        //                    entity.CreatedBy = userIDString;
        //                    entity.LastUpdatedBy = userIDString;
        //                    entity.LastUpdatedDate = ActionTime;
        //                    break;
        //                case EntityState.Modified:
        //                    entity.LastUpdatedDate = ActionTime;
        //                    entity.LastUpdatedBy = userIDString;
        //                    UpdateEntity(changedEntity, entity);
        //                    break;
        //                case EntityState.Deleted:
        //                    entity.IsDeleted = true;
        //                    entity.LastUpdatedDate = ActionTime;
        //                    entity.LastUpdatedBy = userIDString;
        //                    UpdateEntity(changedEntity, entity);
        //                    break;
        //            }

        //            if (iAuditClient != null)
        //            {
        //                var tableName = changedEntity.Metadata.DisplayName();
        //                var ignoreTables = iAuditClient.GetIgnoreTables();
        //                if (ignoreTables.Contains(tableName) | ignoreTables.Contains("*"))
        //                {
        //                    continue;
        //                }
        //                var ignoreColumns = iAuditClient.GetIgnoreColumn();
        //                var auditMessage = changedEntity.State switch
        //                {
        //                    EntityState.Deleted => CreateDeletedMessage(changedEntity, ignoreColumns),
        //                    EntityState.Modified => CreateModifiedMessage(changedEntity, ignoreColumns),
        //                    EntityState.Added => CreateAddedMessage(changedEntity, ignoreColumns),
        //                    _ => null
        //                };

        //                if (auditMessage != null)
        //                {
        //                    if (!auditMessage.Details.Any())
        //                    {
        //                        continue;
        //                    }

        //                    auditMessage.TableName = tableName;
        //                    auditMessage.UserId = userIDString.ToGuid();
        //                    auditMessage.UserAccount = GetClaimAccount();
        //                    auditMessage.ActionTime = ActionTime;
        //                    baseAudits.Add(auditMessage);
        //                }
        //            }
        //        }
        //    }
        //    if ((iAuditClient != null) && (baseAudits != null) && (baseAudits.Count > 0))
        //    {
        //        var temp = baseAudits.Where(x => x.UserId != null && x.Details.Any()).ToList();
        //        if (temp != null && temp.Count > 0)
        //        {
        //            AuditLogs auditLogs = new AuditLogs();
        //            auditLogs.BaseAudits = baseAudits;
        //            auditLogs.UserId = currentUserID.ToGuid();
        //            iAuditClient.CreateAuditLogEvent(auditLogs);
        //        }
        //    }

        //    return true;

        //    BaseAudit CreateAddedMessage(EntityEntry entry, List<string> ignoreColumn)
        //        => entry.Properties.Where(property => !ignoreColumn.Contains(property.Metadata.Name)
        //            ).Aggregate(
        //                   new BaseAudit
        //                   {
        //                       Details = new List<ValueChange>(),
        //                       Action = entry.State.ToString()
        //                   },
        //                   (auditString, property) =>
        //                   {
        //                       if (property.Metadata.Name == "Id" && Guid.TryParse(property.OriginalValue.ToString(), out Guid guid))
        //                       {
        //                           auditString.RecordId = guid;
        //                       }
        //                       else
        //                       {
        //                           auditString.Details.Add(new ValueChange()
        //                           {
        //                               Name = property.Metadata.Name,
        //                               Old = string.Empty,
        //                               New = $"{property.CurrentValue}"
        //                           });
        //                       }
        //                       return auditString;
        //                   });

        //    BaseAudit CreateModifiedMessage(EntityEntry entry, List<string> ignoreColumn)
        //        => entry.Properties.Where(property => (property.IsModified || property.Metadata.IsPrimaryKey()) &&
        //            !ignoreColumn.Contains(property.Metadata.Name)
        //            ).Aggregate(
        //            new BaseAudit
        //            {
        //                Details = new List<ValueChange>(),
        //                Action = entry.State.ToString()
        //            },
        //            (auditString, property) =>
        //            {
        //                if (property.Metadata.Name == "Id" && Guid.TryParse(property.OriginalValue.ToString(), out Guid guid))
        //                {
        //                    auditString.RecordId = guid;
        //                }
        //                else
        //                {
        //                    if (property.CurrentValue.EqualObject(property.OriginalValue))
        //                    {
        //                        return auditString;
        //                    }
        //                    else
        //                    {
        //                        if (property.Metadata.Name == "Deleted")
        //                        {
        //                            auditString.Action = EntityState.Deleted.ToString();
        //                        }
        //                        auditString.Details.Add(new ValueChange()
        //                        {
        //                            Name = property.Metadata.Name,
        //                            Old = $"{property.OriginalValue}",
        //                            New = $"{property.CurrentValue}"
        //                        });
        //                        return auditString;
        //                    }
        //                }
        //                return auditString;
        //            });

        //    BaseAudit CreateDeletedMessage(EntityEntry entry, List<string> ignoreColumn)
        //        => entry.Properties.Where(property => property.Metadata.IsPrimaryKey() &&
        //            !ignoreColumn.Contains(property.Metadata.Name)
        //            ).Aggregate(
        //            new BaseAudit
        //            {
        //                Details = new List<ValueChange>(),
        //                Action = entry.State.ToString()
        //            },
        //            (auditString, property) =>
        //            {
        //                if (property.Metadata.Name == "Id" && Guid.TryParse(property.OriginalValue.ToString(), out Guid guid))
        //                {
        //                    auditString.RecordId = guid;
        //                }
        //                else
        //                {
        //                    auditString.Details.Add(new ValueChange()
        //                    {
        //                        Name = property.Metadata.Name,
        //                        Old = $"{property.OriginalValue}",
        //                        New = string.Empty
        //                    });
        //                }
        //                return auditString;
        //            });
        //}

        public async Task<int> SaveChangesAsync()
        {
            //await UpdateChangesTracker();
            var res = await _context.SaveChangesAsync();
            return res;
        }

        //public string GetClaimUserId()
        //{
        //    try
        //    {
        //        if (_httpContextAccessor.HttpContext != null)
        //        {
        //            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //            var id = identity.FindFirst(CoreConstant.CLAIM_ID);
        //            return id == null ? "" : id.Value;
        //        }
        //    }
        //    catch
        //    {
        //        ;
        //    }
        //    return string.Empty;
        //}

        //public string GetClaimAccount()
        //{
        //    try
        //    {
        //        if (_httpContextAccessor.HttpContext != null)
        //        {
        //            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //            var account = identity.FindFirst(CoreConstant.CLAIM_ACCOUNT);
        //            return account == null ? "" : account.Value;
        //        }
        //    }
        //    catch
        //    {
        //        ;
        //    }
        //    return string.Empty;
        //}

        #endregion
        #region Member Methods
        protected static void UpdateEntity<T>(EntityEntry changedEntity, T entity)
            where T : IBaseEntity
        {
            changedEntity.Property(nameof(BaseEntity.CreatedDate)).IsModified = false;
            entity.LastUpdatedDate = DateTimeOffset.UtcNow;
            changedEntity.State = EntityState.Modified;
        }

        protected static IQueryable<T> BuildPagingQuery(IQueryable<T> query,
                long pageIndex, int pageSize, IList<SortByInfo> orderByExpression)
        {
            return BuildPagingQuery<T>(query, pageIndex, pageSize, orderByExpression);
        }

        protected static IQueryable<S> BuildPagingQuery<S>(IQueryable<S> query,
                long pageIndex, int pageSize, IList<SortByInfo> orderByExpression) where S : class
        {
            var numSkip = (pageIndex - 1) * pageSize;
            var countQuery = query.Count();
            numSkip = numSkip >= 0 && countQuery > numSkip ? numSkip : 0;
            var orderedQuery = BuildQueryWithSort(query, orderByExpression);
            var pagingQuery = orderedQuery.Skip((int)numSkip).Take(pageSize);
            return pagingQuery.AsNoTracking();
        }

        protected static IQueryable<S> BuildPagingQuery<S>(IQueryable<S> query, long pageIndex, int pageSize) where S : class
        {
            var numSkip = (pageIndex - 1) * pageSize;
            var countQuery = query.Count();
            numSkip = numSkip >= 0 && countQuery > numSkip ? numSkip : 0;
            var pagingQuery = query.Skip((int)numSkip).Take(pageSize);
            return pagingQuery.AsNoTracking();
        }

        protected static IQueryable<S> BuildPagingQueryWithTracking<S>(IQueryable<S> query, long pageIndex, int pageSize) where S : class
        {
            var numSkip = (pageIndex - 1) * pageSize;
            var countQuery = query.Count();
            numSkip = numSkip >= 0 && countQuery > numSkip ? numSkip : 0;
            var pagingQuery = query.Skip((int)numSkip).Take(pageSize);
            return pagingQuery.AsNoTracking();
        }

        protected static IOrderedQueryable<T> BuildQueryWithSort(IQueryable<T> query, IList<SortByInfo> orderByExpression)
        {
            return BuildQueryWithSort<T>(query, orderByExpression);
        }
        protected static IOrderedQueryable<S> BuildQueryWithSort<S>(IQueryable<S> query, IList<SortByInfo> orderByExpression) where S : class
        {
            IOrderedQueryable<S> finalQuery = null;
            var entityType = typeof(S);
            if (orderByExpression != null && orderByExpression.Any())
            {
                var orderCounter = 0;
                foreach (var orderByExp in orderByExpression)
                {
                    var propertyInfo = entityType.GetProperty(orderByExp.FieldName);
                    if (propertyInfo != null)
                    {
                        var selectorExp = BuildSelector(entityType, orderByExp.FieldName);

                        MethodInfo genericMethod = null;
                        if (orderCounter == 0)
                        {
                            genericMethod = orderByExp.Ascending ?
                                ORDER_BY_METHOD.MakeGenericMethod(entityType, propertyInfo.PropertyType) :
                                ORDER_BY_DESCENDING_METHOD.MakeGenericMethod(entityType, propertyInfo.PropertyType);
                            finalQuery = (IOrderedQueryable<S>)genericMethod.Invoke(genericMethod, new object[] { query, selectorExp })!;
                        }
                        else
                        {
                            genericMethod = orderByExp.Ascending ?
                                THEN_BY_METHOD.MakeGenericMethod(entityType, propertyInfo.PropertyType) :
                                THEN_BY_DESCENDING_METHOD.MakeGenericMethod(entityType, propertyInfo.PropertyType);
                            finalQuery = (IOrderedQueryable<S>)genericMethod.Invoke(genericMethod, new object[] { finalQuery, selectorExp })!;
                        }
                        orderCounter++;
                    }
                }
            }
            else
            {
                PropertyInfo sortProperty = null;
                var idPropertyInfo = entityType.GetProperty("Id");
                if (idPropertyInfo != null)
                {
                    sortProperty = idPropertyInfo;
                }
                else
                {
                    sortProperty = entityType.GetProperties().FirstOrDefault();
                }

                var selectorExp = BuildSelector(entityType, sortProperty.Name);
                var genericMethod = ORDER_BY_METHOD.MakeGenericMethod(entityType, sortProperty.PropertyType);
                finalQuery = (IOrderedQueryable<S>)genericMethod.Invoke(genericMethod, new object[] { query, selectorExp })!;
            }
            return finalQuery;
        }
        private static LambdaExpression BuildSelector(Type entityType, string fieldName)
        {
            var paramExpression = Expression.Parameter(entityType, "fieldParam");
            var property = Expression.Property(paramExpression, fieldName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { paramExpression });
            return selector;
        }

        protected const string PARAM_DELETE_TILL_DATETIME = "@DeletedTillDateTime";

        protected static readonly MethodInfo ORDER_BY_METHOD = typeof(Queryable).GetMethods()
            .First(m => IsOrderMethodMatched(m, nameof(Queryable.OrderBy)));

        protected static readonly MethodInfo ORDER_BY_DESCENDING_METHOD = typeof(Queryable).GetMethods()
            .First(m => IsOrderMethodMatched(m, nameof(Queryable.OrderByDescending)));

        protected static readonly MethodInfo THEN_BY_METHOD = typeof(Queryable).GetMethods()
            .First(m => IsOrderMethodMatched(m, nameof(Queryable.ThenBy)));

        protected static readonly MethodInfo THEN_BY_DESCENDING_METHOD = typeof(Queryable).GetMethods()
            .First(m => IsOrderMethodMatched(m, nameof(Queryable.ThenByDescending)));
        private static bool IsOrderMethodMatched(MethodInfo method, string methodName)
        {
            return method.Name == methodName && method.IsGenericMethodDefinition && method.GetParameters().Length == 2;
        }
        #endregion
    }
}
