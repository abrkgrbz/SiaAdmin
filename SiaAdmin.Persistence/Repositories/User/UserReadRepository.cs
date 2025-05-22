using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class UserReadRepository : ReadRepository<Domain.Entities.Models.User>, IUserReadRepository
    {
        private readonly DbSet<User> _users;
        private readonly DbSet<UserProfile> _userProfiles;
        private readonly DbSet<UserSurvey> _usersSurvey;
        private readonly DbSet<UserSurveyPoints> _usersSurveyPoints;
        private readonly DbSet<UserSelectIncentive> _usersSelectIncentive;
        private readonly DbSet<UserRecievedGifts> _usersRecievedGifts;
        private readonly DbSet<UserTransactionLogs> _usersTransactionLogs;
        private readonly DbSet<LastSeenAdet> _lastSeenAdets;
        private readonly DbSet<LastSeenSaat> _lastSeenSaat; 
        private readonly DbSet<UserSurveyInfo> _usersSurveyInfo;
        private readonly DbSet<ChurnData> _churnData;
        public UserReadRepository(SiaAdminDbContext context) : base(context)
        {

            _users = context.Set<User>();
            _usersSurvey = context.Set<UserSurvey>();
            _usersSurveyPoints = context.Set<UserSurveyPoints>();
            _usersSelectIncentive = context.Set<UserSelectIncentive>();
            _usersRecievedGifts = context.Set<UserRecievedGifts>();
            _usersTransactionLogs = context.Set<UserTransactionLogs>();
            _usersSurveyInfo = context.Set<UserSurveyInfo>();
            _userProfiles = context.Set<UserProfile>();
            _lastSeenAdets = context.Set<LastSeenAdet>();
            _lastSeenSaat = context.Set<LastSeenSaat>();
            _churnData=context.Set<ChurnData>();
        }


        public List<Guid> ConvertInternalGuid(List<Guid> userGuids)
        { 
            try
            {
                var userDictionary = _users.ToDictionary(x => x.SurveyUserGuid, x => x.InternalGuid);
                var convertedInternalGuids = new ConcurrentBag<Guid>();

                Parallel.ForEach(userGuids, item =>
                {
                    if (userDictionary.TryGetValue(item, out var internalGuid))
                    {
                        convertedInternalGuids.Add(internalGuid);
                    }
                });

                return convertedInternalGuids.ToList();
            }
            catch (Exception e)
            {
                throw new ApiException("Hatalı GUID'lar mevcut");
            }

       
        }

        public bool IfExistUser(string msisdn)
        {
            string query = "select * from Users where [Msisdn] = @m and Active = 1";
            object[] paramItems = new object[]
            { 
                new SqlParameter("@m", msisdn)
            };
            var result = _users.FromSqlRaw(query, paramItems).AsNoTracking().ToList();
            if (result.Count > 0)
            {
                return true;
            }

            return false;
        }

        public List<UserSurvey> GetUserSurveyList(Guid surveyUserGUID)
        {

            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            string sql;
            if (surveyUserGUID==Guid.Parse("A3A62F75-AFE3-4872-BBC9-0BBBCD2A2939"))
            {
                sql =
                    "  \r\nDECLARE @Liste TABLE (\r\n    [SurveyUserGUID] UNIQUEIDENTIFIER NOT NULL,\r\n    [SurveyId] INT NOT NULL,\r\n    [SurveyText] NVARCHAR(MAX) NULL,\r\n    [SurveyDescription] NVARCHAR(MAX) NULL,\r\n    [SurveyLink] NVARCHAR(MAX) NULL,\r\n    [SurveyLinkText] NVARCHAR(MAX) NULL,\r\n    [SurveyValidity] DATETIME NULL,\r\n    [SurveyActive] INT NULL,\r\n    [SurveyStartDate] DATETIME NULL,\r\n    [SurveyRedirect] NVARCHAR(MAX) NULL,\r\n    [SurveyPoints] INT NULL,\r\n    [Mandatory] INT NULL,\r\n    [TimeStamp] DATETIME NOT NULL\r\n); \r\nINSERT INTO @Liste\r\nSELECT *\r\nFROM ( \r\n    SELECT \r\n        u.SurveyUserGUID, \r\n        s.SurveyId, \r\n        s.SurveyText, \r\n        s.SurveyDescription, \r\n        s.SurveyLink, \r\n        s.SurveyLinkText, \r\n        s.SurveyValidity, \r\n        s.SurveyActive, \r\n        s.SurveyStartDate, \r\n        s.SurveyRedirect, \r\n        s.SurveyPoints, \r\n        s.Mandatory, \r\n        s.Timestamp\r\n    FROM Surveys s\r\n    CROSS JOIN (SELECT SurveyUserGUID FROM Users WHERE InternalGUID = @myGUID) u\r\n    WHERE s.Mandatory = 1 \r\n        AND s.SurveyActive = 1 \r\n        AND (s.SurveyStartDate IS NULL OR s.SurveyStartDate <= GETDATE())\r\n        AND (s.SurveyValidity IS NULL OR s.SurveyValidity >= GETDATE())\r\n\r\n    UNION ALL\r\n\t \r\n    SELECT \r\n        u.SurveyUserGUID, \r\n        s.SurveyId, \r\n        s.SurveyText, \r\n        s.SurveyDescription, \r\n        s.SurveyLink, \r\n        s.SurveyLinkText, \r\n        s.SurveyValidity, \r\n        s.SurveyActive, \r\n        s.SurveyStartDate, \r\n        s.SurveyRedirect, \r\n        s.SurveyPoints, \r\n        s.Mandatory, \r\n        s.Timestamp\r\n    FROM Surveys s\r\n    CROSS JOIN (SELECT SurveyUserGUID FROM Users WHERE InternalGUID = @myGUID) u\r\n    WHERE s.Mandatory = 0 \r\n        AND s.SurveyActive = 1  \r\n        AND (s.SurveyStartDate IS NULL OR s.SurveyStartDate <= GETDATE())\r\n        AND (s.SurveyValidity IS NULL OR s.SurveyValidity >= GETDATE())\r\n\r\n    UNION ALL\r\n\t \r\n    SELECT \r\n        u.SurveyUserGUID, \r\n        sa.SurveyId, \r\n        sa.SurveyText, \r\n        sa.SurveyDescription, \r\n        sa.SurveyLink, \r\n        sa.SurveyLinkText, \r\n        sa.SurveyValidity, \r\n        sa.SurveyActive, \r\n        sa.SurveyStartDate, \r\n        sa.SurveyRedirect, \r\n        sa.SurveyPoints, \r\n        0 AS Mandatory, \r\n        sa.Timestamp\r\n    FROM SurveysAssigned sa\r\n    LEFT JOIN Users u ON sa.InternalGUID = u.InternalGUID\r\n    WHERE u.InternalGUID = @myGUID\r\n        AND sa.SurveyActive = 1 \r\n        AND (sa.SurveyStartDate IS NULL OR sa.SurveyStartDate <= GETDATE())\r\n        AND (sa.SurveyValidity IS NULL OR sa.SurveyValidity >= GETDATE())\r\n) AS AllMySurveys \r\nWHERE SurveyId NOT IN (\r\n    SELECT sl.SurveyId \r\n    FROM SurveyLog sl\r\n    LEFT JOIN Users u ON sl.SurveyUserGUID = u.SurveyUserGUID\r\n    WHERE u.InternalGUID = @myGUID \r\n        AND sl.Active IN (1, -1, 2)\r\n)\r\nORDER BY SurveyValidity DESC, SurveyPoints DESC;\r\n \r\nSELECT * FROM @Liste;";
                var testResult = _usersSurvey.FromSqlRaw(sql, parameter).ToList();
                return testResult;
            }
            sql =
               "SET NOCOUNT ON\r\nDECLARE @Liste TABLE\r\n(\r\n       [SurveyUserGUID] [uniqueidentifier] NOT NULL,\r\n       [SurveyId] [int] NOT NULL,\r\n       [SurveyText] [nvarchar](max) NULL,\r\n       [SurveyDescription] [nvarchar](max) NULL,\r\n       [SurveyLink] [nvarchar](max) NULL,\r\n       [SurveyLinkText] [nvarchar](max) NULL,\r\n       [SurveyValidity] [datetime] NULL,\r\n       [SurveyActive] [int] NULL,\r\n       [SurveyStartDate] [datetime] NULL,\r\n       [SurveyRedirect] [nvarchar](max) NULL,\r\n       [SurveyPoints] [int] NULL,\r\n       [Mandatory] [int] NULL,\r\n\t   [TimeStamp] [datetime] NOT NULL\r\n)\r\n\r\ndeclare @kayitsayisi as int = 0; \r\n \r\n\r\ninsert into @Liste\r\nselect * from\r\n(\r\n/*Bu zorunlu anketleri veriyor*/\r\nSELECT\r\nu.SurveyUserGUID, Surveys.SurveyId, Surveys.SurveyText, Surveys.SurveyDescription, Surveys.SurveyLink, Surveys.SurveyLinkText, Surveys.SurveyValidity, Surveys.SurveyActive, Surveys.SurveyStartDate, Surveys.SurveyRedirect, \r\nSurveys.SurveyPoints,Surveys.Mandatory,Surveys.Timestamp\r\nFROM\r\nSurveys CROSS JOIN\r\n(\r\nSELECT SurveyUserGUID FROM Users WHERE (InternalGUID = @myGUID)\r\n) AS u\r\nWHERE\r\n(Surveys.Mandatory = 1) AND \r\n(Surveys.SurveyActive = 1) AND ((Surveys.SurveyStartDate IS NULL) OR (Surveys.SurveyStartDate <= GETDATE())) AND ((Surveys.SurveyValidity IS NULL) OR (Surveys.SurveyValidity >= GETDATE()))\r\nUNION ALL\r\n/*Bu zorunsuz zorunlu anketleri veriyor :)*/\r\nSELECT\r\nu.SurveyUserGUID, Surveys.SurveyId, Surveys.SurveyText, Surveys.SurveyDescription, Surveys.SurveyLink, Surveys.SurveyLinkText, Surveys.SurveyValidity, Surveys.SurveyActive, Surveys.SurveyStartDate, Surveys.SurveyRedirect, \r\nSurveys.SurveyPoints,Surveys.Mandatory,Surveys.Timestamp\r\nFROM\r\nSurveys CROSS JOIN\r\n(\r\nSELECT SurveyUserGUID FROM Users WHERE (InternalGUID = @myGUID)\r\n) AS u\r\nWHERE\r\n(Surveys.Mandatory = 0) AND \r\n(Surveys.SurveyActive = 1) AND Surveys.SurveyId < 5000 AND ((Surveys.SurveyStartDate IS NULL) OR (Surveys.SurveyStartDate <= GETDATE())) AND ((Surveys.SurveyValidity IS NULL) OR (Surveys.SurveyValidity >= GETDATE()))\r\nUNION ALL\r\n/*Burası da assigned olanları veriyor*/\r\nSELECT Users.SurveyUserGUID, SurveysAssigned.SurveyId, SurveysAssigned.SurveyText, SurveysAssigned.SurveyDescription, SurveysAssigned.SurveyLink, SurveysAssigned.SurveyLinkText, SurveysAssigned.SurveyValidity, \r\nSurveysAssigned.SurveyActive, SurveysAssigned.SurveyStartDate, SurveysAssigned.SurveyRedirect, SurveysAssigned.SurveyPoints, 0 as Mandatory,SurveysAssigned.Timestamp\r\nFROM\r\nSurveysAssigned LEFT OUTER JOIN\r\nUsers ON SurveysAssigned.InternalGUID = Users.InternalGUID\r\nWHERE (Users.InternalGUID = @myGUID)\r\nAND\r\n(\r\n(SurveysAssigned.SurveyActive = 1) AND ((SurveysAssigned.SurveyStartDate IS NULL) OR (SurveysAssigned.SurveyStartDate <= GETDATE())) AND ((SurveysAssigned.SurveyValidity IS NULL) OR (SurveysAssigned.SurveyValidity >= GETDATE()))\r\n)\r\n) as AllMySurveys\r\nwhere SurveyId NOT IN\r\n(\r\nSELECT [SurveyId] FROM [SiaLive].[dbo].[SurveyLog]\r\nLEFT OUTER JOIN\r\nUsers ON SurveyLog.SurveyUserGUID = Users.SurveyUserGUID\r\nwhere [InternalGUID] = @myGUID and SurveyLog.Active IN (1,-1,2)\r\n)\r\norder by SurveyValidity DESC, SurveyPoints DESC\r\n\r\n--select * from @Liste;\r\n\r\nselect @kayitsayisi=count(*) from @Liste where SurveyId < 5000 and Mandatory = 1\r\n\r\nIF (@kayitsayisi = 0)\r\nBEGIN\r\n       select * from @Liste;\r\nEND\r\nELSE\r\nBEGIN\r\n       select * from @Liste where SurveyId < 5000 order by Mandatory Desc;\r\nEND\r\n";
            var result = _usersSurvey.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            return result;
        }

        public UserSurveyPoints GetUserSurveyPoints(Guid surveyUserGUID)
        {
            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            var surveyPoint = new UserSurveyPoints();
            string sql =
                "SELECT  ISNULL(sum([SurveyPoints]),0) as SurveyPoints FROM [SiaLive].[dbo].[SurveyLog] where SurveyUserGUID IN \r\n(SELECT [SurveyUserGUID] FROM [SiaLive].[dbo].[Users] where Active = 1 and [InternalGUID] = @myGUID) and  Active = 1 and Approved = 1";
            var result = _usersSurveyPoints.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            surveyPoint.SurveyPoints = result[0].SurveyPoints;
            return surveyPoint;

        }

        public List<UserSelectIncentive> GetUserSelectIncentiveList(Guid surveyUserGUID)
        {
            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            string sql =
                "declare @myGOAL as integer = 0;\r\nSELECT  @myGOAL = ISNULL(sum([SurveyPoints]),0) FROM [SiaLive].[dbo].[SurveyLog] where SurveyUserGUID IN (SELECT [SurveyUserGUID] FROM [SiaLive].[dbo].[Users] where Active = 1 and [InternalGUID] = @myGUID) and  Active = 1 and Approved = 1\r\n \r\nSELECT [Id],[IncentiveText],[Points] FROM [SiaLive].[dbo].[Incentives] where [Active] = 1 and [showInDisplay]=1 and [Points]<= @myGOAL";
            var result = _usersSelectIncentive.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            return result;
        }

        public List<UserRecievedGifts> GetUserRecievedGiftsList(Guid surveyUserGUID)
        {
            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            string sql =
                "SELECT convert(varchar,SurveyLog.Timestamp,104) as Tarih, coalesce(Surveys.SurveyText,Text) as SurveyText, SurveyLog.SurveyPoints, SurveyLog.Active, SurveyLog.Approved FROM SurveyLog LEFT OUTER JOIN Surveys ON SurveyLog.SurveyId = Surveys.SurveyId WHERE (SurveyLog.SurveyUserGUID IN (SELECT top 1 SurveyUserGUID FROM Users WHERE (InternalGUID = @myGUID))) order by Id DESC";
            var result = _usersRecievedGifts.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            return result;
        }

        public List<UserTransactionLogs> GetUserTransactionLogsList(Guid surveyUserGUID)
        {
            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            string sql =
                "SELECT convert(varchar,SurveyLog.Timestamp,104) as Tarih, coalesce(Surveys.SurveyText,Text) as SurveyText, SurveyLog.SurveyPoints*-1 as SurveyPoints, SurveyLog.Active, SurveyLog.Approved FROM SurveyLog LEFT OUTER JOIN Surveys ON SurveyLog.SurveyId = Surveys.SurveyId WHERE (SurveyLog.SurveyUserGUID IN (SELECT top 1 SurveyUserGUID FROM Users WHERE (InternalGUID = @myGUID))) AND SurveyLog.SurveyId = 0 and SurveyLog.SurveyPoints < 0 order by Id DESC";
            var result = _usersTransactionLogs.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            return result;

        }

        public UserProfile GetUserProfile(Guid surveyUserGUID)
        {
            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            var userProfile = new UserProfile();
            string sql =
                "  SELECT [RegionCode]+[Msisdn] as telephone,[Email] as email ,[Name] as username,[Surname] as surname,[ContactChannel] as totalmessaging,[Birthdate] as birthdate,[Sex] as gender,[Location] as location,[MyReference] as reference,[ReferredBy] as referredby FROM  [dbo].[Users] where [InternalGUID] = @myGUID";
            var result = _userProfiles.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            userProfile = result[0];
            return userProfile;
        }

        public List<UserSurveyInfo> GetUserSurveyInfo(Guid surveyUserGUID)
        {
            var parameter = new SqlParameter("@myGUID", SqlDbType.UniqueIdentifier);
            parameter.Value = surveyUserGUID;
            string sql =
                "SELECT convert(varchar,SurveyLog.Timestamp,104) as Tarih, coalesce(Surveys.SurveyText,Text) as SurveyText, SurveyLog.SurveyPoints, SurveyLog.Active, SurveyLog.Approved FROM SurveyLog \r\nLEFT OUTER JOIN Surveys ON SurveyLog.SurveyId = Surveys.SurveyId WHERE (SurveyLog.SurveyUserGUID IN (SELECT top 1 SurveyUserGUID FROM Users WHERE (InternalGUID = @myGUID)))\r\nand Surveys.SurveyId>0\r\norder by Id DESC";
            var result = _usersSurveyInfo.FromSqlRaw(sql, parameter).AsNoTracking().ToList();
            return result;
        }

        public async Task<List<LastSeenAdet>> GetListLastSeenAdet()
        {
            string sql =
                "SELECT \r\n    FORMAT(LastLogin, 'yyyy-MM-dd') AS LastSeen, \r\n    COUNT(*) AS Adet \r\nFROM \r\n    [dbo].[Users] \r\nWHERE \r\n    Active = 0 \r\n    AND LastLogin BETWEEN '2024-01-01' AND GETDATE() \r\nGROUP BY \r\n    FORMAT(LastLogin, 'yyyy-MM-dd');\r\n";
            var result = await _lastSeenAdets.FromSqlRaw(sql).ToListAsync();
            return result;
        }

        public async Task<List<LastSeenSaat>> GetListLastSeenSaat()
        {
            string sql =
                "SELECT \r\n    LastSeen, \r\n    AVG(sss) AS ProfilYasamSaatDegeri \r\nFROM \r\n(\r\n    SELECT \r\n        TOP 90 PERCENT \r\n        FORMAT(LastLogin, 'MM') AS LastSeen,\r\n        CAST(DATEDIFF(SECOND, [RegistrationDate], LastLogin) AS BIGINT) / 60 / 60 AS sss\r\n    FROM \r\n        [SiaLive].[dbo].[Users]\r\n    WHERE \r\n        Active = 0 \r\n        AND LastLogin BETWEEN '2024-01-01' AND GETDATE()\r\n    ORDER BY \r\n        CAST(DATEDIFF(SECOND, [RegistrationDate], LastLogin) AS BIGINT)\r\n) a\r\nGROUP BY \r\n    LastSeen;";
            var result = await _lastSeenSaat.FromSqlRaw(sql).ToListAsync();
            return result;
        }

        public IQueryable<ChurnData> GetListChurnData()
        {
            string sql = @"SELECT [InternalGUID]
      ,[RegistrationDate]
      ,[LastLogin]
      ,[Msisdn]
      ,[Email],
      IIF ((( [Reason]) & (1) = 1), 1,0) as 'cazipdegil',
      IIF ((( [Reason]) & (2) = 2), 1,0) as 'ilgisiz',
      IIF ((( [Reason]) & (4) = 4), 1,0) as 'kimole',
      IIF ((( [Reason]) & (8) = 8), 1,0) as 'korkuyorum',
      IIF ((( [Reason]) & (16) = 16), 1,0) as 'vakitsizim',
      IIF ((( [Reason]) & (32) = 32), 1,0) as 'dusukcene',
      IIF ((( [Reason]) & (64) = 64), 1,0) as 'diger'
      FROM [SiaLive].[dbo].[Users]
      where Active = 0 and Reason is not null ";

           return _churnData.FromSqlRaw(sql);
        }

        public async Task<List<ChurnData>> GetAllChurnData()
        {
            string sql = @"SELECT [InternalGUID]
      ,[RegistrationDate]
      ,[LastLogin]
      ,[Msisdn]
      ,[Email],
      IIF ((( [Reason]) & (1) = 1), 1,0) as 'cazipdegil',
      IIF ((( [Reason]) & (2) = 2), 1,0) as 'ilgisiz',
      IIF ((( [Reason]) & (4) = 4), 1,0) as 'kimole',
      IIF ((( [Reason]) & (8) = 8), 1,0) as 'korkuyorum',
      IIF ((( [Reason]) & (16) = 16), 1,0) as 'vakitsizim',
      IIF ((( [Reason]) & (32) = 32), 1,0) as 'dusukcene',
      IIF ((( [Reason]) & (64) = 64), 1,0) as 'diger'
      FROM [SiaLive].[dbo].[Users]
      where Active = 0 and Reason is not null ";
            return await _churnData.FromSqlRaw(sql).ToListAsync();
        }
    }
}
