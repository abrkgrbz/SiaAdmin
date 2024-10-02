using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Common;
using SiaAdmin.Persistence.Contexts;
using MediatR;
using System.Security.AccessControl;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Utils;

namespace SiaAdmin.Persistence.Repositories
{
    public class WriteRepository<T>:IWriteRepository<T> where T : BaseEntity
    {
        private readonly SiaAdminDbContext _context;

        public WriteRepository(SiaAdminDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return true;
        }

        public bool Remove(T entity)
        {
            EntityEntry<T> entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }
        public async Task<bool> RemoveAsync(int id)
        {
            T model = await Table.FirstOrDefaultAsync(x => x.Id == id);
            return Remove(model);
        }
        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }

        public async Task<int> SaveAsync(string userId=null,bool project=false)
        {
            if (project==true)
            {
                BeforeSaveChanges(userId);
            }
            return await _context.SaveChangesAsync();
        }
        
        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }

        private void BeforeSaveChanges(string userId)
        {
            _context.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLogs || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        if (property.CurrentValue is long || property.CurrentValue is int)
                        {
                            var numericValue = Convert.ToDecimal(property.CurrentValue);
                            if (numericValue < 0)
                            {
                                auditEntry.KeyValues[propertyName] = 0;
                                continue;
                            }
                        }
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    if (auditEntry.TableName == "SurveyAssigned" && property.Metadata.Name.Equals("SurveyId"))
                    {
                        
                        auditEntry.KeyValues["SurveyId"] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            auditEntry.UserId = userId;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.UserId = userId;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                                auditEntry.UserId = userId;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                _context.AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}
