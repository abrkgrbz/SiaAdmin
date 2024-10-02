using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
    {
        private readonly DbSet<User> _users;
        private SiaAdminDbContext _context;
        private ISurveyLogWriteRepository _surveyLogWriteRepository;
        public UserWriteRepository(SiaAdminDbContext context, ISurveyLogWriteRepository surveyLogWriteRepository) : base(context)
        {
            _users = context.Set<User>();
            _context = context;
            _surveyLogWriteRepository = surveyLogWriteRepository;
        }


        public void InsertOrUpdateUser(string regionCode, string msisdn, string ip, string browser, bool checkUser)
        {

            object[] paramItems = new object[]
            {
                new SqlParameter("@r", regionCode),
                new SqlParameter("@m", msisdn),
                new SqlParameter("@i", ip),
                new SqlParameter("@b", browser),
                new SqlParameter("@d",DateTime.Now),
                new SqlParameter("@rd",DateTime.Now)

            };
            if (!checkUser)
            {
                string insertQuery =
                    "Insert into Users (RegionCode,Msisdn,LastLogin,LoginCount,LastBrowser,LastIP,RegistrationDate,Active) values (@r,@m,@d,1,@b,@i,@rd,1)";
                _context.Database.ExecuteSqlRaw(insertQuery, paramItems);

            }
            else
            {
                string updateQuery =
                    "UPDATE Users SET LastLogin = @d, LoginCount = LoginCount + 1,LastBrowser=@b,LastIP=@i where [RegionCode] = @r and [Msisdn] = @m and Active = 1";
                _context.Database.ExecuteSqlRaw(updateQuery, paramItems);

            }

        }

        public async Task<int> DeleteUser(Guid internalGuid)
        {
            string deleteUserQuery =
                "update \r\n[SiaLive].[dbo].[Users]\r\nset \r\n  [Active] = 0\r\nwhere [InternalGUID] = @myGUID\r\n";
            object[] paramItems = new object[]
            {
                new SqlParameter("@myGUID",internalGuid),

            };
            int result = await _context.Database.ExecuteSqlRawAsync(deleteUserQuery, paramItems);
            return result;
        }

        public async Task<int> UpdateUserProfile(string email, string name, string surname, int birthdate, int contactChannel, int sex, int location, Guid internalGuid)
        {
            string updateProfileSqlQuery =
                " set nocount on; \r\ndeclare @referredbyonceki as [varchar](16) = NULL;\r\ndeclare @kisidatetime as datetime;\r\n\r\ndeclare @length as integer = 0;\r\ndeclare @PoolLength as integer = 0;\r\ndeclare @LoopCount as integer = 0;\r\ndeclare @refpuanikisisi as uniqueidentifier;\r\ndeclare @antifraud as integer = 0;\r\n\r\ndeclare @CharPool as nvarchar(50) = '';\r\ndeclare @RandomString as nvarchar(16) = '';\r\n\r\nSET @Length = 16;\r\nSET @CharPool = '2456789ACFGHJKMNPQRTWXYZ'\r\nSET @PoolLength = Len(@CharPool)\r\n\r\nSET @LoopCount = 0\r\nSET @RandomString = ''\r\n\r\nWHILE (@LoopCount < @Length) BEGIN\r\n    SELECT @RandomString = @RandomString + substring(@CharPool,(abs(checksum(newid())) % 26)+1, 1)\r\n    SELECT @LoopCount = @LoopCount + 1\r\nEND\r\n\r\nset @RandomString = convert(varchar,Left(@RandomString,8)) + convert(varchar,Right('000'+DATEPART(DAYOFYEAR, GETDATE()),3)) + convert(varchar,Right(@RandomString,8))\r\nupdate \r\n[SiaLive].[dbo].[Users]\r\nset \r\n  [Email] = @email, \r\n  [Name] = COALESCE([Name], @username), \r\n  [Surname] = COALESCE([Surname], @surname), \r\n  [ContactChannel] = @totalmessaging, \r\n  [Birthdate] = COALESCE([Birthdate], @birthdate), \r\n  [Sex] = COALESCE([Sex] , @gender),\r\n  [Location] = COALESCE([Location] , @location),\r\n  [MyReference] = COALESCE([MyReference] , @RandomString),\r\n  @referredbyonceki = ISNULL([ReferredBy],'X')\r\nwhere [InternalGUID] = @myGUID\r\n";
            object[] paramItems = new object[]
            {
                new SqlParameter("@myGUID",internalGuid),
                new SqlParameter("@username",name),
                new SqlParameter("@email",email),
                new SqlParameter("@totalmessaging",contactChannel),
                new SqlParameter("@surname",surname),
                new SqlParameter("@birthdate",birthdate),
                new SqlParameter("@gender",sex),
                new SqlParameter("@location",location)

            };
            return await _context.Database.ExecuteSqlRawAsync(updateProfileSqlQuery, paramItems);

        }
 

       
    }
}
